<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCourses.aspx.cs" Inherits="MyBahasa.ManageCourses" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Courses - MyBahasa</title>
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
            <h2>Manage Courses</h2>
            <p><a href="adminDashboard.aspx">← Back to Dashboard</a></p>
            <hr />

            <div class="mb-3">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New Course" CssClass="btn-primary" OnClick="btnAddNew_Click" />
            </div>

            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <h4><asp:Label ID="lblFormTitle" runat="server" Text="Add New Course"></asp:Label></h4>

                <div class="mb-3">
                    <label>Title:</label><br />
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Width="100%" />
                </div>

                <div class="mb-3">
                    <label>Description:</label><br />
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" Width="100%" />
                </div>

                <div class="mb-3">
                    <label>Level:</label><br />
                    <asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Beginner" Value="Beginner"></asp:ListItem>
                        <asp:ListItem Text="Intermediate" Value="Intermediate"></asp:ListItem>
                        <asp:ListItem Text="Advanced" Value="Advanced"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label>Active:</label>
                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                </div>

                <asp:HiddenField ID="hfCourseId" runat="server" />

                <div class="mb-3">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-secondary" OnClick="btnCancel_Click" />
                </div>
                <hr />
            </asp:Panel>

            <asp:GridView ID="gvCourses" runat="server" AutoGenerateColumns="False" CssClass="table"
                DataKeyNames="course_id" OnRowCommand="gvCourses_RowCommand">
                <Columns>
                    <asp:BoundField DataField="course_id" HeaderText="ID" />
                    <asp:BoundField DataField="title" HeaderText="Title" />
                    <asp:BoundField DataField="description" HeaderText="Description" />
                    <asp:BoundField DataField="level" HeaderText="Level" />
                    <asp:CheckBoxField DataField="is_active" HeaderText="Active" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button runat="server" CommandName="EditCourse" CommandArgument='<%# Eval("course_id") %>' Text="Edit" CssClass="btn-secondary" />
                            <asp:Button runat="server" CommandName="DeleteCourse" CommandArgument='<%# Eval("course_id") %>' Text="Delete" CssClass="btn-danger" OnClientClick="return confirm('Are you sure you want to delete this course?');" />
                            <asp:Button runat="server" CommandName="ManageLessons" CommandArgument='<%# Eval("course_id") %>' Text="Manage Lessons" CssClass="btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
