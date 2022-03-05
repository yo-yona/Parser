using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SimbirSoftParser
{
    class DBManager
    {
        SqlConnection myConn;
        String site;
        private void CreateDB()
        {
            String str = "CREATE DATABASE Stata";
            SqlCommand createDB = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                createDB.ExecuteNonQuery();
                Console.WriteLine("DataBase is Created Successfully");
            }
            catch (System.Data.SqlClient.SqlException ex) 
            {
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }
        private void CreateTab()
        {
            String str = "CREATE TABLE Cashing (site varchar(255) NOT NULL, word varchar(255) NOT NULL, count int)";
            SqlCommand createTab = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                createTab.ExecuteNonQuery();
                Console.WriteLine("Table is Created Successfully");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public DBManager(string site)
        {
            this.site = site;
            //String connectionString = "Server=localhost;Integrated security=SSPI;AttachDBFilename=" + System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Stata.mdf;database=master";
            //String connectionString = "Server=localhost;AttachDbFileName=" + System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Stat.mdf;Integrated Security=True;MultipleActiveResultSets=True";
            String connectionString = "Server=localhost;Integrated security=SSPI;database=master";
            myConn = new SqlConnection(connectionString);
            CreateDB();
            CreateTab();
        }

        public void PushToDB(Dictionary<string, uint> wordStatistics)
        {
            String str;
            SqlCommand createDB;
            try
            {
                myConn.Open();
                foreach (var KVPair in wordStatistics)
                {
                    str = $"INSERT INTO Cashing (site, word, count) VALUES ('{site}','{KVPair.Key}','{KVPair.Value}')";
                    createDB = new SqlCommand(str, myConn);
                    createDB.ExecuteNonQuery();
                }
                Console.WriteLine("Saved to DB");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public void CheckInDB(string site, ref Dictionary<string, uint> wordStatistics)
        {
            //wordStatistics = new Dictionary<string, uint>();
            String str = $"select * from Cashing where site = '{site}'";
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                SqlDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["word"].ToString() == "РАЗРАБОТКА")
                        Console.WriteLine($"{myReader["word"].ToString()}, {Convert.ToUInt32(myReader["count"])}");
                    wordStatistics.Add(myReader["word"].ToString(), Convert.ToUInt32(myReader["count"]));
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }
    }
}
