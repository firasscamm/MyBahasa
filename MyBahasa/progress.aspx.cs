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
                if (Session["user_id"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                lblUserName.Text = Session["name"].ToString();
                int userId = Convert.ToInt32(Session["user_id"]);

                LoadUserProgress(userId);
                LoadProgressChart(userId);
                SetActiveTab("overview");
            }
        }

        // -------------------------------
        // Overview Data - FIXED QUERY
        // -------------------------------
        private void LoadUserProgress(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                // ✅ FIXED: Better query that properly gets progress data
                string query = @"
                    SELECT 
                        C.title AS CourseTitle,
                        L.title AS LessonTitle,
                        ISNULL(P.completion_percent, 0) AS CompletionPercent,
                        ISNULL(P.score, 0) AS Score,
                        P.last_updated AS LastUpdated
                    FROM Progress P
                    INNER JOIN Lessons L ON P.lesson_id = L.lesson_id
                    INNER JOIN Courses C ON P.course_id = C.course_id
                    WHERE P.user_id = @user_id
                    ORDER BY P.last_updated DESC";

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
        // Lesson Details Data - FIXED QUERY
        // -------------------------------
        private void LoadLessonDetails(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                // ✅ FIXED: Same query as Overview but with more details
                string query = @"
                    SELECT 
                        C.title AS CourseTitle,
                        L.title AS LessonTitle,
                        ISNULL(P.completion_percent, 0) AS CompletionPercent,
                        ISNULL(P.score, 0) AS Score,
                        P.last_updated AS LastUpdated
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

                if (dt.Rows.Count > 0)
                {
                    rptLessonDetails.DataSource = dt;
                    rptLessonDetails.DataBind();
                    lblNoLessons.Visible = false;
                }
                else
                {
                    lblNoLessons.Visible = true;
                }
            }
        }

        // -------------------------------
        // Chart Data - FIXED QUERY
        // -------------------------------
        private void LoadProgressChart(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                // ✅ FIXED: Query that properly groups by course and calculates average score
                string query = @"
                    SELECT 
                        C.title AS CourseTitle,
                        AVG(ISNULL(P.score, 0)) AS AvgScore,
                        AVG(ISNULL(P.completion_percent, 0)) AS AvgCompletion
                    FROM Progress P
                    INNER JOIN Courses C ON P.course_id = C.course_id
                    WHERE P.user_id = @user_id
                    GROUP BY C.course_id, C.title
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
                        values.Add(row["AvgScore"].ToString());
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
        // Practice History - ALREADY WORKING
        // -------------------------------
        private void LoadPracticeHistory(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        A.attempt_id AS AttemptId,
                        L.title AS LessonTitle,
                        A.score AS Score,
                        A.total_questions AS TotalQuestions,
                        A.taken_at AS TakenAt
                    FROM Attempts A
                    INNER JOIN Lessons L ON A.lesson_id = L.lesson_id
                    WHERE A.user_id = @user_id
                    ORDER BY A.taken_at DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptPractice.DataSource = dt;
                rptPractice.DataBind();
                lblNoPractice.Visible = (dt.Rows.Count == 0);
            }
        }

        // -------------------------------
        // Tab Switching Methods
        // -------------------------------
        protected void ShowOverview(object sender, EventArgs e)
        {
            SetActiveTab("overview");
            LoadUserProgress(Convert.ToInt32(Session["user_id"]));
        }

        protected void ShowLessons(object sender, EventArgs e)
        {
            SetActiveTab("lessons");
            LoadLessonDetails(Convert.ToInt32(Session["user_id"]));
        }

        protected void ShowPractice(object sender, EventArgs e)
        {
            SetActiveTab("practice");
            LoadPracticeHistory(Convert.ToInt32(Session["user_id"]));
        }

        private void SetActiveTab(string tabName)
        {
            pnlOverview.Visible = pnlLessons.Visible = pnlPractice.Visible = false;
            tabOverview.CssClass = tabLessons.CssClass = tabPractice.CssClass = "tab";

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