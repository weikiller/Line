using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace LineBotSdkExample
{
    public class MysqlLineID
    {

        // public string ConnectServerName = "server=35.201.237.132;user id=mywaytech;Password=mywaytech168;persist security info=True;database=salesdb";
        public string ConnectServerName = "Database=localdb;Data Source=127.0.0.1;Port=49176;User Id=azure;Password=6#vWHD_$";


        public DataTable DBConnection(string cmd)
        {

            // cmd = "select * from product";


            MySqlDataAdapter da = new MySqlDataAdapter(cmd, ConnectServerName);

            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
            }
            catch (Exception e)
            {
                if (e.ToString() == "Unable to connect to any of the specified MySQL hosts.")
                {
                   // MessageBox.Show("資料庫未連線!!!");

                }
                else
                {
                  //  MessageBox.Show("資料庫錯誤!!!");

                }
            }
            da.Dispose();
            return dt;


        }
        public string DBConnectionInsert(string cmd)
        {
            MySqlDataAdapter da = new MySqlDataAdapter(cmd, ConnectServerName);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                da.Dispose();
                return "true";
            }
            catch (Exception e)
            {
                da.Dispose();
                return e.ToString();
            }

        }

        public bool DBConnectionjudge(string table, string JudgeText)
        {

            try
            {
                string cmd = "SELECT * FROM " + table + " WHERE ID='" + JudgeText + "'";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd, ConnectServerName);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    da.Dispose();
                    return true;
                }
                else
                {
                    da.Dispose();
                    return false;
                }
            }
            catch (Exception e)
            {
                
                return true;
            }
        }
    
    public bool insert( string ID,string Name)
        {
            var conn = new MySqlConnection();
            //try
            //{
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = "127.0.0.1",
                    Database = "localdb",
                    UserID = "azure@localhost",
                    //  Password = "PASSWORD",
                    SslMode = MySqlSslMode.Required,
                };
                
                using ( conn = new MySqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("Opening connection");
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        //command.CommandText = "DROP TABLE IF EXISTS inventory;";
                        //await command.ExecuteNonQueryAsync();
                        //Console.WriteLine("Finished dropping table (if existed)");

                        //command.CommandText = "CREATE TABLE inventory (id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER);";
                        //await command.ExecuteNonQueryAsync();
                        //Console.WriteLine("Finished creating table");

                        command.CommandText = @"INSERT INTO `lineid`(`Name`, `ID`) VALUES (@name1,@name2) ;";
                        command.Parameters.AddWithValue("@name1", Name);
                        command.Parameters.AddWithValue("@name2", ID);


                        command.ExecuteReader();
                        // Console.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
                    }
                    conn.Close();
                    return true;
                    // connection will be closed by the 'using' block
                    //Console.WriteLine("Closing connection");
                }
            //}
            //catch(Exception ex) {
            //    conn.Close();
            //    return false; }

            //Console.WriteLine("Press RETURN to exit");
            //Console.ReadLine();
       }
    }
}