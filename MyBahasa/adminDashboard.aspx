<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminDashboard.aspx.cs" Inherits="MyBahasaWebApp.adminDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Admin Dashboard</title>
    <style>
        body {
            font-family: 'Segoe UI', Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f9fafb;
            color: #333;
        }

        .container {
            max-width: 800px;
            margin: 60px auto;
            background: #fff;
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
            text-align: center;
        }

        h2 {
            color: #007b83;
            margin-bottom: 20px;
        }

        #lblWelcome {
            display: block;
            font-size: 18px;
            margin-bottom: 30px;
            color: #444;
        }

        /* Buttons */
        .btn {
            display: inline-block;
            padding: 10px 25px;
            border-radius: 6px;
            font-size: 16px;
            font-weight: 500;
            text-decoration: none;
            transition: background-color 0.2s ease;
            margin: 8px;
        }

        .btn-logout {
            background-color: #dc3545;
            color: white;
            border: none;
        }

        .btn-logout:hover {
            background-color: #b02a37;
        }

        .btn-manage {
            background-color: #007b83;
            color: white;
        }

        .btn-manage:hover {
            background-color: #00666e;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Admin Dashboard</h2>
            <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label>

            <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn btn-logout" OnClick="btnLogout_Click">Logout</asp:LinkButton>

            <div>
                <a href="manageUsers.aspx" class="btn btn-manage">Manage Users</a>
                <a href="manageCourses.aspx" class="btn btn-manage">Manage Courses</a>
            </div>
        </div>
    </form>
</body>
</html>
