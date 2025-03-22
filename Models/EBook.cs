namespace WJ_HustleForProfit_003.Models
{
    public class EBook
    {
        public int ID { get; set; } // Identity column
        public int UserID { get; set; } // Foreign key for user
        public string EBookName { get; set; } // Required, max length 50
        public string EBookTitle { get; set; } // Optional, max length 500
        public string EBookByline { get; set; } // Optional, max length 500
        public string EBookDescription { get; set; } // Optional, max length 500
        public string EBookAuthor { get; set; } // Optional, max length 500
        public string EBookPublisher { get; set; } // Optional, max length 500
        public string EBookFormat { get; set; } // Optional, max length 500
        public string EBookSerial { get; set; } // Optional, max length 500
        public string EBookVideoURL { get; set; } // Optional, max length 500
        public byte[] EBookCoverImage { get; set; }
        public int? TotalBooks { get; set; }
    }
}
