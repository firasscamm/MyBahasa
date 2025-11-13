<%@ Page Title="My Progress" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Progress.aspx.cs" Inherits="MyBahasa.Progress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .progress-container {
            max-width: 950px;
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
    </style>

    <div class="progress-container">
        <h2>Welcome back, <asp:Label ID="lblUserName" runat="server" /></h2>
        <h3>Your Learning Dashboard</h3>

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
                            <h4><%# Eval("CourseTitle") %></h4>
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
                                <td><%# Eval("Score") %></td>
                                <td><%# Eval("TotalQuestions") %></td>
                                <td><%# Eval("TakenAt", "{0:dd MMM yyyy, hh:mm tt}") %></td>
                                <td>
                                    <a href='ReviewAttempt.aspx?attempt_id=<%# Eval("AttemptId") %>' style="color:#007b83;">View</a>
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
</asp:Content>
