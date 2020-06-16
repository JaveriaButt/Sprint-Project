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
    public partial class ManagingTeams : Form
    {
        public ManagingTeams()
        {
            InitializeComponent();
            uname.Text = Dashboard.uname;
            txt_teamOwner.Text = Dashboard.uname;
            loadDataForManaging();

            Font f = new Font("Times New Roman", 9);
            DataGridViewCellStyle cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.DefaultCellStyle = cs;
            f = new Font("Times New Roman", 9, FontStyle.Bold);
            cs = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.Black, SelectionBackColor = Color.PaleVioletRed, SelectionForeColor = Color.Black, Font = f };
            dataGridView1.ColumnHeadersDefaultCellStyle = cs;
        }
        void loadDataForManaging()
        {
            try
            {
                dataGridView1.Rows.Clear();
                comboBox1.Items.Clear();
                values.Clear();sqlparam.Clear();datatypes.Clear();
                values = new List<string>() { uname.Text };
                sqlparam = new List<string>() { "@username" };
                datatypes = new List<SqlDbType>() { SqlDbType.VarChar };
                Database db = new Database();
                SqlDataReader dr = db.SelectQueries(sqlparam, "LoadTeamData", values, datatypes);
                if (dr.HasRows)
                {
                    int i = 0;
                    while(dr.Read())
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[i].Clone();
                        row.Cells[0].Value = dr[0].ToString();
                        row.Cells[1].Value = dr[1].ToString();
                        row.Cells[2].Value = dr[2].ToString();
                        row.Cells[3].Value = dr[3].ToString();
                        row.Cells[4].Value = "Update";
                        row.Cells[5].Value = "Delete";
                        row.Cells[6].Value = dr[4].ToString();
                        comboBox1.Items.Add(dr[0].ToString());
                        dataGridView1.Rows.Add(row);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error : " + ex);
            }
        }
        List<string> values = new List<string>();
        List<string> sqlparam = new List<string>();
        List<SqlDbType> datatypes = new List<SqlDbType>();
        private void createTeam_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txt_teamName.Text) )
            {
                MessageBox.Show("Please Enter a Valid Team Name");
                return;
            }
            Database db = new Database();
            values.Clear(); sqlparam.Clear(); datatypes.Clear();
            values.Add(txt_teamOwner.Text);
            values.Add(txt_teamName.Text);
            sqlparam.Add("@teamowner");
            sqlparam.Add("@teamname");
            datatypes.Add(SqlDbType.VarChar);
            datatypes.Add(SqlDbType.VarChar);
            bool x = db.InsertionProcedures(sqlparam, values, datatypes, "CreateTeam");
            if (x)
            {
                MessageBox.Show("Team Created Successfully");
                loadDataForManaging();
                return;
            }
            else
            {
                MessageBox.Show("There is a Problem in Creating Team..!\n Make Sure that You are Not Duplicating Team names","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
        }

        private void ManagingTeams_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Dashboard d = new Dashboard(uname.Text);
            d.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0)
            {
                MessageBox.Show("Your Don't Have any Teams", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ( txt_username.Text==string.Empty)
            {
                MessageBox.Show("Please Enter Valid Credentials","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            else
            {
                values.Clear(); sqlparam.Clear(); datatypes.Clear();
                values.Add(comboBox1.SelectedItem.ToString());
                values.Add(uname.Text);
                values.Add(txt_username.Text);
                sqlparam.Add("@teamname");
                sqlparam.Add("@un");
                sqlparam.Add("@mn");
                datatypes.Add(SqlDbType.VarChar);
                datatypes.Add(SqlDbType.VarChar);
                datatypes.Add(SqlDbType.VarChar);
                Database db = new Database();
                bool x = db.InsertionProcedures(sqlparam, values, datatypes, "InsertMember");
               // MessageBox.Show(comboBox1.SelectedItem.ToString() + "," + uname.Text + "," +txt_username.Text);
                if (x)
                {
                    MessageBox.Show("Team Member Added Successfully");
                    loadDataForManaging();
                    txt_username.Text = "";
                    comboBox1.SelectedIndex = -1;
                    return;
                }
                else
                {
                    MessageBox.Show("There is a Problem in Adding Member in Team..!\n Make Sure that You are Not Duplicating Member", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0)
            {
                MessageBox.Show("Your Don't Have any Teams", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txt_username.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Valid Credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                values.Clear(); sqlparam.Clear(); datatypes.Clear();
                values.Add(comboBox1.SelectedItem.ToString());
                values.Add(uname.Text);
                values.Add(txt_username.Text);
                sqlparam.Add("@teamname");
                sqlparam.Add("@un");
                sqlparam.Add("@mn");
                datatypes.Add(SqlDbType.VarChar);
                datatypes.Add(SqlDbType.VarChar);
                datatypes.Add(SqlDbType.VarChar);
                Database db = new Database();
                bool x = db.InsertionProcedures(sqlparam, values, datatypes, "DeleteMember");
                MessageBox.Show(comboBox1.SelectedItem.ToString() + "," + uname.Text + "," + txt_username.Text);
                if (x)
                {
                    MessageBox.Show("Team Member Removed Successfully from Team");
                    loadDataForManaging();
                    return;
                }
                else
                {
                    MessageBox.Show("There is a Problem in Removing Member from Team..!\n Make Sure This Member is not assigned to any Task", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Restart();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;


            try
            {

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != dataGridView1.NewRowIndex && e.ColumnIndex == 5)
                {
                    List<string> values = new List<string>() { dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() };
                    List<string> sqlparam = new List<string>() { "@idd" };
                    List<SqlDbType> datatypes = new List<SqlDbType>() { SqlDbType.Int };
                    DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Delete This Team? It will also remove all the Project, UserTasks ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Database db = new Database();
                        if (db.InsertionProcedures(sqlparam, values, datatypes, "DeleteTeam"))
                        {
                            MessageBox.Show("Team Removed Successfully");
                            loadDataForManaging();
                            return;
                        }
                    }
                }
                else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != dataGridView1.NewRowIndex && e.ColumnIndex == 4)
                {
                    List<string> values = new List<string>() { dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() };
                    List<string> sqlparam = new List<string>() { "@idd","@teamName" };
                    List<SqlDbType> datatypes = new List<SqlDbType>() { SqlDbType.Int,SqlDbType.VarChar };
                    DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Update Team Name?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Database db = new Database();
                        if (db.InsertionProcedures(sqlparam, values, datatypes, "UpdateTeam"))
                        {
                            MessageBox.Show("Team Name Updated Successfully");
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_teamOwner_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_teamName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txt_username_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ManagingTasks t = new ManagingTasks();
            t.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ManagingProjects p = new ManagingProjects();
            p.Show();
            this.Hide();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ManagingTeams t = new ManagingTeams();
            t.Show();
            this.Hide();
        }

        private void uname_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
