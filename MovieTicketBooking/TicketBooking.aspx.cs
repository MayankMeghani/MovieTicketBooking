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
            int movieScheduleId = Convert.ToInt32(Session["SelectedMovieScheduleId"]);
            int availableSeats = Convert.ToInt32(Session["availableSeats"]);
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;

            int noOfSeats = Convert.ToInt32(tbSeats.Text);
            string fetchQuery = @"SELECT ms.MovieScheduleID, ms.MovieId, s.ScreenName, ms.StartTime, ms.HouseFull FROM MovieSchedules ms
                          JOIN Screens s ON s.ScreenId = ms.ScreenId
                          WHERE MovieScheduleId = @MovieScheduleId";

            string insertQuery = @"INSERT INTO Bookings (MovieId, ScreenName, StartTime, NoOfSeats, Status,UserId)
                           VALUES (@MovieId, @ScreenName, @StartTime, @NoOfSeats, 'Booked',@UserId)";

            string updateQuery = @"UPDATE MovieSchedules
                           SET BookedSeats = BookedSeats + @SeatsToBeBooked,
                               HouseFull = @isHouseFull
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
                        string screenName = reader["ScreenName"].ToString();
                        string startTime = reader["StartTime"].ToString();
                        reader.Close();
                        int userId;
                        if (!int.TryParse(Session["UserId"].ToString().Trim(), out userId))
                        {
                            // Handle the error (e.g., UserId is not a valid integer)
                            lblMessage.Visible = true;
                            lblMessage.Text = "Invalid UserId.";
                            return;
                        }

                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                        {
                            insertCmd.Parameters.AddWithValue("@MovieId", movieId);
                            insertCmd.Parameters.AddWithValue("@ScreenName", screenName);
                            insertCmd.Parameters.AddWithValue("@StartTime", startTime);
                            insertCmd.Parameters.AddWithValue("@NoOfSeats", noOfSeats);
                            insertCmd.Parameters.AddWithValue("@UserId", userId);
                            int rowsAffected = insertCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                bool isHouseFull = (noOfSeats == availableSeats);

                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                                {
                                    updateCmd.Parameters.AddWithValue("@SeatsToBeBooked", noOfSeats);
                                    updateCmd.Parameters.AddWithValue("@isHouseFull", isHouseFull ? 1 : 0); // Set bit value (1 for true, 0 for false)
                                    updateCmd.Parameters.AddWithValue("@MovieScheduleId", movieScheduleId);

                                    int updateRows = updateCmd.ExecuteNonQuery();
                                    lblMessage.Text = updateRows > 0 ? "Booking successful!" : "Booking failed!";
                                    lblMessage.ForeColor = System.Drawing.Color.Green;
                                    lblMessage.Visible = true;
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Booking failed!";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                lblMessage.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Movie schedule not found!";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }
        }

    }
}