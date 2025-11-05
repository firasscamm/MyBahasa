using BCrypt.Net;
using System;
using System.Data.SqlClient;

namespace MyBahasa
{
    public partial class register : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(ddlCountry.SelectedValue))
            {
                lblErr.Text = "All fields are required.";
                lblMsg.Text = "";
                return;
            }

            // Hash password
            string hash = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text, workFactor: 12);

            // Connection string
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Users(name, email, password_hash, role, country) VALUES(@name, @email, @password_hash, @role, @country)", con);
                cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@password_hash", hash);

                // 👇 Add this missing parameter
                cmd.Parameters.AddWithValue("@role", "learner");

                cmd.Parameters.AddWithValue("@country", ddlCountry.SelectedValue);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Text = "✅ Registration successful!";
                    lblErr.Text = "";
                    txtName.Text = txtEmail.Text = txtPassword.Text = "";
                    ddlCountry.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    lblErr.Text = "❌ Error: " + ex.Message;
                    lblMsg.Text = "";
                }
            }
        }
    }
}
