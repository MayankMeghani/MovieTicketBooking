using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketBooking
{
    public partial class AddSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;

                string movieQuery = @"SELECT * FROM Movies";
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(movieQuery, con))
                {
                    con.Open();
                    SqlDataReader movieReader = cmd.ExecuteReader();
                    while (movieReader.Read())
                    {
                        ListItem item = new ListItem
                        {
                            Text = movieReader["Title"].ToString(),
                            Value = movieReader["MovieId"].ToString()
                        };
                        ddlMovies.Items.Add(item);
                    }
                    con.Close();
                }

                // Populate Screens Dropdown
                string screenQuery = @"SELECT * FROM Screens";
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(screenQuery, con))
                {
                    con.Open();
                    SqlDataReader screenReader = cmd.ExecuteReader();
                    while (screenReader.Read())
                    {
                        ListItem item = new ListItem
                        {
                            Text = screenReader["ScreenName"].ToString(),
                            Value = screenReader["ScreenId"].ToString()
                        };
                        ddlScreens.Items.Add(item);
                    }
                    con.Close();
                }
            }
        }

        protected void btn_clk_add(object sender, EventArgs e)
        {
            int movieId = Convert.ToInt32(ddlMovies.SelectedValue);
            int screenId = Convert.ToInt32(ddlScreens.SelectedValue);
            string startTime = tbTime.Text;

            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;

            string selectQuery = @"SELECT * FROM MovieSchedules WHERE ScreenId=@screenId AND StartTime=@startTime";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(selectQuery, con))
            {
                cmd.Parameters.AddWithValue("@screenId", screenId);
                cmd.Parameters.AddWithValue("@startTime", startTime);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    reader.Close();

                    string insertQuery = @"INSERT INTO MovieSchedules (MovieId, ScreenId, StartTime) VALUES (@movieId, @screenId, @startTime)";
                    cmd.CommandText = insertQuery;

                    cmd.Parameters.AddWithValue("@movieId", movieId);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMessage.Text = "Movie schedule added successfully.";
                    }
                    else
                    {
                        lblMessage.Text = "Error occurred while adding the schedule.";
                    }
                }
                else
                {
                    lblMessage.Text = "A schedule for the same screen and time already exists.";
                }
                con.Close();
            }
        }

    }
}
