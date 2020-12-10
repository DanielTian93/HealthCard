namespace Core.Helpers
{
    public class WXMPUserInfoDomainModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string AvatarUrl { get; set; }
        public string UnionId { get; set; }
        public Watermark Watermark { get; set; }
    }
    public class Watermark
    {
        public int Timestamp { get; set; }
        public string Appid { get; set; }
    }
}
