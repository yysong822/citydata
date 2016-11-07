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
///WeatherClass 的摘要说明
/// </summary>
public class WeatherClass
{
    public WeatherClass()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    //20120220 syy增加最高温度和最低温度判断，如果出现最高温度和最低温度相等，则给出城市名称提示
    //20120220 syy最高温度、最低温度变量，以便对比
    //20130702 syy记录天气现象
    //20130704 syy记录天气现象(转)
    //20130702 syy记录风力
    //20130702 syy记录风向
    //20130704 syy记录风力(转)
    //20130704 syy记录风向(转)
    /// <summary>
    /// 低温
    /// </summary>
    string _LowTem;
    public string LowTem
    {
        set { _LowTem = value; }
        get { return _LowTem; }
    }
    /// <summary>
    /// 高温
    /// </summary>
    string _HighTem;
    public string HighTem
    {
        set { _HighTem = value; }
        get { return _HighTem; }
    }
    /// <summary>
    /// 天气1
    /// </summary>
    string _Weather01;
    public string Weather01
    {
        set { _Weather01 = value; }
        get { return _Weather01; }
    }
    /// <summary>
    /// 天气2
    /// </summary>
    string _Weather02;
    public string Weather02
    {
        set { _Weather02 = value; }
        get { return _Weather02; }
    }
    /// <summary>
    /// 风向1
    /// </summary>
    string _WindDir01;
    public string WindDir01
    {
        set { _WindDir01 = value; }
        get { return _WindDir01; }
    }
    /// <summary>
    /// 风向2
    /// </summary>
    string _WindDir02;
    public string WindDir02
    {
        set { _WindDir02 = value; }
        get { return _WindDir02; }
    }
    /// <summary>
    ///风力1
    /// </summary>
    string _WindPower01;
    public string WindPower01
    {
        set { _WindPower01 = value; }
        get { return _WindPower01; }
    }
    /// <summary>
    /// 风力2
    /// </summary>
    string _WindPower02;
    public string WindPower02
    {
        set { _WindPower02 = value; }
        get { return _WindPower02; }
    }

    /// <summary>
    /// 低温Element01
    /// </summary>
    string _LowTemElID;
    public string LowTemElID
    {
        set { _LowTemElID = value; }
        get { return _LowTemElID; }
    }
    /// <summary>
    /// 高温Element01
    /// </summary>
    string _HighTemElID;
    public string HighTemElID
    {
        set { _HighTemElID = value; }
        get { return _HighTemElID; }
    }
    
}
