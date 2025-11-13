using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyBahasa
{
    public partial class ManagePhrases : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
        int lessonId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["lesson_id"], out lessonId))
            {
                Response.Redirect("ManageLessons.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadLessonTitle();
                LoadPhrases();
            }
        }

        private void LoadLessonTitle()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT title FROM Lessons WHERE lesson_id=@id", con);
                cmd.Parameters.AddWithValue("@id", lessonId);
                con.Open();
                object result = cmd.ExecuteScalar();
                lblLessonTitle.Text = "Lesson: " + (result != null ? result.ToString() : "Unknown");
            }
        }

        private void LoadPhrases()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM LessonPhrases WHERE lesson_id=@id", con);
                da.SelectCommand.Parameters.AddWithValue("@id", lessonId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPhrases.DataSource = dt;
                gvPhrases.DataBind();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            lblFormTitle.Text = "Add New Phrase";
            hfPhraseId.Value = "";
            txtMalay.Text = txtEnglish.Text = txtPronunciation.Text = txtNote.Text = txtAudio.Text = txtVideo.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd;

                if (hfPhraseId.Value == "")
                {
                    cmd = new SqlCommand(@"
                        INSERT INTO LessonPhrases 
                        (lesson_id, malay_text, english_text, pronunciation, cultural_note, audio_url, video_url)
                        VALUES (@lesson_id, @malay, @english, @pronounce, @note, @audio, @video)", con);
                }
                else
                {
                    cmd = new SqlCommand(@"
                        UPDATE LessonPhrases 
                        SET malay_text=@malay, english_text=@english, pronunciation=@pronounce, 
                            cultural_note=@note, audio_url=@audio, video_url=@video 
                        WHERE phrase_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", hfPhraseId.Value);
                }

                cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                cmd.Parameters.AddWithValue("@malay", txtMalay.Text.Trim());
                cmd.Parameters.AddWithValue("@english", txtEnglish.Text.Trim());
                cmd.Parameters.AddWithValue("@pronounce", txtPronunciation.Text.Trim());
                cmd.Parameters.AddWithValue("@note", txtNote.Text.Trim());
                cmd.Parameters.AddWithValue("@audio", txtAudio.Text.Trim());
                cmd.Parameters.AddWithValue("@video", txtVideo.Text.Trim());
                cmd.ExecuteNonQuery();
            }

            pnlForm.Visible = false;
            LoadPhrases();
            lblMessage.Text = "✅ Phrase saved successfully!";
        }

        protected void gvPhrases_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditPhrase")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM LessonPhrases WHERE phrase_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfPhraseId.Value = dr["phrase_id"].ToString();
                        txtMalay.Text = dr["malay_text"].ToString();
                        txtEnglish.Text = dr["english_text"].ToString();
                        txtPronunciation.Text = dr["pronunciation"].ToString();
                        txtNote.Text = dr["cultural_note"].ToString();
                        txtAudio.Text = dr["audio_url"].ToString();
                        txtVideo.Text = dr["video_url"].ToString();
                        pnlForm.Visible = true;
                        lblFormTitle.Text = "Edit Phrase";
                    }
                }
            }
            else if (e.CommandName == "DeletePhrase")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM LessonPhrases WHERE phrase_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadPhrases();
            }
        }
    }
}
