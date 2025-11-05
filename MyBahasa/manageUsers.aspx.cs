using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MyBahasaWebApp
{
    public partial class manageUsers : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MyBahasaDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null || Session["role"].ToString() != "admin")
                Response.Redirect("login.aspx");

            if (!IsPostBack)
                BindGrid();
        }

        private void BindGrid()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT user_id, name, email, role, country FROM Users", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridViewUsers.DataSource = dt;
                GridViewUsers.DataBind();
            }
        }

        protected void GridViewUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewUsers.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridViewUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewUsers.EditIndex = -1;
            BindGrid();
        }

        protected void GridViewUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int user_id = Convert.ToInt32(GridViewUsers.DataKeys[e.RowIndex].Value.ToString());
            GridViewRow row = GridViewUsers.Rows[e.RowIndex];
            string name = (row.Cells[1].Controls[0] as TextBox).Text;
            string email = (row.Cells[2].Controls[0] as TextBox).Text;
            string role = (row.Cells[3].Controls[0] as TextBox).Text;
            string country = (row.Cells[4].Controls[0] as TextBox).Text;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Users SET name=@name, email=@email, role=@role, country=@country WHERE user_id=@id", con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@id", user_id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridViewUsers.EditIndex = -1;
            BindGrid();
        }

        protected void GridViewUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int user_id = Convert.ToInt32(GridViewUsers.DataKeys[e.RowIndex].Value.ToString());

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE user_id=@id", con);
                cmd.Parameters.AddWithValue("@id", user_id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid();
        }
    }
}
