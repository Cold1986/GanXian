﻿using System;
namespace GanXian.Model
{
	/// <summary>
	/// tablist:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tablist
	{
		public tablist()
		{}
		#region Model
		private int _tabid;
		private string _typename;
		private int _isshow;
		private int _iscarousel;
		private string _remark;
		private DateTime? _createdate;
		private int? _sort;
		private int? _status;
		private string _column1;
		private string _column2;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int tabId
		{
			set{ _tabid=value;}
			get{return _tabid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string typeName
		{
			set{ _typename=value;}
			get{return _typename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int isShow
		{
			set{ _isshow=value;}
			get{return _isshow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int isCarousel
		{
			set{ _iscarousel=value;}
			get{return _iscarousel;}
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
		public int? sort
		{
			set{ _sort=value;}
			get{return _sort;}
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

