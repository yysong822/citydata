using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class test : System.Web.UI.Page
{
    //Model.RecordeData[] mrd = new Model.RecordeData[1000];
    ArrayList al = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        //changePage("166");//翻页测试

        //testSearchStationInfo();
        //testGetDataType();


        //testGetTimeType();
        //testGetAllElement();
        //testinsertStationsInfo();
        //testupdateSetStationsInfo();
       //testdeleteSetStationsInfo();
        //testSearchStationInfo();
        //testinsertSetCP();
        //testupdateSetCP();
        //testdeleteSetCP();

        
        //testSetCPfid();

        //testSetAllSetting();

        //testinsertSetStationsInfoReturnID();
        //testSearchStationInfo2();


        //testGetData();


        //testGetZBstations();
        testGetZBdata();
        //testGetZBdataBySX();
        //testGetZBSX();

        //testGetZBcpid();

        GridView1.DataSource = al;
        GridView1.DataBind();
    }





    public void changePage(string cpid)
    {
        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.RecordeData[] mrd = dbll.getRealStations(cpid);
        for (int i = 0; i < mrd.Length; i++)
        {
            al.Add(mrd[i]);
        }
    }


    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = al;
        GridView1.DataBind();
    }



    public void testSearchStationInfo()
    {
        Model.StationRelation msr = new Model.StationRelation();
        msr.StationID = 54511;
        //msr.StationName = "北";

        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.StationRelation[] msr2 = dbll.searchStationInfo(msr);
        for (int i = 0; i < msr2.Length; i++)
        {
            al.Add(msr2[i]);
        }

    }


    public void testGetDataType()
    {
        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.RecordeData[] mrd2 = dbll.getDataType(null);
        for (int i = 0; i < mrd2.Length; i++)
        {
            al.Add(mrd2[i]);
        }
    }



    public void testGetTimeType()
    {
        Model.RecordeData mrd = new Model.RecordeData();
        mrd.DataType = 1;

        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.RecordeData[] mrd2 = dbll.getTimeType(mrd);
        for (int i = 0; i < mrd2.Length; i++)
        {
            al.Add(mrd2[i]);
        }

    }


    public void testGetAllElement()
    {
        Model.ElementRelation mer = new Model.ElementRelation();
        mer.DataTypeID = 2;

        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.ElementRelation[] mer2 = dbll.getAllElement(mer);
        for (int i = 0; i < mer2.Length; i++)
        {
            al.Add(mer2[i]);
        }

    }


    public void testinsertSetStationsInfo()
    {
        Model.StationRelation msr = new Model.StationRelation();
        msr.StationID = 54511;
        //msr.StationName = "new-beijing3";

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        int done = sbll.setStationsInfo(msr,"insert");
        Label1.Text = done.ToString();

    }

    public void testupdateSetStationsInfo()
    {
        Model.StationRelation msr = new Model.StationRelation();
        msr.StationID = 55555;
        msr.StationName = "new-beijingfff";
        msr.StationTableID = 682;

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        int done = sbll.setStationsInfo(msr, "update");
        Label1.Text = done.ToString();

    }

    public void testdeleteSetStationsInfo()
    {
        Model.StationRelation msr = new Model.StationRelation();
        msr.StationTableID = 683;

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        int done = sbll.setStationsInfo(msr, "delete");
        Label1.Text = done.ToString();

    }


    public void testinsertSetCP()
    {
        Model.ChannelProgram mcp = new Model.ChannelProgram();
        mcp.CP_Name = "测试栏目3";
        mcp.FatherID = 170;

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        int done = sbll.setCP(mcp, "insert");
        Label1.Text = done.ToString();

    }


    public void testupdateSetCP()
    {
        Model.ChannelProgram mcp = new Model.ChannelProgram();
        mcp.CP_Name = "测试0频道";
        mcp.CP_ID = 170;

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        int done = sbll.setCP(mcp, "update");
        Label1.Text = done.ToString();

    }


    public void testdeleteSetCP()
    {
        Model.ChannelProgram mcp = new Model.ChannelProgram();
        mcp.CP_ID = 171;

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        int done = sbll.setCP(mcp, "delete");
        Label1.Text = done.ToString();

    }

    public void testSetCPfid()
    {
        Model.ChannelProgram mcp = new Model.ChannelProgram();
        mcp.FatherID = 172;
        mcp.CP_ID = 170;

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        int done = sbll.setCPnewFather(mcp);
        Label1.Text = done.ToString();

    }


    public void testSetAllSetting()
    {
        //Model.ChannelProgram mcp, Model.ElementRelation[] mer,Model.StationRelation[] msr
        Model.ChannelProgram mcp = new Model.ChannelProgram();
        mcp.CP_ID = 170;
        mcp.DataTypeID = 1;
        mcp.TimeTypeID = 8;


        Model.ElementRelation[] mer = new Model.ElementRelation[3];

        Model.ElementRelation mer_single1 = new Model.ElementRelation();
        mer_single1.ElementID = 1;
        mer_single1.ElementOrder = 0;

        Model.ElementRelation mer_single2 = new Model.ElementRelation();
        mer_single2.ElementID = 3;
        mer_single2.ElementOrder = 1;

        Model.ElementRelation mer_single3 = new Model.ElementRelation();
        mer_single3.ElementID = 7;
        mer_single3.ElementOrder = 2;

        mer[0] = mer_single1;
        mer[1] = mer_single2;
        mer[2] = mer_single3;


        Model.StationRelation[] msr = new Model.StationRelation[3];

        Model.StationRelation msr_single1 = new Model.StationRelation();
        msr_single1.SelectID = 172;
        msr_single1.StationID = 50774;
        msr_single1.StationOrder = 0;

        Model.StationRelation msr_single2 = new Model.StationRelation();
        msr_single2.SelectID = 30;
        msr_single2.StationID = 56778;
        msr_single2.StationOrder = 1;

        Model.StationRelation msr_single3 = new Model.StationRelation();
        msr_single3.SelectID = 512;
        msr_single3.StationID = 59432;
        msr_single3.StationOrder = 2;

        msr[0] = msr_single1;
        msr[1] = msr_single2;
        msr[2] = msr_single3;



        BLL.SettingBLL sbll = new BLL.SettingBLL();
        String[] display = new String[5];
        display = sbll.setAllSetting(mcp, mer, msr);
        Label1.Text = display[0] + "/" + display[1] + "/" + display[2] + "/" + display[3] + "/" + display[4];




    }


    public void testinsertSetStationsInfoReturnID()
    {
        Model.StationRelation msr = new Model.StationRelation();
        msr.StationID = 54511;
        msr.StationName = "beijingReturnID2";

        BLL.SettingBLL sbll = new BLL.SettingBLL();
        //int done = sbll.setStationsInfo(msr, "insert");
        Label1.Text = sbll.insertStationsInfoReturnID(msr).StationTableID.ToString() + "---" + sbll.insertStationsInfoReturnID(msr).SelectID.ToString();

    }


    public void testSearchStationInfo2()
    {
        Model.StationRelation msr = new Model.StationRelation();
        msr.StationID = 54511;
        msr.StationName = "北xx京";

        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.StationRelation msr2 = dbll.searchStationInfo2(msr);
        //Label1.Text = msr2.SelectID + "|" + msr2.StationID + "|" + msr2.StationName + "|" + msr2.StationTableID;

    }



    public void testGetData()
    {
        //public Model.RecordeData[] getData(String cpid)


        //Model.StationRelation msr = new Model.StationRelation();
        //msr.StationID = 54511;
        //msr.StationName = "北";

        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.RecordeData[] msr2 = dbll.getData("75");
        for (int i = 0; i < msr2.Length; i++)
        {
            al.Add(msr2[i]);
        }
    }


    public void testGetZBstations()
    {

        Model.ChannelProgram mcp=new Model.ChannelProgram();
        mcp.DataTypeID = 1;
        mcp.TimeTypeID = 6;



        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.StationRelation[] msr2 = dbll.getZBStations(mcp);
        for (int i = 0; i < msr2.Length; i++)
        {
            al.Add(msr2[i]);
        }
        Label1.Text = msr2.Length.ToString();
    }


    public void testGetZBdata()
    {

        Model.RecordeData mrd = new Model.RecordeData();
        mrd.TimeType = 6;
        mrd.DataType = 1;



        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.RecordeData[] msr2 = dbll.getZBData(mrd);
        for (int i = 0; i < msr2.Length; i++)
        {
            al.Add(msr2[i]);
        }
        Label1.Text = msr2.Length.ToString();
    }


    public void testGetZBdataBySX()
    {

        Model.RecordeData mrd = new Model.RecordeData();
        mrd.Time = 24;
        mrd.TimeType = 6;
        mrd.DataType = 1;



        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.RecordeData[] msr2 = dbll.getZBDataBySX(mrd);
        for (int i = 0; i < msr2.Length; i++)
        {
            al.Add(msr2[i]);
        }
        Label1.Text = msr2.Length.ToString();
    }

    public void testGetZBSX()
    {

        Model.RecordeData mrd = new Model.RecordeData();
        mrd.TimeType = 6;
        mrd.DataType = 1;



        BLL.DataBLL dbll = new BLL.DataBLL();
        Model.RecordeData[] msr2 = dbll.getZBSX(mrd);
        for (int i = 0; i < msr2.Length; i++)
        {
            al.Add(msr2[i]);
        }
        Label1.Text = msr2.Length.ToString();
    }




    public void testGetZBcpid()
    {
        Model.ChannelProgram mcp = new Model.ChannelProgram();
        mcp.DataTypeID = 1;

        BLL.DataBLL dbll = new BLL.DataBLL();
        //int done = sbll.setStationsInfo(msr, "insert");
        Label1.Text = dbll.getZBcpid(mcp).CP_ID.ToString();


    }






}
