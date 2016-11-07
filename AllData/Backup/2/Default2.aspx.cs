using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Model;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

public partial class Default2 : System.Web.UI.Page
{
    BLL.DataBLL dbll = new BLL.DataBLL();

    //20090814byHan，去掉static
    private ArrayList alTransCode = new ArrayList();
    private Model.RecordeData2[] mRecordeData;
    private Model.ChannelProgram[] mChannelInfo;
    private Model.ChannelProgram[] mProgramInfo;
    private Model.ElementRelation[] mElementRelation;

    private int[] noSX = new int[] { 4, 9 };
    private int[] expDataType = new int[] { 2, 13, 17, 18 };

    /// <summary>
    /// syy 20130627 增加前一天的记录，以供对比判断最高温和最低温差大于8度
    /// </summary>
    private Model.RecordeData2[] mRecordeData_Pre;

    ElTransCode[] elTransCode;


    private ExpStation ExpStationMessage;

    private TipMessage TipMessageInfo;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            initDDLChannel("0");

            clearProgram();
        }
    }

    /// <summary>
    /// 初始化栏目
    /// </summary>
    /// <param name="categoryID"></param>
    protected void initDDLChannel(string categoryID)
    {
        clearChannel();
        mChannelInfo = dbll.getCP(categoryID);
        for (int i = 0; i < mChannelInfo.Length; i++)
        {
            ListItem newItem = new ListItem();
            newItem.Text = mChannelInfo[i].CP_Name;
            newItem.Value = mChannelInfo[i].CP_ID.ToString();
            ddl_channel.Items.Add(newItem);
        }
    }

    /// <summary>
    /// 清空频道，全部频道值为all
    /// </summary>
    protected void clearChannel()
    {
        ddl_channel.Items.Clear();
        ListItem newItemChannel = new ListItem();
        newItemChannel.Text = "全部频道";
        newItemChannel.Value = "all";
        ddl_channel.Items.Add(newItemChannel);
    }

    /// <summary>
    /// 清空栏目，全部栏目值为all
    /// </summary>
    protected void clearProgram()
    {
        ddl_program.Items.Clear();
        ListItem newItemProgram = new ListItem();
        newItemProgram.Text = "全部栏目";
        newItemProgram.Value = "all";
        ddl_program.Items.Add(newItemProgram);
    }

    /// <summary>
    /// 显示时效信息
    /// </summary>
    /// <param name="sx"></param>
    protected void fillSX(string[] sx)
    {
        cbl_timeSX.DataSource = sx;
        cbl_timeSX.DataBind();
        if (sx == null || sx.Length == 0)
        {
            showSX(false);
        }
        else
        {
            showSX(true);
        }

        up_sxtxt.Update();
    }

    protected void showSX(bool flag)
    {
        l_sx.Visible = flag;
        cbl_timeSX.Visible = flag;
    }

    /// <summary>
    /// 选择所有实效信息，默认情况下
    /// </summary>
    /// <param name="sx"></param>
    protected void checkedAllSX()
    {
        for (int i = 0; i < cbl_timeSX.Items.Count; i++)
        {
            cbl_timeSX.Items[i].Selected = true;
        }
    }

    /// <summary>
    /// 显示查询按钮
    /// </summary>
    /// <param name="flag"></param>
    protected void showBCommit( bool flag)
    {
        b_commit.Visible = flag;
    }

    /// <summary>
    /// 显示说明信息
    /// </summary>
    /// <param name="flag"></param>
    protected void showIllustration(bool flag)
    {
        p_illustration.Visible = flag;
        UpdatePanel3.Update();
    }

    /// <summary>
    /// 显示数据内容
    /// </summary>
    /// <param name="flag"></param>
    protected void showContent(bool flag)
    {
        p_content.Visible = flag;
        p_print.Visible = flag;
        UpdatePanel2.Update();
    }

    /// <summary>
    /// 显示打印信息
    /// </summary>
    /// <param name="flag"></param>
    protected void showPrint(bool flag)
    {
        //p_print.Visible = flag;
        //p_down.Visible = flag;
    }

    /// <summary>
    /// 频道选择触发事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_channel_SelectedIndexChanged(object sender, EventArgs e)
    {
        //清空表格
        //clearGV(gv_citydata);
        //根据ID重新填充Program
        string channelID = ddl_channel.SelectedValue;
        initDDLProgram(channelID);

        //清空时效
        fillSX(null);
        //清除提交按钮
        showBCommit(false);
        //显示说明信息
        showIllustration(true);
        //清除数据内容显示
        showContent(false);

    }

    /// <summary>
    /// 初始化栏目
    /// </summary>
    /// <param name="channelID"></param>
    protected void initDDLProgram(string channelID)
    {
        clearProgram();
        if (channelID == "all")
        {
            return;
        }
        else
        {
            //mProgramInfo = null;
            mProgramInfo = dbll.getCP(channelID);
            for (int i = 0; i < mProgramInfo.Length; i++)
            {
                ListItem newItem = new ListItem();
                newItem.Text = mProgramInfo[i].CP_Name;
                newItem.Value = mProgramInfo[i].CP_ID + "#" + mProgramInfo[i].DataTypeID + "#" + mProgramInfo[i].TimeTypeID;
                ddl_program.Items.Add(newItem);
            }
        }
    }

    /// <summary>
    /// 初始化时效
    /// </summary>
    /// <param name="channelID"></param>
    /// <param name="programID"></param>
    protected void initDDLTimeSX(string programID, string dtID, string ttID)
    {
        string[] sx;
        //if (programID == "all" || lb_titleTime.Text == "昨天之前数据，不可查看！")
        if (programID == "all")
        {
            return;
        }
        else
        {
            try
            {
                int idtID = Convert.ToInt32(dtID);
                if (noSXDT(idtID))
                {
                    sx = null;
                }
                else
                {
                    int iprogramID = Convert.ToInt32(programID);
                    int ittID = Convert.ToInt32(ttID);
                    sx = dbll.getSX2(iprogramID, idtID, ittID);
                }
                
            }
            catch
            {
                sx = null;
            }


            fillSX(sx);
            checkedAllSX();
        }
    }

    /// <summary>
    /// 是否有时效数据
    /// </summary>
    /// <param name="dtID"></param>
    /// <returns></returns>
    protected bool noSXDT(int dtID)
    {
        for (int i = 0; i < noSX.Length; i++)
        {
            if (dtID == noSX[i])
            {
                return true;
            }
        }
        return false;
    }

    protected void ddl_program_SelectedIndexChanged(object sender, EventArgs e)
    {
        /// <summary>
        /// 清除数据内容显示
        /// 显示说明信息
        /// syy 20130627 清除当前的记录的同时也清除前一天的记录信息
        /// 清除解释代码
        /// 清除Elment关系
        /// </summary>
        resetVar();


        if (ddl_program.SelectedValue == "all")
        {
            //清空时效列表
            fillSX(null);
            //不显示提交按钮
            showBCommit(false);
            return;
        }

        //显示查询按钮
        showBCommit(true);

        string[] programInfo = ddl_program.SelectedValue.Split('#');
        string programID = programInfo[0];
        string dtID = programInfo[1];
        string ttID = programInfo[2];
        //初始化时效
        initDDLTimeSX(programID, dtID, ttID);

    }

    /// <summary>
    /// 查询按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void b_commit_Click(object sender, EventArgs e)
    {
        ////计算时效取值


        List<string> sx = new List<string> ();
        foreach (ListItem li in cbl_timeSX.Items)
        {
            if (li.Selected == true)
            {
                sx.Add(li.Value);
            }
        }

        if (sx != null)
        {
            try
            {
                string[] programInfo = ddl_program.SelectedValue.Split('#');
                string programIDStr = programInfo[0];

                int programID = Convert.ToInt32(programIDStr);
                int dtID = Convert.ToInt32(programInfo[1]);
                int ttID = Convert.ToInt32(programInfo[2]);
                //根据不同的时效取结果集
                //如果是是一个时效或者全部时效，调用getData2函数，效率更高些
                //全部时效，参数为null
                if (sx.Count == cbl_timeSX.Items.Count)
                {
                    mRecordeData = dbll.getData2(programID, dtID, ttID, null, null);
                }
                else
                {
                    switch (sx.Count)
                    {
                        case 0:
                            /// <summary>
                            /// 清除数据内容显示
                            /// 显示说明信息
                            /// syy 20130627 清除当前的记录的同时也清除前一天的记录信息
                            /// 清除解释代码
                            /// 清除Elment关系
                            /// </summary>
                            resetVar();
                            break;

                        case 1:
                            int time = Convert.ToInt32(sx[0]);
                            mRecordeData = dbll.getData2(programID, dtID, ttID, null, time);
                            break;


                        //部分时效，参数为24#48。。。
                        default:
                            string times = "";
                            for (int i = 0; i < sx.Count; i++)
                            {
                                times += sx[i] + "#";
                            }
                            times = times.Substring(0, times.Length - 1);

                            mRecordeData = dbll.getData2X(programID, dtID, ttID, null, times);
                            break;
                    }
                }

                if (mRecordeData != null && mRecordeData.Length != 0)
                {

                    //Element关系，Element01--天气
                    mElementRelation = dbll.getElementsName(programIDStr);
                    //转码实际气象信息内容，Element01--00--晴
                    elTransCode = transElCode(programIDStr);

                    //缺失或者数据异常站点信息
                    ExpStationMessage = new ExpStation();
                    //提示信息。频道、栏目、预报时间、数据类型等等
                    TipMessageInfo = new TipMessage();

                    TipMessageInfo.Channel = ddl_channel.SelectedItem.Text;
                    TipMessageInfo.Program = ddl_program.SelectedItem.Text;
                    int datatypeID = Convert.ToInt32(programInfo[1]);
                    RecordeData[] rdDataType = dbll.getDataType(datatypeID);
                    TipMessageInfo.DataType = rdDataType[0].DataTypeName + TipMessageInfo.DataTypeMessage;

                    //为了避免第一行的数据ReportTime为NULL，取不出来，做循环取出结果值
                    DateTime rt = new DateTime();
                    for (int i = 0; i < mRecordeData.Length; i++)
                    {
                        if (mRecordeData[i].ReportTime != null)
                        {
                            rt = mRecordeData[i].ReportTime.Value;
                            TipMessageInfo.ReportTime = mRecordeData[i].ReportTime.Value.ToString();
                            break;
                        }
                    }

                    //取前一天的天气，进行气温对比
                    DateTime rtYesterday = rt.AddDays(-1);
                    //根据不同的时效取结果集
                    //如果是是一个时效或者全部时效，调用getData2函数，效率更高些
                    //全部时效，参数为null
                    if (sx.Count == cbl_timeSX.Items.Count)
                    {
                        mRecordeData_Pre = dbll.getData2(programID, dtID, ttID, rtYesterday, null);
                    }
                    else
                    {
                        switch (sx.Count)
                        {
                            case 0:
                                mRecordeData_Pre = null; //不会走到
                                break;

                            case 1:
                                int time = Convert.ToInt32(sx[0]);
                                mRecordeData_Pre = dbll.getData2(programID, dtID, ttID, rtYesterday, time);
                                break;
                            
                            
                            //部分时效，参数为24#48。。。
                            default:
                                string times = "";
                                for (int i = 0; i < sx.Count; i++)
                                {
                                    times += sx[i] + "#";
                                }
                                times = times.Substring(0, times.Length - 1);
                                mRecordeData_Pre = dbll.getData2X(programID, dtID, ttID, rtYesterday, times);
                                break;
                        }
                    }
                    

                    gv_citydata.DataSource = mRecordeData;
                    gv_citydata.DataBind();

                    showTipInfo();

                   // Label1.Text = "sssssssssssssssssssssss";
                    //内容显示
                    showContent(true);
                    //清除说明
                    showIllustration(false);
                }
            }
            catch
            {
                resetVar();
            }

        }

    }

    /// <summary>
    /// 清除数据内容显示
    /// 显示说明信息
    /// syy 20130627 清除当前的记录的同时也清除前一天的记录信息
    /// 清除解释代码
    /// 清除Elment关系
    /// </summary>
    protected void resetVar()
    {
        mRecordeData = null;
        //清除数据内容显示
        showContent(false);
        //显示说明信息
        showIllustration(true);
        //syy 20130627 清除当前的记录的同时也清除前一天的记录信息
        mRecordeData_Pre = null;
        //清除解释代码
        alTransCode.Clear();
        //清除Elment关系
        mElementRelation = null;
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
    /// 显示时效信息
    /// </summary>
    /// <param name="datatypeID"></param>
    /// <returns></returns>
    protected bool gridShowSX(int? datatypeID)
    {
        
        bool flg = true;
        if (datatypeID == null)
        {
            flg = false;
            return flg;
        }
        for (int i = 0; i < noSX.Length; i++)
        {
            if (datatypeID == noSX[i])
            {
                flg = false;
                return flg;
            }
        }
        return flg;
    }

    protected string rowHeaderName(string enName)
    {
        string chName = "";
        enName  = enName.ToLower();
        switch (enName)
        {
            case "stationname":
                chName = "站点";
                break;
            case "stationid":
                chName = "站点";
                break;
            case "time":
                chName = "时效";
                break;
            default:
                break;
        }
        return chName;
    }

    /// <summary>
    /// gridview行数据绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_citydata_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (mRecordeData == null)
        {
            return;
        }
        
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = rowHeaderName(e.Row.Cells[1].Text);

                int index = 1;
                if (gridShowSX(mRecordeData[0].DataType))
                {
                    e.Row.Cells[1].Text = rowHeaderName(e.Row.Cells[3].Text);
                    index++;
                }


                for (int i = 0; i < mElementRelation.Length; i++)
                {
                    e.Row.Cells[index].Text = mElementRelation[i].ElementNameCN;
                    index++;
                }

                for (int i = index; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }

            }
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text == "&nbsp;")
                {
                    e.Row.Visible = false;
                    ExpStationMessage.NoDataStationName.Add(e.Row.Cells[1].Text + "(" + e.Row.Cells[0].Text + ")");
                }
                else
                {
                    int rowIndex = e.Row.RowIndex;
                    e.Row.Cells[0].Text = e.Row.Cells[1].Text + "(" + e.Row.Cells[0].Text + ")";

                    int index = 1;
                    if (gridShowSX(mRecordeData[0].DataType))
                    {
                        e.Row.Cells[1].Text = e.Row.Cells[3].Text;
                        index = 2;
                    }
                    //20120220 syy增加最高温度和最低温度判断，如果出现最高温度和最低温度相等，则给出城市名称提示
                    //20120220 syy最高温度、最低温度变量，以便对比
                    //20130702 syy记录天气现象
                    //20130704 syy记录天气现象(转)
                    //20130702 syy记录风力
                    //20130702 syy记录风向
                    //20130704 syy记录风力(转)
                    //20130704 syy记录风向(转)
                    //统一记录在WeatherClass中
                    WeatherClass weather = new WeatherClass();

                    for (int i = 0; i < mElementRelation.Length; i++)
                    {
                        string elementName = mElementRelation[i].ElementName;

                        string valueElement_Code = valueElement(elementName, mRecordeData[rowIndex]);

                        e.Row.Cells[index].Text = decodeEl(mElementRelation[i].ElementCodeType, valueElement_Code);

                        index++;


                        //20120220 syy如果是最高温度，则保存到maxTem变量中，以便一行完成后与最低气温进行对比
                        if (mElementRelation[i].ElementCodeType == 2)
                        {
                            if (mElementRelation[i].ElementNameCN == "最高气温")
                            {
                                weather.HighTem = valueElement_Code;
                                weather.HighTemElID = mElementRelation[i].ElementName.ToString();
                                continue;
                            }
                            //20120220 syy如果是最低气温，则保存到minTem变量中，以便一行完成后与最高气温进行对比
                            if (mElementRelation[i].ElementNameCN == "最低气温")
                            {
                                weather.LowTem = valueElement_Code;
                                weather.LowTemElID = mElementRelation[i].ElementName.ToString();
                                continue;
                            }
                        }

                        //20120220 syy如果是天气现象，则保存到weather变量中，以便一行完成后与最低气温进行交叉查看，如果出现冻雨等天气现象，则提示
                        if (mElementRelation[i].ElementCodeType == 1)
                        {
                            if (mElementRelation[i].ElementNameCN == "天气")
                            {
                                weather.Weather01 = valueElement_Code;
                                continue;
                            }
                            else
                            {
                                weather.Weather02 = valueElement_Code;
                                continue ;
                            }
                        }
                        //20120220 syy如果是风向，以便一行完成后与风力进行交叉查看，如果出现风力》=3级，无风向则提示
                        //风力id=3
                        if (mElementRelation[i].ElementCodeType == 3)
                        {
                            if (mElementRelation[i].ElementNameCN == "风向")
                            {
                                weather.WindDir01 = valueElement_Code;
                                continue ;
                            }
                            else
                            {
                                weather.WindDir02 = valueElement_Code;
                                continue ;
                            }
                        }
                        //20120220 syy如果是风力，以便一行完成后与风向进行交叉查看，如果出现风力》=3级，无风向则提示
                        //风力id=4
                        if (mElementRelation[i].ElementCodeType == 4)
                        {
                            if (mElementRelation[i].ElementNameCN == "风力")
                            {
                                weather.WindPower01 = valueElement_Code;
                                continue ;
                            }
                            else
                            {
                                weather.WindPower02 = valueElement_Code;
                                continue ;
                            }

                        }
                    }

                    for (int i = index; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    //20120220 syy增加最高温度和最低温度判断，如果出现最高温度和最低温度相等，则给出城市名称提示
                    //20120220 syy如果是最高温度，则保存到maxTem变量中，以便一行完成后与最低气温进行对比
                    if (equalTem(weather.HighTem, weather.LowTem))
                    {
                        //EqualMaxMinTemStationName.Add(e.Row.Cells[0].Text);
                        ExpStationMessage.EqualMaxMinTemStationName.Add(e.Row.Cells[0].Text);
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                    }

                    //20130702 syy最高温》=45 || 最低温《=-40
                    if (highTem(weather.HighTem) || lowTem(weather.LowTem))
                    {
                        //LimitTemStationName.Add(e.Row.Cells[0].Text);
                        ExpStationMessage.LimitTemStationName.Add(e.Row.Cells[0].Text);
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                    }

                    //20120702 syy最高温和最低温与前一天数据对比，如果相差8度则提示。
                    if (subTemExp(weather.HighTemElID, weather.HighTem, weather.LowTemElID, weather.LowTem, mRecordeData[rowIndex]))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        //提示信息在函数subTemExp进行加载赋值，因为要显示前一天具体的温度，以便进行对比
                    }

                    //20130702 syy，最低温度》5，天气现象有冻雨、雨夹雪、小雪、中雪、大雪、暴雪
                    if (lTemWethExp(weather.LowTem, weather.Weather01) || lTemWethExp(weather.LowTem, weather.Weather02))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        ExpStationMessage.TempWethExpStationName.Add(e.Row.Cells[0].Text);
                    }

                    //20130702 syy数据中有风力（》3级）且无风向
                    if (windNExp(weather.WindDir01, weather.WindPower01))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        ExpStationMessage.WinNStationName.Add(e.Row.Cells[0].Text);
                    }

                    //20130702 syy数据中有风力（》3级）且无风向（转）
                    if (windNExp(weather.WindDir02, weather.WindPower02))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        ExpStationMessage.WinNStationName.Add(e.Row.Cells[0].Text);
                    }

                    //20130702 syy天气现象为大暴雨
                    if (wethExp(weather.Weather01) || wethExp(weather.Weather02))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        ExpStationMessage.WethExpStationName.Add(e.Row.Cells[0].Text);
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
        }
        catch
        {

        }
    }

    /// <summary>
    /// 温度相差8度
    /// </summary>
    /// <param name="eleHighTemID"></param>
    /// <param name="highTemValueCur"></param>
    /// <param name="eleLowTemID"></param>
    /// <param name="lowTemValueCur"></param>
    /// <param name="rd_cur"></param>
    /// <returns></returns>
    protected bool subTemExp(string eleHighTemID, string highTemValueCur, string eleLowTemID, string lowTemValueCur,RecordeData2 rd_cur)
    {
        bool flag = false;
        if (rd_cur != null && mRecordeData_Pre != null)
        {
            int maxTemSub = 0;
            int minTemSub = 0;

            foreach (RecordeData2 rd_pre in mRecordeData_Pre)
            {
                if (rd_cur.StationID == rd_pre.StationID && rd_cur.Time == rd_pre.Time && rd_cur.DataType == rd_pre.DataType)
                {
                    string highTemValuePre = valueElement(eleHighTemID, rd_pre);
                    string lowTemValuePre = valueElement(eleLowTemID, rd_pre);

                    if (!(eleHighTemID == "" || highTemValueCur == "" || highTemValuePre == "" || highTemValueCur == "99" || highTemValuePre == "99"))
                    {
                        maxTemSub = temNgv(highTemValueCur) - temNgv(highTemValuePre);
                    }
                    if (!(eleLowTemID == "" || lowTemValueCur == "" || lowTemValuePre == "" || lowTemValueCur == "99" || lowTemValuePre == "99"))
                    {
                        minTemSub = temNgv(lowTemValueCur) - temNgv(lowTemValuePre);
                    }

                    if (Math.Abs(maxTemSub) >= 8 || Math.Abs(minTemSub) >= 8)
                    {

                        TemRecordData trd = new TemRecordData();
                        trd.Time = rd_pre.Time.Value;
                        trd.TimeType = rd_pre.TimeType.Value;
                        trd.DataType = rd_pre.DataType.Value;
                        trd.StationID = rd_pre.StationID;
                        trd.StationName = rd_pre.StationName;
                        if (highTemValuePre != "")
                        {
                            trd.HighTem = temNgv(highTemValuePre).ToString();
                        }
                        if (lowTemValuePre != "")
                        {
                            trd.LowTem = temNgv(lowTemValuePre).ToString();
                        }
                        ExpStationMessage.SubTemStationName.Add(trd);
                        
                        flag = true;
                        return flag;
                    }
                    break;
                }
            }
        }
        return flag;
    }

    protected bool equalTem(string maxTem, string minTem)
    {
        //20120220 syy增加最高温度和最低温度判断，如果出现最高温度和最低温度相等，则给出城市名称提示
        //20120220 syy如果是最高温度，则保存到maxTem变量中，以便一行完成后与最低气温进行对比

        bool flag = false;
        if (maxTem == minTem && maxTem != null &&  maxTem != "" && maxTem != "99" && maxTem != "缺测")
        {
            flag = true;
            
            return flag;
        }
        return flag;
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
                
                return isHigh;
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
                return isLow;
            }

        }
        catch
        {

        }
        return isLow;
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
            if (minTem != null && minTem != "")
            {
                int tem_int = Convert.ToInt32(minTem);
                if (tem_int > 5 && tem_int < 50 && wethSpc(weathercode))
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
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
        if (wp != null && wp != "0" && wind == "0")
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
    protected string valueElement(string elementName, RecordeData2 rd_one)
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
    /// 解码Element
    /// </summary>
    /// <param name="elID"></param>
    /// <param name="elCode"></param>
    /// <returns></returns>
    protected string decodeEl(int elID,string elCode)
    {
        string elCH = elCode;
        
        //特殊数据类型解码
        //2.温度，大于50取负值，其他取本身。
        //13.直接录入。
        //17.999.9缺测
        //18.999.9缺测,0.0微量降水
        for (int i = 0; i < expDataType.Length; i++)
        {
            if (elID == expDataType[i])
            {
                elCH = expDataTypeTransCode(elID, elCode);
                return elCH;
            }
        }

        //其他数据类型中的99，标示为缺测
        if (elCode == "99")
        {
            elCH = "缺测";
            return elCH;
        }
        
        //其他数据类型的，进行解码显示
        if (elTransCode != null)
        {
            for (int i = 0; i < elTransCode.Length; i++)
            {
                bool flag = false;
                if (elID == elTransCode[i].ElementCodeType)
                {
                    List<ElCode> elCodeList = elTransCode[i].ELementCode;
                    for (int j = 0; j < elCodeList.Count; j++)
                    {
                        if (elCodeList[j].ElementCode == elCode)
                        {
                            elCH = elCodeList[j].TransCodeCH;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        return elCH;

    }


    /// <summary>
    /// 填充解码ElTransCode
    /// </summary>
    /// <param name="cpid"></param>
    /// <returns></returns>
    protected ElTransCode[] transElCode(string cpid)
    {
        Model.ElementRelation[] mEleTransSetType = dbll.DecordeDataSetType(cpid);
        int countEleTransType = mEleTransSetType.Length;

        ElTransCode[] elTrans = new ElTransCode[countEleTransType];

        for (int i = 0; i < countEleTransType; i++)
        {
            elTrans[i] = new ElTransCode();

            List<ElCode> elTransCH = new List<ElCode>();

            int typeID = mEleTransSetType[i].ElementCodeType;
            Model.ElementRelation[] mDataSet = dbll.DecordeDataSet(typeID.ToString());

            elTrans[i].ElementCodeType = typeID;


            for (int j = 0; j < mDataSet.Length; j++)
            {
                ElCode elCode = new ElCode();
                elCode.ElementCode = mDataSet[j].ElementCode.ToString();
                elCode.TransCodeCH = mDataSet[j].TransCodeName;

                elTransCH.Add(elCode);
            }

            elTrans[i].ELementCode = elTransCH;
        }

        return elTrans;
    }

    /// <summary>
    /// 组织显示message信息，从HashSet中循环写出，为其他Msg转换用
    /// </summary>
    /// <param name="messageStyle"></param>
    /// <param name="hMessage"></param>
    /// <param name="lbTip"></param>
    /// <param name="pTip"></param>
    protected void messageTipHash2Str(string messageStyle, HashSet<string> hMessage, Label lbTip, bool noStationExp, string noExpMessage)
    {
        lbTip.Text = "";
        //pTip.Visible = false;
        if (hMessage != null && hMessage.Count != 0)
        {
            string strMessage = "";

            foreach (var message in hMessage)  //迭代输出
            {
                strMessage += message + "，";
            }

            lbTip.Text = messageStyle + strMessage.Substring(0, strMessage.Length - 1);
            lbTip.ForeColor = Color.Red;
            //pTip.Visible = true;
        }
        else
        {
            if (noStationExp)
            {
                lbTip.Text = noExpMessage;
                lbTip.ForeColor = Color.Blue;
            //    pTip.Visible = true;
            }
        }
    }

    /// <summary>
    /// 组织显示message信息，从list中循环写出，为温度转换用
    /// </summary>
    /// <param name="messageStyle"></param>
    /// <param name="list_trd"></param>
    /// <param name="lbTip"></param>
    /// <param name="pTip"></param>
    protected void messageTipList2Str(string messageStyle, List<TemRecordData> list_trd, Label lbTip)
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
                    lowTemStr = trd.LowTem + "/";
                }

                string highTemStr = "";
                if (trd.HighTem != null && trd.HighTem != "")
                {
                    highTemStr = trd.HighTem + "℃";
                }
                strMessage += trd.StationName + "[" + lowTemStr + highTemStr + "]，";
            }
            lbTip.Text = messageStyle + strMessage.Substring(0, strMessage.Length - 1);
        }

    }

    /// <summary>
    /// 写出信息内容
    /// </summary>
    protected void showTipInfo()
    {
        p_noExistStation.Visible = false; ;
        messageTipHash2Str(ExpStationMessage.NoDataStationMessage, ExpStationMessage.NoDataStationName, lb_noExistStation, true, ExpStationMessage.AllDataStationMessage);
        if (lb_noExistStation.Text != "")
        {
            p_noExistStation.Visible = true;
        }

        p_TepExpTip.Visible = false; ;
        messageTipHash2Str(ExpStationMessage.EqualTemStationMessage, ExpStationMessage.EqualMaxMinTemStationName, lb_equalMaxMinTemTip,  false, null);
        messageTipHash2Str(ExpStationMessage.LimitTemStationMessage, ExpStationMessage.LimitTemStationName, lb_hlTemTip,  false, null);
        messageTipList2Str(ExpStationMessage.SubTemStationMessage, ExpStationMessage.SubTemStationName, lb_subTemTip);
        if (lb_equalMaxMinTemTip.Text != "" || lb_hlTemTip.Text != "" || lb_subTemTip.Text!= "")
        {
            p_TepExpTip.Visible = true;
        }

        p_WethExp.Visible = false;
        messageTipHash2Str(ExpStationMessage.LTemWethStationMessage, ExpStationMessage.TempWethExpStationName, lb_LTempWethExpTip, false, null);
        messageTipHash2Str(ExpStationMessage.WindNStationMessage, ExpStationMessage.WinNStationName, lb_WinNTip, false, null);
        messageTipHash2Str(ExpStationMessage.WethExpStationMessage, ExpStationMessage.WethExpStationName, lb_WethTip, false, null);
        if (lb_LTempWethExpTip.Text != "" || lb_WinNTip.Text != "" || lb_WethTip.Text != "")
        {
            p_WethExp.Visible = true;
        }

        messageTipString();
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    protected void messageTipString()
    {
        lb_tipRT.Text = "";
        lb_tipCP.Text = "";
        lb_tipSX.Text = "";
        lb_tipDT.Text = "";

        lb_tipRT.Text = TipMessageInfo.ReportTime;
        lb_tipCP.Text = TipMessageInfo.Channel + TipMessageInfo.Program;
        lb_tipSX.Text = TipMessageInfo.SX;
        lb_tipDT.Text = TipMessageInfo.DataType;

        colorRT(TipMessageInfo.ReportTime);

        p_tip.Visible = true;
    }

    /// <summary>
    /// reporttime颜色
    /// </summary>
    /// <param name="reporttime"></param>
    protected void colorRT(string reporttime)
    {
        try
        {
            DateTime rtdt = Convert.ToDateTime(reporttime);
            DateTime now = DateTime.Now.Date;

            if (DateTime.Compare(rtdt, now) < 0)
            {
                lb_tipRT.ForeColor = Color.Red;
            }
            else
            {
                lb_tipRT.ForeColor = Color.Blue;
            }
        }
        catch
        {

        }
    }


    /// <summary>
    /// 特殊DataType解码
    /// 2.13.17.18
    /// 特殊数据类型解码
    ///2.温度，大于50取负值，其他取本身。
    ///13.直接录入。
    ///17.999.9缺测
    ///18.999.9缺测,0.0微量降水
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    protected string expDataTypeTransCode(int typeID, string code)
    {
        string returnTransCode = code;
        switch (typeID)
        {
            //2温度，求负值
            case  2:
                try
                {
                    returnTransCode = temNgv(code) + "";
                }
                catch
                {
                }
                break;    

            //13直接录入，跳出
            case  13:
                break;

            //17温度数值（带小数点后一位）
            case  17:
                if (code == "999.9" || code.Length == 6)
                {
                    returnTransCode = "缺测";
                }
                break;

            //18降水量，比17多0.0微量降水的判断
            case  18:
                if (code == "999.9" || code.Length == 6)
                {
                    returnTransCode = "无降水";
                }
                if (code == "0.0")
                {
                    returnTransCode = "微量降水";
                }
                break;

            default:
                break;
        }
        return returnTransCode;
    }





    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void b_save_Click(object sender, EventArgs e)
    {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        int row = gv_citydata.Rows.Count;
        int column = gv_citydata.HeaderRow.Cells.Count;

        for (int j = 0; j < column; j++)
        {

            if (gv_citydata.HeaderRow.Cells[j].Visible == false)
            {
                continue;
            }
            string data = gv_citydata.HeaderRow.Cells[j].Text.ToString();
            sb.Append(formatUnitString(data));
        }

        sb.Append(Environment.NewLine);

        for (int i = 0; i < row; i++)
        {
            if (gv_citydata.Rows[i].Visible == false)
            {
                continue;
            }
            for (int j = 0; j < column; j++)
            {
                if (gv_citydata.Rows[i].Cells[j].Visible == false)
                {
                    continue;
                }

                string data = gv_citydata.Rows[i].Cells[j].Text.ToString();
                sb.Append(formatUnitString(data));
            }
            sb.Append(Environment.NewLine);
        }

        Response.Clear();
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(lb_tipCP.Text + ".txt", System.Text.Encoding.UTF8).ToString());
        Response.ContentType = "application/vnd.ms-word";
        this.EnableViewState = false;
        Response.Write(sb.ToString());
        Response.End();
    }

    /// <summary>
    /// 打印事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void b_print_OnPreRender(object sender, EventArgs e)
    {
        this.b_print.Attributes.Add("onclick", "PrintNote()");
    }

    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <param name="para"></param>
    /// <returns></returns>
    protected string formatUnitString(string para)
    {
        string returnPara = para;
        int LengthUint = 25;
        int length = UnicodeLength(para);
        for (int i = 0; i < LengthUint - length; i++)
        {
            returnPara += " ";
        }

        return returnPara;
    }

    /// <summary>
    /// 格式化长度
    /// </summary>
    /// <param name="para"></param>
    /// <returns></returns>
    protected int UnicodeLength(string para)
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

}
