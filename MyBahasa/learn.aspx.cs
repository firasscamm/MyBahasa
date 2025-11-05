using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MyBahasa
{
    public partial class Learn : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
        string currentLevel = "Beginner";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLessons("Beginner");
                SetActiveTab(btnBeginner);
            }
        }

        private void LoadLessons(string level)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT Lessons.lesson_id, Lessons.title, Lessons.content_text, Courses.level 
                    FROM Lessons 
                    INNER JOIN Courses ON Lessons.course_id = Courses.course_id
                    WHERE Courses.level = @level AND Courses.is_active = 1
                    ORDER BY Lessons.sort_order ASC", con);
                da.SelectCommand.Parameters.AddWithValue("@level", level);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptLessons.DataSource = dt;
                rptLessons.DataBind();
            }
        }

        protected void StartLesson_Command(object sender, CommandEventArgs e)
        {
            string lessonId = e.CommandArgument.ToString();
            Response.Redirect($"Lesson.aspx?lesson_id={lessonId}");
        }

        protected void btnBeginner_Click(object sender, EventArgs e)
        {
            LoadLessons("Beginner");
            SetActiveTab(btnBeginner);
        }

        protected void btnIntermediate_Click(object sender, EventArgs e)
        {
            LoadLessons("Intermediate");
            SetActiveTab(btnIntermediate);
        }

        protected void btnAdvanced_Click(object sender, EventArgs e)
        {
            LoadLessons("Advanced");
            SetActiveTab(btnAdvanced);
        }

        private void SetActiveTab(Button activeButton)
        {
            btnBeginner.CssClass = "tab";
            btnIntermediate.CssClass = "tab";
            btnAdvanced.CssClass = "tab";
            activeButton.CssClass = "tab active";
        }
    }
}
