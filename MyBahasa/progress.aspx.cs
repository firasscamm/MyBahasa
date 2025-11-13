using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace MyBahasa
{
    public partial class Progress : System.Web.UI.Page
    {
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

                LoadSummaryDashboard(userId);
                LoadUserProgress(userId);
                LoadProgressChart(userId);
                SetActiveTab("overview");
            }
        }

        // -------------------------------
        // Summary Dashboard Data
        // -------------------------------
        private void LoadSummaryDashboard(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // Total Lessons Completed
                string lessonsQuery = @"
                    SELECT COUNT(*) FROM Progress 
                    WHERE user_id = @user_id AND completion_percent >= 100";
                SqlCommand lessonsCmd = new SqlCommand(lessonsQuery, con);
                lessonsCmd.Parameters.AddWithValue("@user_id", userId);
                object lessonsResult = lessonsCmd.ExecuteScalar();
                lblTotalLessons.Text = lessonsResult != null ? lessonsResult.ToString() : "0";

                // Average Score
                string avgQuery = @"
                    SELECT ISNULL(AVG(score), 0) FROM Progress 
                    WHERE user_id = @user_id AND score > 0";
                SqlCommand avgCmd = new SqlCommand(avgQuery, con);
                avgCmd.Parameters.AddWithValue("@user_id", userId);
                object avgResult = avgCmd.ExecuteScalar();
                decimal avgScore = avgResult != null ? Convert.ToDecimal(avgResult) : 0;
                lblAvgScore.Text = Math.Round(avgScore, 1).ToString();

                // Overall Progress (Average completion)
                string progressQuery = @"
                    SELECT ISNULL(AVG(completion_percent), 0) FROM Progress 
                    WHERE user_id = @user_id";
                SqlCommand progressCmd = new SqlCommand(progressQuery, con);
                progressCmd.Parameters.AddWithValue("@user_id", userId);
                object progressResult = progressCmd.ExecuteScalar();
                decimal overallProgress = progressResult != null ? Convert.ToDecimal(progressResult) : 0;
                litOverallProgressText.Text = Math.Round(overallProgress, 0).ToString();

                // Quizzes Taken
                string quizzesQuery = @"
                    SELECT COUNT(*) FROM Attempts 
                    WHERE user_id = @user_id";
                SqlCommand quizzesCmd = new SqlCommand(quizzesQuery, con);
                quizzesCmd.Parameters.AddWithValue("@user_id", userId);
                object quizzesResult = quizzesCmd.ExecuteScalar();
                lblQuizzesTaken.Text = quizzesResult != null ? quizzesResult.ToString() : "0";

                // Level-wise Progress
                LoadLevelProgress(con, userId);

                // Motivation Text
                SetMotivationText(overallProgress, avgScore);
            }
        }

        private void LoadLevelProgress(SqlConnection con, int userId)
        {
            // Beginner Progress
            string beginnerQuery = @"
                SELECT ISNULL(AVG(P.completion_percent), 0) 
                FROM Progress P
                INNER JOIN Courses C ON P.course_id = C.course_id
                WHERE P.user_id = @user_id AND C.level = 'Beginner'";
            SqlCommand beginnerCmd = new SqlCommand(beginnerQuery, con);
            beginnerCmd.Parameters.AddWithValue("@user_id", userId);
            object beginnerResult = beginnerCmd.ExecuteScalar();
            decimal beginnerProgress = beginnerResult != null ? Convert.ToDecimal(beginnerResult) : 0;
            litBeginnerProgress.Text = Math.Round(beginnerProgress, 0) + "%";

            // Intermediate Progress
            string intermediateQuery = @"
                SELECT ISNULL(AVG(P.completion_percent), 0) 
                FROM Progress P
                INNER JOIN Courses C ON P.course_id = C.course_id
                WHERE P.user_id = @user_id AND C.level = 'Intermediate'";
            SqlCommand intermediateCmd = new SqlCommand(intermediateQuery, con);
            intermediateCmd.Parameters.AddWithValue("@user_id", userId);
            object intermediateResult = intermediateCmd.ExecuteScalar();
            decimal intermediateProgress = intermediateResult != null ? Convert.ToDecimal(intermediateResult) : 0;
            litIntermediateProgress.Text = Math.Round(intermediateProgress, 0) + "%";

            // Advanced Progress
            string advancedQuery = @"
                SELECT ISNULL(AVG(P.completion_percent), 0) 
                FROM Progress P
                INNER JOIN Courses C ON P.course_id = C.course_id
                WHERE P.user_id = @user_id AND C.level = 'Advanced'";
            SqlCommand advancedCmd = new SqlCommand(advancedQuery, con);
            advancedCmd.Parameters.AddWithValue("@user_id", userId);
            object advancedResult = advancedCmd.ExecuteScalar();
            decimal advancedProgress = advancedResult != null ? Convert.ToDecimal(advancedResult) : 0;
            litAdvancedProgress.Text = Math.Round(advancedProgress, 0) + "%";
        }

        private void SetMotivationText(decimal overallProgress, decimal avgScore)
        {
            if (overallProgress == 0)
            {
                litMotivation.Text = "Start your Bahasa Melayu journey today! Your first lesson awaits.";
            }
            else if (overallProgress < 30)
            {
                litMotivation.Text = "Great start! You're building a solid foundation in Bahasa Melayu.";
            }
            else if (overallProgress < 70)
            {
                litMotivation.Text = "You're making excellent progress! Keep up the consistent learning.";
            }
            else if (overallProgress < 90)
            {
                litMotivation.Text = "Impressive progress! You're becoming quite proficient in Bahasa Melayu.";
            }
            else
            {
                litMotivation.Text = "Outstanding! You're mastering Bahasa Melayu. Consider helping other learners!";
            }

            if (avgScore >= 80)
            {
                litMotivation.Text += " Your high scores show excellent understanding!";
            }
        }

        // -------------------------------
        // Overview Data
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
                        ISNULL(P.score, 0) AS Score,
                        P.last_updated AS LastUpdated,
                        C.level AS Level
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
        // Lesson Details Data
        // -------------------------------
        private void LoadLessonDetails(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        C.title AS CourseTitle,
                        L.title AS LessonTitle,
                        ISNULL(P.completion_percent, 0) AS CompletionPercent,
                        ISNULL(P.score, 0) AS Score,
                        P.last_updated AS LastUpdated,
                        C.level AS Level
                    FROM Progress P
                    INNER JOIN Lessons L ON P.lesson_id = L.lesson_id
                    INNER JOIN Courses C ON P.course_id = C.course_id
                    WHERE P.user_id = @user_id
                    ORDER BY C.level, C.title, L.title";

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
        // Chart Data
        // -------------------------------
        private void LoadProgressChart(int userId)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
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

                    litChartLabels.Text = "[" + string.Join(",", labels) + "]";
                    litChartData.Text = "[" + string.Join(",", values) + "]";
                }
                else
                {
                    litChartLabels.Text = "['No Data']";
                    litChartData.Text = "[0]";
                }
            }
        }

        // -------------------------------
        // Practice History
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
                        A.taken_at AS TakenAt,
                        C.level AS Level
                    FROM Attempts A
                    INNER JOIN Lessons L ON A.lesson_id = L.lesson_id
                    INNER JOIN Courses C ON L.course_id = C.course_id
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