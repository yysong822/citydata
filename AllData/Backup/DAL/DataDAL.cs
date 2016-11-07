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
    public class DataDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AllDataConnectionString"].ConnectionString);
        SqlConnection weathercon = new SqlConnection(ConfigurationManager.ConnectionStrings["WeatherDataConnectionString"].ConnectionString);


        public Model.ChannelProgram[] getCP(String fatherid)
        {
            SqlDataAdapter da = new SqlDataAdapter("getProgrambyFatherid", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@fatherid", Convert.ToInt32(fatherid));
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;

            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            Model.ChannelProgram[] mcp;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mcp = new Model.ChannelProgram[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ChannelProgram mcp_single = new Model.ChannelProgram();

                    mcp_single.CP_ID = getInt(dt.Rows[i]["id"].ToString());
                    mcp_single.CP_Name = dt.Rows[i]["CP_name"].ToString();
                    mcp_single.FatherID = getInt(dt.Rows[i]["FatherID"].ToString());
                    mcp_single.TimeTypeID = getInt(dt.Rows[i]["TT_id"].ToString());
                    mcp_single.DataTypeID = getInt(dt.Rows[i]["DT_id"].ToString());
                    mcp_single.CP_Order = getInt(dt.Rows[i]["CP_order"].ToString());

                    mcp[i] = mcp_single;
                }
            }
            else
            {
                return null;
            }

            return mcp;



        }


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



        public Model.RecordeData[] getData(String cpid)
        {
            string ttid = "";
            string dtid = "";
            SqlDataAdapter da = new SqlDataAdapter("getCP", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);

                DataRow dr = ds.Tables[0].Rows[0];
                ttid = dr["TT_id"].ToString();
                dtid = dr["DT_id"].ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }


            if (ttid == "" || dtid == "")
            {
                return null;
            }


            //对显示内容的数据进行日期限定，只返回当天或前一天数据
            int count = 0;
            string reporttime = DateTime.Today.ToShortDateString() + " " + ttid + ":00:00";
            da = new SqlDataAdapter("getNumberOfRecorde", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par4;
            SqlParameter par5;
            SqlParameter par6;
            par4 = new SqlParameter("@dtid", Convert.ToInt32(dtid));
            par5 = new SqlParameter("@ttid", Convert.ToInt32(ttid));
            par6 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));
            da.SelectCommand.Parameters.Add(par4);
            da.SelectCommand.Parameters.Add(par5);
            da.SelectCommand.Parameters.Add(par6);
            ds = new DataSet();
            //string result = "";
            try
            {
                con.Open();
                da.Fill(ds);

                DataRow dr = ds.Tables[0].Rows[0];
                count = Convert.ToInt32(dr["num"].ToString());
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                count = -1;
            }
            finally
            {
                con.Close();
            }

            if (count < 0)
            {
                return null;
            }

            if (count == 0)
            {
                reporttime = DateTime.Today.AddDays(-1).ToShortDateString() + " " + ttid + ":00:00";
            }



            //-----获得数据--------
            da = new SqlDataAdapter("getData", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            SqlParameter par3;

            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            par1 = new SqlParameter("@dtid", Convert.ToInt32(dtid));
            par2 = new SqlParameter("@ttid", Convert.ToInt32(ttid));
            par3 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));

            da.SelectCommand.Parameters.Add(par0);
            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);
            da.SelectCommand.Parameters.Add(par3);

            ds = new DataSet();
            result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //return null;
            }
            finally
            {
                con.Close();

            }

            //-------------------------

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                    mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                    mrd_single.ReportTime = Convert.ToDateTime(dt.Rows[i]["ReportTime"].ToString());
                    mrd_single.BeginTime = Convert.ToDateTime(dt.Rows[i]["TimeBegin"].ToString());
                    mrd_single.OverTime = Convert.ToDateTime(dt.Rows[i]["TimeOver"].ToString());
                    mrd_single.TimeType = getInt(dt.Rows[i]["TimeType"].ToString());
                    mrd_single.DataType = getInt(dt.Rows[i]["DataType"].ToString());
                    mrd_single.DataTypeName = dt.Rows[i]["DataTypeName"].ToString();
                    mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();

                    //mrd_single.TimeTypeOrder = getInt(dt.Rows[i]["TT_Order"].ToString());
                    //mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());

                    mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                    mrd_single.Element01 = dt.Rows[i]["Element01"].ToString();
                    mrd_single.Element02 = dt.Rows[i]["Element02"].ToString();
                    mrd_single.Element03 = dt.Rows[i]["Element03"].ToString();
                    mrd_single.Element04 = dt.Rows[i]["Element04"].ToString();
                    mrd_single.Element05 = dt.Rows[i]["Element05"].ToString();
                    mrd_single.Element06 = dt.Rows[i]["Element06"].ToString();
                    mrd_single.Element07 = dt.Rows[i]["Element07"].ToString();
                    mrd_single.Element08 = dt.Rows[i]["Element08"].ToString();
                    mrd_single.Element09 = dt.Rows[i]["Element09"].ToString();

                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;



        }



        /// <summary>
        /// 查询指定栏目的城市预报数据（包含无数据城市）
        /// 
        /// 说明：参数中的时间类型编号和预报时间不能同时为空
        /// 1. 时间类型编号为空，必须指定预报时间，返回该预报时间的数据。
        /// 2. 预报时间为空，必须指定时间类型编号，返回当天的预报数据，
        ///    若没有当天的数据，则返回昨天的数据。
        /// 
        /// </summary>
        /// <param name="cpID">栏目编号</param>
        /// <param name="dtID">数据类型编号</param>
        /// <param name="ttID">时间类型编号</param>
        /// <param name="reptTime">预报时间</param>
        /// <param name="time">预报时效</param>
        /// <returns>RecordeData2</returns>
        public Model.RecordeData2[] getData2(int cpID, int dtID, int? ttID, DateTime? reptTime, int? time)
        {
            // 检查ttID和reptTime不能同时为空
            if (ttID == null && reptTime == null)
                return null;

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", cpID), 
                    new SqlParameter("@dtid", dtID), 
                    new SqlParameter("@ttid", ttID), 
                    new SqlParameter("@rptime", reptTime), 
                    new SqlParameter("@time", time)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "sp_getData", paras);

            DataTable dt = new DataTable("AllDataRecorde");
            dt = ds.Tables[0];

            Model.RecordeData2[] mrds = new Model.RecordeData2[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mrds = new Model.RecordeData2[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData2 mrd = new Model.RecordeData2();

                    mrd.StationID = Convert.ToInt32(dt.Rows[i]["stationid"]);
                    mrd.StationName = dt.Rows[i]["name_cn"].ToString();
                    mrd.stationOrder = Convert.ToInt32(dt.Rows[i]["stationOrder"]);

                    // 以下字段可空
                    mrd.Time = Convert.IsDBNull(dt.Rows[i]["Time"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["Time"]);
                    mrd.ReportTime = Convert.IsDBNull(dt.Rows[i]["ReportTime"]) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[i]["ReportTime"]);
                    mrd.BeginTime = Convert.IsDBNull(dt.Rows[i]["TimeBegin"]) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[i]["TimeBegin"]);
                    mrd.OverTime = Convert.IsDBNull(dt.Rows[i]["TimeOver"]) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[i]["TimeOver"]);
                    mrd.TimeType = Convert.IsDBNull(dt.Rows[i]["TimeType"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["TimeType"]);
                    mrd.DataType = Convert.IsDBNull(dt.Rows[i]["DataType"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["DataType"]);

                    mrd.Element01 = Convert.IsDBNull(dt.Rows[i]["Element01"]) ? null : dt.Rows[i]["Element01"].ToString();
                    mrd.Element02 = Convert.IsDBNull(dt.Rows[i]["Element02"]) ? null : dt.Rows[i]["Element02"].ToString();
                    mrd.Element03 = Convert.IsDBNull(dt.Rows[i]["Element03"]) ? null : dt.Rows[i]["Element03"].ToString();
                    mrd.Element04 = Convert.IsDBNull(dt.Rows[i]["Element04"]) ? null : dt.Rows[i]["Element04"].ToString();
                    mrd.Element05 = Convert.IsDBNull(dt.Rows[i]["Element05"]) ? null : dt.Rows[i]["Element05"].ToString();
                    mrd.Element06 = Convert.IsDBNull(dt.Rows[i]["Element06"]) ? null : dt.Rows[i]["Element06"].ToString();
                    mrd.Element07 = Convert.IsDBNull(dt.Rows[i]["Element07"]) ? null : dt.Rows[i]["Element07"].ToString();
                    mrd.Element08 = Convert.IsDBNull(dt.Rows[i]["Element08"]) ? null : dt.Rows[i]["Element08"].ToString();
                    mrd.Element09 = Convert.IsDBNull(dt.Rows[i]["Element09"]) ? null : dt.Rows[i]["Element09"].ToString();

                    mrds[i] = mrd;
                }

            }

            return mrds;
        }


        /// <summary>
        /// 查询指定栏目的城市预报数据（包含无数据城市）
        /// 
        /// 说明：参数中的时间类型编号和预报时间不能同时为空
        /// 1. 时间类型编号为空，必须指定预报时间，返回该预报时间的数据。
        /// 2. 预报时间为空，必须指定时间类型编号，返回当天的预报数据，
        ///    若没有当天的数据，则返回昨天的数据。
        /// 3. 预报时效数组的各时效之间使用#分隔，如：24#48#72。
        ///    
        /// </summary>
        /// <param name="cpID">栏目编号</param>
        /// <param name="dtID">数据类型编号</param>
        /// <param name="ttID">时间类型编号</param>
        /// <param name="reptTime">预报时间</param>
        /// <param name="times">预报时效数组</param>
        /// <returns>RecordeData2</returns>
        public Model.RecordeData2[] getData2X(int cpID, int dtID, int? ttID, DateTime? reptTime, string times)
        {
            // 检查ttID和reptTime不能同时为空
            if (ttID == null && reptTime == null)
                return null;

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", cpID), 
                    new SqlParameter("@dtid", dtID), 
                    new SqlParameter("@ttid", ttID), 
                    new SqlParameter("@rptime", reptTime), 
                    new SqlParameter("@times", times)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "sp_getDataX", paras);

            DataTable dt = new DataTable("AllDataRecorde");
            dt = ds.Tables[0];

            Model.RecordeData2[] mrds = new Model.RecordeData2[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mrds = new Model.RecordeData2[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData2 mrd = new Model.RecordeData2();

                    mrd.StationID = Convert.ToInt32(dt.Rows[i]["stationid"]);
                    mrd.StationName = dt.Rows[i]["name_cn"].ToString();
                    mrd.stationOrder = Convert.ToInt32(dt.Rows[i]["stationOrder"]);

                    // 以下字段可空
                    mrd.Time = Convert.IsDBNull(dt.Rows[i]["Time"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["Time"]);
                    mrd.ReportTime = Convert.IsDBNull(dt.Rows[i]["ReportTime"]) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[i]["ReportTime"]);
                    mrd.BeginTime = Convert.IsDBNull(dt.Rows[i]["TimeBegin"]) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[i]["TimeBegin"]);
                    mrd.OverTime = Convert.IsDBNull(dt.Rows[i]["TimeOver"]) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[i]["TimeOver"]);
                    mrd.TimeType = Convert.IsDBNull(dt.Rows[i]["TimeType"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["TimeType"]);
                    mrd.DataType = Convert.IsDBNull(dt.Rows[i]["DataType"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["DataType"]);

                    mrd.Element01 = Convert.IsDBNull(dt.Rows[i]["Element01"]) ? null : dt.Rows[i]["Element01"].ToString();
                    mrd.Element02 = Convert.IsDBNull(dt.Rows[i]["Element02"]) ? null : dt.Rows[i]["Element02"].ToString();
                    mrd.Element03 = Convert.IsDBNull(dt.Rows[i]["Element03"]) ? null : dt.Rows[i]["Element03"].ToString();
                    mrd.Element04 = Convert.IsDBNull(dt.Rows[i]["Element04"]) ? null : dt.Rows[i]["Element04"].ToString();
                    mrd.Element05 = Convert.IsDBNull(dt.Rows[i]["Element05"]) ? null : dt.Rows[i]["Element05"].ToString();
                    mrd.Element06 = Convert.IsDBNull(dt.Rows[i]["Element06"]) ? null : dt.Rows[i]["Element06"].ToString();
                    mrd.Element07 = Convert.IsDBNull(dt.Rows[i]["Element07"]) ? null : dt.Rows[i]["Element07"].ToString();
                    mrd.Element08 = Convert.IsDBNull(dt.Rows[i]["Element08"]) ? null : dt.Rows[i]["Element08"].ToString();
                    mrd.Element09 = Convert.IsDBNull(dt.Rows[i]["Element09"]) ? null : dt.Rows[i]["Element09"].ToString();

                    mrds[i] = mrd;
                }

            }

            return mrds;
        }



        /// <summary>
        /// syy 20130627 增加前一天数据检索，以便进行最高温和最低温的对比
        /// </summary>
        /// <param name="cpid"></param>
        /// <param name="ttid"></param>
        /// <param name="dtid"></param>
        /// <param name="reporttime"></param>
        /// <returns></returns>
        public Model.RecordeData[] getDataPre(String cpid, String ttid, String dtid, String reporttime)
        {
            try
            {
                DateTime rt_dt = DateTime.ParseExact(reporttime, "yyyy-M-d H:mm:ss", null);
                DateTime rt_pre = rt_dt.AddDays(-1);

                //-----获得数据--------
                SqlDataAdapter da = new SqlDataAdapter("getData", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;


                SqlParameter par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
                SqlParameter par1 = new SqlParameter("@dtid", Convert.ToInt32(dtid));
                SqlParameter par2 = new SqlParameter("@ttid", Convert.ToInt32(ttid));
                SqlParameter par3 = new SqlParameter("@rt", rt_pre);

                da.SelectCommand.Parameters.Add(par0);
                da.SelectCommand.Parameters.Add(par1);
                da.SelectCommand.Parameters.Add(par2);
                da.SelectCommand.Parameters.Add(par3);

                DataSet ds = new DataSet();
                string result = "";
                try
                {
                    con.Open();
                    da.Fill(ds);
                    //return ds;
                }
                catch (Exception ex)
                {
                    result = ex.ToString();
                    //return null;
                }
                finally
                {
                    con.Close();

                }

                //-------------------------

                Model.RecordeData[] mrd;
                if (result == "")
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    mrd = new Model.RecordeData[dt.Rows.Count];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Model.RecordeData mrd_single = new Model.RecordeData();

                        mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                        mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                        mrd_single.ReportTime = Convert.ToDateTime(dt.Rows[i]["ReportTime"].ToString());
                        mrd_single.BeginTime = Convert.ToDateTime(dt.Rows[i]["TimeBegin"].ToString());
                        mrd_single.OverTime = Convert.ToDateTime(dt.Rows[i]["TimeOver"].ToString());
                        mrd_single.TimeType = getInt(dt.Rows[i]["TimeType"].ToString());
                        mrd_single.DataType = getInt(dt.Rows[i]["DataType"].ToString());
                        mrd_single.DataTypeName = dt.Rows[i]["DataTypeName"].ToString();
                        mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();

                        //mrd_single.TimeTypeOrder = getInt(dt.Rows[i]["TT_Order"].ToString());
                        //mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());

                        mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                        mrd_single.Element01 = dt.Rows[i]["Element01"].ToString();
                        mrd_single.Element02 = dt.Rows[i]["Element02"].ToString();
                        mrd_single.Element03 = dt.Rows[i]["Element03"].ToString();
                        mrd_single.Element04 = dt.Rows[i]["Element04"].ToString();
                        mrd_single.Element05 = dt.Rows[i]["Element05"].ToString();
                        mrd_single.Element06 = dt.Rows[i]["Element06"].ToString();
                        mrd_single.Element07 = dt.Rows[i]["Element07"].ToString();
                        mrd_single.Element08 = dt.Rows[i]["Element08"].ToString();
                        mrd_single.Element09 = dt.Rows[i]["Element09"].ToString();

                        mrd[i] = mrd_single;
                    }
                }
                else
                {
                    return null;
                }

                return mrd;
            }
            catch
            {
                return null;
            }

        }




        //总表
        public Model.RecordeData[] getZBData(Model.RecordeData par_mrd)
        {
            //检查数据库是否存在当天数据，如果没有就返回前一天数据
            int count = 0;
            string reporttime = DateTime.Today.ToShortDateString() + " " + par_mrd.TimeType.ToString() + ":00:00";
            SqlDataAdapter da = new SqlDataAdapter("getNumberOfRecorde", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par4;
            SqlParameter par5;
            SqlParameter par6;
            par4 = new SqlParameter("@dtid", par_mrd.DataType);
            par5 = new SqlParameter("@ttid", par_mrd.TimeType);
            par6 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));
            da.SelectCommand.Parameters.Add(par4);
            da.SelectCommand.Parameters.Add(par5);
            da.SelectCommand.Parameters.Add(par6);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);

                DataRow dr = ds.Tables[0].Rows[0];
                count = Convert.ToInt32(dr["num"].ToString());
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                count = -1;
            }
            finally
            {
                con.Close();
            }

            if (count < 0)
            {
                return null;
            }

            if (count == 0)
            {
                reporttime = DateTime.Today.AddDays(-1).ToShortDateString() + " " + par_mrd.TimeType.ToString() + ":00:00";
            }



            //-----获得数据--------
            da = new SqlDataAdapter("getZBData", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            SqlParameter par3;

            par1 = new SqlParameter("@dtid", par_mrd.DataType);
            par2 = new SqlParameter("@ttid", par_mrd.TimeType);
            par3 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));

            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);
            da.SelectCommand.Parameters.Add(par3);

            ds = new DataSet();
            result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //return null;
            }
            finally
            {
                con.Close();

            }

            //-------------------------

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                    mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                    mrd_single.ReportTime = Convert.ToDateTime(dt.Rows[i]["ReportTime"].ToString());
                    mrd_single.BeginTime = Convert.ToDateTime(dt.Rows[i]["TimeBegin"].ToString());
                    mrd_single.OverTime = Convert.ToDateTime(dt.Rows[i]["TimeOver"].ToString());
                    mrd_single.TimeType = getInt(dt.Rows[i]["TimeType"].ToString());
                    mrd_single.DataType = getInt(dt.Rows[i]["DataType"].ToString());
                    mrd_single.DataTypeName = dt.Rows[i]["DataTypeName"].ToString();
                    mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();
                    mrd_single.TimeTypeOrder = getInt(dt.Rows[i]["TT_Order"].ToString());
                    mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());
                    //mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                    mrd_single.Element01 = dt.Rows[i]["Element01"].ToString();
                    mrd_single.Element02 = dt.Rows[i]["Element02"].ToString();
                    mrd_single.Element03 = dt.Rows[i]["Element03"].ToString();
                    mrd_single.Element04 = dt.Rows[i]["Element04"].ToString();
                    mrd_single.Element05 = dt.Rows[i]["Element05"].ToString();
                    mrd_single.Element06 = dt.Rows[i]["Element06"].ToString();
                    mrd_single.Element07 = dt.Rows[i]["Element07"].ToString();
                    mrd_single.Element08 = dt.Rows[i]["Element08"].ToString();
                    mrd_single.Element09 = dt.Rows[i]["Element09"].ToString();

                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;



        }



        //总表Pre
        public Model.RecordeData[] getZBDataPre(Model.RecordeData par_mrd, String reporttime)
        {
            DateTime rt_dt = DateTime.ParseExact(reporttime, "yyyy-M-d H:mm:ss", null);
            DateTime rt_pre = rt_dt.AddDays(-1);

            //-----获得数据--------
            SqlDataAdapter da = new SqlDataAdapter("getZBData", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            SqlParameter par3;

            par1 = new SqlParameter("@dtid", par_mrd.DataType);
            par2 = new SqlParameter("@ttid", par_mrd.TimeType);
            par3 = new SqlParameter("@rt", Convert.ToDateTime(rt_pre));

            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);
            da.SelectCommand.Parameters.Add(par3);

            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //return null;
            }
            finally
            {
                con.Close();

            }

            //-------------------------

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                    mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                    mrd_single.ReportTime = Convert.ToDateTime(dt.Rows[i]["ReportTime"].ToString());
                    mrd_single.BeginTime = Convert.ToDateTime(dt.Rows[i]["TimeBegin"].ToString());
                    mrd_single.OverTime = Convert.ToDateTime(dt.Rows[i]["TimeOver"].ToString());
                    mrd_single.TimeType = getInt(dt.Rows[i]["TimeType"].ToString());
                    mrd_single.DataType = getInt(dt.Rows[i]["DataType"].ToString());
                    mrd_single.DataTypeName = dt.Rows[i]["DataTypeName"].ToString();
                    mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();
                    mrd_single.TimeTypeOrder = getInt(dt.Rows[i]["TT_Order"].ToString());
                    mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());
                    //mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                    mrd_single.Element01 = dt.Rows[i]["Element01"].ToString();
                    mrd_single.Element02 = dt.Rows[i]["Element02"].ToString();
                    mrd_single.Element03 = dt.Rows[i]["Element03"].ToString();
                    mrd_single.Element04 = dt.Rows[i]["Element04"].ToString();
                    mrd_single.Element05 = dt.Rows[i]["Element05"].ToString();
                    mrd_single.Element06 = dt.Rows[i]["Element06"].ToString();
                    mrd_single.Element07 = dt.Rows[i]["Element07"].ToString();
                    mrd_single.Element08 = dt.Rows[i]["Element08"].ToString();
                    mrd_single.Element09 = dt.Rows[i]["Element09"].ToString();

                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;
        }



        //总表根据实效
        public Model.RecordeData[] getZBDataBySX(Model.RecordeData par_mrd)
        {
            //检查数据库是否存在当天数据，如果没有就返回前一天数据
            int count = 0;
            string reporttime = DateTime.Today.ToShortDateString() + " " + par_mrd.TimeType.ToString() + ":00:00";
            SqlDataAdapter da = new SqlDataAdapter("getNumberOfRecorde", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par4;
            SqlParameter par5;
            SqlParameter par6;
            par4 = new SqlParameter("@dtid", par_mrd.DataType);
            par5 = new SqlParameter("@ttid", par_mrd.TimeType);
            par6 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));
            da.SelectCommand.Parameters.Add(par4);
            da.SelectCommand.Parameters.Add(par5);
            da.SelectCommand.Parameters.Add(par6);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);

                DataRow dr = ds.Tables[0].Rows[0];
                count = Convert.ToInt32(dr["num"].ToString());
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                count = -1;
            }
            finally
            {
                con.Close();
            }

            if (count < 0)
            {
                return null;
            }

            if (count == 0)
            {
                reporttime = DateTime.Today.AddDays(-1).ToShortDateString() + " " + par_mrd.TimeType.ToString() + ":00:00";
            }



            //-----获得数据--------
            da = new SqlDataAdapter("getZBDataBySX", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            SqlParameter par3;
            par0 = new SqlParameter("@sx", par_mrd.Time);
            par1 = new SqlParameter("@dtid", par_mrd.DataType);
            par2 = new SqlParameter("@ttid", par_mrd.TimeType);
            par3 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));
            da.SelectCommand.Parameters.Add(par0);
            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);
            da.SelectCommand.Parameters.Add(par3);

            ds = new DataSet();
            result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //return null;
            }
            finally
            {
                con.Close();

            }

            //-------------------------

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                    mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                    mrd_single.ReportTime = Convert.ToDateTime(dt.Rows[i]["ReportTime"].ToString());
                    mrd_single.BeginTime = Convert.ToDateTime(dt.Rows[i]["TimeBegin"].ToString());
                    mrd_single.OverTime = Convert.ToDateTime(dt.Rows[i]["TimeOver"].ToString());
                    mrd_single.TimeType = getInt(dt.Rows[i]["TimeType"].ToString());
                    mrd_single.DataType = getInt(dt.Rows[i]["DataType"].ToString());
                    mrd_single.DataTypeName = dt.Rows[i]["DataTypeName"].ToString();
                    mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();
                    mrd_single.TimeTypeOrder = getInt(dt.Rows[i]["TT_Order"].ToString());
                    mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());
                    //mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                    mrd_single.Element01 = dt.Rows[i]["Element01"].ToString();
                    mrd_single.Element02 = dt.Rows[i]["Element02"].ToString();
                    mrd_single.Element03 = dt.Rows[i]["Element03"].ToString();
                    mrd_single.Element04 = dt.Rows[i]["Element04"].ToString();
                    mrd_single.Element05 = dt.Rows[i]["Element05"].ToString();
                    mrd_single.Element06 = dt.Rows[i]["Element06"].ToString();
                    mrd_single.Element07 = dt.Rows[i]["Element07"].ToString();
                    mrd_single.Element08 = dt.Rows[i]["Element08"].ToString();
                    mrd_single.Element09 = dt.Rows[i]["Element09"].ToString();

                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;



        }



        //总表的实效
        public Model.RecordeData[] getZBSX(Model.RecordeData par_mrd)
        {
            //检查数据库是否存在当天数据，如果没有就返回前一天数据
            int count = 0;
            string reporttime = DateTime.Today.ToShortDateString() + " " + par_mrd.TimeType.ToString() + ":00:00";
            SqlDataAdapter da = new SqlDataAdapter("getNumberOfRecorde", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par4;
            SqlParameter par5;
            SqlParameter par6;
            par4 = new SqlParameter("@dtid", par_mrd.DataType);
            par5 = new SqlParameter("@ttid", par_mrd.TimeType);
            par6 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));
            da.SelectCommand.Parameters.Add(par4);
            da.SelectCommand.Parameters.Add(par5);
            da.SelectCommand.Parameters.Add(par6);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);

                DataRow dr = ds.Tables[0].Rows[0];
                count = Convert.ToInt32(dr["num"].ToString());
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                count = -1;
            }
            finally
            {
                con.Close();
            }

            if (count < 0)
            {
                return null;
            }

            if (count == 0)
            {
                reporttime = DateTime.Today.AddDays(-1).ToShortDateString() + " " + par_mrd.TimeType.ToString() + ":00:00";
            }



            //-----获得数据--------
            da = new SqlDataAdapter("getZBSX", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            SqlParameter par3;

            par1 = new SqlParameter("@dtid", par_mrd.DataType);
            par2 = new SqlParameter("@ttid", par_mrd.TimeType);
            par3 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));

            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);
            da.SelectCommand.Parameters.Add(par3);

            ds = new DataSet();
            result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //return null;
            }
            finally
            {
                con.Close();

            }

            //-------------------------
            /*
                        Model.RecordeData[] mrd;
                        if (result == "")
                        {
                            DataTable dt = new DataTable();
                            dt = ds.Tables[0];
                            mrd = new Model.RecordeData[dt.Rows.Count];

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Model.RecordeData mrd_single = new Model.RecordeData();

                                mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                                mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                                mrd_single.ReportTime = Convert.ToDateTime(dt.Rows[i]["ReportTime"].ToString());
                                mrd_single.BeginTime = Convert.ToDateTime(dt.Rows[i]["TimeBegin"].ToString());
                                mrd_single.OverTime = Convert.ToDateTime(dt.Rows[i]["TimeOver"].ToString());
                                mrd_single.TimeType = getInt(dt.Rows[i]["TimeType"].ToString());
                                mrd_single.DataType = getInt(dt.Rows[i]["DataType"].ToString());
                                mrd_single.DataTypeName = dt.Rows[i]["DataTypeName"].ToString();
                                mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();
                                mrd_single.TimeTypeOrder = getInt(dt.Rows[i]["TT_Order"].ToString());
                                mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());
                                //mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                                mrd_single.Element01 = dt.Rows[i]["Element01"].ToString();
                                mrd_single.Element02 = dt.Rows[i]["Element02"].ToString();
                                mrd_single.Element03 = dt.Rows[i]["Element03"].ToString();
                                mrd_single.Element04 = dt.Rows[i]["Element04"].ToString();
                                mrd_single.Element05 = dt.Rows[i]["Element05"].ToString();
                                mrd_single.Element06 = dt.Rows[i]["Element06"].ToString();
                                mrd_single.Element07 = dt.Rows[i]["Element07"].ToString();
                                mrd_single.Element08 = dt.Rows[i]["Element08"].ToString();
                                mrd_single.Element09 = dt.Rows[i]["Element09"].ToString();

                                mrd[i] = mrd_single;
                            }
                        }
                        else
                        {
                            return null;
                        }

                        return mrd;*/


            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();
                    mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;


        }



        public Model.ChannelProgram getZBcpid(Model.ChannelProgram mcp)
        {
            string cpid = "";
            SqlDataAdapter da = new SqlDataAdapter("getZBcpid", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@dtid", mcp.DataTypeID);
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                DataRow dr = ds.Tables[0].Rows[0];

                cpid = dr["id"].ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return null;
            }
            finally
            {
                con.Close();
            }

            Model.ChannelProgram mcp_back = new Model.ChannelProgram();
            try
            {
                if (cpid == "" || cpid == "0")
                {
                    return null;
                }
                mcp_back.CP_ID = Convert.ToInt32(cpid);
            }
            catch
            {
                return null;
            }

            return mcp_back;


        }




        public Model.RecordeData[] getDataBySX(String cpid, String sx)
        {
            string ttid = "";
            string dtid = "";
            //带判断的存储过程，根据传递进来的参数判断执行哪条SQL语句
            //SqlConnection con = new SqlConnection(connectionString);

            SqlDataAdapter da = new SqlDataAdapter("getCP", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();


                da.Fill(ds);


                DataRow dr = ds.Tables[0].Rows[0];

                //Response.Write(dr["TT_id"] + "-" + dr["DT_id"]);
                ttid = dr["TT_id"].ToString();
                dtid = dr["DT_id"].ToString();

                //GridView1.DataSource = ds;
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            /*
            //对显示内容的数据进行日期限定，只返回当天或前一天数据
            string reporttime = DateTime.Today.ToShortDateString() + " " + ttid + ":00:00";
            string sql_getNumberOfData = "select count(*) from tb_AllDataRecorde where ReportTime='" + reporttime + "' and DataType=" + dtid + " and TimeType=" + ttid;
            con.Open();
            SqlCommand QueryCmd = new SqlCommand(sql_getNumberOfData, con);
            int count = 0;
            try
            {
                count = Convert.ToInt32(QueryCmd.ExecuteScalar());
            }
            catch
            {
                count = -1;
            }

            if (count < 0)
            {
                con.Close();
                return null;
            }
            if (count == 0)
            {
                reporttime = DateTime.Today.AddDays(-1).ToShortDateString() + " " + ttid + ":00:00";
            }
            con.Close();
            */


            //--------------------------------------------------------------
            int count = 0;
            string reporttime = DateTime.Today.ToShortDateString() + " " + ttid + ":00:00";
            da = new SqlDataAdapter("getNumberOfRecorde", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par7;
            SqlParameter par5;
            SqlParameter par6;
            par7 = new SqlParameter("@dtid", Convert.ToInt32(dtid));
            par5 = new SqlParameter("@ttid", Convert.ToInt32(ttid));
            par6 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));
            da.SelectCommand.Parameters.Add(par7);
            da.SelectCommand.Parameters.Add(par5);
            da.SelectCommand.Parameters.Add(par6);
            ds = new DataSet();
            //string result = "";
            try
            {
                con.Open();
                da.Fill(ds);

                DataRow dr = ds.Tables[0].Rows[0];
                count = Convert.ToInt32(dr["num"].ToString());
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                count = -1;
            }
            finally
            {
                con.Close();
            }

            if (count < 0)
            {
                return null;
            }

            if (count == 0)
            {
                reporttime = DateTime.Today.AddDays(-1).ToShortDateString() + " " + ttid + ":00:00";
            }



            da = new SqlDataAdapter("getDataBySX", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            SqlParameter par3;
            SqlParameter par4;

            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            par1 = new SqlParameter("@dtid", Convert.ToInt32(dtid));
            par2 = new SqlParameter("@ttid", Convert.ToInt32(ttid));
            par3 = new SqlParameter("@timeid", Convert.ToInt32(sx));
            par4 = new SqlParameter("@rt", Convert.ToDateTime(reporttime));


            da.SelectCommand.Parameters.Add(par0);
            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);
            da.SelectCommand.Parameters.Add(par3);
            da.SelectCommand.Parameters.Add(par4);

            ds = new DataSet();
            result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
                //GridView1.DataSource = ds;
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //return null;
            }
            finally
            {
                con.Close();

            }
            // return null;


            //-------------------------

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                    mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                    mrd_single.ReportTime = Convert.ToDateTime(dt.Rows[i]["ReportTime"].ToString());
                    mrd_single.BeginTime = Convert.ToDateTime(dt.Rows[i]["TimeBegin"].ToString());
                    mrd_single.OverTime = Convert.ToDateTime(dt.Rows[i]["TimeOver"].ToString());
                    mrd_single.TimeType = getInt(dt.Rows[i]["TimeType"].ToString());
                    mrd_single.DataType = getInt(dt.Rows[i]["DataType"].ToString());
                    mrd_single.DataTypeName = dt.Rows[i]["DataTypeName"].ToString();
                    mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();
                    mrd_single.TimeTypeOrder = getInt(dt.Rows[i]["TT_Order"].ToString());
                    mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());
                    mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                    mrd_single.Element01 = dt.Rows[i]["Element01"].ToString();
                    mrd_single.Element02 = dt.Rows[i]["Element02"].ToString();
                    mrd_single.Element03 = dt.Rows[i]["Element03"].ToString();
                    mrd_single.Element04 = dt.Rows[i]["Element04"].ToString();
                    mrd_single.Element05 = dt.Rows[i]["Element05"].ToString();
                    mrd_single.Element06 = dt.Rows[i]["Element06"].ToString();
                    mrd_single.Element07 = dt.Rows[i]["Element07"].ToString();
                    mrd_single.Element08 = dt.Rows[i]["Element08"].ToString();
                    mrd_single.Element09 = dt.Rows[i]["Element09"].ToString();

                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;



        }



        public Model.RecordeData[] getSX(String cpid)
        {
            string ttid = "";
            string dtid = "";
            //带判断的存储过程，根据传递进来的参数判断执行哪条SQL语句
            //SqlConnection con = new SqlConnection(connectionString);

            SqlDataAdapter da = new SqlDataAdapter("getCP", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();


                da.Fill(ds);


                DataRow dr = ds.Tables[0].Rows[0];

                //Response.Write(dr["TT_id"] + "-" + dr["DT_id"]);
                ttid = dr["TT_id"].ToString();
                dtid = dr["DT_id"].ToString();

                //GridView1.DataSource = ds;
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            //-------------------------
            //带参数的存储过程
            //SqlConnection con = new SqlConnection(connectionString);

            da = new SqlDataAdapter("getSX", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;

            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            par1 = new SqlParameter("@dtid", Convert.ToInt32(dtid));
            par2 = new SqlParameter("@ttid", Convert.ToInt32(ttid));

            da.SelectCommand.Parameters.Add(par0);
            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);

            ds = new DataSet();
            result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
                //GridView1.DataSource = ds;
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                //return null;
            }
            finally
            {
                con.Close();

            }
            //return null;

            //-------------------------

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();
                    mrd_single.Time = getInt(dt.Rows[i]["Time"].ToString());
                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;
        }



        /// <summary>
        /// 查询指定栏目已入库数据的时效
        /// </summary>
        /// <param name="cpID">栏目编号</param>
        /// <param name="dtID">数据类型编号</param>
        /// <param name="ttID">时间类型编号</param>
        /// <returns>string</returns>
        public string[] getSX2(int cpID, int dtID, int ttID)
        {
            // mod by Edward Chan.
            string sql = "SELECT DISTINCT TOP 2000 Time FROM weatherdata.dbo.tb_AllDataRecorde ";
            sql += "WHERE (ReportTime BETWEEN CONVERT(CHAR(10), GETDATE() - 1, 120) AND CONVERT(CHAR(10), GETDATE(), 120)) ";
            sql += "AND StationID IN ( SELECT stationId FROM dbo.tb_StationSetting WHERE CP_id = @cpid ";
            sql += ") AND TimeType = @ttid AND DataType = @dtid ORDER BY Time ";

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", cpID), 
                    new SqlParameter("@ttid", ttID), 
                    new SqlParameter("@dtid", dtID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            string[] times = new string[0];

            if (dt.Rows.Count > 0)
            {
                times = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    times[i] = dt.Rows[i]["Time"].ToString();
                }
            }

            return times;
        }



        public Model.ElementRelation[] DecordeDataSet(String typeid)
        {
            SqlDataAdapter da = new SqlDataAdapter("getDecodeDateSet", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@typeid", Convert.ToInt32(typeid));
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }
            //return null;


            //-------------------------

            Model.ElementRelation[] mer;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mer = new Model.ElementRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ElementRelation mer_single = new Model.ElementRelation();

                    mer_single.TransCodeName = dt.Rows[i]["Name"].ToString();
                    mer_single.ElementCode = dt.Rows[i]["Code"].ToString();

                    mer[i] = mer_single;
                }
            }
            else
            {
                return null;
            }

            return mer;







        }


        public Model.ElementRelation[] DecordeDataSetType(String cpid)
        {
            SqlDataAdapter da = new SqlDataAdapter("getDecodeType", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }
            //return null;


            //-------------------------

            Model.ElementRelation[] mer;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mer = new Model.ElementRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ElementRelation mer_single = new Model.ElementRelation();

                    mer_single.ElementCodeType = getInt(dt.Rows[i]["elCodeTrans"].ToString());
                    mer_single.CodeTypeName = dt.Rows[i]["typeName"].ToString();

                    mer[i] = mer_single;
                }
            }
            else
            {
                return null;
            }

            return mer;


        }



        public Model.ElementRelation[] getElementsName(String cpid)
        {
            SqlDataAdapter da = new SqlDataAdapter("getElementRelation", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            da.SelectCommand.Parameters.Add(par0);
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

            Model.ElementRelation[] mer;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mer = new Model.ElementRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ElementRelation mer_single = new Model.ElementRelation();

                    mer_single.CP_ID = Convert.ToInt32(dt.Rows[i]["CP_id"].ToString());
                    mer_single.ElementID = Convert.ToInt32(dt.Rows[i]["ET_id"].ToString());
                    mer_single.ElementOrder = Convert.ToInt32(dt.Rows[i]["ET_order"].ToString());
                    mer_single.DataTypeID = Convert.ToInt32(dt.Rows[i]["DT_id"].ToString());
                    mer_single.ElementCodeType = Convert.ToInt32(dt.Rows[i]["elCodeTrans"].ToString());
                    mer_single.ElementNameCN = dt.Rows[i]["elDataNameCn"].ToString();
                    mer_single.ElementName = dt.Rows[i]["elName"].ToString();

                    mer[i] = mer_single;
                }
            }
            else
            {
                return null;
            }

            return mer;


        }




        public Model.StationRelation[] getAllStationsModel(String cpid)
        {

            SqlDataAdapter da = new SqlDataAdapter("getAllStations", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            da.SelectCommand.Parameters.Add(par0);
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

            Model.StationRelation[] msr;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                msr = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.StationRelation msr_single = new Model.StationRelation();

                    msr_single.SelectID = Convert.ToInt32(dt.Rows[i]["selectID"].ToString());
                    msr_single.StationTableID = Convert.ToInt32(dt.Rows[i]["selectID"].ToString());
                    msr_single.StationID = Convert.ToInt32(dt.Rows[i]["stationid"].ToString());
                    msr_single.StationName = dt.Rows[i]["name_cn"].ToString();
                    msr[i] = msr_single;
                }
            }
            else
            {
                return null;
            }

            return msr;

        }


        //获得总表站点列表，有重复内容
        public Model.StationRelation[] getZBStations(Model.ChannelProgram mcp)//需要dtid
        {

            SqlDataAdapter da = new SqlDataAdapter("getZBStations", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            SqlParameter par1;
            par0 = new SqlParameter("@dtid", Convert.ToInt32(mcp.DataTypeID));
            par1 = new SqlParameter("@ttid", Convert.ToInt32(mcp.TimeTypeID));
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

            Model.StationRelation[] msr;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                msr = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.StationRelation msr_single = new Model.StationRelation();

                    msr_single.SelectID = Convert.ToInt32(dt.Rows[i]["selectID"].ToString());
                    msr_single.StationTableID = Convert.ToInt32(dt.Rows[i]["selectID"].ToString());
                    msr_single.StationID = Convert.ToInt32(dt.Rows[i]["stationid"].ToString());
                    msr_single.StationName = dt.Rows[i]["name_cn"].ToString();
                    msr[i] = msr_single;
                }
            }
            else
            {
                return null;
            }

            return msr;

        }



        public Model.RecordeData[] getRealStations(String cpid)
        {
            string ttid = "";
            string dtid = "";
            SqlDataAdapter da = new SqlDataAdapter("getCP", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            da.SelectCommand.Parameters.Add(par0);
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);

                DataRow dr = ds.Tables[0].Rows[0];
                ttid = dr["TT_id"].ToString();
                dtid = dr["DT_id"].ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            //-----获得数据--------
            da = new SqlDataAdapter("getRealStations", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //SqlParameter par0;
            SqlParameter par1;
            SqlParameter par2;
            //SqlParameter par3;

            par0 = new SqlParameter("@cpid", Convert.ToInt32(cpid));
            par1 = new SqlParameter("@dtid", Convert.ToInt32(dtid));
            par2 = new SqlParameter("@ttid", Convert.ToInt32(ttid));

            da.SelectCommand.Parameters.Add(par0);
            da.SelectCommand.Parameters.Add(par1);
            da.SelectCommand.Parameters.Add(par2);

            ds = new DataSet();
            result = "";
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

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.StationID = getInt(dt.Rows[i]["StationID"].ToString());
                    mrd_single.selectID = getInt(dt.Rows[i]["selectID"].ToString());
                    mrd_single.stationOrder = getInt(dt.Rows[i]["stationOrder"].ToString());

                    mrd[i] = mrd_single;
                }
            }
            else
            {
                return null;
            }

            return mrd;

        }


        public Model.StationRelation[] getStationsInfo()
        {

            SqlDataAdapter da = new SqlDataAdapter("getStationInfo", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
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

            Model.StationRelation[] msr;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                msr = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    Model.StationRelation msr_single = new Model.StationRelation();

                    msr_single.StationTableID = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    msr_single.SelectID = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    msr_single.StationID = Convert.ToInt32(dt.Rows[i]["stationid"].ToString());
                    msr_single.StationName = dt.Rows[i]["name_cn"].ToString();
                    msr[i] = msr_single;


                }
            }
            else
            {
                return null;
            }

            return msr;

        }

        public Model.StationRelation[] getStationsInfoOrderByName()
        {

            SqlDataAdapter da = new SqlDataAdapter("getStationInfoOrderByName", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
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

            Model.StationRelation[] msr;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                msr = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    Model.StationRelation msr_single = new Model.StationRelation();

                    msr_single.StationTableID = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    msr_single.SelectID = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    msr_single.StationID = Convert.ToInt32(dt.Rows[i]["stationid"].ToString());
                    msr_single.StationName = dt.Rows[i]["name_cn"].ToString();
                    msr[i] = msr_single;


                }
            }
            else
            {
                return null;
            }

            return msr;

        }

        //-----------------------------------
        //对站点的检索查询
        public Model.StationRelation[] searchStationInfo(Model.StationRelation msr)
        {
            int nameLength = 0;
            try
            {
                nameLength = msr.StationName.Length;
            }
            catch
            {
                ;
            }

            SqlDataAdapter da = new SqlDataAdapter();
            if ((msr.StationID > 0) && (nameLength > 0))
            {
                da = new SqlDataAdapter("searchStationInfo", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter par0;
                SqlParameter par1;
                par0 = new SqlParameter("@s_id", msr.StationID);
                par1 = new SqlParameter("@name", msr.StationName);
                da.SelectCommand.Parameters.Add(par0);
                da.SelectCommand.Parameters.Add(par1);
            }
            else
            {
                if (msr.StationID > 0)
                {
                    da = new SqlDataAdapter("searchStationInfoBySID", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter par0;
                    par0 = new SqlParameter("@s_id", msr.StationID);
                    da.SelectCommand.Parameters.Add(par0);

                }
                if (nameLength > 0)
                {
                    da = new SqlDataAdapter("searchStationInfoByName", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter par1;
                    par1 = new SqlParameter("@name", msr.StationName);
                    da.SelectCommand.Parameters.Add(par1);
                }
            }


            DataSet ds = new DataSet();
            string result = "";
            try
            {
                con.Open();
                da.Fill(ds);
                //return ds;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                con.Close();
            }
            //return null;


            //-------------------------

            Model.StationRelation[] mer;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mer = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.StationRelation msr_single = new Model.StationRelation();

                    msr_single.StationTableID = getInt(dt.Rows[i]["id"].ToString());
                    msr_single.SelectID = getInt(dt.Rows[i]["id"].ToString());
                    msr_single.StationID = getInt(dt.Rows[i]["stationid"].ToString());
                    msr_single.StationName = dt.Rows[i]["name_cn"].ToString();

                    mer[i] = msr_single;
                }
            }
            else
            {
                return null;
            }

            return mer;

        }


        //对站点的检索精确查询
        public Model.StationRelation searchStationInfo2(Model.StationRelation msr)
        {
            SqlDataAdapter da = new SqlDataAdapter("searchStationInfo2", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            SqlParameter par1;
            par0 = new SqlParameter("@s_id", msr.StationID);
            par1 = new SqlParameter("@name", msr.StationName);
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
                if (dt.Rows.Count != 0)
                {
                    //id,stationid,name_cn
                    msrback.SelectID = getInt(dt.Rows[0]["id"].ToString());
                    msrback.StationTableID = getInt(dt.Rows[0]["id"].ToString());
                    msrback.StationID = getInt(dt.Rows[0]["stationid"].ToString());
                    msrback.StationName = dt.Rows[0]["name_cn"].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return msrback;

        }



        public Model.RecordeData[] getDataType(int? id)
        {
            /*
            SqlDataAdapter da = new SqlDataAdapter("getDataType", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
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

            Model.RecordeData[] mrd;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrd = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.DataType = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    mrd_single.DataTypeName = dt.Rows[i]["DataType"].ToString();
                    mrd[i] = mrd_single;

                }
            }
            else
            {
                return null;
            }

            return mrd;
            */

            // mod by Edward Chan.
            string sql = "SELECT id, DataType FROM tb_AllDataType ";
            sql += "WHERE (id = @id OR @id IS NULL) ";

            SqlParameter[] paras = {
                    new SqlParameter("@id", id)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("AllDataType");
            dt = ds.Tables[0];

            Model.RecordeData[] mrds = new Model.RecordeData[0];

            if (dt.Rows.Count > 0)
            {
                mrds = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd = new Model.RecordeData();

                    mrd.DataType = Convert.ToInt32(dt.Rows[i]["id"]);
                    mrd.DataTypeName = dt.Rows[i]["DataType"].ToString();

                    mrds[i] = mrd;
                }
            }

            return mrds;
        }


        public Model.RecordeData[] getTimeType(Model.RecordeData mrd)
        {

            SqlDataAdapter da = new SqlDataAdapter("getTimeTypeByDT", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@dtid", mrd.DataType);
            da.SelectCommand.Parameters.Add(par0);
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

            Model.RecordeData[] mrdback;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                mrdback = new Model.RecordeData[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.RecordeData mrd_single = new Model.RecordeData();

                    mrd_single.DataType = Convert.ToInt32(dt.Rows[i]["DT_ID"].ToString());
                    mrd_single.TimeType = Convert.ToInt32(dt.Rows[i]["TimeType"].ToString());
                    mrd_single.TimeTypeName = dt.Rows[i]["TimeTypeName"].ToString();
                    mrd_single.TimeTypeOrder = Convert.ToInt32(dt.Rows[i]["TT_Order"].ToString());
                    mrdback[i] = mrd_single;

                }
            }
            else
            {
                return null;
            }

            return mrdback;


        }





        public Model.ElementRelation[] getAllElement(Model.ElementRelation mer)
        {

            SqlDataAdapter da = new SqlDataAdapter("getAllElement", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter par0;
            par0 = new SqlParameter("@dtid", mer.DataTypeID);
            da.SelectCommand.Parameters.Add(par0);
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

            Model.ElementRelation[] merback;
            if (result == "")
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                merback = new Model.ElementRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ElementRelation mer_single = new Model.ElementRelation();

                    mer_single.ElementID = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    mer_single.ElementName = dt.Rows[i]["elName"].ToString();
                    mer_single.ElementNameCN = dt.Rows[i]["elDataNameCn"].ToString();
                    mer_single.ElementCode = dt.Rows[i]["elCodeTrans"].ToString();
                    merback[i] = mer_single;

                }
            }
            else
            {
                return null;
            }

            return merback;


        }


        /*
        public Boolean loginConfirm(Model.Admin ma)
        {
            string sql_confirm="select count(*) from tb_admin where name='" + ma.UserName + "'and password='" + ma.UserPassword + "'";
            con.Open();
            SqlCommand QueryCmd = new SqlCommand(sql_confirm, con);
            int count = Convert.ToInt32(QueryCmd.ExecuteScalar());
            if (count > 0)
            {
                con.Close();
                return true;
            }

            con.Close();
            return false;

        }
        */



        // ----------------------------------------------------------------------------------
        // Added by Edward Chan.

        //static readonly string connectionString = ConfigurationManager.ConnectionStrings["AllDataConnectionString"].ConnectionString;

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="userID">工号ID</param>
        /// <returns></returns>
        public bool isUserExist(string userID)
        {
            string sql = "SELECT id ,name ,pass ,role ,state ,comm ";
            sql += "FROM dbo.tb_User WHERE id = @userid ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", userID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("User");
            dt = ds.Tables[0];

            return dt.Rows.Count > 0;
        }


        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <returns></returns>
        public Model.User[] getUsers()
        {
            string sql = "SELECT id ,name ,pass ,role ,state ,comm ";
            sql += "FROM dbo.tb_User ";

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);

            DataTable dt = new DataTable("User");
            dt = ds.Tables[0];

            Model.User[] murs = new Model.User[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                murs = new Model.User[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.User mur = new Model.User();

                    mur.UserID = dt.Rows[i]["id"].ToString();
                    mur.UserName = dt.Rows[i]["name"].ToString();
                    //mur.Password = dt.Rows[i]["pass"].ToString();
                    mur.UserRole = dt.Rows[i]["role"].ToString();
                    mur.UserState = dt.Rows[i]["state"].ToString();
                    mur.UserComment = dt.Rows[i]["comm"].ToString();

                    murs[i] = mur;
                }

            }

            return murs;
        }


        /// <summary>
        /// 按条件查询用户信息
        /// </summary>
        /// <param name="mu"></param>
        /// <returns></returns>
        public Model.User[] getUsers(Model.User mu)
        {
            string sql = "SELECT id ,name ,pass ,role ,state ,comm ";
            sql += "FROM dbo.tb_User ";
            sql += "WHERE (id = @userid OR @userid IS NULL) ";
            sql += "AND (pass = @userpwd OR @userpwd IS NULL) ";
            sql += "AND (name = @username OR @username IS NULL) ";
            sql += "AND (role = @userrole OR @userrole IS NULL) ";
            sql += "AND (state = @userstate OR @userstate IS NULL) ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", mu.UserID), 
                    new SqlParameter("@userpwd", mu.Password),
                    new SqlParameter("@username", mu.UserName),
                    new SqlParameter("@userrole", mu.UserRole),
                    new SqlParameter("@userstate", mu.UserState)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("User");
            dt = ds.Tables[0];

            Model.User[] murs = new Model.User[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                murs = new Model.User[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.User mur = new Model.User();

                    mur.UserID = dt.Rows[i]["id"].ToString();
                    mur.UserName = dt.Rows[i]["name"].ToString();
                    //mur.Password = dt.Rows[i]["pass"].ToString();
                    mur.UserRole = dt.Rows[i]["role"].ToString();
                    mur.UserState = dt.Rows[i]["state"].ToString();
                    mur.UserComment = dt.Rows[i]["comm"].ToString();

                    murs[i] = mur;
                }

            }

            return murs;
        }


        /// <summary>
        /// 查询所有制片人的频道栏目设置
        /// </summary>
        /// <returns></returns>
        public Model.ChannelProgramUser[] getCPUsers()
        {
            string sql = "SELECT a.id AS UserID, a.name AS UserName, a.role AS UserRole, a.state AS UserState, c.* ";
            sql += "FROM dbo.tb_User AS a ";
            sql += "LEFT OUTER JOIN dbo.tb_ChannelProgramUser AS b ON a.id = b.UR_id ";
            sql += "LEFT OUTER JOIN ( ";
            sql += "SELECT cp1.id AS ChannelID, cp1.CP_name AS ChannelName, cp1.CP_order AS ChannelOrder ";
            sql += " , cp2.id AS ProgramID, cp2.CP_name AS ProgramName, cp2.CP_order AS ProgramOrder ";
            sql += " , cp2.TT_id, cp2.DT_id ";
            sql += "FROM dbo.tb_ChannelProgram AS cp1 ";
            sql += "INNER JOIN dbo.tb_ChannelProgram AS cp2 ON cp1.id = cp2.FatherID ";
            sql += ") AS c ON b.CP_id = c.ChannelID ";
            sql += "WHERE a.role <> '0' ";
            sql += "ORDER BY CASE WHEN c.ChannelID IS NULL THEN 1 ELSE 0 END, a.id, c.ChannelOrder, c.ProgramOrder ";

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);

            DataTable dt = new DataTable("ChannelProgramUser");
            dt = ds.Tables[0];

            Model.ChannelProgramUser[] mcpus = new Model.ChannelProgramUser[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mcpus = new Model.ChannelProgramUser[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ChannelProgramUser mcpu = new Model.ChannelProgramUser();

                    mcpu.UserID = dt.Rows[i]["UserID"].ToString();
                    mcpu.UserName = dt.Rows[i]["UserName"].ToString();
                    mcpu.UserRole = dt.Rows[i]["UserRole"].ToString();
                    mcpu.UserState = dt.Rows[i]["UserState"].ToString();

                    // 下列字段可空，转换前需要进行判断
                    mcpu.ChannelID = Convert.IsDBNull(dt.Rows[i]["ChannelID"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["ChannelID"]);
                    mcpu.ChannelName = Convert.IsDBNull(dt.Rows[i]["ChannelName"]) ? null : dt.Rows[i]["ChannelName"].ToString();
                    mcpu.ProgramID = Convert.IsDBNull(dt.Rows[i]["ProgramID"]) ? (int?)null : Convert.ToInt32(dt.Rows[i]["ProgramID"]);
                    mcpu.ProgramName = Convert.IsDBNull(dt.Rows[i]["ProgramName"]) ? null : dt.Rows[i]["ProgramName"].ToString();

                    mcpus[i] = mcpu;
                }

            }

            return mcpus;
        }


        /// <summary>
        /// 查询指定制片人的频道栏目设置
        /// </summary>
        /// <param name="userID">工号ID</param>
        /// <returns></returns>
        public Model.ChannelProgramUser[] getCPUsersByUserID(string userID)
        {
            string sql = "SELECT a.id AS ChannelID, a.CP_name AS ChannelName, ";
            sql += "b.id AS ProgramID, b.CP_name AS ProgramName, b.TT_id, b.DT_id ";
            sql += "FROM dbo.tb_ChannelProgram AS a ";
            sql += "INNER JOIN dbo.tb_ChannelProgram AS b ON a.id = b.FatherID ";
            sql += "WHERE a.id IN ( ";
            sql += " SELECT CP_id FROM dbo.tb_ChannelProgramUser AS cpu ";
            sql += "WHERE cpu.UR_id = @UserID ) ";
            sql += "ORDER BY a.CP_order, b.CP_order ";

            SqlParameter[] paras = {
                    new SqlParameter("@UserID", userID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("ChannelProgramUser");
            dt = ds.Tables[0];

            Model.ChannelProgramUser[] mcpus = new Model.ChannelProgramUser[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mcpus = new Model.ChannelProgramUser[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ChannelProgramUser mcpu = new Model.ChannelProgramUser();
                    /*
                    mcpu.UserID = dt.Rows[i]["UserID"].ToString();
                    mcpu.UserName = dt.Rows[i]["UserName"].ToString();
                    mcpu.UserRole = dt.Rows[i]["UserRole"].ToString();
                    mcpu.UserState = dt.Rows[i]["UserState"].ToString();
                    */
                    mcpu.ChannelID = Convert.ToInt32(dt.Rows[i]["ChannelID"]);
                    mcpu.ChannelName = dt.Rows[i]["ChannelName"].ToString();
                    mcpu.ProgramID = Convert.ToInt32(dt.Rows[i]["ProgramID"]);
                    mcpu.ProgramName = dt.Rows[i]["ProgramName"].ToString();

                    mcpu.TimeTypeID = Convert.IsDBNull(dt.Rows[i]["TT_id"]) ? -1 : Convert.ToInt32(dt.Rows[i]["TT_id"]);
                    mcpu.DataTypeID = Convert.IsDBNull(dt.Rows[i]["DT_id"]) ? -1 : Convert.ToInt32(dt.Rows[i]["DT_id"]);

                    mcpus[i] = mcpu;
                }

            }

            return mcpus;
        }


        /// <summary>
        /// 查询所有频道下的栏目信息
        /// </summary>
        /// <returns></returns>
        public Model.ChannelProgramUser[] getCPs()
        {
            string sql = "SELECT a.id AS ChannelID, a.CP_name AS ChannelName, ";
            sql += "b.id AS ProgramID, b.CP_name AS ProgramName, b.TT_id, b.DT_id ";
            sql += "FROM dbo.tb_ChannelProgram AS a ";
            sql += "INNER JOIN dbo.tb_ChannelProgram AS b ON a.id = b.FatherID ";
            sql += "ORDER BY a.CP_order, b.CP_order ";

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);

            DataTable dt = new DataTable("ChannelProgramUser");
            dt = ds.Tables[0];

            Model.ChannelProgramUser[] mcpus = new Model.ChannelProgramUser[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mcpus = new Model.ChannelProgramUser[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ChannelProgramUser mcpu = new Model.ChannelProgramUser();
                    /*
                    mcpu.UserID = dt.Rows[i]["UserID"].ToString();
                    mcpu.UserName = dt.Rows[i]["UserName"].ToString();
                    mcpu.UserRole = dt.Rows[i]["UserRole"].ToString();
                    mcpu.UserState = dt.Rows[i]["UserState"].ToString();
                    */
                    mcpu.ChannelID = Convert.ToInt32(dt.Rows[i]["ChannelID"]);
                    mcpu.ChannelName = dt.Rows[i]["ChannelName"].ToString();
                    mcpu.ProgramID = Convert.ToInt32(dt.Rows[i]["ProgramID"]);
                    mcpu.ProgramName = dt.Rows[i]["ProgramName"].ToString();

                    mcpu.TimeTypeID = Convert.IsDBNull(dt.Rows[i]["TT_id"]) ? -1 : Convert.ToInt32(dt.Rows[i]["TT_id"]);
                    mcpu.DataTypeID = Convert.IsDBNull(dt.Rows[i]["DT_id"]) ? -1 : Convert.ToInt32(dt.Rows[i]["DT_id"]);

                    mcpus[i] = mcpu;
                }

            }

            return mcpus;
        }


        /// <summary>
        /// 按频道栏目ID和用户ID查询频道栏目用户设置
        /// </summary>
        /// <param name="cpID">频道栏目ID</param>
        /// <param name="userID">工号ID</param>
        /// <returns></returns>
        public Model.ChannelProgramUser[] getCPUsersByCPIDAndUserID(int cpID, string userID)
        {
            string sql = "SELECT CP_id, UR_id ";
            sql += "FROM dbo.tb_ChannelProgramUser ";
            sql += "WHERE CP_id = @CPID AND UR_id = @UserID ";

            SqlParameter[] paras = {
                    new SqlParameter("@CPID", cpID),
                    new SqlParameter("@UserID", userID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("ChannelProgramUser");
            dt = ds.Tables[0];

            Model.ChannelProgramUser[] mcpus = new Model.ChannelProgramUser[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mcpus = new Model.ChannelProgramUser[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ChannelProgramUser mcpu = new Model.ChannelProgramUser();

                    mcpu.ChannelID = Convert.ToInt32(dt.Rows[i]["CP_id"]);
                    //mcpu.ChannelName = dt.Rows[i]["ChannelName"].ToString();
                    mcpu.ProgramID = Convert.ToInt32(dt.Rows[i]["UR_id"]);
                    //mcpu.ProgramName = dt.Rows[i]["ProgramName"].ToString();

                    mcpus[i] = mcpu;
                }

            }

            return mcpus;
        }


        /// <summary>
        /// 查询指定制片人已设置的频道
        /// </summary>
        /// <param name="userID">工号ID</param>
        /// <returns></returns>
        public Model.ChannelProgram[] getUsedChannelByUserID(string userID)
        {
            string sql = "SELECT a.* ";
            sql += "FROM dbo.tb_ChannelProgram AS a ";
            sql += "INNER JOIN dbo.tb_ChannelProgramUser AS b ON a.id = b.CP_id ";
            sql += "WHERE b.UR_id = @UserID ";
            sql += "ORDER BY a.CP_order ";

            SqlParameter[] paras = {
                    new SqlParameter("@UserID", userID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("ChannelProgram");
            dt = ds.Tables[0];

            Model.ChannelProgram[] mcps = new Model.ChannelProgram[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mcps = new Model.ChannelProgram[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ChannelProgram mcp = new Model.ChannelProgram();

                    mcp.CP_ID = Convert.ToInt32(dt.Rows[i]["id"]);
                    mcp.CP_Name = dt.Rows[i]["CP_name"].ToString();
                    mcp.FatherID = Convert.ToInt32(dt.Rows[i]["FatherID"]);
                    //mcp.TimeTypeID = Convert.ToInt32(dt.Rows[i]["TT_id"]);
                    //mcp.DataTypeID = Convert.ToInt32(dt.Rows[i]["DT_id"]);
                    mcp.CP_Order = Convert.ToInt32(dt.Rows[i]["CP_order"]);

                    mcps[i] = mcp;
                }
            }

            return mcps;

        }


        /// <summary>
        /// 查询指定制片人未设置的频道
        /// </summary>
        /// <param name="userID">工号ID</param>
        /// <returns></returns>
        public Model.ChannelProgram[] getUsableChannelByUserID(string userID)
        {
            string sql = "SELECT a.* ";
            sql += "FROM dbo.tb_ChannelProgram AS a ";
            sql += "WHERE a.id NOT IN ( ";
            sql += "SELECT CP_id FROM dbo.tb_ChannelProgramUser AS cpu ";
            sql += "WHERE cpu.UR_id = @UserID ) ";
            sql += "AND a.FatherID = 0 ";
            sql += "ORDER BY a.CP_order ";

            SqlParameter[] paras = {
                    new SqlParameter("@UserID", userID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("ChannelProgram");
            dt = ds.Tables[0];

            Model.ChannelProgram[] mcps = new Model.ChannelProgram[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mcps = new Model.ChannelProgram[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ChannelProgram mcp = new Model.ChannelProgram();

                    mcp.CP_ID = Convert.ToInt32(dt.Rows[i]["id"]);
                    mcp.CP_Name = dt.Rows[i]["CP_name"].ToString();
                    mcp.FatherID = Convert.ToInt32(dt.Rows[i]["FatherID"]);
                    //mcp.TimeTypeID = Convert.ToInt32(dt.Rows[i]["TT_id"]);
                    //mcp.DataTypeID = Convert.ToInt32(dt.Rows[i]["DT_id"]);
                    mcp.CP_Order = Convert.ToInt32(dt.Rows[i]["CP_order"]);

                    mcps[i] = mcp;
                }
            }

            return mcps;

        }


        /// <summary>
        /// 查询指定频道已设置的用户
        /// </summary>
        /// <param name="cpID"></param>
        /// <returns></returns>
        public Model.User[] getUsedUserByCPID(int cpID)
        {
            string sql = "SELECT * FROM dbo.tb_User ";
            sql += "WHERE dbo.tb_User.id IN ( ";
            sql += "SELECT UR_id FROM dbo.tb_ChannelProgramUser ";
            sql += "WHERE dbo.tb_ChannelProgramUser.CP_id = @CPID ";
            sql += " ) ORDER BY id ";

            SqlParameter[] paras = {
                    new SqlParameter("@CPID", cpID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("User");
            dt = ds.Tables[0];

            Model.User[] mus = new Model.User[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mus = new Model.User[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.User mu = new Model.User();

                    mu.UserID = dt.Rows[i]["id"].ToString();
                    mu.UserName = dt.Rows[i]["name"].ToString();
                    mu.UserRole = dt.Rows[i]["role"].ToString();
                    mu.UserState = dt.Rows[i]["state"].ToString();

                    mus[i] = mu;
                }
            }

            return mus;
        }


        /// <summary>
        /// 查询指定频道可设置的用户
        /// </summary>
        /// <param name="cpID"></param>
        /// <returns></returns>
        public Model.User[] getUsableUserByCPID(int cpID)
        {
            string sql = "SELECT * FROM dbo.tb_User ";
            sql += "WHERE dbo.tb_User.id NOT IN ( ";
            sql += "SELECT UR_id FROM dbo.tb_ChannelProgramUser ";
            sql += "WHERE dbo.tb_ChannelProgramUser.CP_id = @CPID ";
            sql += " ) AND tb_user.role <> '0' ORDER BY id ";

            SqlParameter[] paras = {
                    new SqlParameter("@CPID", cpID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("User");
            dt = ds.Tables[0];

            Model.User[] mus = new Model.User[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mus = new Model.User[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.User mu = new Model.User();

                    mu.UserID = dt.Rows[i]["id"].ToString();
                    mu.UserName = dt.Rows[i]["name"].ToString();
                    mu.UserRole = dt.Rows[i]["role"].ToString();
                    mu.UserState = dt.Rows[i]["state"].ToString();

                    mus[i] = mu;
                }
            }

            return mus;
        }


        /// <summary>
        /// 查询所有站点信息(包含栏目已配置和未配置的站点)
        /// </summary>
        /// <param name="msr"></param>
        /// <returns></returns>
        public Model.StationRelation[] getStations(Model.StationRelation msr)
        {
            string sql = "SELECT id, stationid, name_cn FROM dbo.tb_StationInfo ";
            sql += "WHERE (@chlid = -1 OR id IN ( ";
            sql += "SELECT DISTINCT a.selectID ";
            sql += "FROM dbo.tb_StationSetting AS a, dbo.tb_ChannelProgram AS b ";
            sql += "WHERE a.CP_id = b.id AND (b.FatherID = @chlid OR @chlid = -1) ";
            sql += "AND (b.id = @prgid OR @prgid = -1) ) ) ";
            sql += "AND (stationId = @staid OR @staid = -1) ";
            sql += "AND (name_cn LIKE '%' + @staname + '%' OR @staname IS NULL) ";

            SqlParameter[] paras = {
                    new SqlParameter("@chlid", msr.Channel_ID),
                    new SqlParameter("@prgid", msr.Program_ID),
                    new SqlParameter("@staid", msr.StationID),
                    new SqlParameter("@staname", msr.StationName)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("StationRelation");
            dt = ds.Tables[0];

            Model.StationRelation[] msrs = new Model.StationRelation[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                msrs = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.StationRelation item = new Model.StationRelation();

                    item.StationTableID = Convert.ToInt32(dt.Rows[i]["id"]);
                    item.StationID = Convert.ToInt32(dt.Rows[i]["stationid"]);
                    item.StationName = dt.Rows[i]["name_cn"].ToString();

                    msrs[i] = item;
                }
            }

            return msrs;
        }


        /// <summary>
        /// 查询指定用户栏目配置的站点信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="msr"></param>
        /// <returns></returns>
        public Model.StationRelation[] getStations(string userID, Model.StationRelation msr)
        {
            string sql = "SELECT id, stationid, name_cn FROM dbo.tb_StationInfo ";
            sql += "WHERE id IN ( ";
            sql += "SELECT DISTINCT selectID FROM dbo.tb_StationSetting AS a ";
            sql += "WHERE CP_id IN ( ";
            sql += "SELECT id FROM dbo.tb_ChannelProgram ";
            sql += "WHERE FatherID IN ( ";
            sql += "SELECT CP_id FROM dbo.tb_ChannelProgramUser ";
            sql += "WHERE (UR_id = @userid) AND (CP_id = @chlid OR @chlid = -1) ) ";
            sql += "AND (id = @prgid OR @prgid = -1) ) ) ";
            sql += "AND (stationId = @staid OR @staid = -1) ";
            sql += "AND (name_cn LIKE '%' + @staname + '%' OR @staname IS NULL) ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", userID),
                    new SqlParameter("@chlid", msr.Channel_ID),
                    new SqlParameter("@prgid", msr.Program_ID),
                    new SqlParameter("@staid", msr.StationID),
                    new SqlParameter("@staname", msr.StationName)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("StationRelation");
            dt = ds.Tables[0];

            Model.StationRelation[] msrs = new Model.StationRelation[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                msrs = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.StationRelation item = new Model.StationRelation();

                    item.StationTableID = Convert.ToInt32(dt.Rows[i]["id"]);
                    item.StationID = Convert.ToInt32(dt.Rows[i]["stationid"]);
                    item.StationName = dt.Rows[i]["name_cn"].ToString();

                    msrs[i] = item;
                }
            }

            return msrs;
        }


        /// <summary>
        /// 查询制片人可管理的站点信息
        /// </summary>
        /// <param name="userID">工号ID</param>
        /// <returns>字典集合(站点索引, 站点引用的栏目数量)</returns>
        public Dictionary<string, string> getUserStations(string userID)
        {
            string sql = "SELECT a.id, a.stationid, a.name_cn, COUNT(DISTINCT CP_id) AS cpcount ";
            sql += "FROM dbo.tb_StationInfo AS a, dbo.tb_StationSetting AS b ";
            sql += "WHERE a.id = b.selectID AND b.selectID IN ( ";
            sql += "SELECT DISTINCT selectID ";
            sql += "FROM dbo.tb_StationSetting AS a1, dbo.tb_ChannelProgram AS b1, dbo.tb_ChannelProgramUser AS c1 ";
            sql += "WHERE a1.CP_id = b1.id AND b1.FatherID = c1.CP_id AND UR_id = @userid ) ";
            sql += "GROUP BY a.id, a.stationid, a.name_cn ";
            sql += "ORDER BY a.id ";

            SqlParameter[] paras = {
                    new SqlParameter("@userid", userID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("StationRelation");
            dt = ds.Tables[0];

            Dictionary<string, string> dicts = new Dictionary<string, string>();

            if (dt.Rows.Count > 0)      // 有内容
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dicts.Add(dt.Rows[i]["id"].ToString(), dt.Rows[i]["cpcount"].ToString());
                }
            }

            return dicts;
        }


        /// <summary>
        /// 查询指定栏目已配置的项目要素
        /// </summary>
        /// <param name="cpID">栏目ID</param>
        /// <returns></returns>
        public Model.ElementRelation[] getUsedElementsByCPID(string cpID)
        {
            string sql = "SELECT id, elName, elDataNameCn, elCodeTrans, DT_id ";
            sql += "FROM dbo.tb_ElementInfo INNER JOIN dbo.tb_ElementSetting ";
            sql += "ON dbo.tb_ElementInfo.id = dbo.tb_ElementSetting.ET_id ";
            sql += "WHERE dbo.tb_ElementSetting.CP_id = @cpid ";
            sql += "ORDER BY ET_order ";

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", cpID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("ElementRelation");
            dt = ds.Tables[0];

            Model.ElementRelation[] mers = new Model.ElementRelation[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mers = new Model.ElementRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ElementRelation item = new Model.ElementRelation();

                    item.ElementID = Convert.ToInt32(dt.Rows[i]["id"]);
                    item.ElementName = dt.Rows[i]["elName"].ToString();
                    item.ElementNameCN = dt.Rows[i]["elDataNameCn"].ToString();

                    item.TransCodeName = dt.Rows[i]["elCodeTrans"].ToString();
                    item.DataTypeID = Convert.ToInt32(dt.Rows[i]["DT_id"]);

                    mers[i] = item;
                }
            }

            return mers;
        }


        /// <summary>
        /// 查询指定栏目未配置的项目要素
        /// </summary>
        /// <param name="cpID">栏目ID</param>
        /// <returns></returns>
        public Model.ElementRelation[] getUsableElementsByCPID(string cpID)
        {
            string sql = "SELECT a.id, elName, elDataNameCn, elCodeTrans, a.DT_id ";
            sql += "FROM dbo.tb_ElementInfo AS a INNER JOIN dbo.tb_ChannelProgram AS b ";
            sql += "ON a.DT_id = b.DT_id ";
            sql += "WHERE a.id NOT IN ( ";
            sql += "SELECT ET_id FROM dbo.tb_ElementSetting ";
            sql += "WHERE CP_id = @cpid ";
            sql += " ) AND b.id = @cpid ";
            sql += "ORDER BY a.id ";

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", cpID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("ElementRelation");
            dt = ds.Tables[0];

            Model.ElementRelation[] mers = new Model.ElementRelation[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                mers = new Model.ElementRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.ElementRelation item = new Model.ElementRelation();

                    item.ElementID = Convert.ToInt32(dt.Rows[i]["id"]);
                    item.ElementName = dt.Rows[i]["elName"].ToString();
                    item.ElementNameCN = dt.Rows[i]["elDataNameCn"].ToString();

                    item.TransCodeName = dt.Rows[i]["elCodeTrans"].ToString();
                    item.DataTypeID = Convert.ToInt32(dt.Rows[i]["DT_id"]);

                    mers[i] = item;
                }
            }

            return mers;
        }


        /// <summary>
        /// 查询指定栏目已配置的站点信息
        /// </summary>
        /// <param name="cpID">栏目ID</param>
        /// <returns></returns>
        public Model.StationRelation[] getUsedStationsByCPID(string cpID)
        {
            string sql = "SELECT id, stationid, name_cn, b.stationOrder ";
            sql += "FROM dbo.tb_StationInfo	AS a ";
            sql += "INNER JOIN ( ";
            sql += "SELECT selectID, stationOrder ";
            sql += "FROM dbo.tb_StationSetting AS b ";
            sql += "WHERE CP_id = @cpid ";
            sql += " ) AS b ON a.id = b.selectID ";
            sql += "ORDER BY b.stationOrder ";

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", cpID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("StationRelation");
            dt = ds.Tables[0];

            Model.StationRelation[] msrs = new Model.StationRelation[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                msrs = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.StationRelation item = new Model.StationRelation();

                    item.StationTableID = Convert.ToInt32(dt.Rows[i]["id"]);
                    item.StationID = Convert.ToInt32(dt.Rows[i]["stationid"]);
                    item.StationName = dt.Rows[i]["name_cn"].ToString();
                    //item.StationOrder = Convert.ToInt32(dt.Rows[i]["stationOrder"]);

                    msrs[i] = item;
                }
            }

            return msrs;
        }


        /// <summary>
        /// 查询指定栏目未配置的站点信息
        /// </summary>
        /// <param name="cpID">栏目ID</param>
        /// <returns></returns>
        public Model.StationRelation[] getUsableStationsByCPID(string cpID)
        {
            string sql = "SELECT id, stationid, name_cn ";
            sql += "FROM dbo.tb_StationInfo ";
            sql += "WHERE id NOT IN ( ";
            sql += "SELECT selectID FROM dbo.tb_StationSetting ";
            sql += "WHERE CP_id = @cpid ) ";
            sql += "ORDER BY name_cn, stationid ";

            SqlParameter[] paras = {
                    new SqlParameter("@cpid", cpID)
            };

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql, paras);

            DataTable dt = new DataTable("StationRelation");
            dt = ds.Tables[0];

            Model.StationRelation[] msrs = new Model.StationRelation[0];

            if (dt.Rows.Count > 0)      // 有内容
            {
                msrs = new Model.StationRelation[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.StationRelation item = new Model.StationRelation();

                    item.StationTableID = Convert.ToInt32(dt.Rows[i]["id"]);
                    item.StationID = Convert.ToInt32(dt.Rows[i]["stationid"]);
                    item.StationName = dt.Rows[i]["name_cn"].ToString();
                    //item.StationOrder = Convert.ToInt32(dt.Rows[i]["stationOrder"]);

                    msrs[i] = item;
                }
            }

            return msrs;
        }

    }
}
