
namespace com.tencent.healthcard.HealthCardOpenAPI
{
    public class RegisterHealthCardReq
    {
        /// <summary>
        /// ΢�������
        /// </summary>
        public string wechatCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// �Ա� �С�Ů
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// ���� 	���塢�����
        /// </summary>
        public string nation { get; set; }
        /// <summary>
        /// ���������� ��ʽ��yyyy-MM-dd
        /// </summary>
        public string birthday { get; set; }
        ///// <summary>
        ///// ֤������
        ///// </summary>
        //public string idCard { get; set; }
        ///// <summary>
        ///// ֤������
        ///// </summary>
        //public string cardType { get; set; }
        /// <summary>
        /// ��ַ
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// ��ϵ��ʽ1
        /// </summary>
        public string phone1 { get; set; }
        /// <summary>
        /// ��ϵ��ʽ2
        /// </summary>
        public string phone2 { get; set; }
        /// <summary>
        /// Ժ��ID
        /// </summary>
        public string patid { get; set; }
        /// <summary>
        /// ֤������
        /// </summary>
        public string idNumber { get; set; }
        /// <summary>
        /// ֤������ 01-�������֤�������ο�֤�����ͱ�����ʧ�ܵ��û�������ʾ�ڿ���ƽ̨�������档
        /// </summary>
        public string idType { get; set; }
        /// <summary>
        /// �û�openId
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 0����ע�� 1 ����ע��
        /// </summary>
        public RegisteredType registeredType { get; set; }
    }
}
