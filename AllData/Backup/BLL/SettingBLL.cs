using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    public class SettingBLL
    {
        DAL.SettingDAL sdal = new DAL.SettingDAL();


        public int setStationsInfo(Model.StationRelation msr,string command)
        {
            return sdal.setStationsInfo(msr,command);
        }


        public Model.StationRelation insertStationsInfoReturnID(Model.StationRelation msr)
        {
            return sdal.insertStationsInfoReturnID(msr);
        }

        public int setCP(Model.ChannelProgram mcp, string command)
        {
            return sdal.setCP(mcp,command);
        }


        public int setCPnewFather(Model.ChannelProgram mcp)//更改栏目所在频道
        {
            return sdal.setCPnewFather(mcp);
        }

        public String[] setAllSetting(Model.ChannelProgram mcp, Model.ElementRelation[] mer, Model.StationRelation[] msr)
        {
            return sdal.setAllSetting(mcp, mer, msr);
        }


        public int setCPnewOrder(Model.ChannelProgram mcp)
        {
            return sdal.setCPnewOrder(mcp);
        }



        // ----------------------------------------------------------------------------------
        // Added by Edward Chan.

        public bool addUser(Model.User mu)
        {
            return sdal.addUser(mu);
        }

        public bool updateUser(Model.User mu)
        {
            return sdal.updateUser(mu);
        }

        /*
        public bool updateUser2(Model.User mu)
        {
            return sdal.updateUser2(mu);
        }
        */

        public bool updatePwd(Model.User mu)
        {
            return sdal.updatePwd(mu);
        }

        public bool deleteUser(string id)
        {
            return sdal.deleteUser(id);
        }

        public bool addCPUser(string userID, int cpID)
        {
            return sdal.addCPUser(userID, cpID);
        }

        public bool delCPUser(string userID, int cpID)
        {
            return sdal.delCPUser(userID, cpID);
        }

        public bool addStationInfoWithSetting(Model.StationRelation msr, out int selectID)
        {
            return sdal.addStationInfoWithSetting(msr, out selectID);
        }

        public bool setCPDataTypeAndTimeType(int cpID, int dtID, int ttID)
        {
            return sdal.updateCPDataTypeAndTimeType(cpID, dtID, ttID);
        }

        public bool addElementAndStationSetting(int cpID, string eleStr, string staStr)
        {
            return sdal.addElementAndStationSetting(cpID, eleStr, staStr);
        }

    }
}
