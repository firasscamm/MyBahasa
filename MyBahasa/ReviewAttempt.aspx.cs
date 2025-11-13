using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace MyBahasa
{
    public partial class ReviewAttempt : System.Web.UI.Page
    {
        private string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user_id"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                int attemptId;
                if (int.TryParse(Request.QueryString["attempt_id"], out attemptId))
                {
                    LoadAttemptReview(attemptId);
                }
                else
                {
                    Response.Redirect("Progress.aspx");
                }
            }
        }

        private void LoadAttemptReview(int attemptId)
        {
            int totalQuestions = 0;
            decimal score = 0;

            List<QuestionReview> questions = new List<QuestionReview>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // Get attempt summary
                SqlCommand cmdAttempt = new SqlCommand("SELECT total_questions, score FROM Attempts WHERE attempt_id=@id", con);
                cmdAttempt.Parameters.AddWithValue("@id", attemptId);
                SqlDataReader reader = cmdAttempt.ExecuteReader();
                if (reader.Read())
                {
                    totalQuestions = Convert.ToInt32(reader["total_questions"]);
                    score = Convert.ToDecimal(reader["score"]);
                }
                reader.Close();

                // Get all questions for this attempt
                SqlCommand qCmd = new SqlCommand(@"
                    SELECT DISTINCT Q.question_id, Q.stem_text
                    FROM AttemptAnswers AA
                    INNER JOIN Questions Q ON AA.question_id = Q.question_id
                    WHERE AA.attempt_id = @attempt_id", con);
                qCmd.Parameters.AddWithValue("@attempt_id", attemptId);

                SqlDataReader qReader = qCmd.ExecuteReader();
                while (qReader.Read())
                {
                    questions.Add(new QuestionReview
                    {
                        QuestionID = Convert.ToInt32(qReader["question_id"]),
                        QuestionText = qReader["stem_text"].ToString(),
                        Choices = new List<ChoiceReview>()
                    });
                }
                qReader.Close();

                // For each question, get all choices and mark selection
                foreach (var q in questions)
                {
                    SqlCommand cCmd = new SqlCommand(@"
                        SELECT 
                            C.choice_id, C.choice_text, C.is_correct,
                            CASE WHEN AA.choice_id = C.choice_id THEN 1 ELSE 0 END AS is_selected
                        FROM Choices C
                        LEFT JOIN AttemptAnswers AA 
                            ON C.choice_id = AA.choice_id AND AA.attempt_id = @attempt_id
                        WHERE C.question_id = @qid", con);

                    cCmd.Parameters.AddWithValue("@attempt_id", attemptId);
                    cCmd.Parameters.AddWithValue("@qid", q.QuestionID);

                    SqlDataReader cReader = cCmd.ExecuteReader();
                    while (cReader.Read())
                    {
                        q.Choices.Add(new ChoiceReview
                        {
                            ChoiceText = cReader["choice_text"].ToString(),
                            IsCorrect = Convert.ToBoolean(cReader["is_correct"]),
                            IsSelected = Convert.ToBoolean(cReader["is_selected"])
                        });
                    }
                    cReader.Close();
                }
            }

            rptReview.DataSource = questions;
            rptReview.DataBind();

            lblScoreSummary.Text = $"Score: {score}% ({questions.Count} questions)";
        }

        // helper models
        public class QuestionReview
        {
            public int QuestionID { get; set; }
            public string QuestionText { get; set; }
            public List<ChoiceReview> Choices { get; set; }
        }

        public class ChoiceReview
        {
            public string ChoiceText { get; set; }
            public bool IsCorrect { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
