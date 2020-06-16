using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sprint_Project
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> values = new List<string>() { username.Text, password.Text };
                List<string> sqlparam = new List<string>() { "@username", "@password" };
                List<SqlDbType> datatypes = new List<SqlDbType>() { SqlDbType.VarChar, SqlDbType.VarChar };
                Database db = new Database();
                SqlDataReader dr = db.SelectQueries(sqlparam, "LoginCheck", values, datatypes);
                if (dr.Read())
                {
                   // MessageBox.Show("Login Successful");
                    Dashboard hp = new Dashboard(username.Text);
                    hp.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid User Info\nPlease Try again");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
