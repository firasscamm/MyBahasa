using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;

namespace MyBahasa
{
    public partial class home : Page
    {
        string cs = WebConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    LoadForumMessages();
                    CheckUserAuthentication();
                }
                catch (Exception ex)
                {
                    // Log error and show user-friendly message
                    lblForumMessage.Text = "Unable to load forum messages. Please try again later.";
                    lblForumMessage.Style["color"] = "red";
                    System.Diagnostics.Debug.WriteLine("Error in Page_Load: " + ex.Message);
                }
            }
        }

        private void LoadForumMessages()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                        SELECT TOP 10 
                            fm.message_id, 
                            fm.message_text, 
                            fm.posted_at, 
                            u.name 
                        FROM ForumMessages fm
                        INNER JOIN Users u ON fm.user_id = u.user_id
                        WHERE fm.is_active = 1
                        ORDER BY fm.posted_at DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptForumMessages.DataSource = dt;
                        rptForumMessages.DataBind();
                        lblNoMessages.Visible = false;
                        rptForumMessages.Visible = true;
                    }
                    else
                    {
                        rptForumMessages.Visible = false;
                        lblNoMessages.Visible = true;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL-specific errors
                lblNoMessages.Text = "Forum temporarily unavailable. Please check back later.";
                lblNoMessages.Visible = true;
                rptForumMessages.Visible = false;
                System.Diagnostics.Debug.WriteLine("SQL Error in LoadForumMessages: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // Handle other errors
                lblNoMessages.Text = "Unable to load forum messages.";
                lblNoMessages.Visible = true;
                rptForumMessages.Visible = false;
                System.Diagnostics.Debug.WriteLine("Error in LoadForumMessages: " + ex.Message);
            }
        }

        private void CheckUserAuthentication()
        {
            if (Session["user_id"] != null)
            {
                // User is logged in - show post form
                pnlPostForm.Visible = true;
                pnlGuestPrompt.Visible = false;
            }
            else
            {
                // User is guest - show login prompt
                pnlPostForm.Visible = false;
                pnlGuestPrompt.Visible = true;
            }
        }

        protected void btnPostMessage_Click(object sender, EventArgs e)
        {
            if (Session["user_id"] == null)
            {
                lblForumMessage.Text = "Please login to post messages.";
                lblForumMessage.Style["color"] = "red";
                return;
            }

            if (string.IsNullOrEmpty(txtMessage.Text.Trim()))
            {
                lblForumMessage.Text = "Please enter a message.";
                lblForumMessage.Style["color"] = "red";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                        INSERT INTO ForumMessages (user_id, message_text, posted_at)
                        VALUES (@user_id, @message_text, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                    cmd.Parameters.AddWithValue("@message_text", txtMessage.Text.Trim());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                // Clear the textbox and show success message
                txtMessage.Text = "";
                lblForumMessage.Text = "Message posted successfully!";
                lblForumMessage.Style["color"] = "green";

                // Reload messages to show the new one
                LoadForumMessages();
            }
            catch (SqlException sqlEx)
            {
                lblForumMessage.Text = "Database error. Please try again.";
                lblForumMessage.Style["color"] = "red";
                System.Diagnostics.Debug.WriteLine("SQL Error in btnPostMessage: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                lblForumMessage.Text = "Error posting message. Please try again.";
                lblForumMessage.Style["color"] = "red";
                System.Diagnostics.Debug.WriteLine("Error in btnPostMessage: " + ex.Message);
            }
        }
    }
}