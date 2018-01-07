using System;
namespace GanXian.Model
{
    /// <summary>
    /// shoppingcart:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class shoppingcart
    {
        public shoppingcart()
        { }
        #region Model
        private int _cartid;
        private string _useropenid;
        private int _productid;
        private int _num;
        private DateTime _createdate;
        private int? _status;
        private string _column1;
        private string _column2;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int cartId
        {
            set { _cartid = value; }
            get { return _cartid; }
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
        public int productId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
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

