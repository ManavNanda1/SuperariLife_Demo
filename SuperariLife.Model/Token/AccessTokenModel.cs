namespace SuperariLife.Model.Token
{
    public class AccessTokenModel
    {
        public string Token { get; set; }
        public int ValidityInMin { get; set; }
        public DateTime ExpiresOnUTC { get; set; }
        public long Id { get; set; }

    }
}
