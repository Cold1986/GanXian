using System;
namespace GanXian.Model
{
    /// <summary>
    /// orderlog:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class orderlog
    {
        public orderlog()
        { }
        #region Model
        private int _id;
        private string _salesno;
        private string _message;
        private string _msgtype;
        private DateTime _createdate;
        private int? _status;
        private string _column1;
        private string _column2;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
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
        public string message
        {
            set { _message = value; }
            get { return _message; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string msgType
        {
            set { _msgtype = value; }
            get { return _msgtype; }
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

