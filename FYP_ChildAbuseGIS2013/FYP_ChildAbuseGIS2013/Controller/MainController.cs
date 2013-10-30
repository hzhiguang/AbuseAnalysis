using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace FYP_ChildAbuseGIS2013.Controller
{
    public class MainController
    {
        private string server = "127.0.0.1";
        private string port = "5432";
        private string userId = "postgres";
        private string pwd = "password";
        private string db = "ChildAbuseAnalysis";
        private NpgsqlConnection conn;

        public MainController() { }

        public NpgsqlConnection startConn()
        {
            bool success = false;
            try
            {
                // PostgeSQL-style connection string
                string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", server, port, userId, pwd, db);
                conn = new NpgsqlConnection(connstring);
                conn.Open();
                success = true;
            }
            catch (Exception e)
            {
                //Console.Write(e.Message.ToString());
            }
            if (success != true)
            {
                conn = null;
            }
            return conn;
        }

        public void closeConn()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}