using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_ChildAbuseGIS2013.Model;
using Npgsql;

namespace FYP_ChildAbuseGIS2013.Controller
{
    public class FileController
    {
        private NpgsqlCommand command;

        public FileController() { }

        public bool insertFile(File file)
        {
            bool success = false;
            MainController conn = new MainController();
            string query = "INSERT INTO file (title, date, path, description, type) Values ('" + file.title + "','" + file.dateTimeTaken + "','" + file.path + "','" + file.desc + "', '" + file.fileType + "')";
            command = new NpgsqlCommand(query, conn.startConn());
            try
            {
                command.ExecuteNonQuery();
                success = true;
            }
            catch (NpgsqlException ex)
            {
                //success = ex.ToString();
            }
            conn.closeConn();
            return success;
        }

        public List<File> retrieveAllFiles()
        {
            List<File> files = new List<File>();
            MainController conn = new MainController();
            string query = "SELECT * From file";
            command = new NpgsqlCommand(query, conn.startConn());

            // Execute a query
            NpgsqlDataReader dr = command.ExecuteReader();

            // Read all rows and output the first column in each row
            while (dr.Read())
            {
                files.Add(new File(int.Parse(dr[0].ToString()), dr[1].ToString(), (DateTime)dr[2], dr[3].ToString(), dr[4].ToString(), dr[5].ToString()));
            }
            conn.closeConn();
            return files;
        }

        public List<File> retrieveAllFilesByType(string type)
        {
            List<File> files = new List<File>();
            MainController conn = new MainController();
            string query = "SELECT * From file WHERE type = '" + type + "'";
            command = new NpgsqlCommand(query, conn.startConn());

            // Execute a query
            NpgsqlDataReader dr = command.ExecuteReader();

            // Read all rows and output the first column in each row
            while (dr.Read())
            {
                files.Add(new File(int.Parse(dr[0].ToString()), dr[1].ToString(), (DateTime)dr[2], dr[3].ToString(), dr[4].ToString(), dr[5].ToString()));
            }
            conn.closeConn();
            return files;
        }

        public File retrieveFileById(int id)
        {
            File file = new File();
            MainController conn = new MainController();
            string query = "SELECT * From file WHERE id = '" + id + "'";
            command = new NpgsqlCommand(query, conn.startConn());

            // Execute a query
            NpgsqlDataReader dr = command.ExecuteReader();

            // Read all rows and output the first column in each row
            while (dr.Read())
            {
                file = new File(int.Parse(dr[0].ToString()), dr[1].ToString(), (DateTime)dr[2], dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            conn.closeConn();
            return file;
        }

        public bool updateFile(File file)
        {
            bool success = false;
            MainController conn = new MainController();
            string query = "UPDATE file SET title = '" + file.title + "', date = '" + file.dateTimeTaken + "', path = '" + file.path + "', description = '" + file.desc + "', type = '" + file.fileType + "' WHERE id = '" + file.ID + "'";
            command = new NpgsqlCommand(query, conn.startConn());
            try
            {
                command.ExecuteNonQuery();
                success = true;
            }
            catch (NpgsqlException ex)
            {
                //success = ex.ToString();
            }
            conn.closeConn();
            return success;
        }
    }
}