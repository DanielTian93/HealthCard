
namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class ReportHISDataReq
    {
        /// <summary>
        /// ��ά��
        /// </summary>
        public string qrCodeText { get; set; }
        /// <summary>
        /// ֤������
        /// </summary>
        public string idCardNumber { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// ʱ��
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// ҽԺID �ĵ��޴˲��� ����ʱĬ������
        /// </summary>
        public string hospitalId { get; set; }
        /// <summary>
        /// �ÿ�����
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// �ÿ�����
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// ֤������
        /// </summary>
        public string cardChannel { get; set; }
        /// <summary>
        /// �ÿ�����
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// �ÿ��ѱ�
        /// </summary>
        public string cardCostTypes { get; set; }
        /// <summary>
        /// ҽԺID ����ʱĬ������
        /// </summary>
        public string hospitalCode { get; set; }
    }
}
