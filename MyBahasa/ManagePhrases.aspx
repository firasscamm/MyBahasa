<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePhrases.aspx.cs" Inherits="MyBahasa.ManagePhrases" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Phrases - MyBahasa</title>
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
        <h2>Manage Phrases</h2>
        <p><a href="ManageLessons.aspx?course_id=<%= Request.QueryString["course_id"] %>">← Back to Lessons</a></p>
        <hr />

        <asp:Label ID="lblLessonTitle" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
        <hr />

        <div class="mb-3">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New Phrase" CssClass="btn-primary" OnClick="btnAddNew_Click" />
        </div>

        <asp:Panel ID="pnlForm" runat="server" Visible="false">
            <h4><asp:Label ID="lblFormTitle" runat="server" Text="Add Phrase"></asp:Label></h4>

            <div class="mb-3">
                <label>Malay Text:</label><br />
                <asp:TextBox ID="txtMalay" runat="server" CssClass="form-control" Width="100%" />
            </div>

            <div class="mb-3">
                <label>English Text:</label><br />
                <asp:TextBox ID="txtEnglish" runat="server" CssClass="form-control" Width="100%" />
            </div>

            <div class="mb-3">
                <label>Pronunciation:</label><br />
                <asp:TextBox ID="txtPronunciation" runat="server" CssClass="form-control" Width="100%" />
            </div>

            <div class="mb-3">
                <label>Cultural Note:</label><br />
                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" Width="100%" />
            </div>

            <div class="mb-3">
                <label>Audio URL:</label><br />
                <asp:TextBox ID="txtAudio" runat="server" CssClass="form-control" Width="100%" />
            </div>

            <div class="mb-3">
                <label>Video URL:</label><br />
                <asp:TextBox ID="txtVideo" runat="server" CssClass="form-control" Width="100%" />
            </div>

            <asp:HiddenField ID="hfPhraseId" runat="server" />

            <div class="mb-3">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-secondary" OnClick="btnCancel_Click" />
            </div>
            <hr />
        </asp:Panel>

        <asp:GridView ID="gvPhrases" runat="server" AutoGenerateColumns="False" CssClass="table"
            DataKeyNames="phrase_id" OnRowCommand="gvPhrases_RowCommand">
            <Columns>
                <asp:BoundField DataField="phrase_id" HeaderText="ID" />
                <asp:BoundField DataField="malay_text" HeaderText="Malay" />
                <asp:BoundField DataField="english_text" HeaderText="English" />
                <asp:BoundField DataField="pronunciation" HeaderText="Pronunciation" />
                <asp:BoundField DataField="cultural_note" HeaderText="Note" />
                <asp:BoundField DataField="audio_url" HeaderText="Audio" />
                <asp:BoundField DataField="video_url" HeaderText="Video" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button runat="server" CommandName="EditPhrase" CommandArgument='<%# Eval("phrase_id") %>' Text="Edit" CssClass="btn-secondary" />
                        <asp:Button runat="server" CommandName="DeletePhrase" CommandArgument='<%# Eval("phrase_id") %>' Text="Delete" CssClass="btn-danger"
                            OnClientClick="return confirm('Are you sure you want to delete this phrase?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
    </div>
</form>
</body>
</html>
