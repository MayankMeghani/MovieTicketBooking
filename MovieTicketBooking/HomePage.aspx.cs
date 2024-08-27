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
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome, " + Session["UserName"].ToString();
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
                    rblMovies.Items.Add(item);

                }
                con.Close();
                string role = Session["Role"] != null ? Session["Role"].ToString().Trim() : string.Empty;

                if (role == "admin")
                {
                    pnlAdmin.Visible= true;
                }
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
                string selectedMovieName = rblMovies.SelectedItem.Text;
                string selectedMovieId=rblMovies.SelectedValue;
                Session["MovieName"] = selectedMovieName;
                Session["MovieId"] = selectedMovieId;

                Response.Redirect("MovieSchedule.aspx");
        }

        protected void btn_clk_LogOut(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("LoginPage.aspx");
        }
    }
}