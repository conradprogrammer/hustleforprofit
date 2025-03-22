using System.Data.SqlClient;
using System.Data;
using System;
using WJ_HustleForProfit_003.Shared;
using WJ_HustleForProfit_003.Models;

namespace WJ_HustleForProfit_003.Services
{
    public class UserBalancePointService
    {
        
        private static UserBalancePointService _instance;
        public static UserBalancePointService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserBalancePointService();
                }
                return _instance;
            }
        }
        public event Action<int> BalanceUpdated;
        private readonly string _connectionString;

        private UserBalancePointService()
        {
            _connectionString = clsConnectionString.GetConnectionString();
        }

        public (int? UpdatedBalance, string Message) ExecuteTransaction(TransactionModel transaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UserBalancePointService", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255)).Value = transaction.UserEmail;
                    command.Parameters.Add(new SqlParameter("@TransactionAmount", SqlDbType.Int)).Value = transaction.RealPointAmount;
                    command.Parameters.Add(new SqlParameter("@TransactionTypeID", SqlDbType.Int)).Value = (int)transaction.TransactionTypeID;
                    command.Parameters.Add(new SqlParameter("@TransactionDescription", SqlDbType.NVarChar, 255)).Value = transaction.TransactionDescription;

                    SqlParameter updatedBalanceParam = new SqlParameter("@UpdatedBalance", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(updatedBalanceParam);

                    SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(messageParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    int? updatedBalance = updatedBalanceParam.Value != DBNull.Value ? (int?)updatedBalanceParam.Value : null;
                    string message = messageParam.Value.ToString();

                    if (updatedBalance.HasValue)
                    {
                        BalanceUpdated?.Invoke(updatedBalance.Value);
                    }

                    return (updatedBalance, message);
                }
            }
        }
    }
}
