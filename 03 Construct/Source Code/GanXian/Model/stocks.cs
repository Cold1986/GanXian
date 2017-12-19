using System;
namespace GanXian.Model
{
	/// <summary>
	/// stocks:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class stocks
	{
		public stocks()
		{}
		#region Model
		private int _id;
		private int _productid;
		private int _inventory;
		private int? _remindnum;
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
		public int productId
		{
			set{ _productid=value;}
			get{return _productid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int inventory
		{
			set{ _inventory=value;}
			get{return _inventory;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? remindNum
		{
			set{ _remindnum=value;}
			get{return _remindnum;}
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

