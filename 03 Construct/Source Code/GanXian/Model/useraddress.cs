using System;
namespace GanXian.Model
{
    /// <summary>
    /// useraddress:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class useraddress
    {
        public useraddress()
        { }
        #region Model
        private int _id;
        private string _useropenid;
        private string _receiver;
        private string _province;
        private string _city;
        private string _county;
        private string _detailaddress;
        private string _phone;
        private string _setasdefault = "0";
        private DateTime _createdate;
        private DateTime? _updatedate;
        private int? _status = 1;
        private string _column1;
        private string _column2;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string userOpenId
        {
            set { _useropenid = value; }
            get { return _useropenid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string receiver
        {
            set { _receiver = value; }
            get { return _receiver; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string province
        {
            set { _province = value; }
            get { return _province; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string city
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string county
        {
            set { _county = value; }
            get { return _county; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string detailAddress
        {
            set { _detailaddress = value; }
            get { return _detailaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SetAsDefault
        {
            set { _setasdefault = value; }
            get { return _setasdefault; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime createDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? updateDate
        {
            set { _updatedate = value; }
            get { return _updatedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string column1
        {
            set { _column1 = value; }
            get { return _column1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string column2
        {
            set { _column2 = value; }
            get { return _column2; }
        }
        #endregion Model

    }
}

