using System;
namespace GanXian.Model
{
	/// <summary>
	/// salesslip:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class salesslip
	{
		public salesslip()
		{}
		#region Model
		private int _salesid;
		private string _salesno;
		private string _useropenid;
		private string _province;
		private string _city;
		private string _county;
		private string _useraddress;
		private string _userphone;
		private decimal? _amount;
		private decimal? _postage;
		private string _wechatsalesid;
		private DateTime _createdate;
		private int? _status;
		private string _column1;
		private string _column2;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int salesId
		{
			set{ _salesid=value;}
			get{return _salesid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string salesNo
		{
			set{ _salesno=value;}
			get{return _salesno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string userOpenId
		{
			set{ _useropenid=value;}
			get{return _useropenid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string province
		{
			set{ _province=value;}
			get{return _province;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string city
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string county
		{
			set{ _county=value;}
			get{return _county;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string userAddress
		{
			set{ _useraddress=value;}
			get{return _useraddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string userPhone
		{
			set{ _userphone=value;}
			get{return _userphone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? postage
		{
			set{ _postage=value;}
			get{return _postage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string wechatSalesId
		{
			set{ _wechatsalesid=value;}
			get{return _wechatsalesid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime createDate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string column1
		{
			set{ _column1=value;}
			get{return _column1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string column2
		{
			set{ _column2=value;}
			get{return _column2;}
		}
		#endregion Model

	}
}

