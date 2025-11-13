<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewAttempt.aspx.cs" Inherits="MyBahasa.ReviewAttempt" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Review Quiz Attempt - MyBahasa</title>
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            background-color: #f9fafb;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 900px;
            margin: 50px auto;
            background: #fff;
            padding: 40px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }

        h2 {
            text-align: center;
            color: #007b83;
        }

        h4 {
            color: #333;
            margin-top: 20px;
        }

        .question {
            border-bottom: 1px solid #eee;
            margin-bottom: 25px;
            padding-bottom: 15px;
        }

        .choice {
            margin-left: 20px;
            padding: 5px 0;
        }

        .correct {
            color: #007b83;
            font-weight: bold;
        }

        .wrong {
            color: #dc3545;
            font-weight: bold;
        }

        .score-box {
            background-color: #f0fdfa;
            border: 2px solid #007b83;
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 20px;
            text-align: center;
            font-size: 18px;
            font-weight: 600;
            color: #007b83;
        }

        .btn {
            display: block;
            width: 100%;
            background-color: #007b83;
            color: #fff;
            border: none;
            padding: 12px 20px;
            border-radius: 8px;
            text-align: center;
            text-decoration: none;
            font-size: 16px;
            cursor: pointer;
            margin-top: 20px;
        }

        .btn:hover {
            background-color: #00666e;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Review Quiz Attempt</h2>
            <hr />

            <div class="score-box">
                <asp:Label ID="lblScoreSummary" runat="server" />
            </div>

            <asp:Repeater ID="rptReview" runat="server">
                <ItemTemplate>
                    <div class="question">
                        <h4>Q<%# Container.ItemIndex + 1 %>: <%# Eval("QuestionText") %></h4>
                        <div class="choices">
                            <asp:Repeater ID="rptChoices" runat="server" DataSource='<%# Eval("Choices") %>'>
                                <ItemTemplate>
                                    <div class="choice <%# Convert.ToBoolean(Eval("IsCorrect")) ? "correct" : (Convert.ToBoolean(Eval("IsSelected")) ? "wrong" : "") %>">
                                        <%# Eval("ChoiceText") %>
                                        <%# Convert.ToBoolean(Eval("IsCorrect")) ? " ✅" : "" %>
                                        <%# Convert.ToBoolean(Eval("IsSelected")) && !Convert.ToBoolean(Eval("IsCorrect")) ? " ❌" : "" %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <a href="Progress.aspx" class="btn">Return to Progress</a>
        </div>
    </form>
</body>
</html>
