using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyBahasa
{
    public partial class ManageChoices : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
        int questionId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["question_id"], out questionId))
            {
                Response.Redirect("ManageQuestions.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadQuestionText();
                LoadChoices();
            }
        }

        private void LoadQuestionText()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT stem_text FROM Questions WHERE question_id=@id", con);
                cmd.Parameters.AddWithValue("@id", questionId);
                con.Open();
                lblQuestionText.Text = "Question: " + (cmd.ExecuteScalar() ?? "Unknown");
            }
        }

        private void LoadChoices()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Choices WHERE question_id=@id", con);
                da.SelectCommand.Parameters.AddWithValue("@id", questionId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvChoices.DataSource = dt;
                gvChoices.DataBind();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            lblFormTitle.Text = "Add New Choice";
            hfChoiceId.Value = "";
            txtChoice.Text = "";
            chkIsCorrect.Checked = false;
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
                if (hfChoiceId.Value == "")
                {
                    cmd = new SqlCommand(@"INSERT INTO Choices (question_id, choice_text, is_correct)
                                           VALUES (@question_id, @text, @correct)", con);
                }
                else
                {
                    cmd = new SqlCommand(@"UPDATE Choices
                                           SET choice_text=@text, is_correct=@correct
                                           WHERE choice_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", hfChoiceId.Value);
                }

                cmd.Parameters.AddWithValue("@question_id", questionId);
                cmd.Parameters.AddWithValue("@text", txtChoice.Text.Trim());
                cmd.Parameters.AddWithValue("@correct", chkIsCorrect.Checked);
                cmd.ExecuteNonQuery();
            }

            pnlForm.Visible = false;
            LoadChoices();
            lblMessage.Text = "✅ Choice saved successfully!";
        }

        protected void gvChoices_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditChoice")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Choices WHERE choice_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfChoiceId.Value = dr["choice_id"].ToString();
                        txtChoice.Text = dr["choice_text"].ToString();
                        chkIsCorrect.Checked = Convert.ToBoolean(dr["is_correct"]);
                        pnlForm.Visible = true;
                        lblFormTitle.Text = "Edit Choice";
                    }
                }
            }
            else if (e.CommandName == "DeleteChoice")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Choices WHERE choice_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadChoices();
            }
        }
    }
}
