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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            Font f = new Font("Times New Roman", 9);
            DataGridViewCellStyle cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.DefaultCellStyle = cs;
            f = new Font("Times New Roman", 9, FontStyle.Bold);
            cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.ColumnHeadersDefaultCellStyle = cs;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            loadDataForManaging();
        }
        public static string uname;
        public Dashboard(string name)
        {
            InitializeComponent();
            label1.Text = name;
            uname = name;
            Font f = new Font("Times New Roman", 9);
            DataGridViewCellStyle cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.DefaultCellStyle = cs;
            f = new Font("Times New Roman", 9, FontStyle.Bold);
            cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.ColumnHeadersDefaultCellStyle = cs;
            loadDataForManaging();
        }
        List<string> values = new List<string>();
        List<string> sqlparam = new List<string>();
        List<SqlDbType> datatypes = new List<SqlDbType>();
        void loadDataForManaging()
        {
            try
            {
                //loading in dropdown

                dataGridView1.Rows.Clear();
                comboteam.Items.Clear();
                comboproject.Items.Clear();
                comboteamid.Items.Clear();
                values.Clear(); sqlparam.Clear(); datatypes.Clear();
                values = new List<string>() { uname };
                sqlparam = new List<string>() { "@username" };
                datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
                Database db = new Database();
                SqlDataReader dr = db.SelectQueries(sqlparam, "ListTeams", values, datatypes);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        comboteam.Items.Add(dr[0].ToString());
                        comboteamid.Items.Add(dr[1].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ManagingTeams teams = new ManagingTeams();
            teams.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ManagingProjects p = new ManagingProjects();
            p.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ManagingTasks t = new ManagingTasks();
            t.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //loading Project
            values.Clear(); sqlparam.Clear(); datatypes.Clear(); 
            values = new List<string>() { uname };
            sqlparam = new List<string>() { "@username" };
            datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
            Database db = new Database();
            SqlDataReader dr = db.SelectQueries(sqlparam, "LoadProjecData", values, datatypes);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr[5].ToString() == comboteamid.Items[comboteam.SelectedIndex].ToString() && dr[6].ToString()==comboprojectid.Items[comboproject.SelectedIndex].ToString())
                    {
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = 100;
                        progressBar1.Value = Convert.ToInt32(dr[3]);
                        label2.Text = Convert.ToInt32(dr[3]).ToString()+" %";
                    }
                }
            }
            else
            {
                MessageBox.Show("Error ");
            }
            //load task info
            values = new List<string>() { uname, comboprojectid.Items[comboproject.SelectedIndex].ToString() };
            sqlparam = new List<string>() { "@username","@pid" };
            datatypes = new List<SqlDbType>() { SqlDbType.VarChar,SqlDbType.Int };
            dr = db.SelectQueries(sqlparam, "LoadTaskData", values, datatypes);
            if (dr.HasRows)
            {
                int i = 0;
                while (dr.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[i].Clone();
                    row.Cells[0].Value = dr[0].ToString();
                    row.Cells[1].Value = dr[1].ToString();
                    row.Cells[2].Value = dr[2].ToString();
                    row.Cells[3].Value = dr[3].ToString();
                    row.Cells[4].Value = dr[4].ToString();

                    dataGridView1.Rows.Add(row);
                }
            }
            else
            {
                MessageBox.Show("You Don't Have any Tasks in This Project");
            }
        }

        private void comboteam_SelectedIndexChanged(object sender, EventArgs e)
        {
            //loading Project
            values.Clear(); sqlparam.Clear(); datatypes.Clear(); comboproject.Items.Clear(); comboprojectid.Items.Clear();
            values = new List<string>() { uname };
            sqlparam = new List<string>() { "@username" };
            datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
            Database db = new Database();
            SqlDataReader dr = db.SelectQueries(sqlparam, "LoadProjecData", values, datatypes);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr[5].ToString() == comboteamid.Items[comboteam.SelectedIndex].ToString())
                    {
                        comboproject.Items.Add(dr[1].ToString());
                        comboprojectid.Items.Add(dr[6].ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("You Don't Have any Projects in this Team");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
