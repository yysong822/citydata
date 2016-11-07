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
///TemRecordData 的摘要说明
/// </summary>
public class TemRecordData
{
    public TemRecordData()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    private int _StationID;
    public int StationID
    {
        set { _StationID = value; }
        get { return _StationID; }
    }

    private string _StationName;
    public string StationName
    {
        set { _StationName = value; }
        get { return _StationName; }
    }

    private int _Time;
    public int Time
    {
        set { _Time = value; }
        get { return _Time; }
    }

    private int _TimeType;
    public int TimeType
    {
        set { _TimeType = value; }
        get { return _TimeType; }
    }

    private int _DataType;
    public int DataType
    {
        set { _DataType = value; }
        get { return _DataType; }
    }

    private string _HighTem;
    public string HighTem
    {
        set { _HighTem = value; }
        get { return _HighTem; }
    }

    private string _LowTem;
    public string LowTem
    {
        set { _LowTem = value; }
        get { return _LowTem; }
    }
}
