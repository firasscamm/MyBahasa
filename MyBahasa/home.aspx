<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="MyBahasa.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Page styling */
        .hero {
            text-align: center;
            padding: 100px 20px 80px;
            max-width: 800px;
            margin: 0 auto;
        }

        .hero h1 {
            font-size: 42px;
            font-weight: 700;
            margin-bottom: 20px;
        }

        .hero p {
            font-size: 18px;
            color: #555;
            margin-bottom: 35px;
            line-height: 1.6;
        }

        .hero-buttons a {
            display: inline-block;
            margin: 0 8px;
            padding: 12px 28px;
            border-radius: 6px;
            font-size: 16px;
            font-weight: 500;
            text-decoration: none;
            transition: background-color 0.2s ease;
        }

        .btn-login {
            background-color: #007b83;
            color: white;
        }

        .btn-login:hover {
            background-color: #00666e;
        }

        .btn-register {
            border: 1px solid #ccc;
            color: #333;
            background-color: transparent;
        }

        .btn-register:hover {
            background-color: #f7f7f7;
        }

        /* Features */
        .features {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 20px;
            max-width: 1100px;
            margin: 60px auto;
        }

        .feature-card {
            flex: 1 1 230px;
            border: 1px solid #ddd;
            border-radius: 10px;
            padding: 25px 20px;
            text-align: center;
            transition: box-shadow 0.3s;
        }

        .feature-card:hover {
            box-shadow: 0 3px 10px rgba(0,0,0,0.1);
        }

        .feature-card h3 {
            font-size: 18px;
            margin-top: 15px;
        }

        .feature-card p {
            color: #666;
            font-size: 14px;
        }

        /* Learning Paths */
        .learning-section {
            text-align: center;
            margin-top: 80px;
        }

        .learning-section h2 {
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 40px;
        }

        .learning-paths {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 20px;
            max-width: 1000px;
            margin: 0 auto 80px;
        }

        .path-card {
            flex: 1 1 280px;
            border: 1px solid #ddd;
            border-radius: 10px;
            padding: 25px;
            text-align: left;
            transition: box-shadow 0.3s;
        }

        .path-card:hover {
            box-shadow: 0 3px 10px rgba(0,0,0,0.1);
        }

        .path-card h3 {
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 10px;
        }

        .path-card ul {
            list-style: none;
            padding: 0;
            margin: 10px 0 20px;
            color: #555;
            font-size: 14px;
        }

        .path-card ul li {
            margin-bottom: 6px;
        }

        .path-btn {
            display: inline-block;
            padding: 10px 20px;
            border: 1px solid #007b83;
            border-radius: 6px;
            color: #007b83;
            text-decoration: none;
            transition: background-color 0.2s;
        }

        .path-btn:hover {
            background-color: #e6f5f6;
        }

        /* Footer tagline */
        .home-footer {
            text-align: center;
            font-size: 14px;
            color: #777;
            padding: 40px 10px 10px;
            border-top: 1px solid #eee;
        }
    </style>

    <!-- Hero Section -->
    <section class="hero">
        <h1>Master Bahasa Melayu with Interactive Lessons</h1>
        <p>
            Learn essential Malay phrases, vocabulary, and pronunciation through engaging lessons designed
            for daily conversations in Malaysia. Perfect for beginners of all ages.
        </p>
        <div class="hero-buttons">
            <a href="login.aspx" class="btn-login">Login</a>
            <a href="register.aspx" class="btn-register">Register</a>
        </div>
    </section>

    <!-- Features Section -->
    <section class="features">
        <div class="feature-card">
            <h3>📘 Structured Modules</h3>
            <p>Learn through carefully designed lessons that progress from basic to advanced topics.</p>
        </div>

        <div class="feature-card">
            <h3>🔊 Audio Pronunciation</h3>
            <p>Listen to native speakers and practice your pronunciation with audio guides.</p>
        </div>

        <div class="feature-card">
            <h3>🏆 Track Progress</h3>
            <p>Monitor your learning journey and celebrate milestones as you advance.</p>
        </div>

        <div class="feature-card">
            <h3>👨‍👩‍👧 All Age Groups</h3>
            <p>Designed for learners of all ages with user-friendly interface and content.</p>
        </div>
    </section>

    <!-- Learning Path Section -->
    <section class="learning-section">
        <h2>Choose Your Learning Path</h2>
        <div class="learning-paths">
            <div class="path-card">
                <h3>Beginner</h3>
                <ul>
                    <li>• Basic greetings</li>
                    <li>• Numbers and counting</li>
                    <li>• Common daily phrases</li>
                    <li>• Simple introductions</li>
                </ul>
                <a href="learn.aspx" class="path-btn">Start Beginner</a>
            </div>

            <div class="path-card">
                <h3>Intermediate</h3>
                <ul>
                    <li>• Shopping and dining</li>
                    <li>• Directions and travel</li>
                    <li>• Making requests</li>
                    <li>• Cultural expressions</li>
                </ul>
                <a href="learn.aspx" class="path-btn">Start Intermediate</a>
            </div>

            <div class="path-card">
                <h3>Advanced</h3>
                <ul>
                    <li>• Business conversations</li>
                    <li>• Idiomatic expressions</li>
                    <li>• Cultural insights</li>
                    <li>• Advanced grammar</li>
                </ul>
                <a href="learn.aspx" class="path-btn">Start Advanced</a>
            </div>
        </div>
    </section>

    <!-- Footer tagline -->
    <div class="home-footer">
        Learn Bahasa Melayu — Your gateway to Malaysian culture and communication.
    </div>
</asp:Content>
