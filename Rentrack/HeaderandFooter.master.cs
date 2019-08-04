﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class HeaderandFooter : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user_id"] == null)
        {
            signuplink.Visible = true;
            headeraddproperty.Visible = false;
            headerfirstname.Visible = false;
            userproflink.Visible = false;
            dblink.Visible = false;
            headerline.Visible = false;
            notifdropdown.Visible = false;
        }
        else if (Session["user_id"] != null)
        {
            BindNotifications();

            //Get First Name
            string huserid = Session["user_id"].ToString();
            string firstName = "";

            String CS = ConfigurationManager.ConnectionStrings["RentrackdbConnectionString"].ConnectionString;
            using (SqlConnection headcon = new SqlConnection(CS))
            {
                headcon.Open();
                SqlCommand getfname = new SqlCommand("SELECT f_name FROM [User] WHERE user_id = '" + huserid + "'", headcon);
                SqlDataReader reader = getfname.ExecuteReader();
                while (reader.Read())
                {
                    firstName = reader[0].ToString();
                }
                reader.Close();
            }
            headerfirstname.Text = firstName;

            signuplink.Visible = false;
            loginlink.Text = "Logout";
            headeraddproperty.Visible = true;
            usericon.Visible = false;
            userproflink.Visible = true;
            headerline.Visible = true;
            headerfaq.Visible = false;
            notifdropdown.Visible = true;
            dblink.Visible = true;

        }

    }

    private void BindNotifications()
    {
        string userid = Session["user_id"].ToString();
        String CS = ConfigurationManager.ConnectionStrings["RentrackdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.[Notification] WHERE receiver_id ='" + userid + "'", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dtnotif = new DataTable();
                    sda.Fill(dtnotif);
                    rptrnotif.DataSource = dtnotif;
                    rptrnotif.DataBind();
                }
            }
        }
    }

    protected void Loginlinkbutton(object sender, EventArgs e)
    {
        if (Session["user_id"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else if (Session["user_id"] != null)
        {
            Session["user_id"] = null;
            Response.Redirect("~/Default.aspx");
        }
    }

    protected void Signuplinkbutton(object sender, EventArgs e)
    {

        Response.Redirect("~/Sign Up.aspx");

    }

    protected void headeraddproperty_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AddProperty.aspx");
    }

    protected void UserProflinkbutton(object sender, EventArgs e)
    {
        Response.Redirect("~/User Profile.aspx");
    }
    protected void Dashboardlinkbutton(object sender, EventArgs e)
    {
        Response.Redirect("~/Dashboard.aspx");
    }
}
