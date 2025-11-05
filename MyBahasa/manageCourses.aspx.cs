using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyBahasa
{
    public partial class ManageCourses : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Courses", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            lblFormTitle.Text = "Add New Course";
            hfCourseId.Value = "";
            txtTitle.Text = txtDescription.Text = "";
            ddlLevel.SelectedIndex = 0;
            chkActive.Checked = true;
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

                if (hfCourseId.Value == "")
                {
                    cmd = new SqlCommand("INSERT INTO Courses (title, description, level, is_active) VALUES (@title, @description, @level, @is_active)", con);
                }
                else
                {
                    cmd = new SqlCommand("UPDATE Courses SET title=@title, description=@description, level=@level, is_active=@is_active WHERE course_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", hfCourseId.Value);
                }

                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@level", ddlLevel.SelectedValue);
                cmd.Parameters.AddWithValue("@is_active", chkActive.Checked);
                cmd.ExecuteNonQuery();
            }

            pnlForm.Visible = false;
            LoadCourses();
            lblMessage.Text = "Course saved successfully!";
        }

        protected void gvCourses_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditCourse")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Courses WHERE course_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfCourseId.Value = dr["course_id"].ToString();
                        txtTitle.Text = dr["title"].ToString();
                        txtDescription.Text = dr["description"].ToString();
                        ddlLevel.SelectedValue = dr["level"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["is_active"]);
                        pnlForm.Visible = true;
                        lblFormTitle.Text = "Edit Course";
                    }
                }
            }
            else if (e.CommandName == "DeleteCourse")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Courses WHERE course_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadCourses();
            }
            else if (e.CommandName == "ManageLessons")
            {
                Response.Redirect("ManageLessons.aspx?course_id=" + id);
            }
        }
    }
}
