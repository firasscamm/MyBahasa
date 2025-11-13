using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyBahasa
{
    public partial class ManageQuestions : System.Web.UI.Page
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
                LoadQuestions();
            }
        }

        private void LoadLessonTitle()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT title FROM Lessons WHERE lesson_id=@id", con);
                cmd.Parameters.AddWithValue("@id", lessonId);
                con.Open();
                lblLessonTitle.Text = "Lesson: " + (cmd.ExecuteScalar() ?? "Unknown");
            }
        }

        private void LoadQuestions()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Questions WHERE lesson_id=@id", con);
                da.SelectCommand.Parameters.AddWithValue("@id", lessonId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvQuestions.DataSource = dt;
                gvQuestions.DataBind();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            lblFormTitle.Text = "Add New Question";
            hfQuestionId.Value = "";
            txtStem.Text = txtExplanation.Text = "";
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
                if (hfQuestionId.Value == "")
                {
                    cmd = new SqlCommand(@"INSERT INTO Questions (lesson_id, stem_text, explanation) 
                                           VALUES (@lesson_id, @stem_text, @explanation)", con);
                }
                else
                {
                    cmd = new SqlCommand(@"UPDATE Questions 
                                           SET stem_text=@stem_text, explanation=@explanation 
                                           WHERE question_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", hfQuestionId.Value);
                }

                cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                cmd.Parameters.AddWithValue("@stem_text", txtStem.Text.Trim());
                cmd.Parameters.AddWithValue("@explanation", (object)txtExplanation.Text.Trim() ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }

            pnlForm.Visible = false;
            LoadQuestions();
            lblMessage.Text = "✅ Question saved successfully!";
        }

        protected void gvQuestions_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditQuestion")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Questions WHERE question_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfQuestionId.Value = dr["question_id"].ToString();
                        txtStem.Text = dr["stem_text"].ToString();
                        txtExplanation.Text = dr["explanation"].ToString();
                        pnlForm.Visible = true;
                        lblFormTitle.Text = "Edit Question";
                    }
                }
            }
            else if (e.CommandName == "DeleteQuestion")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Questions WHERE question_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadQuestions();
            }
            else if (e.CommandName == "ManageChoices")
            {
                Response.Redirect("ManageChoices.aspx?question_id=" + id);
            }
        }
    }
}
