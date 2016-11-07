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
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.IO;
using Model;
using System.Reflection;



public partial class Default : System.Web.UI.Page
{
    BLL.DataBLL dbll = new BLL.DataBLL();

    //20090814byHan，去掉static
    private ArrayList alTransCode = new ArrayList();
    private Model.RecordeData[] mRecordeData;
    private Model.ChannelProgram[] mChannelInfo;
    private Model.ChannelProgram[] mProgramInfo;
    private Model.RecordeData[] mTimeSXInfo;
    private Model.StationRelation[] mStationRelation;
    private Model.ElementRelation[] mElementRelation;
    private string[] stationName;

    private Hashtable equalMaxMinTemStationName;



    /// <summary>
    /// syy 20130627 增加前一天的记录，以供对比判断最高温和最低温差大于8度
    /// </summary>
    private Model.RecordeData[] mRecordeData_Pre;
    /// <summary>
    /// syy 20130627 温度差大于8度的记录
    /// </summary>
    List<TemRecordData> rd_Tem_SubErr;



    /// <summary>
    /// syy 20130701 最高气温》=45 || 最低气温《=-40
    /// </summary>
    private Hashtable hlMaxTemStationName;
    /// <summary>
    /// syy 20130701 最低气温》5，天气现象为冻雨、雨加雪、小雪、中雪、大雪、暴雪
    /// </summary>
    private Hashtable ltempWethExpStationName;
    /// <summary>
    /// syy 20130701 风力》=3，无风向
    /// </summary>
    private Hashtable winNStationName;
    /// <summary>
    /// syy 20130701 天气现象为大暴雨、大暴雪
    /// </summary>
    private Hashtable wethExpStationName;

    private string equalTemMessage = "<li>最高温和最低温一致的城市：";
    private string hlTemMessage = "<li>异常气温(最高温>=45℃,最低温<=-40℃)的城市：";
    private string subTemMessage = "<li>最高气温或者最低气温和前一天预报相差>=8℃的城市：";
    private string lTemWethMessage = "<li>最低气温>5℃,但天气现象为冻雨、雨夹雪、小雪、中雪、大雪、暴雪的城市：";
    private string windNMessage = "<li>有风力(>=3级)，但无风向的城市：";
    private string wethExpMessage = "<li>天气现象为大暴雨、大暴雪的城市：";



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            mChannelInfo = null;
            initDDLChannel("0");
            ListItem newItemProgram = new ListItem();
            newItemProgram.Text = "全部栏目";
            newItemProgram.Value = "全部栏目";
            ddl_program.Items.Add(newItemProgram);
            ListItem newItemTimeSX = new ListItem();
            newItemTimeSX.Text = "选择时效";
            newItemTimeSX.Value = "选择时效";
            ddl_timeSX.Items.Add(newItemTimeSX);
            p_text.Visible = true;
            p_noExistStation.Visible = false;

            //20130702 syy
            p_TepExpTip.Visible = false;
            p_WethExp.Visible = false;

        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="categoryID"></param>
    protected void initDDLChannel(string categoryID)
    {
        ddl_channel.Items.Clear();
        ListItem newItemChannel = new ListItem();
        newItemChannel.Text = "全部频道";
        newItemChannel.Value = "全部频道";
        ddl_channel.Items.Add(newItemChannel);
        if (mChannelInfo == null)
        {
            mChannelInfo = dbll.getCP(categoryID);
        }
        int count = mChannelInfo.Length;
        for (int i = 0; i < count; i++)
        {
            ListItem newItem = new ListItem();
            newItem.Text = mChannelInfo[i].CP_Name;
            newItem.Value = mChannelInfo[i].CP_ID.ToString();
            ddl_channel.Items.Add(newItem);
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="channelID"></param>
    protected void initDDLProgram(string channelID)
    {
        ddl_program.Items.Clear();
        ListItem newItemProgram = new ListItem();
        newItemProgram.Text = "全部栏目";
        newItemProgram.Value = "全部栏目";
        ddl_program.Items.Add(newItemProgram);
        if (channelID == "全部频道")
        {
            return;
        }
        else
        {
            if (mProgramInfo == null)
            {
                mProgramInfo = dbll.getCP(channelID);
            }
            int count = mProgramInfo.Length;
            for (int i = 0; i < count; i++)
            {
                ListItem newItem = new ListItem();
                newItem.Text = mProgramInfo[i].CP_Name;
                newItem.Value = mProgramInfo[i].CP_ID.ToString();
                ddl_program.Items.Add(newItem);
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="channelID"></param>
    /// <param name="programID"></param>
    protected void initDDLTimeSX(string channelID, string programID)
    {
        ddl_timeSX.Items.Clear();
        ListItem newItemTimeSX = new ListItem();
        newItemTimeSX.Text = "选择时效";
        newItemTimeSX.Value = "选择时效";
        ddl_timeSX.Items.Add(newItemTimeSX);
        if (channelID == "全部频道" || programID == "全部栏目" || lb_titleTime.Text == "昨天之前数据，不可查看！")
        {
            return;
        }
        else
        {
            if (mTimeSXInfo == null)
            {
                try
                {
                    mTimeSXInfo = dbll.getSX(programID);
                }
                catch
                {
                    mTimeSXInfo = null;
                }
            }
            if (mTimeSXInfo != null && mTimeSXInfo.Length != 0)
            {
                int countSX = mTimeSXInfo.Length;
                for (int i = 0; i < countSX; i++)
                {
                    ListItem newItem = new ListItem();
                    newItem.Text = mTimeSXInfo[i].Time.ToString();
                    newItem.Value = mTimeSXInfo[i].Time.ToString();
                    ddl_timeSX.Items.Add(newItem);
                }
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// SelectedIndexChanged事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_channel_SelectedIndexChanged(object sender, EventArgs e)
    {
        string channelID = ddl_channel.SelectedValue;
        p_text.Visible = true;
        clearGV(gv_citydata);
        mProgramInfo = null;
        p_tip.Visible = false;
        p_timeInterval.Visible = false;
        p_noExistStation.Visible = false;
        p_print.Visible = false;
        p_down.Visible = false;
        ddl_timeSX.Enabled = true;
        initDDLProgram(channelID);
        initDDLTimeSX(channelID, "全部栏目");

        //20120830 syy 增加最高温最低温相等提示
        //20130702 syy 初始化Tip信息
        initTipInfo();



    }

    /// <summary>
    /// SelectedIndexChanged事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_program_SelectedIndexChanged(object sender, EventArgs e)
    {
        ////////////////////////////////////////////////
        //20130701 增加信息提示部分，包括天气现象、温度差异等等
        //////////////////////////////////////////////////
        initTipInfo();


        string programID = ddl_program.SelectedValue;
        string channelID = ddl_channel.SelectedValue;
        mRecordeData = null;
        //syy 20130627 清除当前的记录的同时也清除前一天的记录信息
        mRecordeData_Pre = null;

        alTransCode.Clear();
        mStationRelation = null;
        mTimeSXInfo = null;
        mElementRelation = null;
        if (programID == "全部栏目")
        {
            p_text.Visible = true;
            p_noExistStation.Visible = false;
            p_tip.Visible = false;
            clearGV(gv_citydata);
            initDDLTimeSX(channelID, programID);
            p_print.Visible = false;
            p_down.Visible = false;
        }
        else
        {
            try//20090814byHan
            {
                clearGV(gv_citydata);
                fillGV(gv_citydata, ddl_program.SelectedValue.Trim(), "选择时效");

                //if (equalMaxMinTemStationName != null && equalMaxMinTemStationName.Count != 0)
                //{
                //    string equalMaxMinTemStationNameStr = "";
                //    foreach (DictionaryEntry de in equalMaxMinTemStationName)
                //    {
                //        equalMaxMinTemStationNameStr += de.Key + ",";
                //    }
                //    lb_equalMaxMinTemTip.Text = "最高温度和最低温度相等的城市有：" + equalMaxMinTemStationNameStr.Substring(0, equalMaxMinTemStationNameStr.Length - 1);
                //    p_TepExpTip.Visible = true;
                //}

                //////////////////////////////////////////////////////////////////////////////////
                //private string equalTemMessage = "最高温和最低温一致的城市：";
                //private string hlTemMessage = "异常气温(最高温>=45℃,最低温<=-40℃)的城市：";
                //private string subTemMessage = "最高气温或者最低气温和前一天预报相差>=8℃的城市：";
                //private string lTemWethMessage = "最低气温>5℃,但天气现象为冻雨、雨夹雪、小雪、中雪、大雪、暴雪的城市：";
                //private string windNMessage = "有风力(>=3级)，但无风向";
                //private string wethExpMessage = "天气现象为大暴雨、大暴雪";
                ////////////////////////////////////////////////////////////////////////////////

                showTipInfo();

                p_text.Visible = false;
                if (lb_titleTime.Text != "昨天之前数据，不可查看！" && lb_noExistStation.Text != "本次查询无数据显示！" && lb_noExistStation.Text != "该栏目还没有进行配置！")
                {
                    initDDLTimeSX(channelID, programID);
                    lb_titleName.Text = ddl_channel.SelectedItem.Text.Trim() + ddl_program.SelectedItem.Text.Trim();
                    lb_titleTimeSX.Text = "全部时效";
                    p_timeInterval.Visible = false;
                    p_print.Visible = true;
                    p_down.Visible = true;
                    p_tip.Visible = true;
                    ddl_timeSX.Enabled = true;
                    if (mRecordeData[0].DataType == 4 || mRecordeData[0].DataType == 9)//sk 没有时效选择ddl
                    {
                        ddl_timeSX.Enabled = false;
                        lb_titleTimeSX.Text = "";
                    }

                }
                else
                {
                    p_print.Visible = false;
                    p_down.Visible = false;
                    if (lb_titleTime.Text == "昨天之前数据，不可查看！")
                    {
                        p_tip.Visible = true;
                        p_noExistStation.Visible = false;
                    }
                    else
                    {
                        p_tip.Visible = false;
                        p_noExistStation.Visible = true;
                    }
                }
            }
            catch { }

        }

    }

    /// <summary>
    /// SelectedIndexChanged事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_timeSX_SelectedIndexChanged(object sender, EventArgs e)
    {
        ////////////////////////////////////////////////
        //20130701 增加信息提示部分，包括天气现象、温度差异等等
        //////////////////////////////////////////////////
        initTipInfo();

        lb_titleTimeSX.Text = ddl_timeSX.SelectedItem.Text.Trim();
        if (lb_titleTimeSX.Text == "选择时效")
        {
            lb_titleTimeSX.Text = "全部时效";
            p_timeInterval.Visible = false;
        }
        else
        {
            lb_titleTimeSX.Text += "小时";
            p_timeInterval.Visible = true;
        }
        /*
         * 根据时效过滤gv
         */
        mRecordeData = null;
        //20130627 清除当前记录同时清除前一天记录信息
        mRecordeData_Pre = null;

        clearGV(gv_citydata);
        try//20090814byHan
        {
            fillGV(gv_citydata, ddl_program.SelectedValue.Trim(), ddl_timeSX.SelectedValue.Trim());
            //if (equalMaxMinTemStationName != null && equalMaxMinTemStationName.Count != 0)
            //{
            //    string equalMaxMinTemStationNameStr = "";
            //    foreach (DictionaryEntry de in equalMaxMinTemStationName)
            //    {
            //        equalMaxMinTemStationNameStr += de.Key + ",";
            //    }
            //    lb_equalMaxMinTemTip.Text = "最高温度和最低温度相等的城市有：" + equalMaxMinTemStationNameStr.Substring(0, equalMaxMinTemStationNameStr.Length - 1);
            //}

            //////////////////////////////////////////////////////////////////////////////////
            //private string equalTemMessage = "最高温和最低温一致的城市：";
            //private string hlTemMessage = "异常气温(最高温>=45℃,最低温<=-40℃)的城市：";
            //private string subTemMessage = "最高气温或者最低气温和前一天预报相差>=8℃的城市：";
            //private string lTemWethMessage = "最低气温>5℃,但天气现象为冻雨、雨夹雪、小雪、中雪、大雪、暴雪的城市：";
            //private string windNMessage = "有风力(>=3级)，但无风向";
            //private string wethExpMessage = "天气现象为大暴雨、大暴雪";
            ////////////////////////////////////////////////////////////////////////////////

            showTipInfo();
        }
        catch { }

    }

    /// <summary>
    /// 清空gridview
    /// </summary>
    /// <param name="gvShow"></param>
    protected void clearGV(GridView gvShow)
    {
        gvShow.DataSource = null;
        gvShow.DataBind();
    }

    /// <summary>
    /// 填充gridview
    /// </summary>
    /// <param name="gvShow"></param>
    /// <param name="programID"></param>
    /// <param name="timeSXID"></param>
    protected void fillGV(GridView gvShow, string programID, string timeSXID)
    {

        if (timeSXID == "选择时效")
        {

            try
            {
                mRecordeData = dbll.getData(programID);

            }
            catch
            {
                mRecordeData = null;
            }

        }
        else
        {

            try
            {
                mRecordeData = dbll.getDataBySX(programID, timeSXID);
            }
            catch
            {
                mRecordeData = null;
            }
        }
        try//20090814byHan
        {
            if (mRecordeData != null && mRecordeData.Length != 0)
            {
                showTip(mRecordeData[0]);
                if (lb_titleTime.Text == "昨天之前数据，不可查看！")
                {
                    gvShow.DataSource = null;
                }
                //有数据，可以与girdview进行数据绑定以及后续操作
                else
                {
                    if (mStationRelation == null)
                    {
                        mStationRelation = dbll.getAllStationsModel(programID);
                    }

                    if (mElementRelation == null)
                    {
                        mElementRelation = dbll.getElementsName(programID);
                    }

                    if (alTransCode.Count == 0)
                    {
                        alTransCode = transCodeArrayList(programID);
                    }



                    string ttid = mRecordeData[0].TimeType + "";
                    string dtid = mRecordeData[0].DataType + "";
                    string rt = mRecordeData[0].ReportTime + "";
                    mRecordeData_Pre = dbll.getDataPre(programID, ttid, dtid, rt);

                    cmpCurTemPre();

                    gvShow.DataSource = mRecordeData;


                    noExistStation(programID);
                }
            }

            else
            {
                if (mRecordeData == null)
                {
                    p_noExistStation.Visible = true;
                    lb_noExistStation.Text = "该栏目还没有进行配置！";
                    lb_noExistStation.ForeColor = System.Drawing.Color.Red;

                }
                else
                {
                    p_noExistStation.Visible = true;
                    lb_noExistStation.Text = "本次查询无数据显示！";
                    lb_noExistStation.ForeColor = System.Drawing.Color.Red;
                }
            }
            gvShow.DataBind();

        }
        catch
        { }

    }

    /// <summary>
    /// 填充解码ArrayList
    /// </summary>
    /// <param name="cpid"></param>
    /// <returns></returns>
    protected ArrayList transCodeArrayList(string cpid)
    {
        Model.ElementRelation[] mDataSetType = dbll.DecordeDataSetType(cpid);
        int countDataType = mDataSetType.Length;
        //ArrayList alTransCode = new ArrayList();
        ArrayList[] alDataSetType = new ArrayList[countDataType];
        Hashtable[] htDataSet = new Hashtable[countDataType];
        for (int i = 0; i < mDataSetType.Length; i++)
        {
            string typeID = mDataSetType[i].ElementCodeType.ToString();
            alDataSetType[i] = new ArrayList();
            alDataSetType[i].Add(typeID);
            htDataSet[i] = new Hashtable();
            Model.ElementRelation[] mDataSet = dbll.DecordeDataSet(typeID);
            for (int j = 0; j < mDataSet.Length; j++)
            {
                htDataSet[i].Add(mDataSet[j].ElementCode.ToString(), mDataSet[j].TransCodeName);
            }
            alDataSetType[i].Add(htDataSet[i]);
            alTransCode.Add(alDataSetType[i]);
        }
        return alTransCode;
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="alData"></param>
    protected void showTip(Model.RecordeData mRData)
    {
        if (mRData == null)
        {
            lb_titleTime.Text = "";
            lb_dataType.Text = "";
        }
        else
        {
            try
            {
                lb_dataType.Text = mRecordeData[0].DataTypeName;
                DateTime TimeZoneBegin = mRData.BeginTime;
                DateTime TimeZoneOver = mRData.OverTime;
                lb_titleTimeZoneBegin.Text = formatTime(TimeZoneBegin) + " 至 ";
                lb_titleTimeZoneOver.Text = formatTime(TimeZoneOver);
            }
            catch
            {

            }

            DateTime dtReport = mRData.ReportTime;
            string strReport = dtReport.ToString("yyyy-MM-dd");
            if (strReport == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                lb_titleTime.Text = "今天：" + formatTime(dtReport);
                lb_titleTime.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                if (strReport == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                {
                    lb_titleTime.Text = "昨天：" + formatTime(dtReport);
                    lb_titleTime.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lb_titleTime.Text = "昨天之前数据，不可查看！";
                    lb_titleTime.ForeColor = System.Drawing.Color.Red;
                    lb_titleName.Text = "";
                    lb_titleTimeSX.Text = "";
                    lb_cityWeather.Text = "";
                    lb_dataType.Text = "";
                    p_timeInterval.Visible = false;
                    p_text.Visible = false;
                    p_noExistStation.Visible = false;
                    mRData = null;
                }
            }
        }
    }

    /// <summary>
    /// 解码
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    protected string transCode(string typeID, string code)
    {
        switch (typeID)
        {
            case "17":
                if (code == "999.9" || code.Length == 6)
                {
                    code = "缺测";
                }
                break;
            case "18":
                if (code == "999.9" || code.Length == 6)
                {
                    code = "无降水";
                }
                if (code == "0.0")
                {
                    code = "微量降水";
                }
                break;
            default:
                if (typeID != "13" && code != "99")
                {
                    int countDataSetType = alTransCode.Count;
                    for (int i = 0; i < countDataSetType; i++)
                    {
                        string elementCodeType = ((ArrayList)alTransCode[i])[0].ToString();
                        if (typeID == elementCodeType)
                        {
                            if (((Hashtable)((ArrayList)alTransCode[i])[1]).Contains(code))
                            {
                                return ((Hashtable)((ArrayList)alTransCode[i])[1])[code].ToString();
                            }
                            else
                            {
                                return "error";
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (code != "")
                    {
                        code = temNgv(code) + "";
                    }

                }
                else
                {
                    if (typeID != "13" && code == "99")
                    {
                        code = "缺测";
                    }
                    else
                    {

                    }
                }
                break;
        }
        return code;
    }

    /// <summary>
    /// gridview行创建
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_citydata_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try//20090814byHan
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells.Clear();
                GridViewRow rowHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell headerSiteCell = new TableHeaderCell();
                headerSiteCell.Text = "站点";
                rowHeader.Cells.Add(headerSiteCell);
                if (mRecordeData[0].DataType != 4 && mRecordeData[0].DataType != 9)
                {
                    TableHeaderCell headerTimeSXCell = new TableHeaderCell();
                    headerTimeSXCell.Text = "时效";
                    rowHeader.Cells.Add(headerTimeSXCell);
                }

                for (int i = 0; i < mElementRelation.Length; i++)
                {
                    TableHeaderCell headerElementCell = new TableHeaderCell();
                    headerElementCell.Text = mElementRelation[i].ElementNameCN;
                    rowHeader.Cells.Add(headerElementCell);
                }
                rowHeader.Visible = true;
                this.gv_citydata.Controls[0].Controls.AddAt(0, rowHeader);
                fillLbGVCN();
            }
            else
            {

            }

        }
        catch { }
    }

    /// <summary>
    /// gridview行数据绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_citydata_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try//20090814byHan
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int countCell = e.Row.Cells.Count;
                int rowIndex = e.Row.RowIndex;
                if (mRecordeData.Length != 0)
                {
                    //try
                    //{
                    e.Row.Cells[0].Text = stationName[rowIndex];
                    if (mRecordeData[rowIndex].DataType == 4 || mRecordeData[rowIndex].DataType == 9)
                    {
                        e.Row.Cells[1].Visible = false;
                    }
                    else
                    {
                        e.Row.Cells[1].Visible = true;
                        e.Row.Cells[1].Text = mRecordeData[rowIndex].Time.ToString();
                    }

                    //20120220 syy增加最高温度和最低温度判断，如果出现最高温度和最低温度相等，则给出城市名称提示
                    //20120220 syy最高温度、最低温度变量，以便对比
                    string minTem = "";
                    string maxTem = "";

                    //20130702 syy记录天气现象
                    string weather01 = "";
                    //20130704 syy记录天气现象(转)
                    string weather02 = "";

                    //20130702 syy记录风力
                    string wd01 = "";
                    //20130702 syy记录风向
                    string wp01 = "";

                    //20130704 syy记录风力(转)
                    string wd02 = "";
                    //20130704 syy记录风向(转)
                    string wp02 = "";

                    for (int i = 0; i < mElementRelation.Length; i++)
                    {
                        string elementName = mElementRelation[i].ElementName;
                        e.Row.Cells[i + 2].Visible = true;

                        string valueElement_Str = valueElement(elementName, mRecordeData[rowIndex]);
                        e.Row.Cells[i + 2].Text = transCode(mElementRelation[i].ElementCodeType.ToString(), valueElement_Str);

                        //20120220 syy如果是最高温度，则保存到maxTem变量中，以便一行完成后与最低气温进行对比
                        if (mElementRelation[i].ElementNameCN == "最高气温")
                        {
                            maxTem = valueElement_Str;
                        }
                        //20120220 syy如果是最低气温，则保存到minTem变量中，以便一行完成后与最高气温进行对比
                        if (mElementRelation[i].ElementNameCN == "最低气温")
                        {
                            minTem = valueElement_Str;
                        }

                        //20120220 syy如果是天气现象，则保存到weather变量中，以便一行完成后与最低气温进行交叉查看，如果出现冻雨等天气现象，则提示
                        if (mElementRelation[i].ElementCodeType == 1)
                        {
                            if (mElementRelation[i].ElementNameCN == "天气")
                            {
                                weather01 = valueElement_Str;
                            }
                            else
                            {
                                weather02 = valueElement_Str;
                            }
                        }

                        //20120220 syy如果是风向，以便一行完成后与风力进行交叉查看，如果出现风力》=3级，无风向则提示
                        //风力id=3
                        if (mElementRelation[i].ElementCodeType == 3)
                        {
                            if (mElementRelation[i].ElementNameCN == "风向")
                            {
                                wd01 = valueElement_Str;
                            }
                            else
                            {
                                wd02 = valueElement_Str;
                            }
                        }
                        //20120220 syy如果是风力，以便一行完成后与风向进行交叉查看，如果出现风力》=3级，无风向则提示
                        //风力id=4
                        if (mElementRelation[i].ElementCodeType == 4)
                        {
                            if (mElementRelation[i].ElementNameCN == "风力")
                            {
                                wp01 = valueElement_Str;
                            }
                            else
                            {
                                wp02 = valueElement_Str;
                            }

                        }
                    }

                    for (int i = 2 + mElementRelation.Length; i < countCell; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    //}
                    //catch
                    //{

                    //}

                    //20120220 syy增加最高温度和最低温度判断，如果出现最高温度和最低温度相等，则给出城市名称提示
                    //20120220 syy如果是最高温度，则保存到maxTem变量中，以便一行完成后与最低气温进行对比
                    if (maxTem == minTem && maxTem != "" && maxTem != "99" && maxTem != "缺测")
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        try
                        {
                            equalMaxMinTemStationName.Add(stationName[rowIndex], maxTem);

                        }
                        catch
                        {

                        }
                    }

                    //20130702 syy最高温》=45 || 最低温《=-40
                    if (highTem(maxTem) || lowTem(minTem))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        try
                        {
                            hlMaxTemStationName.Add(stationName[rowIndex], maxTem);
                        }
                        catch
                        {

                        }
                    }

                    //20120702 syy最高温和最低温与前一天数据对比，如果相差8度则提示。
                    if (rd_Tem_SubErr != null && rd_Tem_SubErr.Count > 0)
                    {
                        for (int m = 0; m < rd_Tem_SubErr.Count; m++)
                        {
                            if (rd_Tem_SubErr[m].StationID == mRecordeData[rowIndex].StationID)
                            {
                                if (mRecordeData[rowIndex].DataType != 4 && mRecordeData[rowIndex].DataType != 9)
                                {
                                    if (rd_Tem_SubErr[m].Time.ToString() == e.Row.Cells[1].Text)
                                    {
                                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                                        try
                                        {
                                            rd_Tem_SubErr[m].StationName = stationName[rowIndex];
                                        }
                                        catch
                                        {

                                        }
                                    }

                                }
                                else
                                {
                                    e.Row.BackColor = System.Drawing.Color.LightSalmon;
                                    try
                                    {
                                        rd_Tem_SubErr[m].StationName = stationName[rowIndex];
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }

                    //20130702 syy，最低温度》5，天气现象有冻雨、雨夹雪、小雪、中雪、大雪、暴雪
                    if (lTemWethExp(minTem, weather01) || lTemWethExp(minTem, weather02))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        try
                        {
                            ltempWethExpStationName.Add(stationName[rowIndex], weather01);
                        }
                        catch
                        {

                        }
                    }

                    //20130702 syy数据中有风力（》3级）且无风向
                    if (windNExp(wd01, wp01))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        try
                        {
                            winNStationName.Add(stationName[rowIndex], wp01);
                        }
                        catch
                        {

                        }
                    }

                    //20130702 syy数据中有风力（》3级）且无风向（转）
                    if (windNExp(wd02, wp02))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        try
                        {
                            winNStationName.Add(stationName[rowIndex], wp02);
                        }
                        catch
                        {

                        }
                    }

                    //20130702 syy天气现象为大暴雨
                    if (wethExp(weather01) || wethExp(weather02))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        try
                        {
                            wethExpStationName.Add(stationName[rowIndex], weather01);
                        }
                        catch
                        {

                        }
                    }
                }

                //目前最后一行导出不出来，将最后一行保存到不可视ListBox中，以便导出最后一行数据20091103
                if (rowIndex == mRecordeData.Length - 1)
                {
                    lbLastLine.Items.Clear();
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        ListItem liLLElement = new ListItem();
                        liLLElement.Value = e.Row.Cells[i].Text;
                        liLLElement.Text = e.Row.Cells[i].Text;
                        lbLastLine.Items.Add(liLLElement);
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //当鼠标停留时更改背景色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#E2DED6'");
                //当鼠标移开时还原背景色
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }
        }
        catch { }
    }

    /// <summary>
    /// 填充列名以便导出20091103
    /// </summary>
    protected void fillLbGVCN()
    {
        lbGVCN.Items.Clear();
        ListItem liStation = new ListItem();
        liStation.Value = "站点";
        liStation.Text = "站点";
        lbGVCN.Items.Add(liStation);
        ListItem liTime = new ListItem();
        liTime.Value = "时效";
        liTime.Text = "时效";
        lbGVCN.Items.Add(liTime);
        for (int i = 0; i < mElementRelation.Length; i++)
        {
            ListItem liElement = new ListItem();
            liElement.Value = mElementRelation[i].ElementNameCN;
            liElement.Text = mElementRelation[i].ElementNameCN;
            lbGVCN.Items.Add(liElement);
        }
    }


    /// <summary>
    /// 格式显示时间
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    protected string formatTime(DateTime dt)
    {
        string fomatTimeStr = dt.ToString("yyyy-MM-dd") + "  " + dt.Hour.ToString() + "时";
        return fomatTimeStr;
    }

    /// <summary>
    /// 填充无数据站点和站点名称
    /// </summary>
    /// <param name="programID"></param>
    protected void noExistStation(string programID)
    {
        int rowCountAllStation = mStationRelation.Length;
        int rowCountSelectedStation = mRecordeData.Length;
        stationName = new string[rowCountSelectedStation];
        string noExistCity = "";
        int j = 0;
        for (int i = 0; i < rowCountSelectedStation; i++)
        {
            if (mRecordeData[i].StationID == mStationRelation[j].StationID)
            {
                stationName[i] = (mStationRelation[j].StationName) + "（" + mRecordeData[i].StationID.ToString() + "）";
            }
            else
            {
                //改进算法，原来如果第一个站点就是缺测的则显示不出来
                if (i == 0)
                {
                    noExistCity += mStationRelation[j].StationName + "（" + mStationRelation[j].StationID.ToString() + "）" + "、";
                    i--;
                    j++;
                }
                else
                {
                    j++;
                    while (mRecordeData[i].StationID != mStationRelation[j].StationID)
                    {
                        noExistCity += mStationRelation[j].StationName + "（" + mStationRelation[j].StationID.ToString() + "）" + "、";
                        j++;
                    }
                    stationName[i] = (mStationRelation[j].StationName) + "（" + mRecordeData[i].StationID.ToString() + "）";
                }
            }
        }

        for (j++; j < rowCountAllStation; j++)
        {
            noExistCity += mStationRelation[j].StationName + "（" + mStationRelation[j].StationID.ToString() + "）" + "、";
        }
        noExistCity = noExistCity.Replace("noname（0）、", "");
        if (noExistCity == "")
        {
            lb_noExistStation.Text = "本次查询无缺数据站点";
            lb_noExistStation.ForeColor = System.Drawing.Color.Blue;
            p_noExistStation.Visible = true;
        }
        else
        {
            lb_noExistStation.Text = "无数据站点：" + noExistCity.Substring(0, noExistCity.Length - 1);
            lb_noExistStation.ForeColor = System.Drawing.Color.Red;
            p_noExistStation.Visible = true;
        }

    }


    /// <summary>
    /// 导出txt20091103
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bTxt_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int singleLengthFirst = 25;
        int singleLength = 10;
        int rowCount = gv_citydata.Rows.Count;
        int columnCount = lbGVCN.Items.Count;

        //该生成文档的显示内容提示
        string tip = lb_titleName.Text + "          ";
        if (p_timeInterval.Visible == true)
        {
            tip += lb_titleTimeZoneBegin.Text + lb_titleTimeZoneOver.Text;
        }
        else
        {
            tip += lb_titleTimeSX.Text;
        }
        sb.Append(tip);
        sb.Append(Environment.NewLine);
        sb.Append(Environment.NewLine);

        //列名
        for (int i = 0; i < columnCount; i++)
        {
            if (i == 0)
            {
                string columnName = lbGVCN.Items[i].Value;
                sb.Append(columnName);
                int spaceLength = singleLengthFirst - changeUnicodeLength(columnName.Trim());
                for (int j = 0; j < spaceLength; j++)
                {
                    sb.Append(" ");
                }
            }
            else
            {
                string columnName = lbGVCN.Items[i].Value;
                sb.Append(columnName);
                int spaceLength = singleLength - changeUnicodeLength(columnName.Trim());
                for (int j = 0; j < spaceLength; j++)
                {
                    sb.Append(" ");
                }
            }

        }
        sb.Append(Environment.NewLine);

        //值
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (j == 0)
                {
                    string columnValue = gv_citydata.Rows[i].Cells[j].Text.ToString();
                    sb.Append(columnValue);
                    int spaceLength = singleLengthFirst - changeUnicodeLength(columnValue.Trim());
                    for (int k = 0; k < spaceLength; k++)
                    {
                        sb.Append(" ");
                    }

                }
                else
                {
                    string columnValue = gv_citydata.Rows[i].Cells[j].Text.ToString();
                    sb.Append(columnValue);
                    int spaceLength = singleLength - changeUnicodeLength(columnValue.Trim());
                    for (int k = 0; k < spaceLength; k++)
                    {
                        sb.Append(" ");
                    }
                }
            }
            sb.Append(Environment.NewLine);
        }

        //最后一行，最后一行显示不出来，暂以此表示20091103
        for (int i = 0; i < columnCount; i++)
        {
            if (i == 0)
            {
                string columnName = lbLastLine.Items[i].Value;
                sb.Append(columnName);
                int spaceLength = singleLengthFirst - changeUnicodeLength(columnName.Trim());
                for (int j = 0; j < spaceLength; j++)
                {
                    sb.Append(" ");
                }
            }
            else
            {
                string columnName = lbLastLine.Items[i].Value;
                sb.Append(columnName);
                int spaceLength = singleLength - changeUnicodeLength(columnName.Trim());
                for (int j = 0; j < spaceLength; j++)
                {
                    sb.Append(" ");
                }
            }
        }

        // mod by Edward Chan
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename="
            + HttpUtility.UrlEncode(lb_titleName.Text + ".txt", System.Text.Encoding.UTF8));//txt名
        Response.ContentType = "application/vnd.ms-word";
        Response.Charset = "";
        Response.Write(sb.ToString());
        Response.End();
    }

    protected int changeUnicodeLength(string para)
    {
        int lengthOra = para.Length;
        int returnLength = lengthOra;
        for (int i = 0; i < lengthOra; i++)
        {
            if ((int)para[i] > 128)
            {
                returnLength++;
            }
        }
        return returnLength;
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///20130702/////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 20130702 与前一天温度对比
    /// </summary>
    protected void cmpCurTemPre()
    {
        rd_Tem_SubErr = new List<TemRecordData>();

        string maxTemEl = "";
        string minTemEl = "";


        for (int i = 0; i < mElementRelation.Length; i++)
        {
            string elementName = mElementRelation[i].ElementNameCN;

            switch (elementName)
            {
                case "最高气温":
                    maxTemEl = mElementRelation[i].ElementName;

                    break;
                case "最低气温":
                    minTemEl = mElementRelation[i].ElementName;

                    break;
                default:
                    break;
            }

        }
        try
        {
            if (mRecordeData != null && mRecordeData_Pre != null)
            {
                foreach (RecordeData rd_cur in mRecordeData)
                {

                    foreach (RecordeData rd_pre in mRecordeData_Pre)
                    {
                        if (rd_cur.StationID == rd_pre.StationID && rd_cur.DataType == rd_pre.DataType && rd_cur.Time == rd_pre.Time)
                        {
                            int maxTemSub = 0;
                            int minTemSub = 0;

                            string maxTemValueCur = valueElement(maxTemEl, rd_cur);
                            string minTemValueCur = valueElement(minTemEl, rd_cur);
                            string maxTemValuePre = valueElement(maxTemEl, rd_pre);
                            string minTemValuePre = valueElement(minTemEl, rd_pre);
                            if (!(maxTemEl == "" || maxTemValueCur == "" || maxTemValuePre == "" || maxTemValueCur == "99" || maxTemValuePre == "99"))
                            {
                                maxTemSub = temNgv(maxTemValueCur) - temNgv(maxTemValuePre);
                            }
                            if (!(minTemEl == "" || minTemValueCur == "" || minTemValuePre == "" || minTemValueCur == "99" || minTemValuePre == "99"))
                            {
                                minTemSub = temNgv(minTemValueCur) - temNgv(minTemValuePre);
                            }

                            if (Math.Abs(maxTemSub) >= 8 || Math.Abs(minTemSub) >= 8)
                            {
                                TemRecordData trd = new TemRecordData();
                                trd.Time = rd_pre.Time;
                                trd.TimeType = rd_pre.TimeType;
                                trd.DataType = rd_pre.DataType;
                                trd.StationID = rd_pre.StationID;
                                if (maxTemValuePre != "")
                                {
                                    trd.HighTem = temNgv(maxTemValuePre).ToString();
                                }
                                if (minTemValuePre != "")
                                {
                                    trd.LowTem = temNgv(minTemValuePre).ToString();
                                }
                                rd_Tem_SubErr.Add(trd);
                                //rd_Tem_Err.Add(rd_pre);
                            }


                            //如果有满足需求的，就跳出本次判断，以减少判断次数
                            break;
                        }

                    }
                }
            }
        }

        catch
        {

        }


    }

    /// <summary>
    /// 组织显示message信息，从list中循环写出，为温度转换用
    /// </summary>
    /// <param name="messageStyle"></param>
    /// <param name="list_trd"></param>
    /// <param name="lbTip"></param>
    /// <param name="pTip"></param>
    protected void messageTipList2Str(string messageStyle, List<TemRecordData> list_trd, Label lbTip, Panel pTip)
    {
        lbTip.Text = "";
        if (list_trd != null && list_trd.Count != 0)
        {
            string strMessage = "";

            foreach (TemRecordData trd in list_trd)
            {
                string lowTemStr = "";
                if (trd.LowTem != null && trd.LowTem != "")
                {
                    lowTemStr = trd.LowTem + "℃/";
                }

                string highTemStr = "";
                if (trd.HighTem != null && trd.HighTem != "")
                {
                    highTemStr = trd.HighTem + "℃";
                }
                strMessage += trd.StationName + "【" + lowTemStr + highTemStr + "】，";
            }
            lbTip.Text = messageStyle + strMessage.Substring(0, strMessage.Length - 1);
            pTip.Visible = true;
        }

    }

    /// <summary>
    /// 温度取负值
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    protected int temNgv(string code)
    {
        int temngv_str = Convert.ToInt32(code);
        if (temngv_str >= 50 && temngv_str != 99)
        {
            temngv_str = 50 - temngv_str;
        }
        return temngv_str;
    }

    /// <summary>
    /// 最高温是否超过45度
    /// </summary>
    /// <param name="maxTem"></param>
    /// <returns></returns>
    protected bool highTem(string maxTem)
    {
        bool isHigh = false;
        try
        {
            int tem_int = Convert.ToInt32(maxTem);
            if (tem_int >= 45 && tem_int < 50)
            {
                isHigh = true;
            }
        }
        catch
        {

        }

        return isHigh;
    }

    /// <summary>
    /// 最低温是否低于-40度
    /// </summary>
    /// <param name="minTem"></param>
    /// <returns></returns>
    protected bool lowTem(string minTem)
    {
        bool isLow = false;
        try
        {
            int tem_int = Convert.ToInt32(minTem);
            if (tem_int != 99 && tem_int >= 90)
            {
                isLow = true;
            }

        }
        catch
        {

        }
        return isLow;
    }

    /// <summary>
    /// 组织显示message信息，从Hashtable中循环写出，为其他Msg转换用
    /// </summary>
    /// <param name="messageStyle"></param>
    /// <param name="htMessage"></param>
    /// <param name="lbTip"></param>
    /// <param name="pTip"></param>
    protected void messageTipHt2Str(string messageStyle, Hashtable htMessage, Label lbTip, Panel pTip)
    {
        lbTip.Text = "";
        if (htMessage != null && htMessage.Count != 0)
        {
            string strMessage = "";
            foreach (DictionaryEntry de in htMessage)
            {
                strMessage += de.Key + ",";
            }
            lbTip.Text = messageStyle + strMessage.Substring(0, strMessage.Length - 1);
            pTip.Visible = true;
        }
    }

    /// <summary>
    /// 最低温》5度，但出现意外天气现象，例如冻雨
    /// </summary>
    /// <param name="minTem"></param>
    /// <param name="weathercode"></param>
    /// <returns></returns>
    protected bool lTemWethExp(string minTem, string weathercode)
    {
        try
        {
            int tem_int = Convert.ToInt32(minTem);
            if (tem_int > 5 && tem_int < 50 && wethSpc(weathercode))
            {
                return true;
            }
        }
        catch
        {
        }
        return false;
    }

    protected bool wethSpc(string weathercode)
    {
        //ArrayList weatherCode = alTransCode[1] as ArrayList;

        //冻雨 19
        //雨夹雪 06
        //小雪 14
        //中雪 15
        //大雪 16
        //暴雪 17
        if (weathercode == "19" || weathercode == "06" || weathercode == "14" || weathercode == "15" || weathercode == "16" || weathercode == "17")
        {
            return true;
        }
        return false;
    }

    /// <summary>
    ///  天气现象为大暴雨
    /// </summary>
    /// <param name="weathercode"></param>
    /// <returns></returns>
    protected bool wethExp(string weathercode)
    {
        //ArrayList weatherCode = alTransCode[1] as ArrayList;

        //大暴雨
        if (weathercode == "11")
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 风力》3，但无风向
    /// </summary>
    /// <param name="wind"></param>
    /// <param name="wp"></param>
    /// <returns></returns>
    protected bool windNExp(string wind, string wp)
    {
        if (wp != "0" && wind == "0")
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 根据Elemnt元素，取Elemnt中的值，并替换原行帮定中的代码
    /// </summary>
    /// <param name="elementName"></param>
    /// <param name="rd_one"></param>
    /// <returns></returns>
    protected string valueElement(string elementName, RecordeData rd_one)
    {
        string value = "";
        switch (elementName)
        {
            case "Element01":
                value = rd_one.Element01;

                break;
            case "Element02":
                value = rd_one.Element02;

                break;
            case "Element03":
                value = rd_one.Element03;

                break;
            case "Element04":
                value = rd_one.Element04;

                break;
            case "Element05":
                value = rd_one.Element05;

                break;
            case "Element06":
                value = rd_one.Element06;

                break;
            case "Element07":
                value = rd_one.Element07;

                break;
            case "Element08":
                value = rd_one.Element08;

                break;
            case "Element09":
                value = rd_one.Element09;

                break;

            default:
                break;
        }
        return value;
    }

    /// <summary>
    /// 初始化Tip控件以及变量
    /// </summary>
    protected void initTipInfo()
    {
        lb_equalMaxMinTemTip.Text = "";
        equalMaxMinTemStationName = new Hashtable();

        lb_subTemTip.Text = "";
        rd_Tem_SubErr = new List<TemRecordData>();

        lb_hlTemTip.Text = "";
        hlMaxTemStationName = new Hashtable();

        lb_LTempWethExpTip.Text = "";
        ltempWethExpStationName = new Hashtable();

        lb_WinNTip.Text = "";
        winNStationName = new Hashtable();

        lb_WethTip.Text = "";
        wethExpStationName = new Hashtable();

        p_noExistStation.Visible = false;
        p_TepExpTip.Visible = false;
        p_WethExp.Visible = false;
    }

    /// <summary>
    /// 写出信息内容
    /// </summary>
    protected void showTipInfo()
    {
        messageTipHt2Str(equalTemMessage, equalMaxMinTemStationName, lb_equalMaxMinTemTip, p_TepExpTip);

        messageTipHt2Str(hlTemMessage, hlMaxTemStationName, lb_hlTemTip, p_TepExpTip);

        //messageTipHt2Str(subTemMessage, equalMaxMinTemStationName, lb_subTemTip, p_TepExpTip);
        messageTipList2Str(subTemMessage, rd_Tem_SubErr, lb_subTemTip, p_TepExpTip);

        messageTipHt2Str(lTemWethMessage, ltempWethExpStationName, lb_LTempWethExpTip, p_WethExp);

        messageTipHt2Str(windNMessage, winNStationName, lb_WinNTip, p_WethExp);

        messageTipHt2Str(wethExpMessage, wethExpStationName, lb_WethTip, p_WethExp);
    }
}
