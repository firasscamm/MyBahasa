<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="MyBahasa.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Overall page */
        body {
            background-color: #f9fafb;
            font-family: 'Segoe UI', Arial, sans-serif;
        }

        /* Card container */
        .form-container {
            max-width: 420px;
            margin: 100px auto 80px;
            padding: 40px 35px;
            background-color: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
        }

        .form-container h2 {
            text-align: center;
            margin-bottom: 30px;
            font-size: 26px;
            color: #007b83;
        }

        /* Inputs */
        .form-container input[type="text"],
        .form-container input[type="email"],
        .form-container input[type="password"] {
            width: 100%;
            padding: 12px 14px;
            margin-bottom: 18px;
            border: 1px solid #ddd;
            border-radius: 8px;
            font-size: 15px;
            transition: border-color 0.2s ease;
        }

        .form-container input:focus {
            border-color: #007b83;
            outline: none;
        }

        /* Button */
        .form-container button,
        .form-container input[type="submit"],
        .form-container .aspNetButton {
            width: 100%;
            padding: 12px;
            background-color: #007b83;
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: 500;
            cursor: pointer;
            transition: background-color 0.2s ease;
        }

        .form-container button:hover,
        .form-container input[type="submit"]:hover {
            background-color: #00666e;
        }

        /* Messages */
        .form-container .error {
            color: #d32f2f;
            text-align: center;
            margin-bottom: 15px;
            font-size: 14px;
        }

        .form-container .success {
            color: #007b83;
            text-align: center;
            margin-bottom: 15px;
            font-size: 14px;
        }

        /* Register link */
        .form-footer {
            text-align: center;
            margin-top: 20px;
            font-size: 14px;
            color: #555;
        }

        .form-footer a {
            color: #007b83;
            text-decoration: none;
            font-weight: 500;
        }

        .form-footer a:hover {
            text-decoration: underline;
        }
    </style>

    <div class="form-container">
        <h2>Login</h2>

        <asp:Label ID="lblErr" runat="server" CssClass="error"></asp:Label>

        <asp:TextBox ID="txtEmail" runat="server" Placeholder="Email" TextMode="Email"></asp:TextBox>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Placeholder="Password"></asp:TextBox>

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="aspNetButton" OnClick="btnLogin_Click" />

        <asp:Label ID="lblMsg" runat="server" CssClass="success"></asp:Label>

        <div class="form-footer">
            Don't have an account? <a href="register.aspx">Register here</a>
        </div>
    </div>
</asp:Content>
