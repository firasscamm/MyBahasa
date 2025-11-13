<%@ Page Title="My Progress" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Progress.aspx.cs" Inherits="MyBahasa.Progress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .progress-container {
            max-width: 1100px;
            margin: 40px auto;
            background-color: #fff;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            padding: 40px;
        }

        h2 {
            text-align: center;
            color: #007b83;
            margin-bottom: 10px;
        }

        h3 {
            text-align: center;
            color: #444;
            margin-bottom: 25px;
        }

        /* Summary Dashboard */
        .summary-dashboard {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 40px;
            padding: 25px;
            background: linear-gradient(135deg, #f8fdff 0%, #f0f9fa 100%);
            border-radius: 12px;
            border: 1px solid #e0f7fa;
        }

        .stat-card {
            background: white;
            padding: 20px;
            border-radius: 10px;
            text-align: center;
            box-shadow: 0 2px 8px rgba(0,123,131,0.1);
            border-left: 4px solid #007b83;
            transition: transform 0.2s ease;
        }

        .stat-card:hover {
            transform: translateY(-2px);
        }

        .stat-number {
            font-size: 32px;
            font-weight: 700;
            color: #007b83;
            margin-bottom: 5px;
        }

        .stat-label {
            font-size: 14px;
            color: #666;
            font-weight: 500;
        }

        .progress-ring {
            width: 80px;
            height: 80px;
            border-radius: 50%;
            background: conic-gradient(#007b83 0% var(--progress), #e0f7fa 0% 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 10px;
            position: relative;
        }

        .progress-ring-inner {
            width: 60px;
            height: 60px;
            background: white;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: 600;
            color: #007b83;
        }

        .level-progress {
            margin-top: 15px;
        }

        .level-bar {
            height: 8px;
            background: #e0e0e0;
            border-radius: 4px;
            margin: 5px 0;
            overflow: hidden;
        }

        .level-fill {
            height: 100%;
            background: #007b83;
            border-radius: 4px;
            transition: width 0.3s ease;
        }

        .level-label {
            font-size: 12px;
            color: #666;
            display: flex;
            justify-content: space-between;
        }

        .motivation-card {
            grid-column: 1 / -1;
            background: linear-gradient(135deg, #007b83, #009688);
            color: white;
            padding: 20px;
            border-radius: 10px;
            text-align: center;
        }

        .motivation-text {
            font-size: 16px;
            margin-bottom: 10px;
            font-weight: 500;
        }

        .next-lesson {
            background: rgba(255,255,255,0.2);
            padding: 8px 15px;
            border-radius: 20px;
            font-size: 14px;
            display: inline-block;
        }

        .tabs {
            display: flex;
            justify-content: center;
            border-bottom: 2px solid #eee;
            margin-bottom: 25px;
        }

        .tab {
            padding: 10px 25px;
            cursor: pointer;
            color: #555;
            font-weight: 500;
            transition: all 0.3s;
            text-decoration: none;
            border: none;
            background: none;
        }

        .tab:hover {
            color: #007b83;
        }

        .tab.active {
            color: #007b83;
            border-bottom: 3px solid #007b83;
        }

        .overview-card {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background-color: #f9fafb;
            border-radius: 10px;
            padding: 20px 30px;
            margin-bottom: 20px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.05);
            transition: transform 0.2s ease;
        }

        .overview-card:hover {
            transform: translateX(5px);
        }

        .overview-card h4 {
            color: #007b83;
            margin-bottom: 8px;
        }

        .overview-card p {
            margin: 0;
            color: #555;
        }

        .progress-bar-container {
            background-color: #e0e0e0;
            border-radius: 8px;
            height: 10px;
            width: 250px;
            margin-top: 6px;
        }

        .progress-bar {
            height: 10px;
            border-radius: 8px;
            background-color: #007b83;
            transition: width 0.3s ease;
        }

        .stats {
            text-align: right;
            font-weight: 500;
            color: #333;
        }

        .placeholder {
            text-align: center;
            color: #999;
            font-size: 15px;
            padding: 40px 0;
        }

        .chart-container {
            width: 100%;
            max-width: 850px;
            margin: 20px auto 30px;
        }

        #progressChart {
            width: 100% !important;
            height: 350px !important;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 15px;
        }

        th {
            background-color: #007b83;
            color: white;
            padding: 10px;
            text-align: left;
        }

        td {
            padding: 8px;
            border-bottom: 1px solid #eee;
        }

        tr:hover {
            background-color: #f9f9f9;
        }

        .badge {
            display: inline-block;
            padding: 3px 8px;
            background: #e0f7fa;
            color: #007b83;
            border-radius: 12px;
            font-size: 11px;
            font-weight: 600;
            margin-left: 5px;
        }
    </style>

    <div class="progress-container">
        <h2>Welcome back, <asp:Label ID="lblUserName" runat="server" /></h2>
        <h3>Your Learning Dashboard</h3>

        <!-- Summary Dashboard -->
        <div class="summary-dashboard">
            <div class="stat-card">
                <div class="stat-number"><asp:Label ID="lblTotalLessons" runat="server" Text="0" /></div>
                <div class="stat-label">Lessons Completed</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-number"><asp:Label ID="lblAvgScore" runat="server" Text="0" />%</div>
                <div class="stat-label">Average Score</div>
            </div>
            
            <div class="stat-card">
                <div class="progress-ring" style="--progress: 0%">
                    <div class="progress-ring-inner">
                        <asp:Literal ID="litOverallProgressText" runat="server" Text="0" />%
                    </div>
                </div>
                <div class="stat-label">Overall Progress</div>
            </div>
            
            <div class="stat-card">
                <div class="stat-number"><asp:Label ID="lblQuizzesTaken" runat="server" Text="0" /></div>
                <div class="stat-label">Quizzes Taken</div>
            </div>

            <div class="motivation-card">
                <div class="motivation-text">
                    <asp:Literal ID="litMotivation" runat="server" Text="Keep going! You're making great progress learning Bahasa Melayu." />
                </div>
                <div class="next-lesson">
                    <asp:HyperLink ID="lnkNextLesson" runat="server" Text="Continue Learning" NavigateUrl="~/learn.aspx" />
                </div>
            </div>
        </div>

        <!-- Level Progress -->
        <div class="level-progress">
            <div class="level-label">
                <span>Beginner</span>
                <span><asp:Literal ID="litBeginnerProgress" runat="server" Text="0%" /></span>
            </div>
            <div class="level-bar">
                <div class="level-fill" style="width: 0%"></div>
            </div>
            
            <div class="level-label">
                <span>Intermediate</span>
                <span><asp:Literal ID="litIntermediateProgress" runat="server" Text="0%" /></span>
            </div>
            <div class="level-bar">
                <div class="level-fill" style="width: 0%"></div>
            </div>
            
            <div class="level-label">
                <span>Advanced</span>
                <span><asp:Literal ID="litAdvancedProgress" runat="server" Text="0%" /></span>
            </div>
            <div class="level-bar">
                <div class="level-fill" style="width: 0%"></div>
            </div>
        </div>

        <div class="chart-container">
            <canvas id="progressChart"></canvas>
        </div>

        <!-- Tabs -->
        <div class="tabs">
            <asp:LinkButton ID="tabOverview" runat="server" CssClass="tab active" OnClick="ShowOverview">Overview</asp:LinkButton>
            <asp:LinkButton ID="tabLessons" runat="server" CssClass="tab" OnClick="ShowLessons">Lesson Details</asp:LinkButton>
            <asp:LinkButton ID="tabPractice" runat="server" CssClass="tab" OnClick="ShowPractice">Practice History</asp:LinkButton>
        </div>

        <!-- Overview -->
        <asp:Panel ID="pnlOverview" runat="server">
            <asp:Repeater ID="rptProgress" runat="server">
                <ItemTemplate>
                    <div class="overview-card">
                        <div>
                            <h4><%# Eval("CourseTitle") %> <span class="badge"><%# Eval("Level") %></span></h4>
                            <p>Lesson: <%# Eval("LessonTitle") %></p>
                            <div class="progress-bar-container">
                                <div class="progress-bar" style='width:<%# Eval("CompletionPercent") %>%'></div>
                            </div>
                            <p style="font-size:13px; color:#777;">Completion: <%# Eval("CompletionPercent") %>%</p>
                        </div>
                        <div class="stats">
                            Score: <%# Eval("Score") %>%
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Label ID="lblNoData" runat="server" ForeColor="Gray" Font-Size="14px" Visible="false"
                Text="You haven't started any lessons yet. Go to the Learn page to begin!"></asp:Label>
        </asp:Panel>

        <!-- Lesson Details -->
        <asp:Panel ID="pnlLessons" runat="server" Visible="false">
            <table>
                <thead>
                    <tr>
                        <th>Course</th>
                        <th>Level</th>
                        <th>Lesson</th>
                        <th>Completion %</th>
                        <th>Score</th>
                        <th>Last Updated</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptLessonDetails" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("CourseTitle") %></td>
                                <td><span class="badge"><%# Eval("Level") %></span></td>
                                <td><%# Eval("LessonTitle") %></td>
                                <td><%# Eval("CompletionPercent") %>%</td>
                                <td><%# Eval("Score") %>%</td>
                                <td><%# Eval("LastUpdated", "{0:dd MMM yyyy}") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <asp:Label ID="lblNoLessons" runat="server" Visible="false"
                ForeColor="Gray" Font-Size="14px"
                Text="You haven't started any lessons yet. Go to the Learn page to begin!"></asp:Label>
        </asp:Panel>

        <!-- Practice History -->
        <asp:Panel ID="pnlPractice" runat="server" Visible="false">
            <table>
                <thead>
                    <tr>
                        <th>Lesson</th>
                        <th>Level</th>
                        <th>Score</th>
                        <th>Total Questions</th>
                        <th>Date Taken</th>
                        <th>Review</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptPractice" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("LessonTitle") %></td>
                                <td><span class="badge"><%# Eval("Level") %></span></td>
                                <td><%# Eval("Score") %>%</td>
                                <td><%# Eval("TotalQuestions") %></td>
                                <td><%# Eval("TakenAt", "{0:dd MMM yyyy, hh:mm tt}") %></td>
                                <td>
                                    <a href='ReviewAttempt.aspx?attempt_id=<%# Eval("AttemptId") %>' style="color:#007b83; font-weight:500;">Review</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <asp:Label ID="lblNoPractice" runat="server" Visible="false"
                ForeColor="Gray" Font-Size="14px"
                Text="You haven't completed any quizzes yet. Go to a lesson and click 'Take Quiz' to begin!"></asp:Label>
        </asp:Panel>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        // Initialize the progress chart
        window.onload = function () {
            var ctx = document.getElementById('progressChart').getContext('2d');
            var progressChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: <asp:Literal ID="litChartLabels" runat="server" Text="['No Data']" />,
                    datasets: [{
                        label: 'Average Score (%)',
                        data: <asp:Literal ID="litChartData" runat="server" Text="[0]" />,
                        backgroundColor: '#007b83',
                        borderColor: '#00666e',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            max: 100,
                            ticks: {
                                callback: function (value) {
                                    return value + '%';
                                }
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            display: true,
                            position: 'top'
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    return context.dataset.label + ': ' + context.raw + '%';
                                }
                            }
                        }
                    }
                }
            });

            // Update progress rings with actual values
            var overallProgress = document.getElementById('<%= litOverallProgressText.ClientID %>').textContent;
            var progressRing = document.querySelector('.progress-ring');
            if (progressRing) {
                progressRing.style.setProperty('--progress', overallProgress + '%');
            }

            // Update level progress bars
            var beginnerWidth = '<%= litBeginnerProgress.Text.Replace("%", "") %>';
            var intermediateWidth = '<%= litIntermediateProgress.Text.Replace("%", "") %>';
            var advancedWidth = '<%= litAdvancedProgress.Text.Replace("%", "") %>';
            
            var levelFills = document.querySelectorAll('.level-fill');
            if (levelFills.length >= 3) {
                levelFills[0].style.width = beginnerWidth + '%';
                levelFills[1].style.width = intermediateWidth + '%';
                levelFills[2].style.width = advancedWidth + '%';
            }
        };
    </script>
</asp:Content>