
namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class GetDynamicQRCodeReq
    {
        public string healthCardId { get; set; }
        public string idType { get; set; }
        public string idNumber { get; set; }
        public int codeType { get; set; }
    }
}
