using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketBooking
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_clk_sbm(object sender, EventArgs e)
        {
            string Email = tbEmail.Text;
            string Name = tbName.Text;
            String Password = tbPassword.Text;
            String Role;
            if (rbAdmin.Checked)
            {
                Role = "admin";
            }
            else
            {
                Role = "user";
            }
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDBContext"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            
            string SelectQuery = @"SELECT * FROM USERS WHERE Email=@Email";
            command.CommandText = SelectQuery;
            command.Parameters.AddWithValue("@Email", Email);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                connection.Close();
                string insertQuery = @"INSERT INTO USERS (Name,Email,Password,Role) VALUES (@Name,@Email,@Password,@Role)";
                command.CommandText= insertQuery;
                command.Parameters.AddWithValue("@Name",Name);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@Role", Role);
                connection.Open();
                int res = command.ExecuteNonQuery();
                if (res == 0) {
                    lblMessage.Text = "Unable to Create New User";
                }
                else
                {
                    lblMessage.Text = "New User Created Succesfully";
                }
                connection.Close();

            }
            else
            {
                lblMessage.Text = "User with Same Email exist";
            }
        }
    }
}