namespace WJ_HustleForProfit_003.Models
{
    public class UserProfile
    {
        public int ID { get; set; } // Identity column
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string PrimaryPhone { get; set; }
        public int UserTypeID { get; set; }
        public string UserType { get; set; }    
        public int HustlePoints { get; set; }   
        public int HustleCredits { get; set; }  

        public UserProfile(string userEmail)
        {
            UserEmail = userEmail;
        }
    }
}
