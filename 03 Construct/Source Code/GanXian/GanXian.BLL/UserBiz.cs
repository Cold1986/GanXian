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
                    conn.Execute(sqlCommandText, user);
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

        public bool updateUserPhone(users user)
        {
            bool res = false;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"update Users set phone=@phone where openid =@openid ";
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
        /// 用户新增地址
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool insertUserAddress(useraddress user)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    if (user.SetAsDefault == "1")
                    {
                        //将该用户原先默认地址设为非默认
                        string updateSQL = "update useraddress set SetAsDefault=0 where status=1 and SetAsDefault=1 and userOpenId=@userOpenId ";
                        conn.Execute(updateSQL, user, transaction);
                    }
                    string sqlCommandText = @"insert into useraddress(userOpenId,receiver,province,city,county,detailAddress,Phone,SetAsDefault,createDate,status) values(@userOpenId,@receiver,@province,@city,@county,@detailAddress,@Phone,@SetAsDefault,@createDate,@status) ";
                    conn.Execute(sqlCommandText, user, transaction);
                    //提交事务
                    transaction.Commit();
                    res = true;
                }
                catch (Exception e)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// 更新用户收货地址
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool updateUserAddress(useraddress user)
        {
            bool res = false;
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    if (user.SetAsDefault == "1")
                    {
                        //将该用户原先默认地址设为非默认
                        string updateSQL = "update useraddress set SetAsDefault=0 where status=1 and SetAsDefault=1 and userOpenId=@userOpenId ";
                        conn.Execute(updateSQL, user, transaction);
                    }
                    string sqlCommandText = @"update useraddress set receiver=@receiver,province=@province,city=@city,county=@county,detailAddress=@detailAddress,Phone=@Phone,SetAsDefault=@SetAsDefault,updateDate=@updateDate where status=1 and userOpenId=@userOpenId and id=@Id";
                    conn.Execute(sqlCommandText, user, transaction);
                    //提交事务
                    transaction.Commit();
                    res = true;
                }
                catch (Exception e)
                {
                    //出现异常，事务Rollback
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return res;
        }
        /// <summary>
        /// 获取用户收货地址信息
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public List<useraddress> getUserAddressList(string userOpenId)
        {
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT a.userOpenId,a.Id,a.receiver,b.name as province,c.name as city ,d.name as county ,a.detailAddress,a.Phone,a.SetAsDefault 
                                            FROM ganxian.useraddress a
                                            inner join district b on a.province=b.id
                                            left join district c on a.city=c.id
                                            left join district d on a.county=d.id
                                            where a.status=1 and userOpenId=@userOpenId order by a.SetAsDefault desc,a.Id";
                List<useraddress> userAddressList = conn.Query<useraddress>(sqlCommandText, new { userOpenId = userOpenId }).ToList();
                return userAddressList;
            }
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="userOpenId">用户微信openId</param>
        /// <param name="addressId"地址id</param>
        /// <returns></returns>
        public bool deleteUserAddress(string userOpenId, string addressId)
        {
            bool res = false;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"update useraddress set status=0 where status=1 and userOpenId=@userOpenId and id=@id ";
                    conn.Execute(sqlCommandText, new { userOpenId = userOpenId, id = addressId });
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
        /// 将地址设为默认地址
        /// </summary>
        /// <param name="userOpenId">用户微信OpenId</param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public bool setDefaultAddress(string userOpenId, string addressId)
        {
            bool res = false;
            try
            {
                using (IDbConnection conn = DapperHelper.MySqlConnection())
                {
                    string sqlCommandText = @"update useraddress set SetAsDefault=0 where status=1 and userOpenId=@userOpenId and SetAsDefault=1;
                                                update useraddress set SetAsDefault=1 where status=1 and userOpenId=@userOpenId and id=@id;";
                    conn.Execute(sqlCommandText, new { userOpenId = userOpenId, id = addressId });
                    res = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
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

        /// <summary>
        /// 根据用户收货地址id获取具体信息
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public useraddress_extension getUserAddressById(string userOpenId, string addressId)
        {
            useraddress_extension userAddressInfo = new useraddress_extension();
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                try
                {
                    string sqlCommandText = @"SELECT a.*,b.name as provinceName,c.name as cityName ,d.name as countyName 
                                                FROM ganxian.useraddress a
                                                inner join district b on a.province = b.id
                                                left join district c on a.city = c.id
                                                left join district d on a.county = d.id
                                                where a.status = 1 and userOpenId =@userOpenId and a.id=@id ";
                    userAddressInfo = conn.Query<useraddress_extension>(sqlCommandText, new { userOpenId = userOpenId, id = addressId }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return userAddressInfo;
        }


        public List<users> getUserList()
        {
            using (IDbConnection conn = DapperHelper.MySqlConnection())
            {
                string sqlCommandText = @"SELECT distinct(nickname),case sex when 1 then '男' when 2 then '女' else '未知' end as sex,province,city,country,phone,createDate FROM ganxian.users order by createDate desc;";
                List<users> userList = conn.Query<users>(sqlCommandText).ToList();
                return userList;
            }
        }
    }
}
