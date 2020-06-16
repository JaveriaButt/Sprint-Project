using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Configuration;

namespace Sprint_Project
{
    class Database
    //Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
    {
        public static string conString = "Data Source=.;Initial Catalog=Sprint;Integrated Security=True";
        SqlConnection con = new SqlConnection(conString);
        private SqlDataReader dr;

        public Database()
        {
        }
        //select procedures which need parameters
        public SqlDataReader SelectQueries(List<string> sqlpara, string proname, List<string> sqlvalues, List<SqlDbType> dbtypes)
        {
            try
            {
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand(proname, con);
                SqlDataReader dr;
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < sqlvalues.Count; i++)
                {
                    cmd.Parameters.Add(sqlpara[i], dbtypes[i]).Value = sqlvalues[i];
                }
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Dispose();
                return dr;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return dr;
            }

        }
        //insertion, deletion, updation procedures call
        public bool InsertionProcedures(List<string> param,List<string> values,List<SqlDbType> dbtypes, string procedure)
        {
            try
            {
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand(procedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < values.Count; i++)
                {
                    cmd.Parameters.Add(param[i], dbtypes[i]).Value = values[i];
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return false;
            }
        }
        //method for those procedures which don't need any parameter
        public SqlDataReader SelectProc(string proname)
        {
            try
            {
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand(proname, con);
                SqlDataReader dr;
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Dispose();
                return dr;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return dr;
            }
        }
    }
}
