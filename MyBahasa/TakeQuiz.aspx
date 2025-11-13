<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakeQuiz.aspx.cs" Inherits="MyBahasa.TakeQuiz" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Take Quiz - MyBahasa</title>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background:#f9fafb; margin:0; }
        .container { max-width: 900px; margin: 50px auto; background:#fff; padding:40px; border-radius:10px; box-shadow:0 4px 10px rgba(0,0,0,.1); }
        h2 { text-align:center; color:#007b83; margin-bottom:10px; }
        h4 { margin:25px 0 10px; color:#333; }
        .question { margin-bottom:30px; padding-bottom:15px; border-bottom:1px solid #eee; }
        .choices { margin-left: 10px; }
        .choice { display:flex; align-items:center; gap:8px; margin:6px 0; cursor:pointer; font-size:15px; color:#333; }
        .btn { background:#007b83; color:#fff; border:0; padding:12px 20px; border-radius:8px; font-size:16px; cursor:pointer; }
        .btn:hover { background:#00666e; }
        .message { text-align:center; margin-top:25px; font-size:18px; color:#007b83; font-weight:600; }
        
        /* Ensure radio buttons are properly styled */
        input[type="radio"] {
            margin: 0;
            cursor: pointer;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="container">
        <h2><asp:Label ID="lblLessonTitle" runat="server" /></h2>
        <hr />

        <asp:Panel ID="pnlQuiz" runat="server">
            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div class="question">
                        <h4>Q<%# Container.ItemIndex + 1 %>: <%# Eval("StemText") %></h4>

                        <!-- store question id for submit -->
                        <asp:HiddenField ID="hfQuestionId" runat="server" Value='<%# Eval("QuestionID") %>' />

                        <div class="choices">
                            <asp:Repeater ID="rptChoices" runat="server" OnItemDataBound="rptChoices_ItemDataBound">
                                <ItemTemplate>
                                    <label class="choice">
                                        <!-- real ASP radio (single answer enforced via GroupName in code-behind) -->
                                        <asp:RadioButton ID="rbChoice" runat="server" />
                                        <!-- store choice id beside the radio -->
                                        <asp:HiddenField ID="hfChoiceId" runat="server" Value='<%# Eval("ChoiceID") %>' />
                                        <asp:Literal ID="litChoiceText" runat="server" Text='<%# Eval("ChoiceText") %>'></asp:Literal>
                                    </label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Button ID="btnSubmit" runat="server" Text="Submit Quiz" CssClass="btn" OnClick="btnSubmit_Click" />
        </asp:Panel>

        <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
    </div>
</form>
</body>
</html>