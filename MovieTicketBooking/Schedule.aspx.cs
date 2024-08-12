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
    public partial class Schedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string MovieName = Session["MovieName"].ToString();
            int MovieId = int.Parse(Session["MovieId"].ToString());
            lblTitle.Text =  string.Format(@"Schedule for Movie: {0}",MovieName);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MovieDbContext"].ConnectionString;
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = $"Select ShowTimeId,ScreenId,StartTime from ShowTimes where MovieId = {MovieId}";
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            gdvSchedule.DataSource = reader;
            gdvSchedule.DataBind();

        }

        protected void gdvSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnClick(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "checkAvailability")
            {
                lblTitle.Text = "available";
            }

        }
    }
}