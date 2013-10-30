using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_ChildAbuseGIS2013.Model;
using Npgsql;

namespace FYP_ChildAbuseGIS2013.Controller
{
    public class LocationController
    {
        private NpgsqlCommand command;

        public LocationController() { }

        public string insertLocation(Location loc)
        {
            //bool success = false;
            string a = "lalala";
            MainController conn = new MainController();
            string geomValue = "ST_GeomFromText('POINT('" + loc.x + "' '" + loc.y + "')', 3414)";
            string query = "INSERT INTO location (address, x, y, geom) Values ('" + loc.address + "','" + loc.x + "','" + loc.y + "', '" + geomValue + "')";
            command = new NpgsqlCommand(query, conn.startConn());
            try
            {
                command.ExecuteNonQuery();
                //success = true;
            }
            catch (NpgsqlException ex)
            {
                a = ex.ToString();
            }
            conn.closeConn();
            return a;
        }

        public List<Location> retrieveAllLocations()
        {
            List<Location> locs = new List<Location>();
            MainController conn = new MainController();
            string query = "SELECT * From location";
            command = new NpgsqlCommand(query, conn.startConn());

            // Execute a query
            NpgsqlDataReader dr = command.ExecuteReader();

            // Read all rows and output the first column in each row
            while (dr.Read())
            {
                locs.Add(new Location(int.Parse(dr[0].ToString()), dr[1].ToString(), double.Parse(dr[2].ToString()), double.Parse(dr[3].ToString())));
            }
            conn.closeConn();
            return locs;
        }

        public Location retrieveLocationById(int id)
        {
            Location loc = new Location();
            MainController conn = new MainController();
            string query = "SELECT * From location WHERE id = '" + id + "'";
            command = new NpgsqlCommand(query, conn.startConn());

            // Execute a query
            NpgsqlDataReader dr = command.ExecuteReader();

            // Read all rows and output the first column in each row
            while (dr.Read())
            {
                loc = new Location(int.Parse(dr[0].ToString()), dr[1].ToString(), double.Parse(dr[2].ToString()), double.Parse(dr[3].ToString()));
            }
            conn.closeConn();
            return loc;
        }

        public bool updateLocation(Location loc)
        {
            bool success = false;
            MainController conn = new MainController();
            string query = "UPDATE location SET address = '" + loc.address + "', x = '" + loc.x + "', y = '" + loc.y + "', geom = ST_GeomFromText('POINT('" + loc.x + "' '" + loc.y + "')', 3414) WHERE id = '" + loc.ID + "'";
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