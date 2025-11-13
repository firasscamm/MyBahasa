using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

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
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Courses ORDER BY course_id DESC", con);
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
            chkActive.Checked = true; // default new courses to active
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
                    // INSERT new course
                    cmd = new SqlCommand(@"
                        INSERT INTO Courses (title, description, level, is_active)
                        VALUES (@title, @description, @level, @is_active)", con);
                }
                else
                {
                    // UPDATE existing course
                    cmd = new SqlCommand(@"
                        UPDATE Courses 
                        SET title=@title, description=@description, level=@level, is_active=@is_active 
                        WHERE course_id=@id", con);
                    cmd.Parameters.AddWithValue("@id", hfCourseId.Value);
                }

                bool activeValue = chkActive.Checked || hfCourseId.Value == ""; // default to active if new

                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@level", ddlLevel.SelectedValue);
                cmd.Parameters.AddWithValue("@is_active", activeValue);

                cmd.ExecuteNonQuery();
            }

            pnlForm.Visible = false;
            LoadCourses();
            lblMessage.Text = "✅ Course saved successfully!";
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
                // ✅ Redirect to ManageLessons with course ID
                Response.Redirect("ManageLessons.aspx?course_id=" + id);
            }
        }

        protected void gvCourses_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            // Optional visual enhancement: highlight inactive courses
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                bool isActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "is_active"));
                if (!isActive)
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f8d7da");
                }
            }
        }
    }
}
