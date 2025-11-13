<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Lesson.aspx.cs" Inherits="MyBahasa.Lesson" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Lesson</title>
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            background-color: #f9fafb;
            display: flex;  
            flex-direction: column;
            align-items: center;
            justify-content: center;
            height: 100vh;
            margin: 0;
        }

        .lesson-container {
            background-color: #fff;
            padding: 50px;
            border-radius: 16px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
            max-width: 1100px;
            width: 90%;
            text-align: center;
            display: flex;
            flex-direction: column;
            justify-content: flex-start;
            min-height: 720px;
        }

        .lesson-title {
            font-size: 28px;
            font-weight: 600;
            color: #007b83;
            margin-bottom: 40px;
        }

        .content-section {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 60px;
            flex-wrap: wrap;
            margin-bottom: 40px;
        }

        .video-box {
            flex: 1;
            min-width: 420px;
        }

        iframe {
            width: 100%;
            height: 380px;
            border-radius: 12px;
            border: none;
        }

        .info-box {
            flex: 1;
            min-width: 420px;
            text-align: left;
            display: flex;
            flex-direction: column;
            justify-content: center;
        }

        .lang-label {
            text-transform: uppercase;
            font-size: 13px;
            font-weight: 600;
            color: #777;
            margin-bottom: 5px;
        }

        .phrase {
            font-size: 36px;
            font-weight: 700;
            color: #222;
            margin-bottom: 25px;
        }

        .translation {
            font-size: 22px;
            font-weight: 600;
            color: #444;
            margin-bottom: 25px;
        }

        .pronunciation {
            font-size: 18px;
            color: #666;
            margin-bottom: 25px;
        }

        .note {
            font-size: 15px;
            color: #999;
            margin-bottom: 25px;
        }

        audio {
            width: 100%;
            margin-top: 10px;
        }

        .progress-text {
            font-size: 14px;
            color: #666;
            margin: 15px 0;
            text-align: center;
        }

        .buttons {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin-top: auto;
        }

        .btn {
            background-color: #007b83;
            color: white;
            border: none;
            padding: 12px 22px;
            font-size: 16px;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.2s;
        }

        .btn:hover {
            background-color: #00666e;
        }

        .btn-disabled {
            background-color: #ccc;
            cursor: not-allowed;
        }

        .return-btn {
            background-color: #555;
            width: 100%;
            margin-top: 25px;
        }

        .return-btn:hover {
            background-color: #333;
        }

        .fade {
            animation: fadeIn 0.4s ease;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(5px); }
            to { opacity: 1; transform: translateY(0); }
        }

        @media (max-width: 900px) {
            .content-section {
                flex-direction: column;
                align-items: center;
                gap: 40px;
            }

            .info-box {
                text-align: center;
                align-items: center;
            }

            audio {
                width: 80%;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="lesson-container fade">
            <asp:Label ID="lblLessonTitle" runat="server" CssClass="lesson-title"></asp:Label>

            <div class="content-section">
                <div class="video-box">
                    <asp:Literal ID="ltlVideo" runat="server"></asp:Literal>
                </div>

                <div class="info-box">
                    <div class="lang-label">MALAY</div>
                    <asp:Label ID="lblMalayText" runat="server" CssClass="phrase"></asp:Label>

                    <div class="lang-label">ENGLISH (UK)</div>
                    <asp:Label ID="lblEnglishText" runat="server" CssClass="translation"></asp:Label>

                    <div class="lang-label">PRONUNCIATION</div>
                    <asp:Label ID="lblPronunciation" runat="server" CssClass="pronunciation"></asp:Label>

                    <asp:Label ID="lblNote" runat="server" CssClass="note"></asp:Label>

                    <div class="lang-label">AUDIO</div>
                    <asp:Literal ID="ltlAudio" runat="server"></asp:Literal>
                </div>
            </div>

            <asp:Label ID="lblProgress" runat="server" CssClass="progress-text"></asp:Label>

            <div class="buttons">
                <asp:Button ID="btnPrev" runat="server" Text="Previous" CssClass="btn" OnClick="btnPrev_Click" />
                <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn" OnClick="btnNext_Click" />
                <asp:Button ID="btnQuiz" runat="server" Text="Take Quiz" CssClass="btn" OnClick="btnQuiz_Click" Visible="false" />

            </div>

            <asp:Button ID="btnReturn" runat="server" Text="Return to Learn" CssClass="btn return-btn" OnClick="btnReturn_Click" />
        </div>
    </form>
</body>
</html>
