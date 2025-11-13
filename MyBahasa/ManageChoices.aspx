<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageChoices.aspx.cs" Inherits="MyBahasa.ManageChoices" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Choices - MyBahasa</title>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background-color: #f5f6fa; margin: 0; padding: 0; }
        .container { max-width: 900px; margin: 50px auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 3px 10px rgba(0,0,0,0.1); }
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
            <h2>Manage Choices</h2>
            <p><a href="ManageQuestions.aspx?lesson_id=<%= Request.QueryString["lesson_id"] %>">← Back to Questions</a></p>
            <hr />

            <asp:Label ID="lblQuestionText" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
            <hr />

            <div class="mb-3">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New Choice" CssClass="btn-primary" OnClick="btnAddNew_Click" />
            </div>

            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <h4><asp:Label ID="lblFormTitle" runat="server" Text="Add Choice"></asp:Label></h4>

                <div class="mb-3">
                    <label>Choice Text:</label><br />
                    <asp:TextBox ID="txtChoice" runat="server" CssClass="form-control" Width="100%" />
                </div>

                <div class="mb-3">
                    <label><asp:CheckBox ID="chkIsCorrect" runat="server" /> Is Correct</label>
                </div>

                <asp:HiddenField ID="hfChoiceId" runat="server" />

                <div class="mb-3">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-secondary" OnClick="btnCancel_Click" />
                </div>
                <hr />
            </asp:Panel>

            <asp:GridView ID="gvChoices" runat="server" AutoGenerateColumns="False" CssClass="table"
                DataKeyNames="choice_id" OnRowCommand="gvChoices_RowCommand">
                <Columns>
                    <asp:BoundField DataField="choice_id" HeaderText="ID" />
                    <asp:BoundField DataField="choice_text" HeaderText="Choice" />
                    <asp:CheckBoxField DataField="is_correct" HeaderText="Correct?" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button runat="server" CommandName="EditChoice" CommandArgument='<%# Eval("choice_id") %>' Text="Edit" CssClass="btn-secondary" />
                            <asp:Button runat="server" CommandName="DeleteChoice" CommandArgument='<%# Eval("choice_id") %>' Text="Delete" CssClass="btn-danger" OnClientClick="return confirm('Delete this choice?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
