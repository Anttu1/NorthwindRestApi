namespace NorthwindRestApi.Models
{
    public class LoggedUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int AccessId { get; set; }
        public string? Token { get; set; }
    }
}
