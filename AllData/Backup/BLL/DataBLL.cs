using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    public class DataBLL
    {
        DAL.DataDAL ddal = new DAL.DataDAL();

        public Model.ChannelProgram[] getCP(String fatherid)
        {
            return ddal.getCP(fatherid);
        }


        public Model.RecordeData[] getData(String cpid)
        {
            return ddal.getData(cpid);
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
            return ddal.getData2(cpID, dtID, ttID, reptTime, time);
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
            return ddal.getData2X(cpID, dtID, ttID, reptTime, times);
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
            return ddal.getDataPre(cpid, ttid, dtid, reporttime);
        }

        public Model.RecordeData[] getZBData(Model.RecordeData par_mrd)
        {
            return ddal.getZBData(par_mrd);
        }


        /// <summary>
        /// syy 20130627 增加前一天数据检索，以便进行最高温和最低温的对比 ZB
        /// </summary>
        /// <param name="cpid"></param>
        /// <param name="ttid"></param>
        /// <param name="dtid"></param>
        /// <param name="reporttime"></param>
        /// <returns></returns>
        public Model.RecordeData[] getZBDataPre(Model.RecordeData par_mrd, String reporttime)
        {
            return ddal.getZBDataPre(par_mrd, reporttime);
        }

        public Model.RecordeData[] getZBDataBySX(Model.RecordeData par_mrd)
        {
            return ddal.getZBDataBySX(par_mrd);
        }

        public Model.RecordeData[] getDataBySX(String cpid, String sx)
        {
            return ddal.getDataBySX(cpid, sx);
        }

        public Model.RecordeData[] getZBSX(Model.RecordeData par_mrd)
        {
            return ddal.getZBSX(par_mrd);
        }


        public Model.ChannelProgram getZBcpid(Model.ChannelProgram mcp)
        {
            return ddal.getZBcpid(mcp);
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
            return ddal.getSX2(cpID, dtID, ttID);
        }

        public Model.RecordeData[] getSX(string cpID)
        {
            return ddal.getSX(cpID);
        }


        public Model.ElementRelation[] DecordeDataSet(String typeid)
        {
            return ddal.DecordeDataSet(typeid);
        }

        public Model.ElementRelation[] DecordeDataSetType(String cpid)
        {
            return ddal.DecordeDataSetType(cpid);
        }



        public Model.ElementRelation[] getElementsName(String cpid)
        {
            return ddal.getElementsName(cpid);
        }


        public Model.StationRelation[] getAllStationsModel(String cpid)
        {
            return ddal.getAllStationsModel(cpid);
        }


        public Model.StationRelation[] getZBStations(Model.ChannelProgram mcp)
        {
            return ddal.getZBStations(mcp);
        }

        public Model.RecordeData[] getRealStations(String cpid)
        {
            return ddal.getRealStations(cpid);
        }


        public Model.StationRelation[] getStationsInfo()
        {
            return ddal.getStationsInfo();
        }

        public Model.StationRelation[] getStationsInfoOrderByName()
        {
            return ddal.getStationsInfoOrderByName();
        }

        public Model.StationRelation[] searchStationInfo(Model.StationRelation msr)
        {
            return ddal.searchStationInfo(msr);
        }

        public Model.StationRelation searchStationInfo2(Model.StationRelation msr)
        {
            return ddal.searchStationInfo2(msr);
        }


        public Model.RecordeData[] getDataType(int? id)
        {
            return ddal.getDataType(id);
        }


        public Model.RecordeData[] getTimeType(Model.RecordeData mrd)
        {
            return ddal.getTimeType(mrd);
        }


        public Model.ElementRelation[] getAllElement(Model.ElementRelation mer)
        {
            return ddal.getAllElement(mer);
        }

        /*
        public Boolean loginConfirm(Model.Admin ma)
        {
            return ddal.loginConfirm(ma);
        }
         * */


        // ----------------------------------------------------------------------------------
        // Added by Edward Chan.

        public bool isUserExist(string userID)
        {
            return ddal.isUserExist(userID);
        }

        public Model.User[] getUsers(Model.User mu)
        {
            return mu == null ? ddal.getUsers() : ddal.getUsers(mu);
        }

        public Model.ChannelProgramUser[] getCPUsers()
        {
            return ddal.getCPUsers();
        }

        public Model.ChannelProgramUser[] getCPUsersByUserID(string userID)
        {
            return ddal.getCPUsersByUserID(userID);
        }

        public Model.ChannelProgramUser[] getCPs()
        {
            return ddal.getCPs();
        }

        public Model.ChannelProgramUser[] getCPUsersByCPIDAndUserID(int cpID, string userID)
        {
            return ddal.getCPUsersByCPIDAndUserID(cpID, userID);
        }

        public Model.ChannelProgram[] getUsedChannelByUserID(string userID)
        {
            return ddal.getUsedChannelByUserID(userID);
        }

        public Model.ChannelProgram[] getUsableChannelByUserID(string userID)
        {
            return ddal.getUsableChannelByUserID(userID);
        }

        public Model.User[] getUsedUserByCPID(int cpID)
        {
            return ddal.getUsedUserByCPID(cpID);
        }

        public Model.User[] getUsableUserByCPID(int cpID)
        {
            return ddal.getUsableUserByCPID(cpID);
        }

        public Model.StationRelation[] getStations(string userID, Model.StationRelation msr)
        {
            return userID == null ? ddal.getStations(msr) : ddal.getStations(userID, msr);
        }

        public Dictionary<string, string> getUserStations(string userID)
        {
            return ddal.getUserStations(userID);
        }

        public Model.ElementRelation[] getUsedElementsByCPID(string cpID)
        {
            return ddal.getUsedElementsByCPID(cpID);
        }

        public Model.ElementRelation[] getUsableElementsByCPID(string cpID)
        {
            return ddal.getUsableElementsByCPID(cpID);
        }

        public Model.StationRelation[] getUsedStationsByCPID(string cpID)
        {
            return ddal.getUsedStationsByCPID(cpID);
        }

        public Model.StationRelation[] getUsableStationsByCPID(string cpID)
        {
            return ddal.getUsableStationsByCPID(cpID);
        }

    }
}
