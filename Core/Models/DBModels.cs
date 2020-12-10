/*----------------------------------------------------------------
Copyright (C) 2017-2022 iMaxSys Co.,Ltd.
All rights reserved.

文件: DBModels.cs
摘要: DBModels实体类
说明:

当前：1.0
----------------------------------------------------------------*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.DBModels
{

	/// <summary>
	/// ChatMember
	/// (成员表chat_member)
	/// </summary>
	[Table("chat_member")]
	public class ChatMember
	{

		/// <summary>
		/// 成员主键
		/// </summary>
		[Key]		
		[Column("Mem_Id")]
		public int MemId { get; set; }

		/// <summary>
		/// 关注主键值
		/// </summary>
				
		[Column("Sub_Id")]
		public int? SubId { get; set; }

		/// <summary>
		/// 病例号
		/// </summary>
				
		[Column("Mem_Check_Card")]
		public string MemCheckCard { get; set; }

		/// <summary>
		/// 成员姓名
		/// </summary>
				
		[Column("Mem_Name")]
		public string MemName { get; set; }

		/// <summary>
		/// 0:男1:女
		/// </summary>
				
		[Column("Sex")]
		public int? Sex { get; set; }

		/// <summary>
		/// 成员身份证
		/// </summary>
				
		[Column("Mem_Card")]
		public string MemCard { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Med_Address")]
		public string MedAddress { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Parent_Name")]
		public string ParentName { get; set; }

		/// <summary>
		/// 生日
		/// </summary>
				
		[Column("Birthday")]
		public DateTime? Birthday { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
				
		[Column("Relation_Users")]
		public int? RelationUsers { get; set; }

		/// <summary>
		/// 医院编码
		/// </summary>
				
		[Column("Hosp_Code")]
		public string HospCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Mem_Tel")]
		public string MemTel { get; set; }

		/// <summary>
		/// 是否有效，1有效，0无效，2审批中
		/// </summary>
				
		[Column("Is_Use")]
		public int? IsUse { get; set; }

		/// <summary>
		/// 默认就诊人
		/// </summary>
				
		[Column("Is_Default")]
		public int? IsDefault { get; set; }

		/// <summary>
		/// 是否认证
		/// </summary>
				
		[Column("Is_Auth")]
		public int? IsAuth { get; set; }

		/// <summary>
		/// 身份证照片路径
		/// </summary>
				
		[Column("Id_Photo_Url")]
		public string IdPhotoUrl { get; set; }

		/// <summary>
		/// 是否是小孩
		/// </summary>
				
		[Column("Is_Child")]
		public int? IsChild { get; set; }
	}

	/// <summary>
	/// ChatUser
	/// (微信关注表chat_user)
	/// </summary>
	[Table("chat_user")]
	public class ChatUser
	{

		/// <summary>
		/// 关注主键值
		/// </summary>
		[Key]		
		[Column("Sub_Id")]
		public int SubId { get; set; }

		/// <summary>
		/// OpenID
		/// </summary>
				
		[Column("Openid")]
		public string Openid { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
				
		[Column("Nickname")]
		public string Nickname { get; set; }

		/// <summary>
		/// 性别
		/// </summary>
				
		[Column("Sex")]
		public int? Sex { get; set; }

		/// <summary>
		/// 省份
		/// </summary>
				
		[Column("Province")]
		public string Province { get; set; }

		/// <summary>
		/// 城市
		/// </summary>
				
		[Column("City")]
		public string City { get; set; }

		/// <summary>
		/// 头像
		/// </summary>
				
		[Column("Headimgurl")]
		public string Headimgurl { get; set; }

		/// <summary>
		/// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
		/// </summary>
				
		[Column("Unionid")]
		public string Unionid { get; set; }

		/// <summary>
		/// 状态(1:关注,0:取消关注)
		/// </summary>
				
		[Column("State")]
		public bool? State { get; set; }

		/// <summary>
		/// 关注日期
		/// </summary>
				[NotMappedAttribute]	
		[Column("Sub_Time")]
		public DateTime? SubTime { get; set; }

		/// <summary>
		/// 医院编码
		/// </summary>
				
		[Column("Hosp_Code")]
		public string HospCode { get; set; }

		/// <summary>
		/// 用户类型,0:微信,1:支付宝
		/// </summary>
				
		[Column("Sub_Type")]
		public int? SubType { get; set; }
	}

	/// <summary>
	/// HisRecharge
	/// (his充值记录)
	/// </summary>
	[Table("his_recharge")]
	public class HisRecharge
	{

		/// <summary>
		/// 
		/// </summary>
		[Key]		
		[Column("Id")]
		public long Id { get; set; }

		/// <summary>
		/// 就诊卡号
		/// </summary>
				
		[Column("Patient_No")]
		public string PatientNo { get; set; }

		/// <summary>
		/// 支付类型
		/// </summary>
				
		[Column("Payment_Way")]
		public string PaymentWay { get; set; }

		/// <summary>
		/// 订单号
		/// </summary>
				
		[Column("Out_Trade_No")]
		public string OutTradeNo { get; set; }

		/// <summary>
		/// 充值金额
		/// </summary>
				
		[Column("Amount")]
		public string Amount { get; set; }

		/// <summary>
		/// 用户ID(OPEN_ID)
		/// </summary>
				
		[Column("User_Id")]
		public string UserId { get; set; }

		/// <summary>
		/// 交易时间
		/// </summary>
				
		[Column("Payment_Time")]
		public string PaymentTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Buyer_Logon_Id")]
		public string BuyerLogonId { get; set; }

		/// <summary>
		/// 充值返回信息
		/// </summary>
				
		[Column("Content")]
		public string Content { get; set; }

		/// <summary>
		/// 0失败 1成功
		/// </summary>
				
		[Column("Status")]
		public int Status { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Notify_Source")]
		public string NotifySource { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Source_Type")]
		public string SourceType { get; set; }
	}

	/// <summary>
	/// RegisterHealthCardErrorLog
	/// ()
	/// </summary>
	[Table("register_health_card_error_log")]
	public class RegisterHealthCardErrorLog
	{

		/// <summary>
		/// 
		/// </summary>
		[Key]		
		[Column("Id")]
		public long Id { get; set; }

		/// <summary>
		/// 请求数据
		/// </summary>
				
		[Column("Data")]
		public string Data { get; set; }

		/// <summary>
		/// 错误信息
		/// </summary>
				
		[Column("Info")]
		public string Info { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
				
		[Column("Create_Time")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("From")]
		public string From { get; set; }
	}

	/// <summary>
	/// RegisterHealthCardInfo
	/// ()
	/// </summary>
	[Table("register_health_card_info")]
	public class RegisterHealthCardInfo
	{

		/// <summary>
		/// 
		/// </summary>
		[Key]		
		[Column("Id")]
		public long Id { get; set; }

		/// <summary>
		/// 微信openid
		/// </summary>
				
		[Column("Openid")]
		public string Openid { get; set; }

		/// <summary>
		/// 医院id
		/// </summary>
				
		[Column("Patid")]
		public string Patid { get; set; }

		/// <summary>
		/// 二维码文本根据当地卫健委要求返回静态码或动态码
		/// </summary>
				
		[Column("Qrcode_Text")]
		public string QrcodeText { get; set; }

		/// <summary>
		/// 健康卡ID
		/// </summary>
				
		[Column("Health_Card_Id")]
		public string HealthCardId { get; set; }

		/// <summary>
		/// 主索引ID根据当地卫健委要求返回或者不返回
		/// </summary>
				
		[Column("Phid")]
		public string Phid { get; set; }

		/// <summary>
		/// 扩展字段返回卡管注册接口响应包的全部内容（不含图片数据）
		/// </summary>
				
		[Column("Admin_Ext")]
		public string AdminExt { get; set; }

		/// <summary>
		/// 证件号码	
		/// </summary>
				
		[Column("Id_Number")]
		public string IdNumber { get; set; }

		/// <summary>
		/// 证件类型
		/// </summary>
				
		[Column("Id_Type")]
		public string IdType { get; set; }

		/// <summary>
		/// 注册类型 1新用户 2批量注册
		/// </summary>
				
		[Column("Type")]
		public int Type { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Create_Time")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Mem_Id")]
		public int MemId { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Phone")]
		public string Phone { get; set; }

		/// <summary>
		/// 
		/// </summary>
				
		[Column("Status")]
		public int Status { get; set; }
	}

	
}
