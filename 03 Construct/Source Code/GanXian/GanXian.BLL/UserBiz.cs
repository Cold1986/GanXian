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


        private string getAllDistrict()
        {
            string res = string.Empty;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    //复制到mysql 中执行
 //                   string sqlCommandText = @" select  CONCAT("[", group_concat(json),"]")  from
 //(
 //-- 省
 // select case  when tb_cityJson.cityChild is not NULL then  CONCAT("{'id':'", id, "','name':'", name, "','child':", tb_cityJson.cityChild, "}")
 //   when  tb_cityJson.cityChild is NULL then CONCAT("{'id':'", id, "','name':'", name, "'}") end
 //   as json,
 //   name, a.parent_id  FROM district a
 //left join(
 //    select  CONCAT("[", group_concat(json), "]") as cityChild, parent_id from(
 //       select case  when tb_countyJson.countyChild is not NULL then  CONCAT("{'id':'", b.id, "','name':'", b.name, "','child':", tb_countyJson.countyChild, "}")

 //       when  tb_countyJson.countyChild is NULL then CONCAT("{'id':'", b.id, "','name':'", b.name, "'}") end
 //       as json,
 //       --tb_countyJson.countyChild,
 //       b.id, b.name, b.parent_id  FROM district b

 //       inner join district a on a.id = b.parent_id

 //       left join(
 //           select  CONCAT("[", group_concat(json), "]") as countyChild, parent_id from(
 //             select CONCAT("{'id':'", c.id, "','name':'", c.name, "'}") as json, c.id, c.name, c.parent_id  FROM district c

 //            inner join district b on b.id = c.parent_id

 //           inner join district a on a.id = b.parent_id

 //            where a.parent_id = 0
 //            ) tb group by parent_id
 //       ) as tb_countyJson on b.id = tb_countyJson.parent_id

 //        where a.parent_id = 0
 //   ) tb group by parent_id
 //) as tb_cityJson on a.id = tb_cityJson.parent_id
 //where a.parent_id = 0
 //) tbAll group by parent_id";
                    //res = conn.ExecuteScalar(sqlCommandText).ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }
    }
}
