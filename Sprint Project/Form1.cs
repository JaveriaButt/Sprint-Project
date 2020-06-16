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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login l = new Login();
            l.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> values = new List<string>() { username.Text,serial.Text };
            List<string> sqlparam = new List<string>() { "@username","@email" };
            List<SqlDbType> datatypes = new List<SqlDbType>() { SqlDbType.VarChar,SqlDbType.VarChar };
            //lets define serial key.. p1)o2(i3*u4&y5^
            if (username.Text == "" || password.Text == "" || serial.Text == "")
            {
                MessageBox.Show("Invalid Credentials..! Can't Create Account");
            }
            else
            {
                Database db = new Database();

                SqlDataReader dr = db.SelectQueries(sqlparam, "CheckUser", values, datatypes);
                if (dr.Read())
                {
                    //if username doesnot exist then register user
                    if (dr[0].ToString() == "0")
                    {
                        values.Add(password.Text);
                        sqlparam.Add("@password");
                        datatypes.Add(SqlDbType.VarChar);
                        bool x = db.InsertionProcedures(sqlparam, values, datatypes, "RegisterUser");
                        if (x)
                        {
                            MessageBox.Show("Account Created Successfully");
                            //closing this form and loading homepage
                            Dashboard hp = new Dashboard(username.Text);
                            hp.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Error in Creating Account \n Please Try again");
                        }
                    }
                    //if user name already exist then give error and don't register user
                    else
                    {
                        MessageBox.Show("User Name already Exists");
                        return;
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
