using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WJ_HustleForProfit_003.Models;
using WJ_HustleForProfit_003.Shared;

namespace WJ_HustleForProfit_003.Services
{
    public class UserProfileService
    {
        private string connectionString = clsConnectionString.GetConnectionString();
        public UserProfileService()
        {

        }

        public UserProfile UserGetProfileByEmail(string email)
        {
            UserProfile userProfile = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_UserGetProfileByEmail", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserEmail", email);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userProfile = new UserProfile(email)
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PrimaryPhone = reader["PrimaryPhone"].ToString(),
                            UserTypeID = Convert.ToInt32(reader["UserTypeID"]),
                            UserType = reader["UserType"].ToString(),
                            HustlePoints = Convert.ToInt32(reader["HustlePoints"]),
                            HustleCredits = Convert.ToInt32(reader["HustleCredits"])
                        };
                    }
                }
            }

            return userProfile;
        }        

        public List<string> UserGetSettingsByEmail(string email)
        {
            List<string> settings = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_UserGetSettingsByEmail", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserProfileEmail", email);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        settings.Add(reader["SettingName"].ToString());
                    }
                }
            }

            return settings;
        }
    }
}
