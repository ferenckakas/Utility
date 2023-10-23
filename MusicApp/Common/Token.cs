using System;

namespace Common
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
    }
}
