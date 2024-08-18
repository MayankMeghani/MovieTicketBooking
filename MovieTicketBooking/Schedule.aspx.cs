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
    public partial class Schedule1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindScheduleGrid();
            }
        }
        private void BindScheduleGrid()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string query = @"SELECT ms.MovieScheduleID, m.Title, m.Duration, s.ScreenName, s.Capacity,  ms.StartTime, ms.BookedSeats, ms.HouseFull FROM MovieSchedules ms 
            JOIN  Movies m ON ms.MovieId=m.MovieId
            JOIN Screens s ON ms.ScreenId=s.ScreenId";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                gdvSchedule.DataSource = cmd.ExecuteReader();
                gdvSchedule.DataBind();
                con.Close();
            }

        }
        protected void gdvSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}