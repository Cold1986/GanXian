using System;
namespace GanXian.Model
{
	/// <summary>
	/// productchangelog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class productchangelog
	{
		public productchangelog()
		{}
		#region Model
		private int _logid;
		private string _operator;
		private DateTime _logtime;
		private int _productid;
		private string _productname;
		private string _specs;
		private decimal _originalprice;
		private decimal? _discountedprice;
		private DateTime? _discountedexpireddate;
		private decimal? _cost;
		private string _pic1;
		private string _pic2;
		private string _pic3;
		private string _pic4;
		private string _showpic;
		private string _origin;
		private string _nw;
		private string _storagecondition;
		private string _remark;
		private DateTime? _createdate;
		private int? _status=1;
		private string _column1;
		private string _column2;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int LogId
		{
			set{ _logid=value;}
			get{return _logid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Operator
		{
			set{ _operator=value;}
			get{return _operator;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LogTime
		{
			set{ _logtime=value;}
			get{return _logtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int productId
		{
			set{ _productid=value;}
			get{return _productid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string specs
		{
			set{ _specs=value;}
			get{return _specs;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal originalPrice
		{
			set{ _originalprice=value;}
			get{return _originalprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? discountedPrice
		{
			set{ _discountedprice=value;}
			get{return _discountedprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? discountedExpiredDate
		{
			set{ _discountedexpireddate=value;}
			get{return _discountedexpireddate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? cost
		{
			set{ _cost=value;}
			get{return _cost;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pic1
		{
			set{ _pic1=value;}
			get{return _pic1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pic2
		{
			set{ _pic2=value;}
			get{return _pic2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pic3
		{
			set{ _pic3=value;}
			get{return _pic3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pic4
		{
			set{ _pic4=value;}
			get{return _pic4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string showPic
		{
			set{ _showpic=value;}
			get{return _showpic;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string origin
		{
			set{ _origin=value;}
			get{return _origin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string nw
		{
			set{ _nw=value;}
			get{return _nw;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string storageCondition
		{
			set{ _storagecondition=value;}
			get{return _storagecondition;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createDate
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

