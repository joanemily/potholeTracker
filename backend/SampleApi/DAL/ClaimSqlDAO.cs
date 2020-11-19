using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Claim = SampleApi.Models.Claim;

namespace SampleApi.DAL
{
    public class ClaimSqlDAO: IClaimDAO
    {

        private readonly string connectionString;

        public ClaimSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        
        public int FileClaim(Models.Claim claim)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = $"INSERT INTO Claims (ClaimantName, ClaimantAddress, DateOfAccident, DateFiled,  Phone, Cost, PotHoleId, UserId) " +
                        $"VALUES (@Name, @Address, @DateOfAccident, @DateFiled, @Phone, @Cost, @PotHoleId, @UserId); Select @@Identity;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@PotHoleId", claim.PotholeId);
                    cmd.Parameters.AddWithValue("@DateOfAccident", claim.DateOfAccident);
                    cmd.Parameters.AddWithValue("@Name", claim.Name);
                    cmd.Parameters.AddWithValue("@Cost", claim.Cost);
                    cmd.Parameters.AddWithValue("@UserId", claim.UserId);
                    cmd.Parameters.AddWithValue("@DateFiled", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Address", claim.Address);
                    cmd.Parameters.AddWithValue("@Phone", claim.Phone);



                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public IList<Claim> ViewAllClaims()
        {
            List<Claim> claims = new List<Claim>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Claims ORDER BY 'DateFiled'", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Claim claim = MapRowToClaim(reader);
                        claims.Add(claim);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return claims;
        }

        private Claim MapRowToClaim(SqlDataReader reader)
        {
            Claim claim = new Claim();

            claim.Name = Convert.ToString(reader["ClaimantName"]);
            claim.Address = Convert.ToString(reader["ClaimantAddress"]);
            claim.Cost = Convert.ToDecimal(reader["Cost"]);
            claim.DateOfAccident = Convert.ToDateTime(reader["DateOfAccident"]);
            claim.Phone = Convert.ToString(reader["Phone"]);
            claim.PotholeId = Convert.ToInt32(reader["PotholeId"]);
            claim.UserId = Convert.ToInt32(reader["UserId"]);
            claim.DateFiled = Convert.ToDateTime(reader["DateFiled"]);

            return claim;

        }
    }
}
