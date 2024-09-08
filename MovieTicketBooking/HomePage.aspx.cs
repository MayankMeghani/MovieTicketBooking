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
                lblWelcome.Text = "Welcome, " + Session["UserName"].ToString();
                LoadMovies();
                CheckAdminRole();
            }
        }

        private void LoadMovies()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Movies", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                StringBuilder movieList = new StringBuilder();

                while (reader.Read())
                {
                    string movieId = reader["MovieId"].ToString();
                    string title = reader["Title"].ToString();
                    string poster = reader["Poster"].ToString();
                    string posterUrl = ResolveUrl("~/UploadedPhotos/" + poster);

                    movieList.Append("<div class='movie-item'>");
                    movieList.Append($"<label for='movie_{movieId}'>");
                    movieList.Append($"<img src='{posterUrl}' alt='{title}' style='width:100px;height:auto;' />");
                    movieList.Append("<br>");
                    movieList.Append($"<input type='radio' id='movie_{movieId}' name='rblMovies' value='{movieId}' />");
                    movieList.Append($"<input type='hidden' id='title_{movieId}' name='title_{movieId}' value='{title}' />");
                    movieList.Append($"<span>{title}</span>");
                    movieList.Append("</label>");
                    movieList.Append("</div>");
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
                // lblMessage.Text = "Please select a movie.";
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