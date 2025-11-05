using System;
using System.Data.SqlClient;
using System.Configuration;

namespace MyBahasa
{
    public partial class dbTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string conStr = ConfigurationManager.ConnectionStrings["MyBahasaDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    con.Open();
                    lblStatus.Text = "✅ Database connection successful!";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "❌ Connection failed: " + ex.Message;
                }
            }
        }
    }
}
