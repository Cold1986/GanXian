using CommonLib;
using Dapper;
using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanXian.BLL
{
    public class UserBiz
    {
        public static UserBiz CreateNew()
        {
            return new UserBiz();
        }

        /// <summary>
        /// 根据openId获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public users getUserInfoByOpenId(string openId)
        {
            users user = new users();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string sqlCommandText = "select * from Users where openid=@openid";
                    user = conn.Query<users>(sqlCommandText, new { openid = openId }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return user;
        }

        /// <summary>
        /// 插入用户微信信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool insertUserWeChatInfo(users user)
        {
            bool res = false;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"insert into Users(openid,nickname,sex,province,city,country,headimgurl,privilege,unionid,createDate,updateDate,status) values(@openid,@nickname,@sex,@province,@city,@country,@headimgurl,@privilege,@unionid,@createDate,@updateDate,@status) ";
                    conn.Query(sqlCommandText, user);
                    res = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        /// <summary>
        /// 更新用户微信信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool updateUserWeChatInfo(users user)
        {
            bool res = false;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"update Users set nickname=@nickname,sex=@sex,province=@province,city=@city,country=@country,headimgurl=@headimgurl,privilege=@privilege,updateDate=now(),unionid=@unionid where openid =@openid ";
                    conn.Query(sqlCommandText, user);
                    res = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        public void insertUserPhone()
        {

        }

        public void updateUserPhone()
        {

        }
    }
}
