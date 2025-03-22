using System;

namespace WJ_HustleForProfit_003.Models
{
    public class UserSettingType
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
        public string SettingName { get; set; }
    }
}
