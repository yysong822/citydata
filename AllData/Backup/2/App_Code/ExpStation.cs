using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Model;

/// <summary>
///ExpStation 的摘要说明
/// </summary>
public class ExpStation
{
    public ExpStation()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
        _NoDataStationMessage = "<li>无数据的站点城市：";
        _EqualTemStationMessage = "<li>最高温和最低温一致的站点城市：";
        _LimitTemStationMessage = "<li>异常气温(最高温>=45℃,最低温<=-40℃)的站点城市：";
        _SubTemStationMessage = "<li>最高气温或者最低气温和前一天预报相差>=8℃的站点城市：";
        _LTemWethStationMessage = "<li>最低气温>5℃,但天气现象为冻雨、雨夹雪、小雪、中雪、大雪、暴雪的站点城市：";
        _WindNStationMessage = "<li>有风力(>=3级)，但无风向的站点城市：";
        _WethExpStationMessage = "<li>天气现象为大暴雨、大暴雪的站点城市：";


        _NoDataStationName = new HashSet<string>();
        _EqualMaxMinTemStationName = new HashSet<string>();
        _LimitTemStationName = new HashSet<string>();
        _SubTemStationName = new List<TemRecordData>();
        _TempWethExpStationName = new HashSet<string>();
        _WinNStationName = new HashSet<string>();
        _WethExpStationName = new HashSet<string>();

        _AllDataStationMessage = "<li>本次查询无缺失数据站点";

    }
    ///////////////////////////////////////////////////////////
    //提示信息
    //////////////////////////////////////////////////////////
    /// <summary>
    /// 无数据站点
    /// </summary>
    private string _NoDataStationMessage;
    public string NoDataStationMessage
    {
        set { _NoDataStationMessage = value; }
        get { return _NoDataStationMessage; }
    }
    /// <summary>
    /// 相同最高温最低温
    /// </summary>
    private string _EqualTemStationMessage;
    public string EqualTemStationMessage
    {
        set { _EqualTemStationMessage = value; }
        get { return _EqualTemStationMessage; }
    }

    /// <summary>
    /// syy 20130701 最高气温》=45 || 最低气温《=-40
    /// </summary>
    private string _LimitTemStationMessage;
    public string LimitTemStationMessage
    {
        set { _LimitTemStationMessage = value; }
        get { return _LimitTemStationMessage; }
    }

    private string _SubTemStationMessage;
    public string SubTemStationMessage
    {
        set { _SubTemStationMessage = value; }
        get { return _SubTemStationMessage; }
    }

    /// <summary>
    /// syy 20130701 最低气温》5，天气现象为冻雨、雨加雪、小雪、中雪、大雪、暴雪
    /// </summary>
    private string _LTemWethStationMessage;
    public string LTemWethStationMessage
    {
        set { _LTemWethStationMessage = value; }
        get { return _LTemWethStationMessage; }
    }

    /// <summary>
    /// syy 20130701 风力》=3，无风向
    /// </summary>
    private string _WindNStationMessage;
    public string WindNStationMessage
    {
        set { _WindNStationMessage = value; }
        get { return _WindNStationMessage; }
    }

    /// <summary>
    /// syy 20130701 天气现象为大暴雨、大暴雪
    /// </summary>
    private string _WethExpStationMessage;
    public string WethExpStationMessage
    {
        set { _WethExpStationMessage = value; }
        get { return _WethExpStationMessage; }
    }

    /// <summary>
    /// 所有数据站点齐全
    /// </summary>
    private string _AllDataStationMessage;
    public string AllDataStationMessage
    {
        set { _AllDataStationMessage = value; }
        get { return _AllDataStationMessage; }
    }

    ///////////////////////////////////////////////////////////
    //数据内容
    //////////////////////////////////////////////////////////
    /// <summary>
    /// 无数据站点
    /// </summary>
    private HashSet<string> _NoDataStationName;
    public HashSet<string> NoDataStationName
    {
        get { return _NoDataStationName; }
        set { _NoDataStationName = value; }
    }

    /// <summary>
    /// 相同最高温最低温
    /// </summary>
    private HashSet<string> _EqualMaxMinTemStationName;
    public HashSet<string> EqualMaxMinTemStationName
    {
        get { return _EqualMaxMinTemStationName; }
        set { _EqualMaxMinTemStationName = value; }
    }

    /// <summary>
    /// syy 20130701 最高气温》=45 || 最低气温《=-40
    /// </summary>
    private HashSet<string> _LimitTemStationName;
    public HashSet<string> LimitTemStationName
    {
        get { return _LimitTemStationName; }
        set { _LimitTemStationName = value; }
    }

    /// <summary>
    /// syy 20130701 最低气温》5，天气现象为冻雨、雨加雪、小雪、中雪、大雪、暴雪
    /// </summary>
    private HashSet<string> _TempWethExpStationName;
    public HashSet<string> TempWethExpStationName
    {
        get { return _TempWethExpStationName; }
        set { _TempWethExpStationName = value; }
    }

    /// <summary>
    /// syy 20130701 风力》=3，无风向
    /// </summary>
    private HashSet<string> _WinNStationName;
    public HashSet<string> WinNStationName
    {
        get { return _WinNStationName; }
        set { _WinNStationName = value; }
    }

    /// <summary>
    /// syy 20130701 天气现象为大暴雨、大暴雪
    /// </summary>
    private HashSet<string> _WethExpStationName;
    public HashSet<string> WethExpStationName
    {
        get { return _WethExpStationName; }
        set { _WethExpStationName = value; }
    }

    /// <summary>
    /// syy 20130701 天气现象为大暴雨、大暴雪
    /// </summary>
    private List<TemRecordData> _SubTemStationName;
    public List<TemRecordData> SubTemStationName
    {
        get { return _SubTemStationName; }
        set { _SubTemStationName = value; }
    }
}
