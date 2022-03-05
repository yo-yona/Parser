using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SimbirSoftParser
{
    class DBManager
    {
        SqlConnection myConn;
        String site;
        private void CreateDB(bool create=true)
        {
            String str, res;
            if (create)
            {
                str = "CREATE DATABASE State";
                res = "БД успешно создана";
            }
            else
            {
                str = "DROP DATABASE IF EXISTS State";
                res = "БД удалена";
            }

            SqlCommand createDB = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                createDB.ExecuteNonQuery();
                Console.WriteLine(res);
            }
            catch (System.Data.SqlClient.SqlException ex) 
            {
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
            String str = "CREATE TABLE Cash (site varchar(255) NOT NULL, word varchar(255) NOT NULL, count int)";
            SqlCommand createTab = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                createTab.ExecuteNonQuery();
                Console.WriteLine("Таблица создана");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
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
            String connectionString = "Server=localhost;Integrated security=SSPI;database=master";
            myConn = new SqlConnection(connectionString);
            CreateDB(false); 
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
                    str = $"INSERT INTO Cash (site, word, count) VALUES ('{site}','{KVPair.Key}','{KVPair.Value}')";
                    createDB = new SqlCommand(str, myConn);
                    createDB.ExecuteNonQuery();
                }
                Console.WriteLine("Сохранено в БД");
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
            String str = $"select * from Cash where site = '{site}'";
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                SqlDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    Console.WriteLine($"{myReader["word"].ToString()}, {Convert.ToUInt32(myReader["count"])}");
                    foreach (var letter in System.Text.Encoding.UTF8.GetBytes(myReader["word"].ToString()))
                        Console.Write($" {letter}");
                    Console.Write("\n");
                    wordStatistics.Add(myReader["word"].ToString(), Convert.ToUInt32(myReader["count"]));
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine(ex.Message);
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
