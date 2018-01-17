using System;
namespace GanXian.Model
{
	/// <summary>
	/// district:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class district
	{
		public district()
		{}
		#region Model
		private int? _id;
		private string _name;
		private int? _parent_id;
		private string _initial;
		private string _initials;
		private string _pinyin;
		private string _extra;
		private string _suffix;
		private string _code;
		private string _area_code;
		private int? _order;
		/// <summary>
		/// 
		/// </summary>
		public int? id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? parent_id
		{
			set{ _parent_id=value;}
			get{return _parent_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string initial
		{
			set{ _initial=value;}
			get{return _initial;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string initials
		{
			set{ _initials=value;}
			get{return _initials;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pinyin
		{
			set{ _pinyin=value;}
			get{return _pinyin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string extra
		{
			set{ _extra=value;}
			get{return _extra;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string suffix
		{
			set{ _suffix=value;}
			get{return _suffix;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string code
		{
			set{ _code=value;}
			get{return _code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string area_code
		{
			set{ _area_code=value;}
			get{return _area_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? order
		{
			set{ _order=value;}
			get{return _order;}
		}
		#endregion Model

	}
}

