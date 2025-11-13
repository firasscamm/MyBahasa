using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace MyBahasa
{
    public partial class Learn : System.Web.UI.Page
    {
        string cs = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ❌ No redirect if not logged in — guests can view the page
                LoadCoursesAndLessons();
            }
        }

        private void LoadCoursesAndLessons(string levelFilter = "")
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                string query = @"
                    SELECT L.lesson_id, L.title, L.content_text, C.level
                    FROM Lessons L
                    INNER JOIN Courses C ON L.course_id = C.course_id
                    WHERE C.is_active = 1";

                if (!string.IsNullOrEmpty(levelFilter))
                {
                    query += " AND C.level = @level";
                }

                query += @"
                    ORDER BY 
                        CASE 
                            WHEN C.level = 'Beginner' THEN 1
                            WHEN C.level = 'Intermediate' THEN 2
                            WHEN C.level = 'Advanced' THEN 3
                            ELSE 4 
                        END, L.title";

                SqlCommand cmd = new SqlCommand(query, con);
                if (!string.IsNullOrEmpty(levelFilter))
                    cmd.Parameters.AddWithValue("@level", levelFilter);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptLessons.DataSource = dt;
                rptLessons.DataBind();

                lblMessage.Text = dt.Rows.Count == 0 ? "No lessons available for this level yet." : "";
            }
        }

        // ✅ Filter Tabs
        protected void btnAll_Click(object sender, EventArgs e)
        {
            ResetTabs();
            btnAll.CssClass = "tab active";
            LoadCoursesAndLessons();
        }

        protected void btnBeginner_Click(object sender, EventArgs e)
        {
            ResetTabs();
            btnBeginner.CssClass = "tab active";
            LoadCoursesAndLessons("Beginner");
        }

        protected void btnIntermediate_Click(object sender, EventArgs e)
        {
            ResetTabs();
            btnIntermediate.CssClass = "tab active";
            LoadCoursesAndLessons("Intermediate");
        }

        protected void btnAdvanced_Click(object sender, EventArgs e)
        {
            ResetTabs();
            btnAdvanced.CssClass = "tab active";
            LoadCoursesAndLessons("Advanced");
        }

        private void ResetTabs()
        {
            btnAll.CssClass = btnBeginner.CssClass = btnIntermediate.CssClass = btnAdvanced.CssClass = "tab";
        }

        // ✅ Only logged-in users can start lessons
        protected void StartLesson_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int lessonId = Convert.ToInt32(e.CommandArgument);

            if (Session["user_id"] == null)
            {
                // Redirect guest to login, preserving the intended lesson
                Response.Redirect($"login.aspx?returnUrl=Lesson.aspx?lesson_id={lessonId}");
            }
            else
            {
                Response.Redirect($"Lesson.aspx?lesson_id={lessonId}");
            }
        }
    }
}
