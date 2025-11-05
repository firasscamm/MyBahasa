using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Collections.Generic;

namespace MyBahasa
{
    public partial class Progress : System.Web.UI.Page
    {
        protected string chartLabels = "[]";
        protected string chartData = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure user is logged in
                if (Session["user_id"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                lblUserName.Text = Session["name"].ToString();
                int userId = Convert.ToInt32(Session["user_id"]);

                // Load initial data for Overview tab
                LoadUserProgress(userId);
                LoadProgressChart(userId);
                SetActiveTab("overview");
            }
        }

        // -------------------------------
        // Load Data for Overview Tab
        // -------------------------------
        private void LoadUserProgress(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        C.title AS CourseTitle,
                        L.title AS LessonTitle,
                        ISNULL(P.completion_percent, 0) AS CompletionPercent,
                        ISNULL(P.score, 0) AS Score
                    FROM Progress P
                    INNER JOIN Lessons L ON P.lesson_id = L.lesson_id
                    INNER JOIN Courses C ON P.course_id = C.course_id
                    WHERE P.user_id = @user_id
                    ORDER BY C.title, L.title";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptProgress.DataSource = dt;
                rptProgress.DataBind();
                lblNoData.Visible = (dt.Rows.Count == 0);
            }
        }

        // -------------------------------
        // Load Chart for Overview Tab
        // -------------------------------
        private void LoadProgressChart(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        C.title AS CourseTitle,
                        AVG(ISNULL(P.completion_percent, 0)) AS AvgCompletion
                    FROM Progress P
                    INNER JOIN Courses C ON P.course_id = C.course_id
                    WHERE P.user_id = @user_id
                    GROUP BY C.title
                    ORDER BY C.title";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    List<string> labels = new List<string>();
                    List<string> values = new List<string>();

                    foreach (DataRow row in dt.Rows)
                    {
                        labels.Add("'" + row["CourseTitle"].ToString().Replace("'", "’") + "'");
                        values.Add(row["AvgCompletion"].ToString());
                    }

                    chartLabels = "[" + string.Join(",", labels) + "]";
                    chartData = "[" + string.Join(",", values) + "]";
                }
                else
                {
                    chartLabels = "['No Data']";
                    chartData = "[0]";
                }
            }
        }

        // -------------------------------
        // Tab Switching Logic
        // -------------------------------
        protected void ShowOverview(object sender, EventArgs e)
        {
            SetActiveTab("overview");
        }

        protected void ShowLessons(object sender, EventArgs e)
        {
            SetActiveTab("lessons");
            // Placeholder message (future phase)
            pnlLessons.Controls.Clear();
            pnlLessons.Controls.Add(new System.Web.UI.LiteralControl("<div class='placeholder'>Lesson Details will appear here.</div>"));
        }

        protected void ShowPractice(object sender, EventArgs e)
        {
            SetActiveTab("practice");
            // Placeholder message (future phase)
            pnlPractice.Controls.Clear();
            pnlPractice.Controls.Add(new System.Web.UI.LiteralControl("<div class='placeholder'>Practice History will appear here.</div>"));
        }

        // -------------------------------
        // Helper: Manage Active Tabs
        // -------------------------------
        private void SetActiveTab(string tabName)
        {
            pnlOverview.Visible = pnlLessons.Visible = pnlPractice.Visible = false;

            tabOverview.CssClass = "tab";
            tabLessons.CssClass = "tab";
            tabPractice.CssClass = "tab";

            switch (tabName)
            {
                case "overview":
                    pnlOverview.Visible = true;
                    tabOverview.CssClass = "tab active";
                    break;
                case "lessons":
                    pnlLessons.Visible = true;
                    tabLessons.CssClass = "tab active";
                    break;
                case "practice":
                    pnlPractice.Visible = true;
                    tabPractice.CssClass = "tab active";
                    break;
            }
        }
    }
}
