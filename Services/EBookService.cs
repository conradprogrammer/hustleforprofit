using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using WJ_HustleForProfit_003.Models;
using WJ_HustleForProfit_003.Shared;

namespace WJ_HustleForProfit_003.Services
{
    public class EBookService
    {
        private string connectionString = clsConnectionString.GetConnectionString();

        public EBookService()
        {

        }

        // Method to upsert an EBook
        public int EBookUpsert(EBook ebook)
        {
            int newEBookID = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_EBookUpsert", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add parameters and handle null values
                command.Parameters.Add("@ID", SqlDbType.Int).Value = ebook.ID > 0 ? (object)ebook.ID : DBNull.Value;
                command.Parameters.AddWithValue("@UserID", ebook.UserID);
                command.Parameters.AddWithValue("@eBookName", ebook.EBookName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookTitle", ebook.EBookTitle ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookByline", ebook.EBookByline ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookDescription", ebook.EBookDescription ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookAuthor", ebook.EBookAuthor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookPublisher", ebook.EBookPublisher ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookFormat", ebook.EBookFormat ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookSerial", ebook.EBookSerial ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@eBookVideoURL", ebook.EBookVideoURL ?? (object)DBNull.Value);
                if (ebook.EBookCoverImage != null)
                {
                    command.Parameters.Add("@eBookCoverImage", SqlDbType.VarBinary).Value = ebook.EBookCoverImage;
                }
                else
                {
                    command.Parameters.Add("@eBookCoverImage", SqlDbType.VarBinary).Value = DBNull.Value;
                }
                SqlParameter outputParam = new SqlParameter("@NewEBookID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                connection.Open();
                command.ExecuteNonQuery();

                // Retrieve the output value from the stored procedure
                newEBookID = Convert.ToInt32(outputParam.Value);
            }

            return newEBookID;
        }
        // Method to retrieve eBooks by UserID
        public List<EBook> GetEBooksByUserID(int userID)
        {
            List<EBook> ebooks = new List<EBook>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_EBookGetByUserID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserID", userID);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EBook ebook = new EBook
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            EBookName = reader["eBookName"].ToString(),
                            EBookTitle = reader["eBookTitle"] != DBNull.Value ? reader["eBookTitle"].ToString() : null,
                            EBookByline = reader["eBookByline"] != DBNull.Value ? reader["eBookByline"].ToString() : null,
                            EBookDescription = reader["eBookDescription"] != DBNull.Value ? reader["eBookDescription"].ToString() : null,
                            EBookAuthor = reader["eBookAuthor"] != DBNull.Value ? reader["eBookAuthor"].ToString() : null,
                            EBookPublisher = reader["eBookPublisher"] != DBNull.Value ? reader["eBookPublisher"].ToString() : null,
                            EBookFormat = reader["eBookFormat"] != DBNull.Value ? reader["eBookFormat"].ToString() : null,
                            EBookSerial = reader["eBookSerial"] != DBNull.Value ? reader["eBookSerial"].ToString() : null,
                            EBookVideoURL = reader["eBookVideoURL"] != DBNull.Value ? reader["eBookVideoURL"].ToString() : null,
                            TotalBooks = reader["TotalBooks"] != DBNull.Value ? (int?)Convert.ToInt32(reader["TotalBooks"]) : null
                        };

                        ebooks.Add(ebook);
                    }
                }
            }

            return ebooks;
        }

        public int EBookChapterUpsert(EBookChapter chapter)
        {
            int newChapterID = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_EBookChapterUpsert", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add parameters with their values
                command.Parameters.Add("@ID", SqlDbType.Int).Value = chapter.ID > 0 ? (object)chapter.ID : DBNull.Value;
                command.Parameters.Add("@EBookID", SqlDbType.Int).Value = chapter.EBookID;
                command.Parameters.Add("@EBookChapterID", SqlDbType.Int).Value = chapter.EBookChapterID;
                command.Parameters.Add("@EBookChapterTitle", SqlDbType.NVarChar, 500).Value = chapter.EBookChapterTitle ?? (object)DBNull.Value;
                command.Parameters.Add("@EBookChapterText", SqlDbType.NVarChar, -1).Value = chapter.EBookChapterText ?? (object)DBNull.Value;

                SqlParameter outputParam = new SqlParameter("@NewChapterID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                connection.Open();
                command.ExecuteNonQuery();

                newChapterID = Convert.ToInt32(outputParam.Value);
            }

            return newChapterID;
        }

        public List<EBookChapter> GetEBookChaptersByEBookID(int eBookID)
        {
            List<EBookChapter> chapters = new List<EBookChapter>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_EBookChaptersGetByEBookID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@EBookID", eBookID);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EBookChapter chapter = new EBookChapter
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            DateCreated = Convert.ToDateTime(reader["datecreated"]),
                            Active = Convert.ToBoolean(reader["active"]),
                            EBookID = Convert.ToInt32(reader["EBookID"]),
                            EBookChapterID = Convert.ToInt32(reader["EBookChapterID"]),
                            EBookChapterTitle = reader["EBookChapterTitle"].ToString(),
                            EBookChapterText = reader["EBookChapterText"].ToString()
                        };
                        chapters.Add(chapter);
                    }
                }
            }

            return chapters;
        }

    }
}
