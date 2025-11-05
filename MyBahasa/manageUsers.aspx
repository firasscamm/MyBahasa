<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageUsers.aspx.cs" Inherits="MyBahasaWebApp.manageUsers" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Users</title>
    <style>
        body {
            font-family: 'Segoe UI', Arial, sans-serif;
            background-color: #f9fafb;
            color: #333;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 1000px;
            margin: 60px auto;
            background: #fff;
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.05);
        }

        h2 {
            text-align: center;
            color: #007b83;
            margin-bottom: 30px;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            border: 1px solid #ddd;
            margin-top: 20px;
        }

        th, td {
            padding: 12px;
            text-align: left;
            border-bottom: 1px solid #eee;
        }

        th {
            background-color: #007b83;
            color: white;
            font-weight: 600;
        }

        tr:nth-child(even) {
            background-color: #f3fdfd;
        }

        a {
            color: #007b83;
            font-weight: 500;
            text-decoration: none;
            margin-right: 10px;
        }

        a:hover {
            color: #00666e;
            text-decoration: underline;
        }

        .back-link {
            display: inline-block;
            margin-top: 25px;
            color: #555;
            text-decoration: none;
            font-size: 14px;
        }

        .back-link:hover {
            color: #007b83;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Manage Users</h2>

            <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="user_id"
                OnRowEditing="GridViewUsers_RowEditing"
                OnRowCancelingEdit="GridViewUsers_RowCancelingEdit"
                OnRowUpdating="GridViewUsers_RowUpdating"
                OnRowDeleting="GridViewUsers_RowDeleting"
                CssClass="table table-striped">
                <Columns>
                    <asp:BoundField DataField="user_id" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="email" HeaderText="Email" />
                    <asp:BoundField DataField="role" HeaderText="Role" />
                    <asp:BoundField DataField="country" HeaderText="Country" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>

            <a href="adminDashboard.aspx" class="back-link">← Back to Dashboard</a>
        </div>
    </form>
</body>
</html>
