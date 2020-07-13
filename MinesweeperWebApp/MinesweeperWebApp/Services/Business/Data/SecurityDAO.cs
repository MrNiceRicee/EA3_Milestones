using MinesweeperWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MinesweeperWebApp.Services.Business.Data
{
    public class SecurityDAO
    {
        string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=LoginApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        // go through the database to see if the login credentials are correct
        public bool CheckUserCreds(User user)
        {
            //
            bool success = false;

            string queryString = "SELECT * FROM dbo.Users WHERE USERNAME = @username AND PASSWORD = @password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@USERNAME", System.Data.SqlDbType.VarChar, 50).Value = user.Username;
                command.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar, 50).Value = user.Password;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        success = true;
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
            return success;
        }

        public int RegisterUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO dbo.Users VALUES(@FirstName,@LastName,@Sex,@Age,@Address,@Email,@Username,@Password)";

                SqlCommand command = new SqlCommand(sqlQuery, connection);

                command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar, 1000).Value = user.ID;
                command.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, 1000).Value = user.FirstName;
                command.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, 1000).Value = user.LastName;
                command.Parameters.Add("@Sex", System.Data.SqlDbType.VarChar, 1000).Value = user.Sex;
                command.Parameters.Add("@Age", System.Data.SqlDbType.VarChar, 1000).Value = user.Age;
                command.Parameters.Add("@Address", System.Data.SqlDbType.VarChar, 1000).Value = user.Address;
                command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 1000).Value = user.Email;
                command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 1000).Value = user.Username;
                command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 1000).Value = user.Password;

                connection.Open();
                int newID = command.ExecuteNonQuery();

                return newID;
            }
        }
    }
}