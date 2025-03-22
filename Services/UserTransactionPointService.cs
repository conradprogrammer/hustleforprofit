using System;
using System.Data;
using System.Data.SqlClient;
using WJ_HustleForProfit_003.Shared;

namespace WJ_HustleForProfit_003.Services
{
    public class UserTransactionPointService
    {
        private readonly string _connectionString;

        private static UserTransactionPointService _instance;
        public static UserTransactionPointService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserTransactionPointService();
                }
                return _instance;
            }
        }

        private UserTransactionPointService()
        {
            _connectionString = clsConnectionString.GetConnectionString(); ;
        }

        public (bool IsTransactionPossible, string Message) VerifyBalance(string email, int estimateAmount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UserTransactionService", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255)).Value = email;
                    command.Parameters.Add(new SqlParameter("@EstimateAmount", SqlDbType.Int)).Value = estimateAmount;

                    SqlParameter isTransactionPossibleParam = new SqlParameter("@IsTransactionPossible", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(isTransactionPossibleParam);

                    SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(messageParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    bool isTransactionPossible = (bool)isTransactionPossibleParam.Value;
                    string message = messageParam.Value.ToString();

                    return (isTransactionPossible, message);
                }
            }
        }
    }
}
