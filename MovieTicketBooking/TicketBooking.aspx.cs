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
    public partial class MovieTicketBooking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rvSeats.MaximumValue = Session["availableSeats"].ToString();
                rvSeats.ErrorMessage = "Only "+Session["availableSeats"].ToString()+" Seats are left";
            }
        }

        protected void btn_sbm_book(object sender, EventArgs e)
        {
            int movieScheduleId = Convert.ToInt32(Session["SelectedMovieScheduleId"]); // or Session["MovieScheduleId"]
            int availableSeats = Convert.ToInt32(Session["availableSeats"]);
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
           
            int noOfSeats = Convert.ToInt32(tbSeats.Text);
            string fetchQuery = @"SELECT ms.MovieScheduleID, ms.MovieId, s.ScreenName, ms.StartTime,ms.HouseFull FROM MovieSchedules ms
            JOIN Screens s ON s.ScreenId=ms.ScreenId
            WHERE MovieScheduleId = @MovieScheduleId";

            string insertQuery = @"INSERT INTO Bookings (MovieId, ScreenName,StartTime,NoOfSeats,Status)
            VALUES (@MovieId, @ScreenName,@StartTime,@NoOfSeats,'Booked')";

            string updateQuery = @"UPDATE MovieSchedules
            SET BookedSeats = BookedSeats + @SeatsTobeBook,
            HouseFull = @isHousefull
            WHERE MovieScheduleId = @MovieScheduleId;";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand fetchCmd = new SqlCommand(fetchQuery, con))
                {
                    fetchCmd.Parameters.AddWithValue("@MovieScheduleId", movieScheduleId);
                    con.Open();
                    SqlDataReader reader = fetchCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int movieId = Convert.ToInt32(reader["MovieId"]);
                        int screenName = Convert.ToInt32(reader["ScreenName"]);
                        string startTime = reader["StartTime"].ToString();
                        reader.Close(); // Close the reader before executing the insert command

                        // Insert into Bookings table
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                        {
                            insertCmd.Parameters.AddWithValue("@MovieId", movieId);
                            insertCmd.Parameters.AddWithValue("@ScreenName", screenName);
                            insertCmd.Parameters.AddWithValue("@StartTime", startTime);
                            insertCmd.Parameters.AddWithValue("@NoofSeats", noOfSeats);
                            //insertCmd.Parameters.AddWithValue("@BookingDate", DateTime.Now);

                            int rowsAffected = insertCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {

                                string isHouseFull = (noOfSeats==availableSeats)?"true":"false";
                                using (SqlCommand updateCmd=new SqlCommand(updateQuery, con))
                                {
                                    updateCmd.Parameters.AddWithValue("@SeatsTobeBook",noOfSeats);
                                    updateCmd.Parameters.AddWithValue("@HouseFull", isHouseFull);
                                    updateCmd.Parameters.AddWithValue("@MovieScheduleId",movieScheduleId);

                                    int row = updateCmd.ExecuteNonQuery();
                                    lblMessage.Text = rowsAffected > 0 ? "Booking successful!" : "Booking failed!";
                                    lblMessage.Visible = true;
                                }
                            }

                            lblMessage.Text = rowsAffected > 0 ? "Booking successful!" : "Booking failed!";
                            lblMessage.Visible = true;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Movie schedule not found!";
                        lblMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        protected void tbSeats_TextChanged(object sender, EventArgs e)
        {

        }
    }
}