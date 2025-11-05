using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyBahasa
{
    public partial class ManageLessons : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
        int courseId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["course_id"], out courseId))
            {
                Response.Redirect("ManageCourses.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourseTitle();
                LoadLessons();
            }
        }

        private void LoadCourseTitle()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT title FROM Courses WHERE course_id=@id", con);
                cmd.Parameters.AddWithValue("@id", courseId);
                con.Open();
                object result = cmd.ExecuteScalar();
                lblCourseTitle.Text = "Course: " + (result != null ? result.ToString() : "Unknown");
            }
        }

        private void LoadLessons()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Lessons WHERE course_id=@id", con);
                da.SelectCommand.Parameters.AddWithValue("@id", courseId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvLessons.DataSource = dt;
                gvLessons.DataBind();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            lblFormTitle.Text = "Add New Lesson";
            hfLessonId.Value = "";
            txtTitle.Text = txtContent.Text = txtAudio.Text = txtVideo.Text = "";
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

                if (hfLessonId.Value == "")
                {
                    cmd = new SqlCommand(@"INSERT INTO Lessons (course_id, title, content_text, audio_url, video_url) 
                                           VALUES (@course_id, @title, @content_text, @audio_url, @video_url)", con);
                }
                else
                {
                    cmd = new SqlCommand(@"UPDATE Lessons 
                                           SET title=@title, content_text=@content_text, audio_url=@audio_url, video_url=@video_url 
                                           WHERE lesson_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", hfLessonId.Value);
                }

                cmd.Parameters.AddWithValue("@course_id", courseId);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@content_text", txtContent.Text.Trim());
                cmd.Parameters.AddWithValue("@audio_url", txtAudio.Text.Trim());
                cmd.Parameters.AddWithValue("@video_url", txtVideo.Text.Trim());
                cmd.ExecuteNonQuery();
            }

            pnlForm.Visible = false;
            LoadLessons();
            lblMessage.Text = "✅ Lesson saved successfully!";
        }

        protected void gvLessons_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditLesson")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Lessons WHERE lesson_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfLessonId.Value = dr["lesson_id"].ToString();
                        txtTitle.Text = dr["title"].ToString();
                        txtContent.Text = dr["content_text"].ToString();
                        txtAudio.Text = dr["audio_url"].ToString();
                        txtVideo.Text = dr["video_url"].ToString();
                        pnlForm.Visible = true;
                        lblFormTitle.Text = "Edit Lesson";
                    }
                }
            }
            else if (e.CommandName == "DeleteLesson")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Lessons WHERE lesson_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadLessons();
            }
            else if (e.CommandName == "ManagePhrases")
            {
                Response.Redirect("ManagePhrases.aspx?lesson_id=" + id);
            }
        }
    }
}
