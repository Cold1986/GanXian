using System;
namespace GanXian.Model
{
	/// <summary>
	/// sales2products:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sales2products
	{
		public sales2products()
		{}
		#region Model
		private int _id;
		private int _salesid;
		private int _productid;
		private string _originalprice;
		private string _discountedprice;
		private string _nw;
		private int _num;
		private string _salesprice;
		private DateTime _createdate;
		private int? _status;
		private string _column1;
		private string _column2;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int salesId
		{
			set{ _salesid=value;}
			get{return _salesid;}
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
		public string originalPrice
		{
			set{ _originalprice=value;}
			get{return _originalprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string discountedPrice
		{
			set{ _discountedprice=value;}
			get{return _discountedprice;}
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
		public int num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string salesPrice
		{
			set{ _salesprice=value;}
			get{return _salesprice;}
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

