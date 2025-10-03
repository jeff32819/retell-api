namespace RetellApi.Models
{
    public class LookupResponse
    {
        public bool success { get; set; }
        public bool isFound { get; set; }
        public string chatId { get; set; }
        public ChatLookupResponse result { get; set; }
    }
}
