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
    public partial class ManagingProjects : Form
    {
        public ManagingProjects()
        {
            InitializeComponent(); 
            Font f = new Font("Times New Roman", 9);
            DataGridViewCellStyle cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.DefaultCellStyle = cs;
            f = new Font("Times New Roman", 9, FontStyle.Bold);
            cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.ColumnHeadersDefaultCellStyle = cs;
            uname.Text = Dashboard.uname;
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
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                values.Clear(); sqlparam.Clear(); datatypes.Clear();
                values = new List<string>() { uname.Text };
                sqlparam = new List<string>() { "@username" };
                datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
                Database db = new Database();
                SqlDataReader dr = db.SelectQueries(sqlparam, "ListTeams", values, datatypes);
                if(dr.HasRows)
                {
                    while (dr.Read())
                    {
                        comboBox1.Items.Add(dr[0].ToString());
                        comboBox2.Items.Add(dr[1].ToString());
                    }
                }
                //loading in grid
                dataGridView1.Rows.Clear();
              //  comboBox1.Items.Clear();
                //comboBox2.Items.Clear();
                values.Clear(); sqlparam.Clear(); datatypes.Clear();

                values = new List<string>() { uname.Text };
                sqlparam = new List<string>() { "@username" };
                datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
                dr = db.SelectQueries(sqlparam, "LoadProjecData", values, datatypes);
                if (dr.HasRows)
                {
                    int i = 0;
                    while (dr.Read())
                    {
                        UpdateProject(Convert.ToInt32(dr[6]));
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[i].Clone();
                        row.Cells[0].Value = dr[0].ToString();
                        row.Cells[1].Value = dr[1].ToString();
                        row.Cells[2].Value = dr[2].ToString();
                        row.Cells[3].Value = dr[3].ToString();

                        row.Cells[4].Value = dr[4].ToString();
                        row.Cells[5].Value = "Update";
                        row.Cells[6].Value = "Delete";
                        row.Cells[7].Value = dr[6].ToString();
                        
                         dataGridView1.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
        }
        void UpdateProject(int v)
        {

            values = new List<string>() { v.ToString() };
            sqlparam = new List<string>() { "@idd" };
            datatypes = new List<SqlDbType>() { SqlDbType.Int };
            Database db = new Database();
            db.InsertionProcedures(sqlparam, values, datatypes, "UpdateProjectCompletion");
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            try
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != dataGridView1.NewRowIndex && e.ColumnIndex == 6)
                {
                    List<string> values = new List<string>() { dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString(),uname.Text };
                    List<string> sqlparam = new List<string>() { "@idd","@username" };
                    List<SqlDbType> datatypes = new List<SqlDbType>() { SqlDbType.Int,SqlDbType.VarChar };
                    DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Delete This Project? It will also remove all the UserTasks ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Database db = new Database();
                        SqlDataReader dr = db.SelectQueries(sqlparam, "DeleteProject", values, datatypes);
                        while (dr.Read())
                        {
                            if(dr[0].ToString()!="0")

                            {
                                db.InsertionProcedures(sqlparam, values, datatypes, "OnDeletionProjectUpdate");
                                MessageBox.Show("Project Removed Successfully");
                                loadDataForManaging();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("You Can't Remove this Project Because You are not Project Manager of it");
                                loadDataForManaging();
                                return;
                            }
                        }
                    }
                }
                else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != dataGridView1.NewRowIndex && e.ColumnIndex == 5)
                {
                    List<string> values = new List<string>() { dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() };
                    List<string> sqlparam = new List<string>() { "@idd", "@pName" };
                    List<SqlDbType> datatypes = new List<SqlDbType>() { SqlDbType.Int, SqlDbType.VarChar };
                    DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Update Project Name?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Database db = new Database();
                        if (db.InsertionProcedures(sqlparam, values, datatypes, "UpdateProject"))
                        {
                            MessageBox.Show("Project Name Updated Successfully");
                            loadDataForManaging();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
        }

        private void ManagingProjects_Load(object sender, EventArgs e)
        {

        }

        private void createTeam_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0)
            {
                MessageBox.Show("Your Don't Have any Teams", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txt_proName.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Valid Credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                values.Clear(); sqlparam.Clear(); datatypes.Clear();
                values.Add(comboBox2.Items[comboBox1.SelectedIndex].ToString());
                values.Add(txt_proName.Text);
                values.Add(uname.Text);
                sqlparam.Add("@tid");
                sqlparam.Add("@proName");
                sqlparam.Add("@username");
                datatypes.Add(SqlDbType.Int);
                datatypes.Add(SqlDbType.VarChar);
                datatypes.Add(SqlDbType.VarChar);
                Database db = new Database();
                bool x = db.InsertionProcedures(sqlparam, values, datatypes, "CreateProject");
               // MessageBox.Show(comboBox1.SelectedItem.ToString() + "," + uname.Text + "," + txt_username.Text);
                if (x)
                {
                    MessageBox.Show("Project Added Successfully");
                    loadDataForManaging();
                    return;
                }
                else
                {
                    MessageBox.Show("There is a Problem in Adding the Project in Team..!\n Make Sure that You are Entering Valid Info", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
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

        private void ManagingProjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Restart();
        }
    }
}
