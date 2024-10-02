namespace Assignment1.Models
{
    public class AppSecrets
    {
        public UserSecrets Manager { get; set; }
        public UserSecrets Employee { get; set; }
    }

    public class UserSecrets
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
