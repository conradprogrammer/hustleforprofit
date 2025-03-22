using System;

namespace WJ_HustleForProfit_003.Models
{
    public class EBookChapter
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
        public int EBookID { get; set; }
        public int EBookChapterID { get; set; }
        public string EBookChapterTitle { get; set; }
        public string EBookChapterText { get; set; }
    }
}
