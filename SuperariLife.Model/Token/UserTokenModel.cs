namespace SuperariLife.Model.Token
{
    public class TokenModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailId { get; set; }
        public string FullName { get; set; }
        public DateTime TokenValidTo { get; set; }
        public string? UserImage { get; set; }   
        public bool IsSuperAdmin { get; set; }
    }
}
