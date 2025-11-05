using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace MyBahasa
{
    public partial class Lesson : System.Web.UI.Page
    {
        private static List<Phrase> phrases = new List<Phrase>();
        private static int currentIndex = 0;

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
                        UpdateProgress(userId, courseId, lessonId);
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

                // Get title and video
                SqlCommand cmdLesson = new SqlCommand("SELECT title, video_url FROM Lessons WHERE lesson_id = @lesson_id", con);
                cmdLesson.Parameters.AddWithValue("@lesson_id", lessonId);
                SqlDataReader lessonReader = cmdLesson.ExecuteReader();

                string videoUrl = "";
                if (lessonReader.Read())
                {
                    lblLessonTitle.Text = lessonReader["title"].ToString();
                    videoUrl = lessonReader["video_url"]?.ToString() ?? "";
                }
                lessonReader.Close();

                // Load lesson phrases
                SqlCommand cmd = new SqlCommand(@"SELECT malay_text, english_text, pronunciation, cultural_note, audio_url 
                                                  FROM LessonPhrases 
                                                  WHERE lesson_id = @lesson_id", con);
                cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                SqlDataReader reader = cmd.ExecuteReader();

                phrases.Clear();
                while (reader.Read())
                {
                    phrases.Add(new Phrase
                    {
                        Malay = reader["malay_text"].ToString(),
                        English = reader["english_text"].ToString(),
                        Pronunciation = reader["pronunciation"].ToString(),
                        Note = reader["cultural_note"].ToString(),
                        Audio = reader["audio_url"].ToString()
                    });
                }
                reader.Close();

                // ✅ Embed YouTube video if exists
                if (!string.IsNullOrEmpty(videoUrl))
                {
                    string embedUrl = ConvertToEmbedUrl(videoUrl);
                    videoContainer.InnerHtml = $"<iframe src='{embedUrl}' allowfullscreen></iframe>";
                }
                else
                {
                    videoContainer.InnerHtml = "";
                }
            }

            currentIndex = 0;
        }

        private string ConvertToEmbedUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            if (url.Contains("watch?v="))
                return url.Replace("watch?v=", "embed/");
            else if (url.Contains("youtu.be/"))
                return url.Replace("youtu.be/", "www.youtube.com/embed/");
            else
                return url;
        }

        private void UpdateProgress(int userId, int courseId, int lessonId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    IF EXISTS (SELECT 1 FROM Progress WHERE user_id = @user_id AND lesson_id = @lesson_id)
                        UPDATE Progress 
                        SET completion_percent = 100, last_updated = GETDATE()
                        WHERE user_id = @user_id AND lesson_id = @lesson_id
                    ELSE
                        INSERT INTO Progress (user_id, course_id, lesson_id, completion_percent, score, last_updated)
                        VALUES (@user_id, @course_id, @lesson_id, 100, 0, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@course_id", courseId);
                cmd.Parameters.AddWithValue("@lesson_id", lessonId);

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
            if (phrases.Count == 0)
            {
                lblMalayText.Text = "No phrases available.";
                lblEnglishText.Text = "";
                lblPronunciation.Text = "";
                lblNote.Text = "";
                ltlAudio.Text = "";
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                return;
            }

            Phrase current = phrases[currentIndex];
            lblMalayText.Text = current.Malay;
            lblEnglishText.Text = current.English;
            lblPronunciation.Text = current.Pronunciation;
            lblNote.Text = current.Note;

            ltlAudio.Text = !string.IsNullOrEmpty(current.Audio)
                ? $"<audio controls><source src='{current.Audio}' type='audio/mpeg'></audio>"
                : "";

            btnPrev.Enabled = currentIndex > 0;
            btnNext.Enabled = currentIndex < phrases.Count - 1;

            btnPrev.CssClass = btnPrev.Enabled ? "btn" : "btn btn-disabled";
            btnNext.CssClass = btnNext.Enabled ? "btn" : "btn btn-disabled";

            lblProgress.Text = $"Phrase {currentIndex + 1} of {phrases.Count}";
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex < phrases.Count - 1)
                currentIndex++;
            DisplayPhrase();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
                currentIndex--;
            DisplayPhrase();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("learn.aspx");
        }

        public class Phrase
        {
            public string Malay { get; set; }
            public string English { get; set; }
            public string Pronunciation { get; set; }
            public string Note { get; set; }
            public string Audio { get; set; }
        }
    }
}
