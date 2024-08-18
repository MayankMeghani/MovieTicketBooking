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
    public partial class UpdateMovieForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from Movies";
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = reader["Title"].ToString();
                    item.Value = reader["MovieId"].ToString();
                    ddlMovies.Items.Add(item);

                }
                con.Close();
            }
        }
        protected void btn_clk_fetch(object sender, EventArgs e)
        {
            ViewState["selectedMovie"] = ddlMovies.SelectedValue;
            int movieID = Convert.ToInt32(ddlMovies.SelectedValue);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string query = "SELECT * FROM MOVIES WHERE MovieId = @movieId";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@movieId", movieID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) 
                {
                    tbTitle.Text = reader["Title"].ToString();
                    tbGenre.Text = reader["Genre"].ToString();
                    tbDuration.Text = reader["Duration"].ToString();
                    bool isActive = Convert.ToBoolean(reader["IsActive"]);
                    rblActive.SelectedValue = isActive ? "True" : "False";
                    DateTime releaseDate = Convert.ToDateTime(reader["ReleaseDate"]);
                    tbDate.Text = releaseDate.ToString("yyyy-MM-dd");
                    
                }

                con.Close();
                panelDetails.Visible = true;
            }
        }

        protected void btn_sbm_update(object sender, EventArgs e)
        {
            string title = tbTitle.Text;
            string genre = tbGenre.Text;
            string duration = tbDuration.Text;
            bool isActive = Convert.ToBoolean(rblActive.SelectedValue);
            string date = tbDate.Text;
            int movieId = Convert.ToInt32(ViewState["selectedMovie"]);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string query = "UPDATE Movies  set Title = @title, Genre = @genre,Duration = @duration,isActive = @isActive, ReleaseDate = @date WHERE MovieId=@movieID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@duration", duration);
                cmd.Parameters.AddWithValue("@isActive", isActive);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@MovieId", movieId);
                con.Open();
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    panelDetails.Visible = false;
                    lblMessage.Text = "Movie Updated Succesfully";
                }
                else
                {
                    lblMessage.Text = "Error While updating Movie";
                }

            }

        }
    }
}