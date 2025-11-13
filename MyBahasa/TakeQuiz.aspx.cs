using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyBahasa
{
    public partial class TakeQuiz : Page
    {
        private readonly string conStr =
            WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

        // Cache questions (with choices) for this render
        private List<QuestionModel> _questions;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // require login to take a quiz
                if (Session["user_id"] == null)
                {
                    Response.Redirect("login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
                    return;
                }

                if (int.TryParse(Request.QueryString["lesson_id"], out int lessonId))
                {
                    LoadLessonTitle(lessonId);
                    _questions = LoadQuestions(lessonId);
                    // stash in ViewState for ItemDataBound usage on first render
                    ViewState["questions"] = _questions;
                    rptQuestions.DataSource = _questions;
                    rptQuestions.DataBind();
                }
                else
                {
                    Response.Redirect("learn.aspx");
                }
            }
            else
            {
                // recover questions list for postback events
                _questions = ViewState["questions"] as List<QuestionModel>;
            }
        }

        private void LoadLessonTitle(int lessonId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("SELECT title FROM Lessons WHERE lesson_id=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", lessonId);
                con.Open();
                lblLessonTitle.Text = cmd.ExecuteScalar()?.ToString() ?? "Quiz";
            }
        }

        private List<QuestionModel> LoadQuestions(int lessonId)
        {
            var list = new List<QuestionModel>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // questions
                using (SqlCommand qCmd = new SqlCommand(
                           "SELECT question_id, stem_text FROM Questions WHERE lesson_id=@id ORDER BY question_id", con))
                {
                    qCmd.Parameters.AddWithValue("@id", lessonId);
                    using (SqlDataReader qReader = qCmd.ExecuteReader())
                    {
                        while (qReader.Read())
                        {
                            list.Add(new QuestionModel
                            {
                                QuestionID = (int)qReader["question_id"],
                                StemText = qReader["stem_text"].ToString(),
                                Choices = new List<ChoiceModel>()
                            });
                        }
                    }
                }

                // choices for each question
                foreach (var q in list)
                {
                    using (SqlCommand cCmd = new SqlCommand(
                               "SELECT choice_id, choice_text, is_correct FROM Choices WHERE question_id=@qid ORDER BY choice_id", con))
                    {
                        cCmd.Parameters.AddWithValue("@qid", q.QuestionID);
                        using (SqlDataReader cReader = cCmd.ExecuteReader())
                        {
                            while (cReader.Read())
                            {
                                q.Choices.Add(new ChoiceModel
                                {
                                    ChoiceID = (int)cReader["choice_id"],
                                    ChoiceText = cReader["choice_text"].ToString(),
                                    IsCorrect = Convert.ToBoolean(cReader["is_correct"])
                                });
                            }
                        }
                    }
                }
            }

            return list;
        }

        // Bind child choices for each question and set radio GroupName
        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var question = (QuestionModel)e.Item.DataItem;

            var rptChoices = (Repeater)e.Item.FindControl("rptChoices");
            rptChoices.DataSource = question.Choices;
            rptChoices.DataBind();

            // Store question ID in a HiddenField
            var hfQuestionId = (HiddenField)e.Item.FindControl("hfQuestionId");
            if (hfQuestionId != null)
            {
                hfQuestionId.Value = question.QuestionID.ToString();
            }
        }

        // For each choice row: set the RadioButton GroupName to "q{QuestionID}"
        protected void rptChoices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            // Get parent question RepeaterItem
            var parentQuestionItem = (RepeaterItem)((Repeater)e.Item.NamingContainer).NamingContainer;

            // Get question ID from HiddenField
            var hfQuestionId = (HiddenField)parentQuestionItem.FindControl("hfQuestionId");
            string qid = hfQuestionId?.Value ?? "0";

            var rb = (RadioButton)e.Item.FindControl("rbChoice");

            // Enforce single-answer by group per question
            if (rb != null)
            {
                rb.GroupName = "question_" + qid;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_questions == null || _questions.Count == 0) return;

            int userId = Convert.ToInt32(Session["user_id"]);
            int lessonId = Convert.ToInt32(Request.QueryString["lesson_id"]);

            int totalQuestions = 0, correctAnswers = 0;
            int attemptId = 0;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // create attempt
                using (SqlCommand cmdAttempt = new SqlCommand(@"
                    INSERT INTO Attempts (user_id, lesson_id, score, total_questions, correct_answers, taken_at)
                    OUTPUT INSERTED.attempt_id
                    VALUES (@user_id, @lesson_id, 0, 0, 0, SYSDATETIME())", con))
                {
                    cmdAttempt.Parameters.AddWithValue("@user_id", userId);
                    cmdAttempt.Parameters.AddWithValue("@lesson_id", lessonId);
                    attemptId = (int)cmdAttempt.ExecuteScalar();
                }

                // walk questions on the page to detect selected choice
                foreach (RepeaterItem qItem in rptQuestions.Items)
                {
                    var hfQ = (HiddenField)qItem.FindControl("hfQuestionId");
                    if (hfQ == null) continue;

                    int questionId = Convert.ToInt32(hfQ.Value);
                    totalQuestions++;

                    // find selected radio in this question
                    int? selectedChoiceId = null;
                    var choicesRepeater = (Repeater)qItem.FindControl("rptChoices");
                    foreach (RepeaterItem cItem in choicesRepeater.Items)
                    {
                        var rb = (RadioButton)cItem.FindControl("rbChoice");
                        if (rb != null && rb.Checked)
                        {
                            var hfChoiceId = (HiddenField)cItem.FindControl("hfChoiceId");
                            selectedChoiceId = Convert.ToInt32(hfChoiceId.Value);
                            break;
                        }
                    }

                    if (selectedChoiceId == null) continue;

                    bool isCorrect;
                    using (SqlCommand checkCmd = new SqlCommand("SELECT is_correct FROM Choices WHERE choice_id=@id", con))
                    {
                        checkCmd.Parameters.AddWithValue("@id", selectedChoiceId.Value);
                        isCorrect = Convert.ToBoolean(checkCmd.ExecuteScalar());
                    }

                    if (isCorrect) correctAnswers++;

                    using (SqlCommand saveCmd = new SqlCommand(@"
                        INSERT INTO AttemptAnswers (attempt_id, question_id, choice_id, is_correct)
                        VALUES (@attempt_id, @question_id, @choice_id, @is_correct)", con))
                    {
                        saveCmd.Parameters.AddWithValue("@attempt_id", attemptId);
                        saveCmd.Parameters.AddWithValue("@question_id", questionId);
                        saveCmd.Parameters.AddWithValue("@choice_id", selectedChoiceId.Value);
                        saveCmd.Parameters.AddWithValue("@is_correct", isCorrect);
                        saveCmd.ExecuteNonQuery();
                    }
                }

                var score = (totalQuestions > 0)
                    ? Math.Round((decimal)correctAnswers / totalQuestions * 100, 2)
                    : 0;

                using (SqlCommand updateCmd = new SqlCommand(@"
                    UPDATE Attempts SET score=@score, total_questions=@tq, correct_answers=@ca
                    WHERE attempt_id=@id", con))
                {
                    updateCmd.Parameters.AddWithValue("@score", score);
                    updateCmd.Parameters.AddWithValue("@tq", totalQuestions);
                    updateCmd.Parameters.AddWithValue("@ca", correctAnswers);
                    updateCmd.Parameters.AddWithValue("@id", attemptId);
                    updateCmd.ExecuteNonQuery();
                }
            }

            // Calculate score and update Progress table
            var finalScore = (totalQuestions > 0)
                ? Math.Round((decimal)correctAnswers / totalQuestions * 100, 2)
                : 0;

            // Update Progress table with quiz score
            UpdateProgressWithQuizScore(userId, lessonId, finalScore);

            // Redirect to ReviewAttempt page
            Response.Redirect($"ReviewAttempt.aspx?attempt_id={attemptId}");
        }

        // Method to update Progress table with quiz score
        private void UpdateProgressWithQuizScore(int userId, int lessonId, decimal score)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"git
                    IF EXISTS (SELECT 1 FROM Progress WHERE user_id = @user_id AND lesson_id = @lesson_id)
                        UPDATE Progress 
                        SET score = @score, completion_percent = 100, last_updated = GETDATE()
                        WHERE user_id = @user_id AND lesson_id = @lesson_id
                    ELSE
                        INSERT INTO Progress (user_id, course_id, lesson_id, completion_percent, score, last_updated)
                        VALUES (@user_id, (SELECT course_id FROM Lessons WHERE lesson_id = @lesson_id), @lesson_id, 100, @score, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                cmd.Parameters.AddWithValue("@score", score);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Data models
        [Serializable]
        public class QuestionModel
        {
            public int QuestionID { get; set; }
            public string StemText { get; set; }
            public List<ChoiceModel> Choices { get; set; }
        }

        [Serializable]
        public class ChoiceModel
        {
            public int ChoiceID { get; set; }
            public string ChoiceText { get; set; }
            public bool IsCorrect { get; set; }
        }
    }
}