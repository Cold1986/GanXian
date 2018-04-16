using System;
namespace GanXian.Model
{
	/// <summary>
	/// managerusers:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class managerusers
	{
		public managerusers()
		{}
		#region Model
		private int _userid;
		private string _accountname;
		private string _psd;
		private DateTime _createdate;
		private DateTime? _updatedate;
		private int? _status=1;
		private string _column1;
		private string _column2;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int userId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string accountName
		{
			set{ _accountname=value;}
			get{return _accountname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string psd
		{
			set{ _psd=value;}
			get{return _psd;}
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
		public DateTime? updateDate
		{
			set{ _updatedate=value;}
			get{return _updatedate;}
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

