<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageQuestions.aspx.cs" Inherits="MyBahasa.ManageQuestions" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Questions - MyBahasa</title>
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            background-color: #f5f6fa;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 900px;
            margin: 50px auto;
            background: #fff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 3px 10px rgba(0,0,0,0.1);
        }

        .btn-primary { background-color: #007b7f; border: none; padding: 8px 16px; color: #fff; border-radius: 6px; cursor: pointer; }
        .btn-danger { background-color: #dc3545; border: none; padding: 8px 16px; color: #fff; border-radius: 6px; cursor: pointer; }
        .btn-secondary { background-color: #6c757d; border: none; padding: 8px 16px; color: #fff; border-radius: 6px; cursor: pointer; }
        .mb-3 { margin-bottom: 15px; }
        a { text-decoration: none; color: #007b7f; }
        a:hover { text-decoration: underline; }
        table { width: 100%; border-collapse: collapse; margin-top: 15px; }
        th, td { padding: 10px; border: 1px solid #ddd; text-align: left; }
        th { background-color: #007b7f; color: white; }
        tr:nth-child(even) { background-color: #f9f9f9; }
        tr:hover { background-color: #f1f1f1; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Manage Questions</h2>
            <p><a href="ManageLessons.aspx?course_id=<%= Request.QueryString["course_id"] %>">← Back to Lessons</a></p>
            <hr />

            <asp:Label ID="lblLessonTitle" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
            <hr />

            <div class="mb-3">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New Question" CssClass="btn-primary" OnClick="btnAddNew_Click" />
            </div>

            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <h4><asp:Label ID="lblFormTitle" runat="server" Text="Add Question"></asp:Label></h4>

                <div class="mb-3">
                    <label>Question Text:</label><br />
                    <asp:TextBox ID="txtStem" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" Width="100%" />
                </div>

                <div class="mb-3">
                    <label>Explanation (optional):</label><br />
                    <asp:TextBox ID="txtExplanation" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" Width="100%" />
                </div>

                <asp:HiddenField ID="hfQuestionId" runat="server" />

                <div class="mb-3">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-secondary" OnClick="btnCancel_Click" />
                </div>
                <hr />
            </asp:Panel>

            <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="False" CssClass="table"
                DataKeyNames="question_id" OnRowCommand="gvQuestions_RowCommand">
                <Columns>
                    <asp:BoundField DataField="question_id" HeaderText="ID" />
                    <asp:BoundField DataField="stem_text" HeaderText="Question" />
                    <asp:BoundField DataField="explanation" HeaderText="Explanation" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button runat="server" CommandName="EditQuestion" CommandArgument='<%# Eval("question_id") %>' Text="Edit" CssClass="btn-secondary" />
                            <asp:Button runat="server" CommandName="DeleteQuestion" CommandArgument='<%# Eval("question_id") %>' Text="Delete" CssClass="btn-danger" OnClientClick="return confirm('Delete this question?');" />
                            <asp:Button runat="server" CommandName="ManageChoices" CommandArgument='<%# Eval("question_id") %>' Text="Manage Choices" CssClass="btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
