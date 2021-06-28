using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AD_RH
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownList1.Visible = false;
            Label1.Text = "";
            GridView1.Visible = false;
            Label3.Visible = false;
            Button5.Visible = false;
            textbox3.Visible = false;
            cleangrid1();
        }

        public void cleangrid1()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();
            dt.Rows.Add(dt.NewRow());
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        protected void Button5_Click(object sender, EventArgs e)
        {
            textbox3.Visible = true;
            Label3.Visible = true;
            Button5.Visible = true;
            GridView1.Visible = true;
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["AVA"]);
            String sql = " select [from] from DirectoryRelations where [to] = '" + textbox3.Text + "' and name = 'Profiles'";
            List<ADO> mail = new List<ADO>();
            ADO ado;
            List<ADO> list = new List<ADO>();
            Label1.Text = "Profils de " + textbox3.Text;
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Role", typeof(string));
                }
                DataRow NewRow;
            while (reader.Read())
            {
                NewRow = dt.NewRow();
                NewRow[0] = reader[0].ToString();
                dt.Rows.Add(NewRow);
            }
            connection.Close();
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            textbox3.Visible = true;
            Label3.Visible = true;
            Button5.Visible = true;
        } // recherche des profil d'un utilisateur précis

        protected void Button2_Click(object sender, EventArgs e)
        {
            GridView1.Visible = true;
            Label1.Visible = false;
            String sql = "select [from],[to] from DirectoryRelations inner join DirectoryResourceLocks on locked=1 and [resource] = [to] where  name = 'Profiles' order by [to]";
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["AVA"]);
            List<ADO> mail = new List<ADO>();
            ADO ado;
            List<ADO> list = new List<ADO>();
            Label1.Text = "Profils de " + textbox3.Text;
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Utilisateur", typeof(string));
                dt.Columns.Add("Role", typeof(string));
            }
            DataRow NewRow;
            while (reader.Read())
            {
                NewRow = dt.NewRow();
                NewRow[0] = reader[1].ToString();
                NewRow[1] = reader[0].ToString();
                dt.Rows.Add(NewRow);
            }
            connection.Close();
            GridView1.DataSource = dt;
            GridView1.DataBind();
        } // liste les utilisateur actif ayant un profil

        protected void Button3_Click(object sender, EventArgs e)
        {
            Label1.Visible = true;
            DropDownList1.Visible = true;
            DropDownList1.Items.Clear();
            String sql = "select [from] from DirectoryRelations where  name = 'Profiles' group by [from]  order by [from]";
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["AVA"]);
            connection.Open();
            DropDownList1.Items.Add("--Choix du profil--");
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DropDownList1.Items.Add(reader[0].ToString());
            }

            connection.Close();
        } // recherche par profil

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList1.Visible = true;
            Label1.Text = "Profil : " + DropDownList1.SelectedValue;
            Label1.Visible = true;
            String selecter_one = DropDownList1.SelectedValue;
            String sql = "select [to] from DirectoryRelations where [from] = '" + selecter_one + "' and name = 'Profiles' order by [to]";
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["AVA"]);
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Utilisateur", typeof(string));
            }
            DataRow NewRow;
            int i = 0;
            while (reader.Read())
            {
                NewRow = dt.NewRow();
                NewRow[0] = reader[0].ToString();
                dt.Rows.Add(NewRow);
            }
            connection.Close();
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Visible = true;
        }
    }
}