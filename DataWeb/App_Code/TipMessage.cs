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

/// <summary>
///TipMessage 的摘要说明
/// </summary>
public class TipMessage
{
    public TipMessage()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
        _DataTypeMessage = "数据类型城市预报";
    }

    private string _ReportTime; //预报时间
    public string ReportTime
    {
        set { _ReportTime = value; }
        get { return _ReportTime; }
    }

    private string _Channel ; //频道栏目
    public string Channel
    {
        set { _Channel = value; }
        get { return _Channel; }
    }

    private string _Program ;//栏目
    public string Program
    {
        set { _Program = value; }
        get { return _Program; }
    }

    private string _SX ; //时效
    public string SX
    {
        set { _SX = value; }
        get { return _SX; }
    }

    private string _DataType ; //数据类型
    public string DataType
    {
        set { _DataType = value; }
        get { return _DataType; }
    }

    private string _DataTypeMessage; //数据类型城市预报
    public string DataTypeMessage
    {
        set { _DataTypeMessage = value; }
        get { return _DataTypeMessage; }
    }
}
