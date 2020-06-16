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
    public partial class ManagingTasks : Form
    {
        public ManagingTasks()
        {
            InitializeComponent();
            uname.Text = Dashboard.uname;

            Font f = new Font("Times New Roman", 9);
            DataGridViewCellStyle cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.DefaultCellStyle = cs;
            f = new Font("Times New Roman", 9, FontStyle.Bold);
            cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.ColumnHeadersDefaultCellStyle = cs;
            loadDataForManaging();
            loadUserTasks();
        }
        List<string> values = new List<string>();
        List<string> sqlparam = new List<string>();
        List<SqlDbType> datatypes = new List<SqlDbType>();
        void loadUserTasks()
        {
            values.Clear(); sqlparam.Clear(); datatypes.Clear();
            dataGridView1.Rows.Clear();
            values = new List<string>() { uname.Text };
            sqlparam = new List<string>() { "@username" };
            datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
            Database db = new Database();
            SqlDataReader dr = db.SelectQueries(sqlparam, "ListUserTasks", values, datatypes);
            if(dr.HasRows)
            {
                int i = 0;
                while(dr.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[i].Clone();
                    row.Cells[0].Value = dr[0].ToString();
                    row.Cells[1].Value = dr[1].ToString();
                    row.Cells[2].Value = dr[2].ToString();
                    row.Cells[3].Value = dr[3].ToString();

                    row.Cells[4].Value = dr[4].ToString();
                    row.Cells[5].Value = dr[5].ToString();
                    row.Cells[6].Value = "Update";
                    row.Cells[7].Value = dr[6].ToString();
                    dataGridView1.Rows.Add(row);
                }
            }
        }
        void loadDataForManaging()
        {
            try
            {
                //loading in dropdown

                dataGridView1.Rows.Clear();
                comboteam.Items.Clear();
                comboproject.Items.Clear(); 
                comboteamid.Items.Clear();
                combomember.Items.Clear();
                values.Clear(); sqlparam.Clear(); datatypes.Clear();
                values = new List<string>() { uname.Text };
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
        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Restart();
        }

        private void ManagingTasks_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Dashboard d = new Dashboard(uname.Text);
            d.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ManagingTeams t = new ManagingTeams();
            t.Show();
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

        private void ManagingTasks_Load(object sender, EventArgs e)
        {

        }

        private void comboteamid_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboteam_SelectedIndexChanged(object sender, EventArgs e)
        {
            //loading TeamMembers
            values.Clear(); sqlparam.Clear(); datatypes.Clear();combomember.Items.Clear(); combomemberid.Items.Clear();
            values = new List<string>() { comboteamid.Items[comboteam.SelectedIndex].ToString() };
            sqlparam = new List<string>() { "@tid" };
            datatypes = new List<SqlDbType>() { SqlDbType.Int};
            Database db = new Database();
            SqlDataReader dr = db.SelectQueries(sqlparam, "LoadUsersOfATeam", values, datatypes);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    combomember.Items.Add(dr[0].ToString());
                    combomemberid.Items.Add(dr[1].ToString());
                }
            }
            //loading Project
            values.Clear(); sqlparam.Clear(); datatypes.Clear();comboproject.Items.Clear(); comboprojectid.Items.Clear();
            values = new List<string>() { uname.Text };
            sqlparam = new List<string>() { "@username" };
            datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
            db = new Database();
            dr = db.SelectQueries(sqlparam, "LoadProjecData", values, datatypes);
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

        private void createTeam_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txt_taskName.Text) || !isNumeric(hoursRequired.Text, System.Globalization.NumberStyles.Integer))
            {
                MessageBox.Show("Please Enter Valid Details");
                return;
            }
            if(comboteam.SelectedIndex==-1 || comboproject.SelectedIndex==-1 || combomember.SelectedIndex==-1)
            {
                MessageBox.Show("Please Make Sure You have selected Team, Project and Member ");
                return;
            }
            try
            {
                values.Clear(); sqlparam.Clear(); datatypes.Clear();
                values.Add(comboteamid.Items[comboteam.SelectedIndex].ToString());
                values.Add(comboprojectid.Items[comboproject.SelectedIndex].ToString());
                values.Add(combomemberid.Items[combomember.SelectedIndex].ToString());
                values.Add(txt_taskName.Text);
                values.Add(hoursRequired.Text);
                values.Add(uname.Text);
                sqlparam.Add("@tid");
                sqlparam.Add("@pid");
                sqlparam.Add("@mid");
                sqlparam.Add("@taskname");
                sqlparam.Add("@time");
                sqlparam.Add("@username");
                datatypes.Add(SqlDbType.Int);
                datatypes.Add(SqlDbType.Int);
                datatypes.Add(SqlDbType.Int);
                datatypes.Add(SqlDbType.VarChar);
                datatypes.Add(SqlDbType.VarChar);
                datatypes.Add(SqlDbType.VarChar);
                Database db = new Database();
                if(db.InsertionProcedures(sqlparam, values, datatypes, "CreateTast"))
                {
                    MessageBox.Show("Task Created and Assigned Successfully");
                    loadUserTasks();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : "+ex);
                return;
            }
        }
        public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            try
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != dataGridView1.NewRowIndex && e.ColumnIndex == 6)
                {
                    values = new List<string>() { dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString(), uname.Text, dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() };
                    sqlparam = new List<string>() { "@idd", "@username","@time" };
                    datatypes = new List<SqlDbType>() { SqlDbType.Int, SqlDbType.VarChar,SqlDbType.Int };

                    Database db = new Database();
                    if(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString())> int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()))
                    {
                        MessageBox.Show("Please Enter Valid Spent Hours Values","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                    SqlDataReader dr = db.SelectQueries(sqlparam, "UpdateTaskTime", values, datatypes);
                    while (dr.Read())
                    {
                        values = new List<string>() { dr[0].ToString() };
                        sqlparam = new List<string>() { "@idd"};
                        datatypes = new List<SqlDbType>() { SqlDbType.Int};
                        if (db.InsertionProcedures(sqlparam, values, datatypes, "UpdateProjectCompletion"))
                        {
                            MessageBox.Show("Task Time Updated Successfully");
                            loadUserTasks();
                        }

                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
