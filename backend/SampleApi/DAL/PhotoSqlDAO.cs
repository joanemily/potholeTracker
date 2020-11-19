using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public class PhotoSqlDAO: IPhotoDAO
    {
        //Dependency Injection
        private readonly string connectionString;

        public PhotoSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }


        /// <summary>
        /// Send a query to the SQL database to save the photo information for future use.
        /// </summary>
        /// <param name="photo">Sends in the information that needs to be saved.</param>
        /// <returns>The photo id number of the photo just created in the system.</returns>

        public int UploadPhoto(Photo photo)
        {
            try
            {
                //Create the connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Open the connection
                    conn.Open();

                    //Create the SQL query
                    string sql = $"INSERT INTO Photos (PhotoPath, PotholeId, DateUploaded, UserId) VALUES (@Path, @PotHoleId, @DateUploaded, @UserId); Select @@Identity;";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //Set the parameters
                    cmd.Parameters.AddWithValue("@PotHoleId", photo.PotholeId);
                    cmd.Parameters.AddWithValue("@DateUploaded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Path", photo.Path);
                    cmd.Parameters.AddWithValue("@UserId", photo.UserId);

                    //Get the new ID number
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            //If there is an error, throw an exception.
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create a SQL query to get the information for one photo when called.
        /// </summary>
        /// <param name="photoId">Gets a photo by id</param>
        /// <returns>Photo</returns>
        public Photo GetPhoto(int photoId)
        {
            //Create an object to hold the photo
            Photo photo = new Photo();
            try
            {
                //Create a connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Create a query
                    string sql = @"
Select * 
FROM Photos 
WHERE Id = @id;";
                    //Open the connection
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //Add the id parameter
                    cmd.Parameters.AddWithValue("@id", photoId);

                    //Return the data
                    SqlDataReader reader = cmd.ExecuteReader();
                    //Map the data into the photo object
                    while (reader.Read())
                    {
                        photo = MapRowToPhoto(reader);
                        return photo;
                    }
                }
            }

            //If there is an error throw an exception.
            catch (SqlException ex)
            {
                throw ex;
            }
            //Return the photo to be displayed.
            return photo;
        }

        /// <summary>
        /// Map the data received from SQL to a photo object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Photo MapRowToPhoto(SqlDataReader reader)
        {
            Photo photo = new Photo();

            photo.Id = Convert.ToInt32(reader["Id"]);
            photo.PotholeId = Convert.ToInt32(reader["PotHoleId"]);
            photo.Path = Convert.ToString(reader["PhotoPath"]);
            photo.UserId = Convert.ToInt32(reader["UserId"]);
            photo.DateUploaded = Convert.ToDateTime(reader["DateUploaded"]);
            return photo;

        }

        /// <summary>
        /// Get a list of photos to display when the detail page is called up.
        /// </summary>
        /// <param name="potholeId">The pothole id</param>
        /// <returns>A list of photos</returns>
        public List<Photo> GetPhotos(int potholeId)
        {
            //Create a list of photos
            List<Photo> photos = new List<Photo>();
            try
            {
                //Create a SQL connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //Create a SQL query
                    string sql = @"
Select * 
FROM Photos 
WHERE PotHoleID = @id;";

                    //Open the connection
                    conn.Open();

                    //Send the query
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //Add the parameters
                    cmd.Parameters.AddWithValue("@id", potholeId);

                    //Get the data
                    SqlDataReader reader = cmd.ExecuteReader();

                    //Map the data into a new photo object.
                    while (reader.Read())
                    {
                        Photo photo = MapRowToPhoto(reader);
                        //Add that object to the list.
                        photos.Add(photo);
                    }
                }
            }

            //If there is an error, throw an exception
            catch (SqlException ex)
            {
                throw ex;
            }

            //Return the list of photos
            return photos;
        }
    }
}
