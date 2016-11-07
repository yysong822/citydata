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

/// <summary>
///ElCode 的摘要说明
/// </summary>
public class ElTransCode
{
    public ElTransCode()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //

        _ELementCode = new List<ElCode>();

    }

    ////元素名，如果：Element01
    //private int _Element_Name;
    //public int Element_Name
    //{
    //    set { _Element_Name = value; }
    //    get { return _Element_Name; }
    //}

    //transCode表里面的type字段
    private int _ElementCodeType;
    public int ElementCodeType
    {
        set { _ElementCodeType = value; }
        get { return _ElementCodeType; }
    }

    private List<ElCode> _ELementCode;
    public List<ElCode> ELementCode
    {
        set { _ELementCode = value; }
        get { return _ELementCode; }
    }
    
}

public class ElCode
{
    public ElCode()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    //transCode表里面的Code字段
    private string _ElementCode;
    public string ElementCode
    {
        set { _ElementCode = value; }
        get { return _ElementCode; }
    }

    //transCode表里面的Name字段
    private string _TransCodeCH;
    public string TransCodeCH
    {
        set { _TransCodeCH = value; }
        get { return _TransCodeCH; }
    }
}
