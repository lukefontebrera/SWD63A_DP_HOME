namespace WebApp.Models
{
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public IEnumerable<String> Errors { get; set;  }
    }
}
