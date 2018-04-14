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
        { }
        #region Model
        private int _salesid;
        private string _salesno;
        private string _useropenid;
        private string _receiver;
        private string _province;
        private string _city;
        private string _county;
        private string _detailaddress;
        private string _phone;
        private decimal? _amount;
        private decimal? _postage;
        private decimal? _adminchangeamount;
        private decimal? _adminchangepostage;
        private string _wechatsalesid;
        private DateTime _createdate;
        private DateTime? _paydate;
        private DateTime? _deliverydate;
        private int? _status;
        private int? _display = 1;
        private string _column1;
        private string _column2;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int salesId
        {
            set { _salesid = value; }
            get { return _salesid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string salesNo
        {
            set { _salesno = value; }
            get { return _salesno; }
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
        public decimal? amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? postage
        {
            set { _postage = value; }
            get { return _postage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? adminChangeAmount
        {
            set { _adminchangeamount = value; }
            get { return _adminchangeamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? adminChangePostage
        {
            set { _adminchangepostage = value; }
            get { return _adminchangepostage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string wechatSalesId
        {
            set { _wechatsalesid = value; }
            get { return _wechatsalesid; }
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
        public DateTime? payDate
        {
            set { _paydate = value; }
            get { return _paydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? deliveryDate
        {
            set { _deliverydate = value; }
            get { return _deliverydate; }
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
        public int? display
        {
            set { _display = value; }
            get { return _display; }
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

