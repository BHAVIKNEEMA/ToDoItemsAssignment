namespace ToDo.Models
{
    public class User
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string passwordHash { get; set; }
        public string email { get; set; }
        public string isActive { get; set; }
        public string roles { get; set; }
    }
}
