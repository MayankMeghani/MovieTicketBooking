using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketBooking
{
    public partial class Schedule1 : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;

        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDBContext"].ConnectionString;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;

            if (!IsPostBack)
            {
                BindScheduleGrid();
                string role = Session["Role"] != null ? Session["Role"].ToString().Trim() : string.Empty;
                if(role == "admin")
                {
                    hlAddSchedule.Visible = true;
                    gdvSchedule.Columns[0].Visible = true;
                }
            }
        }

        private void BindScheduleGrid()
        {
            string query = @"
            SELECT ms.MovieScheduleID, m.Title, m.Duration, s.ScreenId, s.ScreenName, s.Capacity, ms.StartTime, ms.BookedSeats, ms.HouseFull 
            FROM MovieSchedules ms 
            JOIN Movies m ON ms.MovieId = m.MovieId
            JOIN Screens s ON ms.ScreenId = s.ScreenId";

            command.CommandText = query;
            connection.Open();
            gdvSchedule.DataSource = command.ExecuteReader();
            gdvSchedule.DataBind();
            connection.Close();
        }

        protected void gdvSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the MovieScheduleID from DataKeys collection
            int movieScheduleId = Convert.ToInt32(gdvSchedule.DataKeys[e.RowIndex].Value);

            string query = @"DELETE FROM MovieSchedules WHERE MovieScheduleID = @MovieScheduleId";
            command.CommandText = query;

            // Clear any existing parameters
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@MovieScheduleId", movieScheduleId);

            try
            {
                connection.Open();
                int res = command.ExecuteNonQuery();
                connection.Close();

                if (res == 0)
                {
                    lblMessage.Text = "Error occurred while deleting the record.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                }
                else
                {
                    gdvSchedule.EditIndex = -1;
                    BindScheduleGrid();
                    lblMessage.Text = $"Schedule with ID {movieScheduleId} was deleted successfully.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"An error occurred: {ex.Message}";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }
            finally
            {
            }
        }

        protected void gdvSchedule_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdvSchedule.EditIndex = e.NewEditIndex;
            BindScheduleGrid();
        }

        protected void gdvSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int MovieScheduleId = Convert.ToInt32(gdvSchedule.DataKeys[e.RowIndex]["MovieScheduleId"]);
            String StartTime = e.NewValues["StartTime"].ToString();
            int ScreenId = Convert.ToInt32(gdvSchedule.DataKeys[e.RowIndex]["ScreenId"]);


            string selectQuery = @"SELECT * FROM MovieSchedules WHERE ScreenId=@ScreenId AND StartTime=@StartTime";
            command.CommandText= selectQuery;
            command.Parameters.AddWithValue("@ScreenId", ScreenId);
            command.Parameters.AddWithValue("@StartTime", StartTime);
            lblMessage.Text = ScreenId + " " + StartTime;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                reader.Close();
                String Query = @"UPDATE MovieSchedules SET StartTime= @StartTime WHERE MovieScheduleId=@MovieScheduleId";
                command.CommandText = Query;
                command.Parameters.AddWithValue("@MovieScheduleId", MovieScheduleId);
                int res = command.ExecuteNonQuery();

                connection.Close();
                if (res == 0)
                {
                    lblMessage.Text = "Unable update schedule";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                }
                else
                {
                    gdvSchedule.EditIndex = -1;
                    BindScheduleGrid();
                    lblMessage.Text = "Schedule Updated SucessFully";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;
                }
            }
            else
            {

                connection.Close();
                lblMessage.Text = "Schedule With same screen and Start Time is present";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }




        }

        protected void gdvSchedule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gdvSchedule.EditIndex = -1;
            BindScheduleGrid();
        }

    }
}
