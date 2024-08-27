using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace MovieTicketBooking
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_clk_sbm(object sender, EventArgs e)
        {

            string Email = tbEmail.Text;
            String Password = tbPassword.Text;
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDBContext"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            string SelectQuery = @"SELECT * FROM USERS WHERE Email=@Email AND Password=@Password" ;
            command.CommandText = SelectQuery;
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Password", Password);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Session["UserId"] = reader["Id"].ToString();
                Session["UserName"] = reader["Name"].ToString();
                Session["Role"]= reader["Role"].ToString();
                connection.Close();
                Response.Redirect("HomePage.aspx");
            }
            else
            {
                connection.Close();
                lblMessage.Text = "Enter Valid Creadentials";
            }
        }
    
    }
}