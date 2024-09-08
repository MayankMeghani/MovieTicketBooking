using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
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
            if (!string.IsNullOrEmpty(ddlMovies.SelectedValue))
            {
                try
                {
                    ViewState["selectedMovie"] = ddlMovies.SelectedValue;
                    ViewState["selectedMovieName"] = ddlMovies.SelectedItem.Text;
                    int movieID = Convert.ToInt32(ddlMovies.SelectedValue);

                    using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString))
                    {
                        string query = "SELECT * FROM Movies WHERE MovieId = @movieId";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@movieId", movieID);
                            con.Open();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    tbTitle.Text = reader["Title"].ToString();
                                    tbGenre.Text = reader["Genre"].ToString();
                                    tbDuration.Text = reader["Duration"].ToString();
                                    ViewState["Poster"] = reader["Poster"].ToString();

                                    string filename = reader["Poster"].ToString().Trim();
                                    string posterUrl = ResolveUrl("~/UploadedPhotos/" + filename);
                                    imgPoster.ImageUrl = posterUrl;

                                    
                                    bool isActive = Convert.ToBoolean(reader["IsActive"]);
                                    rblActive.SelectedValue = isActive ? "True" : "False";

                                    DateTime releaseDate = Convert.ToDateTime(reader["ReleaseDate"]);
                                    tbDate.Text = releaseDate.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    lblMessage.Text = "Movie not found.";
                                }
                            }
                        }

                        panelDetails.Visible = true; 
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error fetching movie details: " + ex.Message;
                }
            }
            else
            {
                lblMessage.Text = "Please select a movie.";
            }
        }

        protected void btn_sbm_update(object sender, EventArgs e)
        {

            
            ProcessFileUpload();

            if (ViewState["Error"] != null)
            {
                lblMessage.Text = ViewState["Error"].ToString();
                return;
            }
            string fileName= ViewState["Poster"].ToString();
            string title = tbTitle.Text;
            string genre = tbGenre.Text;
            string duration = tbDuration.Text;
            bool isActive = Convert.ToBoolean(rblActive.SelectedValue);
            string date = tbDate.Text;
            int movieId = Convert.ToInt32(ViewState["selectedMovie"]);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string query = "UPDATE Movies  set Title = @title, Genre = @genre,Duration = @duration,isActive = @isActive, ReleaseDate = @date, Poster = @poster WHERE MovieId=@movieID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@duration", duration);
                cmd.Parameters.AddWithValue("@isActive", isActive);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@MovieId", movieId);
                cmd.Parameters.AddWithValue("@poster", fileName);
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

        protected void btn_clk_preview(object sender, EventArgs e)
        {
                ProcessFileUpload();
        }
        private void ProcessFileUpload()
        {
            if (FileUpload.HasFile)
            {
                if (IsImage(FileUpload.PostedFile))
                {
                    try
                    {

                        string uploadDirectory = Server.MapPath("~/UploadedPhotos/");

                        RemovePreviousImage(uploadDirectory);

                        string movieTitle = tbTitle.Text;

                        string fileExtension = Path.GetExtension(FileUpload.FileName);
                        //string sanitizedTitle = SanitizeFileName(movieTitle);
                        string fileName = movieTitle + fileExtension;

                        string savePath = Path.Combine(uploadDirectory, fileName);

                        FileUpload.SaveAs(savePath);

                        string PreviewUrl = ResolveUrl("~/UploadedPhotos/" + fileName);
                        imgPoster.ImageUrl = PreviewUrl;
                        ViewState["Poster"] = fileName;

                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error displaying file: " + ex.Message;
                    }
                }
                else
                {
                    lblMessage.Text = "Selcet an image file.";
                    ViewState["Error"] = "Selcet an image file.";
                }
            }
            else
            {
                lblMessage.Text = "No file selected(current poster will remain.)";
            }
        }

        private void RemovePreviousImage(string uploadDirectory)
        {

            string previousFilename = ViewState["Poster"].ToString();
            if (!string.IsNullOrEmpty(previousFilename))
            {
                string previousFilePath = Path.Combine(uploadDirectory, previousFilename);
                if (File.Exists(previousFilePath))
                {
                    try
                    {
                        File.Delete(previousFilePath);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error deleting previous file: " + ex.Message);
                    }
                }
            }
        }
        private bool IsImage(HttpPostedFile postedFile)
        {
            string[] validFileTypes = { "image/jpeg", "image/png", "image/gif", "image/jpg", "image/webp" };
            return Array.Exists(validFileTypes, fileType => fileType == postedFile.ContentType);
        }

    }
}