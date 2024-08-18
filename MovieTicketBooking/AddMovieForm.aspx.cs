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
    public partial class AddMovie : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_sbt_add(object sender, EventArgs e)
        {
            string title = tbTitle.Text;
            string genre = tbGenre.Text;
            string duration = tbDuration.Text;
            string date = tbDate.Text;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string query = "INSERT INTO Movies (Title,Genre,Duration,ReleaseDate) Values (@title,@genre,@duration,@date)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@duration", duration); 
                cmd.Parameters.AddWithValue("@date", date);
                con.Open();
                int res=cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    lblHeading.Text = "Movie Added Succesfully";
                }
                else
                {
                    lblHeading.Text = "Error While Adding Movie";
                }
            }
        }
    }
}