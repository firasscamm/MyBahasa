using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace MyBahasa
{
    public partial class Lesson : System.Web.UI.Page
    {
        // ✅ FIXED: Using ViewState instead of static variables
        private List<Phrase> Phrases
        {
            get
            {
                if (ViewState["Phrases"] == null)
                    ViewState["Phrases"] = new List<Phrase>();
                return (List<Phrase>)ViewState["Phrases"];
            }
            set { ViewState["Phrases"] = value; }
        }

        private int CurrentIndex
        {
            get
            {
                return ViewState["CurrentIndex"] != null ? (int)ViewState["CurrentIndex"] : 0;
            }
            set { ViewState["CurrentIndex"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int lessonId;
                if (Request.QueryString["lesson_id"] != null && int.TryParse(Request.QueryString["lesson_id"], out lessonId))
                {
                    LoadLessonData(lessonId);

                    if (Session["user_id"] != null)
                    {
                        int userId = Convert.ToInt32(Session["user_id"]);
                        int courseId = GetCourseIdFromLesson(lessonId);
                        InitializeProgress(userId, courseId, lessonId);
                    }

                    DisplayPhrase();
                }
                else
                {
                    Response.Redirect("learn.aspx");
                }
            }
        }

        private void LoadLessonData(int lessonId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                SqlCommand cmdLesson = new SqlCommand("SELECT title FROM Lessons WHERE lesson_id = @lesson_id", con);
                cmdLesson.Parameters.AddWithValue("@lesson_id", lessonId);
                object title = cmdLesson.ExecuteScalar();
                lblLessonTitle.Text = title?.ToString() ?? "Lesson";

                SqlCommand cmd = new SqlCommand(@"
                    SELECT malay_text, english_text, pronunciation, cultural_note, audio_url, video_url
                    FROM LessonPhrases 
                    WHERE lesson_id = @lesson_id
                    ORDER BY phrase_id", con);
                cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                SqlDataReader reader = cmd.ExecuteReader();

                Phrases.Clear();
                while (reader.Read())
                {
                    Phrases.Add(new Phrase
                    {
                        Malay = reader["malay_text"].ToString(),
                        English = reader["english_text"].ToString(),
                        Pronunciation = reader["pronunciation"].ToString(),
                        Note = reader["cultural_note"].ToString(),
                        Audio = reader["audio_url"].ToString(),
                        Video = reader["video_url"].ToString()
                    });
                }
                reader.Close();
            }
            CurrentIndex = 0;
        }

        private void InitializeProgress(int userId, int courseId, int lessonId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM Progress WHERE user_id = @user_id AND lesson_id = @lesson_id)
                        INSERT INTO Progress (user_id, course_id, lesson_id, completion_percent, score, last_updated)
                        VALUES (@user_id, @course_id, @lesson_id, 0, 0, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@course_id", courseId);
                cmd.Parameters.AddWithValue("@lesson_id", lessonId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateProgressPartial(int completedCount, int totalCount)
        {
            if (Session["user_id"] == null) return;

            int userId = Convert.ToInt32(Session["user_id"]);
            int lessonId = Convert.ToInt32(Request.QueryString["lesson_id"]);
            int courseId = GetCourseIdFromLesson(lessonId);

            decimal percent = Math.Round(((decimal)completedCount / totalCount) * 100, 2);

            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
            IF EXISTS (SELECT 1 FROM Progress WHERE user_id = @user_id AND lesson_id = @lesson_id)
                UPDATE Progress 
                SET completion_percent = @percent, last_updated = GETDATE()
                WHERE user_id = @user_id AND lesson_id = @lesson_id
            ELSE
                INSERT INTO Progress (user_id, course_id, lesson_id, completion_percent, score, last_updated)
                VALUES (@user_id, @course_id, @lesson_id, @percent, 0, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@course_id", courseId);
                cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                cmd.Parameters.AddWithValue("@percent", percent);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private int GetCourseIdFromLesson(int lessonId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT course_id FROM Lessons WHERE lesson_id = @lesson_id", con);
                cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void DisplayPhrase()
        {
            if (Phrases.Count == 0)
            {
                lblMalayText.Text = "No phrases available.";
                lblEnglishText.Text = "";
                lblPronunciation.Text = "";
                lblNote.Text = "";
                ltlAudio.Text = "";
                ltlVideo.Text = "";
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                return;
            }

            Phrase current = Phrases[CurrentIndex];
            lblMalayText.Text = current.Malay;
            lblEnglishText.Text = current.English;
            lblPronunciation.Text = current.Pronunciation;
            lblNote.Text = current.Note;

            ltlAudio.Text = !string.IsNullOrEmpty(current.Audio)
                ? $"<audio controls><source src='{current.Audio}' type='audio/mpeg'></audio>"
                : "";

            ltlVideo.Text = !string.IsNullOrEmpty(current.Video)
                ? $"<iframe width='560' height='315' src='{current.Video.Replace("watch?v=", "embed/")}' " +
                  "title='Phrase Video' frameborder='0' allowfullscreen></iframe>"
                : "";

            UpdateProgressPartial(CurrentIndex + 1, Phrases.Count);

            btnPrev.Enabled = CurrentIndex > 0;
            btnNext.Enabled = CurrentIndex < Phrases.Count - 1;
            btnPrev.CssClass = btnPrev.Enabled ? "btn" : "btn btn-disabled";
            btnNext.CssClass = btnNext.Enabled ? "btn" : "btn btn-disabled";

            btnQuiz.Visible = (CurrentIndex == Phrases.Count - 1);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentIndex < Phrases.Count - 1)
                CurrentIndex++;
            DisplayPhrase();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentIndex > 0)
                CurrentIndex--;
            DisplayPhrase();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("learn.aspx");
        }

        protected void btnQuiz_Click(object sender, EventArgs e)
        {
            int lessonId = Convert.ToInt32(Request.QueryString["lesson_id"]);
            Response.Redirect("TakeQuiz.aspx?lesson_id=" + lessonId);
        }

        // ✅ FIXED: Added [Serializable] attribute to make Phrase class serializable
        [Serializable]
        public class Phrase
        {
            public string Malay { get; set; }
            public string English { get; set; }
            public string Pronunciation { get; set; }
            public string Note { get; set; }
            public string Audio { get; set; }
            public string Video { get; set; }
        }
    }
}