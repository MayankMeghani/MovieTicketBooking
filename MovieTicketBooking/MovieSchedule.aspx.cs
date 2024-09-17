using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketBooking
{
    public partial class Schedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["MovieName"] != null && Session["MovieId"] != null)
                {
                    string MovieName = Session["MovieName"].ToString();
                    int MovieId = Convert.ToInt32(Session["MovieId"]);
                    lblTitle.Text = $"Schedule for Movie: {MovieName}";
                    BindMovieSchedule(MovieId);
                }
                else
                {
                    lblTitle.Text = "Please Select a movie first";
                }
            }
        }

        private void BindMovieSchedule(int movieId)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string query = "SELECT MovieScheduleId, ScreenId, StartTime, HouseFull FROM MovieSchedules WHERE MovieId = @MovieId";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MovieId", movieId);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)  // Check if any rows are returned
                    {
                        gdvMovieSchedule.DataSource = reader;
                        gdvMovieSchedule.DataBind();
                        lblMessage.Visible = false;  // Hide the message label if data is found
                    }
                    else
                    {
                        // If no rows are found, show the message
                        gdvMovieSchedule.DataSource = null;
                        gdvMovieSchedule.DataBind();
                        lblMessage.Text = "Movie is yet not scheduled.";
                        lblMessage.Visible = true;  // Show the message label
                    }
                }
                con.Close();
            }
        }
        

        protected void btnClick(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "checkAvailability")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gdvMovieSchedule.Rows[rowIndex];
                int movieScheduleId = Convert.ToInt32(row.Cells[0].Text);
                bool IsHouseFull = Convert.ToBoolean(gdvMovieSchedule.DataKeys[rowIndex]["HouseFull"]);

                if (!IsHouseFull)
                {

                    string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
                    string query = @"
                    SELECT ms.BookedSeats, s.ScreenName AS ScreenName, s.Capacity 
                    FROM MovieSchedules ms
                    JOIN Screens s ON ms.ScreenId = s.ScreenId
                    WHERE ms.MovieScheduleId = @MovieScheduleId";

                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@MovieScheduleId", movieScheduleId);
                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int bookedSeats = Convert.ToInt32(reader["BookedSeats"]);
                                    string screenName = reader["ScreenName"].ToString();
                                    int capacity = Convert.ToInt32(reader["Capacity"]);
                                    int availableSeats = capacity - bookedSeats;
                                    Session["availableSeats"] = availableSeats;
                                    lblAvailability.Text = $"Screen Type: {screenName}, Capacity: {capacity}, Available seats: {availableSeats}";
                                    PnlBook.Visible = true;
                                    btnBook.Visible = true;
                                    Session["SelectedMovieScheduleId"] = movieScheduleId;

                                }
                                else
                                {
                                    lblAvailability.Text = "No data found for this schedule.";
                                    PnlBook.Visible = false;

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblAvailability.Text = "An error occurred: " + ex.Message;
                        PnlBook.Visible = false;

                    }
                }
                else
                {
                    lblAvailability.Text = "Show is HouseFull";
                    PnlBook.Visible = true;
                    btnBook.Visible = false;
                }
            }
        }

        protected void Btb_Book(object sender, EventArgs e)
        {
            if (Session["SelectedMovieScheduleId"] == null)
            {
                lblAvailability.Text= "first check availability for booking";
            }
            else
            {
                Response.Redirect("TicketBooking.aspx");
            }
        }

        
    }
}