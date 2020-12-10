
namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class VerifyQRCodeReq
    {
        public string qrCodeText { get; set; }
        public string terminalId { get; set; }
        public string time { get; set; }
        public string medicalStep { get; set; }
        public string channelCode { get; set; }
        public string channelName { get; set; }
        public string useCityName { get; set; }
        public string useCityCode { get; set; }
        public string hospitalCode { get; set; }
        public string hospitalName { get; set; }
        public string orgId { get; set; }
        public string name { get; set; }
        public string idCard { get; set; }
        public string useScene { get; set; }
        public string useType { get; set; }
        public string useChannel { get; set; }
    }
}
