using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public class PotholeSqlDAO : IPotholeDAO
    {
        //Dependency injection
        private readonly string connectionString;

        public PotholeSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //Get a list of potholes to display on the home page.
        public IList<Pothole> GetPotholes()

        {
            //Create a list to hold all the potholes
            List<Pothole> Potholes = new List<Pothole>();

            try
            {
                //Create a connection 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Open the connection
                    conn.Open();
                    //Create the SQL query
                    SqlCommand cmd = new SqlCommand("SELECT * FROM PotHole WHERE IsArchived=0 ORDER BY 'DateReported'", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        //Take the object and map them to a pothole object
                        Pothole pothole = MapRowToPothole(reader);
                        //Add the new objec to the list.
                        Potholes.Add(pothole);
                    }

                }
            }
            //If there is an error throw an exception.
            catch (SqlException ex)
            {
                throw ex;
            }
            //Send back the list of potholes.
            return Potholes;

        }

        //Get a single pothole by id.
        public Pothole GetSinglePothole(int potholeId)
        {
            //Creat a pothole object.
            Pothole pothole = new Pothole();
            try
            {
                //Create a SQL connection.
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Create a SQL query
                    string sql = @"
Select * 
FROM PotHole 
WHERE PotHoleId = @id;";
                    //Open the connection
                    conn.Open();
                    //Send the SQL query
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    //Add the parameters
                    cmd.Parameters.AddWithValue("@id", potholeId);
                    //Get the data back from the database.
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        //map the data to the pothole object
                        pothole = MapRowToPothole(reader);
                        //Send the pothole object back for display
                        return pothole;
                    }
                }
            }
            //If there is an error, throw an exception.
            catch (SqlException ex)
            {
                throw ex;
            }
            return pothole;
        }


        //Get a list of potholes that is filtered by the status.
        public IList<Pothole> GetFilteredPotholes(string filter)

        {
            //Create a list of potholes
            List<Pothole> Potholes = new List<Pothole>();

            try
            {
                //Create a SQL connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Open the connection.
                    conn.Open();
                    //Create the SQL query
                    SqlCommand cmd = new SqlCommand("SELECT * FROM PotHole WHERE RepairStatus = @filter ORDER BY Severity DESC", conn);
                    //Add the parameters.
                    cmd.Parameters.AddWithValue("@filter", filter);
                    //Get the data back.
                    SqlDataReader reader = cmd.ExecuteReader();



                    while (reader.Read())
                    {
                        //Map the data to a pothole object.
                        Pothole pothole = MapRowToPothole(reader);
                        //Add the new object to the list.
                        Potholes.Add(pothole);
                    }

                }
            }
            //If there is an error, throw an exception.
            catch (SqlException ex)
            {
                throw ex;
            }
            //Return the list.
            return Potholes;

        }

        //Update the pothole with new information.
        public void UpdatePothole(Pothole pothole)
        {
            try
            {
                //Create a SQL connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Create a SQL query
                    string sql = @"
UPDATE PotHole 
SET RepairBeginDate = @startDate, 
RepairFinishDate = @endDate, 
DateInspected = @inspectDate,
Severity = @severity,
IsArchived = @isArchived
WHERE PotHoleId = @id;";
                    //Open the connection
                    conn.Open();
                    //Send the query
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    //Add the parameters.
                    cmd.Parameters.AddWithValue("@startDate", pothole.DateBeginRepair.HasValue ? (Object)pothole.DateBeginRepair.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@endDate", pothole.DateFinishRepair.HasValue ? (Object)pothole.DateFinishRepair.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@inspectDate", pothole.DateInspected.HasValue ? (Object)pothole.DateInspected.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@severity", pothole.Severity.HasValue ? (Object)pothole.Severity.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@isArchived", pothole.IsArchived.HasValue ? (Object)pothole.IsArchived.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", pothole.Id);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            //If there is an error, throw an exception.
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //Map the data that came in to an object.
        private Pothole MapRowToPothole(SqlDataReader reader)
        {
            //Create the object to map the data to.
            Pothole pothole = new Pothole();

            //If some of the elements are null, then make the properties null.
            if (reader["DateInspected"] is DBNull)
            {
                pothole.DateInspected = null;
            }
            else
            {
                pothole.DateInspected = Convert.ToDateTime(reader["DateInspected"]);
            }

            if (reader["RepairBeginDate"] is DBNull)
            {
                pothole.DateBeginRepair = null;
            }
            else
            {
                pothole.DateBeginRepair = Convert.ToDateTime(reader["RepairBeginDate"]);
            }

            if (reader["RepairFinishDate"] is DBNull)
            {
                pothole.DateFinishRepair = null;
            }
            else
            {
                pothole.DateFinishRepair = Convert.ToDateTime(reader["RepairFinishDate"]);
            }

            pothole.Id = Convert.ToInt32(reader["PotHoleId"]);
            pothole.Location = Convert.ToString(reader["PotHoleLocation"]);
            pothole.DateReported = Convert.ToDateTime(reader["DateReported"]);
            pothole.RepairStatus = Convert.ToString(reader["RepairStatus"]);
            pothole.Severity = (reader["Severity"] is DBNull) ? null : (int?)Convert.ToInt32(reader["Severity"]);
            pothole.IsArchived = (reader["IsArchived"] is DBNull) ? null : (int?)Convert.ToInt32(reader["IsArchived"]);
            return pothole;

        }


        public void DeletePothole(int potholeId)

        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"
Delete from PotHole 
WHERE PotHoleId = @id;";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", potholeId);

                    cmd.ExecuteNonQuery();
                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int AddPothole(Pothole pothole)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = $"INSERT INTO PotHole (PotHoleLocation, DateReported, DateInspected, RepairBeginDate,  RepairFinishDate, Severity, RepairStatus) VALUES (@PotHoleLocation, @DateReported, NULL, NULL, NULL, @Severity, 'Reported'); Select @@Identity;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@PotHoleLocation", pothole.Location);
                    cmd.Parameters.AddWithValue("@DateReported", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Severity", pothole.Severity);
                    cmd.Parameters.AddWithValue("@RepairStatus", "Reported");
                                       
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        
    }
}
