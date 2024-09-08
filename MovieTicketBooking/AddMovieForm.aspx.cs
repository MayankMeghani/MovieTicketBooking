using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
            if (FileUpload.HasFile && IsImage(FileUpload.PostedFile))
            {
                try
                {
                    string uploadFolder = Server.MapPath("~/UploadedPhotos/");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileName = Path.GetFileName(FileUpload.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);
                    FileUpload.SaveAs(filePath);

                    string title = tbTitle.Text;
                    string genre = tbGenre.Text;
                    string duration = tbDuration.Text;
                    string date = tbDate.Text;

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
                    string query = "INSERT INTO Movies (Title,Genre,Duration,ReleaseDate,Poster) Values (@title,@genre,@duration,@date,@Poster)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@genre", genre);
                        cmd.Parameters.AddWithValue("@duration", duration);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@poster", fileName);
                        con.Open();
                        int res = cmd.ExecuteNonQuery();
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
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                lblMessage.Text = "Please upload a valid image file.";
            }

        }
        private bool IsImage(HttpPostedFile postedFile)
        {
            string[] validFileTypes = { "image/jpeg", "image/png", "image/gif", "image/jpg" , "image/webp" };
            return Array.Exists(validFileTypes, fileType => fileType == postedFile.ContentType);
        }
    }
}