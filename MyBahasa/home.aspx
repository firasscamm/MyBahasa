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

        /* Forum Styling */
        .forum-section {
            margin: 80px auto;
            max-width: 1000px;
            padding: 0 20px;
        }

        .forum-container {
            background: white;
            border-radius: 12px;
            padding: 30px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            margin-bottom: 30px;
        }

        .forum-message {
            border: 1px solid #eee;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 15px;
            border-left: 4px solid #007b83;
            transition: all 0.2s ease;
        }

        .forum-message:hover {
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            transform: translateY(-1px);
        }

        .post-form-container {
            background: white;
            border-radius: 12px;
            padding: 30px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            transition: all 0.3s ease;
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

    <!-- Discussion Forum Section -->
    <section class="forum-section">
        <h2 style="text-align: center; margin-bottom: 40px; color: #007b83;">Community Discussion Forum</h2>
        
        <!-- Forum Messages Display -->
        <div class="forum-container">
            <h3 style="color: #007b83; margin-bottom: 20px;">Recent Discussions</h3>
            
            <asp:Repeater ID="rptForumMessages" runat="server">
                <ItemTemplate>
                    <div class="forum-message">
                        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 10px;">
                            <strong style="color: #007b83;"><%# Eval("name") %></strong>
                            <span style="color: #666; font-size: 12px;"><%# Eval("posted_at", "{0:MMM dd, yyyy HH:mm}") %></span>
                        </div>
                        <p style="margin: 0; color: #333; line-height: 1.5;"><%# Eval("message_text") %></p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            
            <asp:Label ID="lblNoMessages" runat="server" Text="No messages yet. Be the first to start the discussion!" 
                      Visible="false" Style="text-align: center; color: #666; display: block; padding: 40px;"></asp:Label>
        </div>

        <!-- Post Message Form -->
        <div class="post-form-container">
            <asp:Panel ID="pnlPostForm" runat="server" Visible="false">
                <h3 style="color: #007b83; margin-bottom: 20px;">Post a Message</h3>
                <div style="margin-bottom: 20px;">
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="4" 
                                placeholder="Share your thoughts, ask questions, or help other learners..."
                                Style="width: 100%; padding: 15px; border: 1px solid #ddd; border-radius: 8px; font-family: 'Segoe UI', sans-serif; resize: vertical;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage"
                                              ErrorMessage="Please enter a message" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <asp:Button ID="btnPostMessage" runat="server" Text="Post Message" 
                           OnClick="btnPostMessage_Click" 
                           Style="background-color: #007b83; color: white; border: none; padding: 12px 30px; border-radius: 6px; cursor: pointer; font-size: 16px;" />
            </asp:Panel>

            <!-- Login/Register Prompt for Guests -->
            <asp:Panel ID="pnlGuestPrompt" runat="server" Visible="false">
                <div style="text-align: center; padding: 30px;">
                    <h4 style="color: #007b83; margin-bottom: 15px;">Join the Discussion</h4>
                    <p style="color: #666; margin-bottom: 20px;">Login or register to participate in the community forum and connect with other learners.</p>
                    <div>
                        <a href="login.aspx" class="btn-login" style="display: inline-block; margin: 0 8px; padding: 12px 28px; border-radius: 6px; font-size: 16px; font-weight: 500; text-decoration: none; transition: background-color 0.2s ease; background-color: #007b83; color: white;">Login</a>
                        <a href="register.aspx" class="btn-register" style="display: inline-block; margin: 0 8px; padding: 12px 28px; border-radius: 6px; font-size: 16px; font-weight: 500; text-decoration: none; transition: background-color 0.2s ease; border: 1px solid #ccc; color: #333; background-color: transparent;">Register</a>
                    </div>
                </div>
            </asp:Panel>

            <asp:Label ID="lblForumMessage" runat="server" Text="" Style="display: block; margin-top: 15px; padding: 10px; border-radius: 4px;"></asp:Label>
        </div>
    </section>

    <!-- Footer tagline -->
    <div class="home-footer">
        Learn Bahasa Melayu — Your gateway to Malaysian culture and communication.
    </div>
</asp:Content>