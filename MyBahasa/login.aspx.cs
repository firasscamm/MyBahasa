using System;
using System.Data.SqlClient;
using System.Configuration;
using BCrypt.Net;

namespace MyBahasa
{
    public partial class login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblErr.Text = "⚠️ Please enter both email and password.";
                lblMsg.Text = "";
                return;
            }

            string conStr = ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT user_id, name, password_hash, role 
                  FROM Users WHERE email = @e", con))
            {
                cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());

                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        string storedHash = dr["password_hash"].ToString();
                        bool verified = BCrypt.Net.BCrypt.Verify(txtPassword.Text, storedHash);

                        if (verified)
                        {
                            Session["user_id"] = dr["user_id"];
                            Session["name"] = dr["name"].ToString();
                            Session["role"] = dr["role"].ToString();

                            string role = Session["role"].ToString().Trim().ToLower();

                            lblMsg.Text = "✅ Login successful!";
                            lblErr.Text = "";

                            if (role == "admin")
                                Response.Redirect("adminDashboard.aspx");
                            else
                                Response.Redirect("progress.aspx");
                        }
                        else
                        {
                            lblErr.Text = "❌ Invalid password.";
                            lblMsg.Text = "";
                        }
                    }
                    else
                    {
                        lblErr.Text = "❌ No user found with this email.";
                        lblMsg.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    lblErr.Text = "❌ Error: " + ex.Message;
                }
            }
        }
    }
}
