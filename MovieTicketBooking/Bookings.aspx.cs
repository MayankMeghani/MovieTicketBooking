using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketBooking
{
    public partial class Bookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string role = Session["Role"] != null ? Session["Role"].ToString().Trim() : string.Empty;
                BindBookingsGrid(role);


            }

        }

        private void BindBookingsGrid(string role)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;

            string query;
            if (role == "admin")
            {
                query = @"
                SELECT b.BookingId,u.Name As UserName, m.Title, ScreenName , b.StartTime,b.NoOfSeats,b.Status
                FROM Bookings b
                JOIN Movies m ON b.MovieId = m.MovieId
                JOIN Users u ON u.Id = b.UserId";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    gdvBookings.DataSource = cmd.ExecuteReader();
                    gdvBookings.DataBind();
                    con.Close();
                    BindScreensDropDown();
                }
            }
            else
            {
                query = @"
                SELECT b.BookingId, m.Title, ScreenName , b.StartTime,b.NoOfSeats,b.Status
                FROM Bookings b
                JOIN Movies m ON b.MovieId = m.MovieId WHERE b.UserId=@UserId";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(Session["UserID"]));
                    con.Open();
                    gdvBookings.DataSource = cmd.ExecuteReader();
                    gdvBookings.DataBind();
                    con.Close();
                }
            }


        }
        private void BindScreensDropDown()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string screenQuery = "SELECT * FROM SCREENS";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand screencmd = new SqlCommand(screenQuery, con);
            con.Open();
            SqlDataReader screenReader = screencmd.ExecuteReader();
            while (screenReader.Read())
            {
                ListItem item = new ListItem();
                item.Text = screenReader["screenName"].ToString();
                item.Value = screenReader["ScreenName"].ToString();
                ddlScreen.Items.Add(item);
            }
            con.Close();

        }


        protected void btn_clk_update(object sender, EventArgs e)
        {

            int bookingId = Convert.ToInt32(ViewState["bookingId"]);
            string screenName = ddlScreen.SelectedValue;
            string startTime = tbTime.Text;
            string status = rblStatus.SelectedValue;

            string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            string Query = "UPDATE BOOKINGS SET ScreenName=@screenName, StartTime=@startTime, Status =@status where BookingId=@bookingId ";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(Query, con))
            {
                cmd.Parameters.AddWithValue("@screenName", screenName);
                cmd.Parameters.AddWithValue("@startTime", startTime);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@bookingId", bookingId);
                con.Open();
                int res = cmd.ExecuteNonQuery();
                panelModify.Visible = false;
                if (res > 0)
                {
                    lblMessage.Text = "Booking updated";
                }
                else
                {
                    lblMessage.Text = "Error occured";
                }
            }
        }

        protected void btn_clk(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "modify")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gdvBookings.Rows[rowIndex];
                int bookingId = Convert.ToInt32(row.Cells[1].Text);
                ViewState["bookingId"] = bookingId;
                string screenName = row.Cells[3].Text;

                string connectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
                string query = @"
                SELECT b.BookingId, m.Title, ScreenName, b.StartTime,b.NoOfSeats,b.Status
                FROM Bookings b
                JOIN Movies m ON b.MovieId = m.MovieId
                WHERE b.BookingId = @bookingId";
                SqlConnection con = new SqlConnection(connectionString);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@bookingId", bookingId);
                    con.Open();
                    SqlDataReader readear = cmd.ExecuteReader();
                    if (readear.Read())
                    {
                        tbTime.Text = readear["StartTime"].ToString();
                    }
                    con.Close();

                }

                lblTxt.Text = "Updating Booking with id: " + bookingId.ToString();


                panelModify.Visible = true;
            }
        }


        protected void btn_clk_add(object sender, EventArgs e)
        {

        }

        protected void gdvBookings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string role = Session["Role"] != null ? Session["Role"].ToString().Trim() : string.Empty;

                bool userIsAdmin = role == "admin";

                if (!userIsAdmin)
                {
                    e.Row.Cells[0].Visible = false;
                }
            }
        }

    }
}