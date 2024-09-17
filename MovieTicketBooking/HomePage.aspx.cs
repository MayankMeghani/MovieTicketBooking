using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketBooking
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome, ";
                LoadMovies();
                CheckAdminRole();
            }
        }

        private void LoadMovies()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Movies WHERE IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                StringBuilder movieList = new StringBuilder();

                while (reader.Read())
                {
                    string movieId = reader["MovieId"].ToString();
                    string title = reader["Title"].ToString();
                    string poster = reader["Poster"].ToString();
                    string posterUrl = ResolveUrl("~/UploadedPhotos/" + poster);

                    // Wrap the entire div in a label associated with the radio button
                    movieList.Append($"<label for='movie_{movieId}' style='cursor: pointer; flex-basis: 30%; margin: 10px;'>");
                    movieList.Append("<div class='movie-item' style='border: 1px solid #ccc; padding: 10px; border-radius: 10px; text-align: center;'>");

                    // Movie poster image
                    movieList.Append($"<img src='{posterUrl}' alt='{title}' style='width: 100%; height: 200px; object-fit: cover; border-radius: 8px;' />");

                    // Movie details
                    movieList.Append("<div style='padding: 10px;'>");

                    // Radio button - hidden but accessible by clicking the whole label
                    movieList.Append($"<input type='radio' id='movie_{movieId}' name='rblMovies' value='{movieId}'  />");

                    // Hidden input for title (for form processing, if necessary)
                    movieList.Append($"<input type='hidden' id='title_{movieId}' name='title_{movieId}' value='{title}' />");

                    // Movie title
                    movieList.Append($"<span style='display: block; font-size: 20px; font-weight: 600; color: #34495e;'>{title}</span>");

                    movieList.Append("</div>");
                    movieList.Append("</div>");
                    movieList.Append("</label>");
                }


                ltlMovieList.Text = movieList.ToString();
            }
        }

        private void CheckAdminRole()
        {
            string role = Session["Role"] as string;
            pnlAdmin.Visible = (role?.Trim() == "admin");
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            string selectedMovieId = Request.Form["rblMovies"];
            if (!string.IsNullOrEmpty(selectedMovieId))
            {
                string selectedMovieTitle = Request.Form[$"title_{selectedMovieId}"];
                Session["MovieId"] = selectedMovieId;
                Session["MovieName"] = selectedMovieTitle;
                Response.Redirect("MovieSchedule.aspx");
            }
            else
            {
                // Consider adding a label to display this message
                lblMessage.Visible = true;
                lblMessage.Text = "Please select a movie.";
            }
        }

        protected void btn_clk_LogOut(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("LoginPage.aspx");
        }

    }
}