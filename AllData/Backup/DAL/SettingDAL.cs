using System;
using System.Collections.Generic;
using System.Text;
//using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using Book.DAL;     // sqlhelper

namespace DAL
{

    public class SettingDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AllDataConnectionString"].ConnectionString);

        public int getInt(string int2string)
        {
            int back = -1;
            try
            {
                back = Convert.ToInt32(int2string);
                return back;
            }
            catch
            {
                return back;
            }
        }

        public int setStationsInfo(Model.StationRelation msr,string command)//insert\update\delete
        {

            int flag = 1;//0表示操作成功，1表示操作失败

            SqlCommand CustomerComm;
            SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;

            switch (command)
            {
                case "update":

                    CustomerComm = new SqlCommand("updateStationInfo", con);
                    CustomerComm.CommandType = CommandType.StoredProcedure;

                    par0 = new SqlParameter("@station_id", msr.StationID);
                    par1 = new SqlParameter("@station_name", msr.StationName);
                    par2 = new SqlParameter("@id", msr.StationTableID);

                    CustomerComm.Parameters.Add(par0);
                    CustomerComm.Parameters.Add(par1);
                    CustomerComm.Parameters.Add(par2);
                    break;

                case "delete":

                    CustomerComm = new SqlCommand("deleteStationInfo", con);
                    CustomerComm.CommandType = CommandType.StoredProcedure;

                    par2 = new SqlParameter("@id", msr.StationTableID);

                    CustomerComm.Parameters.Add(par2);
                    break;

                default://insert
                    CustomerComm = new SqlCommand("insertStationInfo", con);
                    CustomerComm.CommandType = CommandType.StoredProcedure;

                    par0 = new SqlParameter("@station_id", msr.StationID);
                    par1 = new SqlParameter("@station_name", msr.StationName);

                    CustomerComm.Parameters.Add(par0);
                    CustomerComm.Parameters.Add(par1);
                    break;
            }

            int done;
            string result = "";

            try
            {
                con.Open();
                done = CustomerComm.ExecuteNonQuery();
                flag = 0;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return flag;

        }






        public Model.StationRelation insertStationsInfoReturnID(Model.StationRelation msr)//insert
        {
            /*
                        int flag = 1;//0表示操作成功，1表示操作失败

                        SqlCommand CustomerComm;
                        SqlParameter par0;
                        SqlParameter par1;



                        CustomerComm = new SqlCommand("insertStationInfoReturnID", con);
                                CustomerComm.CommandType = CommandType.StoredProcedure;

                                par0 = new SqlParameter("@station_id", msr.StationID);
                                par1 = new SqlParameter("@station_name", msr.StationName);

                                CustomerComm.Parameters.Add(par0);
                                CustomerComm.Parameters.Add(par1);


                        int done;
                        string result = "";

                        try
                        {
                            con.Open();
                            done = CustomerComm.ExecuteNonQuery();
                            flag = 0;
                        }
                        catch (Exception ex)
                        {
                            result = ex.ToString();
                        }
                        finally
                        {
                            con.Close();
                        }

                        return flag;

            */


            SqlDataAdapter da = new SqlDataAdapter("insertStationInfoReturnID", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            SqlParameter par1;
            par0 = new SqlParameter("@station_id", msr.StationID);
            par1 = new SqlParameter("@station_name", msr.StationName);
            da.SelectCommand.Parameters.Add(par0);
            da.SelectCommand.Parameters.Add(par1);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            Model.StationRelation msrback = new Model.StationRelation();
            if (result == "")
            {

                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                msrback.SelectID = getInt(dt.Rows[0]["id"].ToString());
                msrback.StationTableID = getInt(dt.Rows[0]["id"].ToString());

            }
            else
            {
                return null;
            }

            return msrback;





        }










        public int setCP(Model.ChannelProgram mcp,string command)//增加cp表内容，频道和栏目
        {

            int flag = 1;//0表示操作成功，1表示操作失败

            SqlCommand CustomerComm;
            SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;

            switch (command)
            {
                case "update":

                    CustomerComm = new SqlCommand("updateCP", con);
                    CustomerComm.CommandType = CommandType.StoredProcedure;

                    par0 = new SqlParameter("@id", mcp.CP_ID);
                    par1 = new SqlParameter("@cp_name", mcp.CP_Name);

                    CustomerComm.Parameters.Add(par0);
                    CustomerComm.Parameters.Add(par1);
                    break;

                case "delete":
                    
                    CustomerComm = new SqlCommand("deleteCP", con);
                    CustomerComm.CommandType = CommandType.StoredProcedure;

                    par0 = new SqlParameter("@id", mcp.CP_ID);

                    CustomerComm.Parameters.Add(par0);
                    break;

                default://insert
                    CustomerComm = new SqlCommand("insertCP", con);
                    CustomerComm.CommandType = CommandType.StoredProcedure;

                    par0 = new SqlParameter("@cp_name", mcp.CP_Name);
                    par1 = new SqlParameter("@father_id", mcp.FatherID);
                    par2 = new SqlParameter("@cp_order", 999);

                    CustomerComm.Parameters.Add(par0);
                    CustomerComm.Parameters.Add(par1);
                    CustomerComm.Parameters.Add(par2);
                    break;
            }

            int done;
            string result = "";

            try
            {
                con.Open();
                done = CustomerComm.ExecuteNonQuery();
                flag = 0;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return flag;
        }


        public int setCPnewFather(Model.ChannelProgram mcp)//更改栏目所在频道
        {
            int flag = 1;//0表示操作成功，1表示操作失败

            SqlCommand CustomerComm;
            SqlParameter par0;
            SqlParameter par1;

            CustomerComm = new SqlCommand("updateCPfid", con);
            CustomerComm.CommandType = CommandType.StoredProcedure;

            par0 = new SqlParameter("@id", mcp.CP_ID);
            par1 = new SqlParameter("@fid", mcp.FatherID);

            CustomerComm.Parameters.Add(par0);
            CustomerComm.Parameters.Add(par1);

            int done;
            string result = "";

            try
            {
                con.Open();
                done = CustomerComm.ExecuteNonQuery();
                flag = 0;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return flag;
        }



        public int setCPnewOrder(Model.ChannelProgram mcp)
        {
            int flag = 1;//0表示操作成功，1表示操作失败

            SqlCommand CustomerComm;
            SqlParameter par0;
            SqlParameter par1;

            CustomerComm = new SqlCommand("updateCPorder", con);
            CustomerComm.CommandType = CommandType.StoredProcedure;

            par0 = new SqlParameter("@id", mcp.CP_ID);
            par1 = new SqlParameter("@cp_order", mcp.CP_Order);

            CustomerComm.Parameters.Add(par0);
            CustomerComm.Parameters.Add(par1);

            int done=0;
            string result = "";

            try
            {
                con.Open();
                done = CustomerComm.ExecuteNonQuery();
                flag = 0;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }


            if (done > 0)
            {
                return flag;
            }



            return 1;
        }



        public String[] setAllSetting(Model.ChannelProgram mcp, Model.ElementRelation[] mer,Model.StationRelation[] msr)
        {

            //更新栏目的数据类型
            String[] flag = new String[5];

            SqlCommand CustomerCommUpdateCP;
            SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;

            CustomerCommUpdateCP = new SqlCommand("updateCPdate", con);
            CustomerCommUpdateCP.CommandType = CommandType.StoredProcedure;

            par0 = new SqlParameter("@id", mcp.CP_ID);
            par1 = new SqlParameter("@dtid", mcp.DataTypeID);
            par2 = new SqlParameter("@ttid", mcp.TimeTypeID);

            CustomerCommUpdateCP.Parameters.Add(par0);
            CustomerCommUpdateCP.Parameters.Add(par1);
            CustomerCommUpdateCP.Parameters.Add(par2);

            int done;
            string result = "";

            try
            {
                con.Open();
                done = CustomerCommUpdateCP.ExecuteNonQuery();
                flag[0] = "done";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                flag[0] = "error";
            }
            finally
            {
                con.Close();
            }

            //删除现有elements的配置
            SqlCommand CustomerCommDelEleSetting;
            SqlParameter par3;
            CustomerCommDelEleSetting = new SqlCommand("deleteEleSetting", con);
            CustomerCommDelEleSetting.CommandType = CommandType.StoredProcedure;
            par3 = new SqlParameter("@cpid", mcp.CP_ID);
            CustomerCommDelEleSetting.Parameters.Add(par3);

            try
            {
                con.Open();
                done = CustomerCommDelEleSetting.ExecuteNonQuery();
                flag[1] = "done";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                flag[1] = "error";
            }
            finally
            {
                con.Close();
            }

            //删除现有station的配置
            SqlCommand CustomerCommDelStationSetting;
            SqlParameter par4;
            CustomerCommDelStationSetting = new SqlCommand("deleteStationSetting", con);
            CustomerCommDelStationSetting.CommandType = CommandType.StoredProcedure;
            par4 = new SqlParameter("@cpid", mcp.CP_ID);
            CustomerCommDelStationSetting.Parameters.Add(par4);

            try
            {
                con.Open();
                done = CustomerCommDelStationSetting.ExecuteNonQuery();
                flag[2] = "done";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                flag[2] = "error";
            }
            finally
            {
                con.Close();
            }




            //插入天气元素配置
            for (int i = 0; i < mer.Length; i++)
            {
                if (insertElementSetting(mer[i].ElementID, mcp.CP_ID, mer[i].ElementOrder))
                {
                    flag[3] = "done";
                }
                else
                {
                    flag[3] = "error";
                    i = mer.Length + 1;
                }
            }


            //插入站点配置
            for (int i = 0; i < msr.Length; i++)
            {
                if (insertStationSetting(msr[i].SelectID, msr[i].StationID, mcp.CP_ID, msr[i].StationOrder))
                {
                    flag[4] = "done";
                }
                else
                {
                    flag[4] = "error";
                    i = msr.Length + 1;
                }
            }


            return flag;


        }


        public Boolean insertElementSetting(int eid, int cpid, int order)
        {
            Boolean flag = false;

            SqlCommand CustomerComm;
            SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;

            CustomerComm = new SqlCommand("insertEleSetting", con);
            CustomerComm.CommandType = CommandType.StoredProcedure;

            par0 = new SqlParameter("@etid", eid);
            par1 = new SqlParameter("@cpid", cpid);
            par2 = new SqlParameter("@et_order", order);

            CustomerComm.Parameters.Add(par0);
            CustomerComm.Parameters.Add(par1);
            CustomerComm.Parameters.Add(par2);

            int done;
            string result = "";

            try
            {
                con.Open();
                done = CustomerComm.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return flag;


        }



        public Boolean insertStationSetting(int selectid, int stationid, int cpid, int s_order)
        {
            Boolean flag = false;

            SqlCommand CustomerComm;
            SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            SqlParameter par3;

            CustomerComm = new SqlCommand("insertStationSetting", con);
            CustomerComm.CommandType = CommandType.StoredProcedure;

            par0 = new SqlParameter("@s_id", selectid);
            par1 = new SqlParameter("@stationid", stationid);
            par2 = new SqlParameter("@cpid", cpid);
            par3 = new SqlParameter("@s_order", s_order);

            CustomerComm.Parameters.Add(par0);
            CustomerComm.Parameters.Add(par1);
            CustomerComm.Parameters.Add(par2);
            CustomerComm.Parameters.Add(par3);

            int done;
            string result = "";

            try
            {
                con.Open();
                done = CustomerComm.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return flag;

        }



        // ----------------------------------------------------------------------------------
        // Added by Edward Chan.

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="mu"></param>
        /// <returns></returns>
        public bool addUser(Model.User mu)
        {
            string sql = "INSERT INTO dbo.tb_User ( id, name, pass, role, state, comm ) ";
            sql += "VALUES ( @userid, @username, @userpwd, @userrole, @userstate, @usercomm ) ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", mu.UserID), 
                    new SqlParameter("@userpwd", mu.Password), 
                    new SqlParameter("@username", mu.UserName), 
                    new SqlParameter("@userrole", mu.UserRole), 
                    new SqlParameter("@userstate", mu.UserState), 
                    new SqlParameter("@usercomm", mu.UserComment)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval == 1;

        }


        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="mu"></param>
        /// <returns></returns>
        public bool updateUser(Model.User mu)
        {
            string sql = "UPDATE dbo.tb_User ";
            sql += "SET name = @username, role = @userrole, state = @userstate ";
            sql += "WHERE id = @userid ";

            SqlParameter[] paras = {
                    new SqlParameter("@username", mu.UserName), 
                    //new SqlParameter("@userpwd", mu.Password), 
                    new SqlParameter("@userrole", mu.UserRole), 
                    new SqlParameter("@userstate", mu.UserState), 
                    //new SqlParameter("@usercomm", mu.UserComment), 
                    new SqlParameter("@userid", mu.UserID)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval == 1;

        }


        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="mu"></param>
        /// <returns></returns>
        public bool updatePwd(Model.User mu)
        {
            string sql = "UPDATE dbo.tb_User ";
            sql += "SET pass = @userpwd ";
            sql += "WHERE id = @userid ";

            SqlParameter[] paras = {
                    new SqlParameter("@userpwd", mu.Password), 
                    new SqlParameter("@userid", mu.UserID)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval == 1;

        }


        /*
        public bool updateUser2(Model.User mu)
        {
            string sql = "DECLARE @sql NVARCHAR(MAX)";
            sql += "SET @sql = N'UPDATE dbo.tb_User SET name = @username '";
            sql += "+ CASE WHEN @userpwd IS NULL THEN '' ELSE N', pass = @userpwd' END ";
            sql += "+ N', role = @userrole , state = @userstate'";
            sql += "+ CASE WHEN @usercomm IS NULL THEN '' ELSE N', comm = @usercomm' END ";
            sql += "+ N' WHERE id = @userid'";
            sql += " EXEC sp_executesql @sql";

            SqlParameter[] paras = {
                    new SqlParameter("@username", mu.UserName), 
                    new SqlParameter("@userpwd", mu.Password), 
                    new SqlParameter("@userrole", mu.UserRole), 
                    new SqlParameter("@userstate", mu.UserState), 
                    new SqlParameter("@usercomm", mu.UserComment), 
                    new SqlParameter("@userid", mu.UserID)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval == 1;

        }
        */


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool deleteUser(string id)
        {
            string sql = "DELETE FROM dbo.tb_User ";
            sql += "WHERE id = @userid ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", id)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval > 0;

        }


        /// <summary>
        /// 添加频道栏目用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cpID"></param>
        /// <returns></returns>
        public bool addCPUser(string userID, int cpID)
        {
            string sql = "INSERT INTO dbo.tb_ChannelProgramUser ( UR_id, CP_id ) ";
            sql += "VALUES ( @userid, @cpid ) ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", userID), 
                    new SqlParameter("@cpid", cpID)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval == 1;
        }


        /// <summary>
        /// 删除频道栏目用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cpID"></param>
        /// <returns></returns>
        public bool delCPUser(string userID, int cpID)
        {
            string sql = "DELETE FROM dbo.tb_ChannelProgramUser ";
            sql += "WHERE UR_id = @userid AND CP_id = @cpid ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", userID), 
                    new SqlParameter("@cpid", cpID)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval == 1;
        }
        
        
        /// <summary>
        /// 添加站点配置信息
        /// </summary>
        /// <param name="msr"></param>
        /// <param name="selectID"></param>
        /// <returns></returns>
        public bool addStationInfoWithSetting(Model.StationRelation msr, out int selectID)
        {
            SqlParameter outpara = new SqlParameter("@selid", SqlDbType.Int, 4);
            outpara.Direction = ParameterDirection.Output;

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", msr.CP_ID), 
                    new SqlParameter("@staid", msr.StationID), 
                    new SqlParameter("@staname", msr.StationName), 
                    outpara
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_addStationInfoWithSetting", paras);

            selectID = Convert.ToInt32(outpara.Value);

            return retval > 0;
        }


        /// <summary>
        /// 更新指定栏目的时间类型和数据类型
        /// </summary>
        /// <param name="cpID"></param>
        /// <param name="dtID"></param>
        /// <param name="ttID"></param>
        /// <returns></returns>
        public bool updateCPDataTypeAndTimeType(int cpID, int dtID, int ttID)
        {
            string sql = "UPDATE dbo.tb_ChannelProgram ";
            sql += "SET TT_id = @ttid, DT_id = @dtid ";
            sql += "WHERE id = @id ";

            SqlParameter[] paras = {
                    new SqlParameter("@ttid", ttID), 
                    new SqlParameter("@dtid", dtID), 
                    new SqlParameter("@id", cpID)
            };

            int retval = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
            return retval == 1;
        }


        /// <summary>
        /// 添加栏目配置信息
        /// </summary>
        /// <param name="cpID">栏目ID</param>
        /// <param name="eleStr">预报要素ID集合</param>
        /// <param name="staStr">站点条目集合</param>
        /// <returns></returns>
        public bool addElementAndStationSetting(int cpID, string eleStr, string staStr)
        {
            SqlParameter retpara = new SqlParameter("@ReturnValue", SqlDbType.Int, 4);
            retpara.Direction = ParameterDirection.ReturnValue;

            SqlParameter[] paras = {
                    new SqlParameter("@cpId", cpID), 
                    new SqlParameter("@eleArr", eleStr), 
                    new SqlParameter("@staArr", staStr), 
                    retpara
            };

            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_addElementAndStationSetting", paras);

            return Convert.ToInt32(retpara.Value) == 0;
        }

    }

}
