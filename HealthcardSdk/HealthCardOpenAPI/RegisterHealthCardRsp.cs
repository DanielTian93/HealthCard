
namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class RegisterHealthCardRsp
    {
        public string qrCodeText { get; set; }
        public string healthCardId { get; set; }
        public string adminExt { get; set; }
        public string phid { get; set; }
        public string unbindOrderId { get; set; }
        public string unbindURL { get; set; }
    }
}
