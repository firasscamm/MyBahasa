<%@ Page Title="Learn" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Learn.aspx.cs" Inherits="MyBahasa.Learn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .tabs {
            display: flex;
            justify-content: center;
            margin: 30px 0;
        }

        .tab {
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            margin: 0 8px;
            cursor: pointer;
            background-color: #f5f5f5;
            color: #333;
            font-weight: 500;
        }

        .tab.active {
            background-color: #007b83;
            color: white;
        }

        .lesson-grid {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 25px;
        }

        .lesson-card {
            background-color: white;
            width: 300px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            padding: 20px;
            transition: all 0.2s ease-in-out;
        }

        .lesson-card:hover {
            transform: translateY(-5px);
        }

        .lesson-title {
            font-size: 20px;
            color: #007b83;
            font-weight: bold;
            margin-bottom: 8px;
        }

        .lesson-desc {
            color: #555;
            font-size: 14px;
            margin-bottom: 12px;
        }

        .badge {
            background-color: #e0f7f5;
            color: #007b83;
            font-size: 12px;
            border-radius: 5px;
            padding: 3px 8px;
            display: inline-block;
            margin-bottom: 8px;
        }

        .btn-start {
            width: 100%;
            background-color: #007b83;
            color: white;
            border: none;
            padding: 10px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 15px;
            font-weight: 500;
        }

        .btn-start:hover {
            background-color: #005f63;
        }
    </style>

    <div class="container">
        <h2 style="text-align:center; margin-top:40px;">Learning Modules</h2>
        <p style="text-align:center; color:#555;">Choose a lesson to start learning Bahasa Melayu phrases and vocabulary</p>

        <div class="tabs">
            <asp:Button ID="btnBeginner" runat="server" Text="Beginner" CssClass="tab active" OnClick="btnBeginner_Click" />
            <asp:Button ID="btnIntermediate" runat="server" Text="Intermediate" CssClass="tab" OnClick="btnIntermediate_Click" />
            <asp:Button ID="btnAdvanced" runat="server" Text="Advanced" CssClass="tab" OnClick="btnAdvanced_Click" />
        </div>

        <div class="lesson-grid">
            <asp:Repeater ID="rptLessons" runat="server">
                <ItemTemplate>
                    <div class="lesson-card">
                        <div class="lesson-title"><%# Eval("title") %></div>
                        <div class="lesson-desc"><%# Eval("content_text") %></div>
                        <span class="badge"><%# Eval("level") %></span>
                        <br />
                        <asp:Button runat="server" CssClass="btn-start" Text="Start Lesson"
                            CommandArgument='<%# Eval("lesson_id") %>'
                            OnCommand="StartLesson_Command" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
