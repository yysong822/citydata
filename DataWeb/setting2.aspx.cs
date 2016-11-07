using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 新栏目配置页
/// By Edward Chan
/// </summary>
public partial class setting2 : System.Web.UI.Page
{
    BLL.DataBLL dbll = new BLL.DataBLL();
    BLL.SettingBLL sbll = new BLL.SettingBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["UserName"] == null
            || Session["UserRole"] == null)
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您还没有登录！');window.location.href='login.aspx';</script>");
            return;
        }


        if (!IsPostBack)
        {
            tvProgramDataBind();
            ddlDataTypeDataBind();

            lbTips.Text = "请选择一个频道！";
        }

    }


    #region 数据绑定

    private void tvProgramDataBind()
    {
        if (tvProgram.Nodes.Count > 0)
        {
            tvProgram.Nodes.Clear();
        }

        Model.ChannelProgramUser[] mcpus = null;

        // 如果登陆用户是管理员，显示所有的频道配置
        if (Session["UserRole"] != null && Session["UserRole"].ToString() == "0")
        {
            // 管理员角色：0
            mcpus = dbll.getCPs();
        }
        // 如果登陆用户是制片人，仅显示当前用户的频道配置
        else if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
        {
            // 制片人角色：1
            mcpus = dbll.getCPUsersByUserID(Session["UserID"].ToString());
        }

        if (mcpus == null || mcpus.Length == 0)      // 该用户没有指定任何频道
        {
            lblLeftMessage.Text = "没有可管理的频道栏目！<br/>请联系管理员！";
        }
        else
        {
            // Channel第一项
            TreeNode rootNode = new TreeNode(mcpus[0].ChannelName, mcpus[0].ChannelID.ToString());   // text, value
            tvProgram.Nodes.Add(rootNode);

            // Program第一项
            rootNode.ChildNodes.Add(new TreeNode(mcpus[0].ProgramName, mcpus[0].ProgramID.ToString() 
                + "#" + mcpus[0].TimeTypeID.ToString() + "#" + mcpus[0].DataTypeID.ToString()));

            TreeNode leafNode = rootNode;
            for (int i = 1; i < mcpus.Length; i++)
            {
                leafNode = addCPNode(leafNode, mcpus[i - 1], mcpus[i]);
            }

            //tvProgram.CollapseAll();
        }

        // 管理员折叠所有频道栏目
        if (Session["UserRole"] != null && Session["UserRole"].ToString() == "0")
            tvProgram.CollapseAll();

    }

    private TreeNode addCPNode(TreeNode prevNode, Model.ChannelProgramUser prevItem, Model.ChannelProgramUser currItem)
    {
        TreeNode currNode = prevNode;

        if (currItem.ChannelID != null && prevItem.ChannelID != currItem.ChannelID)
        {
            currNode = new TreeNode();
            currNode.Value = currItem.ChannelID.ToString();
            currNode.Text = currItem.ChannelName;
            tvProgram.Nodes.Add(currNode);
        }

        if (currItem.ProgramID != null)
        {
            TreeNode programNode = new TreeNode();
            programNode.Value = currItem.ProgramID.ToString() + "#" + currItem.TimeTypeID.ToString() + "#" + currItem.DataTypeID.ToString();
            programNode.Text = currItem.ProgramName;
            currNode.ChildNodes.Add(programNode);
        }

        return currNode;
    }


    protected void ddlDataTypeDataBind()
    {
        ddlDataType.Items.Clear();

        Model.RecordeData[] mDataType = dbll.getDataType(null);
        for (int i = 0; i < mDataType.Length; i++)
        {
            ListItem liDataType = new ListItem();
            liDataType.Value = mDataType[i].DataType.ToString();
            liDataType.Text = mDataType[i].DataTypeName;
            ddlDataType.Items.Add(liDataType);
        }

        ddlDataType.Items.Insert(0, new ListItem("-- 请选择 --", "-1"));
    }

    protected void ddlTimeTypeDataBind(string dataTypeID)
    {
        ddlTimeType.Items.Clear();

        Model.RecordeData dataType = new Model.RecordeData();
        dataType.DataType = Convert.ToInt32(dataTypeID);

        Model.RecordeData[] mTimeType = dbll.getTimeType(dataType);
        for (int i = 0; i < mTimeType.Length; i++)
        {
            ListItem liTimeType = new ListItem();
            liTimeType.Value = mTimeType[i].TimeType.ToString();
            liTimeType.Text = mTimeType[i].TimeTypeName;
            ddlTimeType.Items.Add(liTimeType);
        }

        ddlTimeType.Items.Insert(0, new ListItem("-- 请选择 --", "-1"));
    }

    protected void lbDataTypeChangeDataBind(string dataTypeID)
    {
        lbDataTypeUsable.Items.Clear();

        Model.ElementRelation mer = new Model.ElementRelation();
        mer.DataTypeID = Convert.ToInt32(dataTypeID);

        Model.ElementRelation[] mers = dbll.getAllElement(mer);
        for (int i = 0; i < mers.Length; i++)
        {
            ListItem li = new ListItem();
            li.Value = mers[i].ElementID.ToString();
            li.Text = mers[i].ElementNameCN;
            lbDataTypeUsable.Items.Add(li);
        }
    }

    protected void lbDataTypeUsableDataBind(string programID)
    {
        lbDataTypeUsable.Items.Clear();

        Model.ElementRelation[] mers = dbll.getUsableElementsByCPID(programID);
        for (int i = 0; i < mers.Length; i++)
        {
            ListItem li = new ListItem();
            li.Value = mers[i].ElementID.ToString();
            li.Text = mers[i].ElementNameCN;
            lbDataTypeUsable.Items.Add(li);
        }
    }

    protected void lbDataTypeUsedDataBind(string programID)
    {
        lbDataTypeUsed.Items.Clear();

        Model.ElementRelation[] mers = dbll.getUsedElementsByCPID(programID);
        for (int i = 0; i < mers.Length; i++)
        {
            ListItem li = new ListItem();
            li.Value = mers[i].ElementID.ToString();
            li.Text = mers[i].ElementNameCN;
            lbDataTypeUsed.Items.Add(li);
        }
    }

    protected void lbStationUsableDataBind()
    {
        lbStationUsable.Items.Clear();

        Model.StationRelation[] msrs = dbll.getStationsInfoOrderByName();
        for (int i = 0; i < msrs.Length; i++)
        {
            ListItem li = new ListItem();
            li.Value = msrs[i].StationID.ToString() + "#" + msrs[i].StationTableID.ToString();
            li.Text = msrs[i].StationName + " ( " + msrs[i].StationID.ToString() + " ) ";
            lbStationUsable.Items.Add(li);
        }
    }

    protected void lbStationUsableDataBind(string programID)
    {
        lbStationUsable.Items.Clear();

        Model.StationRelation[] msrs = dbll.getUsableStationsByCPID(programID);
        for (int i = 0; i < msrs.Length; i++)
        {
            ListItem li = new ListItem();
            li.Value = msrs[i].StationID.ToString() + "#" + msrs[i].StationTableID.ToString();
            li.Text = msrs[i].StationName + " ( " + msrs[i].StationID.ToString() + " ) ";
            lbStationUsable.Items.Add(li);
        }
    }

    protected void lbStationUsedDataBind(string programID)
    {
        lbStationUsed.Items.Clear();

        Model.StationRelation[] msrs = dbll.getUsedStationsByCPID(programID);
        for (int i = 0; i < msrs.Length; i++)
        {
            ListItem li = new ListItem();
            li.Value = msrs[i].StationID.ToString() + "#" + msrs[i].StationTableID.ToString();
            li.Text = msrs[i].StationName + " ( " + msrs[i].StationID.ToString() + " ) ";
            lbStationUsed.Items.Add(li);
        }
    }

    #endregion


    private void lblMessageSetting()
    {
        if (ddlDataType.SelectedValue == "-1" || ddlTimeType.SelectedValue == "-1")      // 该栏目还没有配置过
        {
            lbMessage.Text = "请选择栏目使用的数据类型和时间类型！";
        }
        else
        {
            if (lbDataTypeUsable.Items.Count == 0)
            {
                lbMessage.Text = "您已选择了全部项目！";
            }
            else if (lbDataTypeUsed.Items.Count == 0)
            {
                lbMessage.Text = "共 " + lbDataTypeUsed.Items.Count + " 个项目可选！";
            }
            else
            {
                lbMessage.Text = "已选择 " + lbDataTypeUsed.Items.Count + " 个项目；可选择 "
                    + lbDataTypeUsable.Items.Count + " 个项目。";
            }

            if (lbStationUsed.Items.Count == 0)
            {
                lbMessage.Text += "<br/>共 " + lbStationUsable.Items.Count + " 个城市可选！";
            }
            else
            {
                lbMessage.Text += "<br/>已选择 " + lbStationUsed.Items.Count + " 个城市。";
            }

        }
    }


    protected void ddlDataType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlTimeTypeDataBind(ddlDataType.SelectedValue);

        // 如果修改该栏目使用的数据类型，则显示新类型的所有要素
        lbDataTypeChangeDataBind(ddlDataType.SelectedValue);
        lbDataTypeUsedDataBind("-1");
    }


    protected void trProgram_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (tvProgram.SelectedNode != null && tvProgram.SelectedNode.Parent != null)    // 栏目
        {
            lbTips.Text = "已选栏目：" + tvProgram.SelectedNode.Text;
            //    + "<br/>请选择栏目使用的数据类型和时间类型！";

            // nodeValue: {0}: 栏目ID; {1}: 时间类型ID; {2}: 数据类型ID
            string[] nodeValue = tvProgram.SelectedNode.Value.Split('#');

            // 1. 绑定数据类型和时间类型
            //ddlDataTypeDataBind();
            ddlTimeTypeDataBind(nodeValue[2]);

            // 避免错误：不能在DropDownList中选择多个项，不推荐使用.Selected属性设置DropDownList选择项
            ddlDataType.SelectedIndex = -1;
            ListItem dataTypeItem = ddlDataType.Items.FindByValue(nodeValue[2]);
            ddlDataType.SelectedIndex = ddlDataType.Items.IndexOf(dataTypeItem);

            ddlTimeType.SelectedIndex = -1;
            ListItem timeTypeItem = ddlTimeType.Items.FindByValue(nodeValue[1]);
            ddlTimeType.SelectedIndex = ddlTimeType.Items.IndexOf(timeTypeItem);

            // 2. 绑定可选项目和已选项目
            lbDataTypeUsableDataBind(nodeValue[0]);
            lbDataTypeUsedDataBind(nodeValue[0]);


            // 3. 绑定可选城市和已选城市
            lbStationUsableDataBind();      // mod by Edward Chan：可能出现一个栏目添加两个相同城市的情况
            lbStationUsedDataBind(nodeValue[0]);


            lblMessageSetting();


        }
        else if (tvProgram.SelectedNode != null && tvProgram.SelectedNode.Parent == null)   // 频道
        {
            tvProgram.CollapseAll();
            tvProgram.SelectedNode.ExpandAll();

            lbTips.Text = "已选频道：" + tvProgram.SelectedNode.Text;
                //+ "<br/>请选择该频道的一个栏目！";

            ddlDataTypeDataBind();
            ddlTimeTypeDataBind("-1");

            lbDataTypeUsable.Items.Clear();
            lbDataTypeUsed.Items.Clear();
            lbStationUsable.Items.Clear();
            lbStationUsed.Items.Clear();


            lbMessage.Text = "请选择该频道的一个栏目！";

        }
    }


    protected void btnSettingOper_Click(object sender, EventArgs e)
    {
        // nodeValue: {0}: 栏目ID; {1}: 时间类型ID; {2}: 数据类型ID
        string[] nodeValue = tvProgram.SelectedNode.Value.Split('#');

        string dataTypeReqStr = Request.Form[hfDataTypeUsed.UniqueID].ToString();   // 预报要素
        string stationReqStr = Request.Form[hfStationUsed.UniqueID].ToString();     // 站点信息

        // datatype bundle example: 1;2;...
        //string[] dataTypeReqArr = dataTypeReqStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        // station bundle example: 54511#-1|北京;59287#-1|广州;...
        //string[] stationReqArr = stationReqStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        /*
        for (int i = 0; i < stationReqArr.Length; i++)
        {
            // entries bundle: [0]:站点号 [1]:主键值 [2]:站点名称
            string[] entries = stationReqArr[i].Split(new char[] { '#', '|' }, StringSplitOptions.RemoveEmptyEntries);
        }
        */

        // 0. 更新TreeView节点Value
        tvProgram.SelectedNode.Value = nodeValue[0] + "#" + ddlTimeType.SelectedValue + "#" + ddlDataType.SelectedValue;

        int programID = Convert.ToInt32(nodeValue[0]);
        int dataTypeID = Convert.ToInt32(ddlDataType.SelectedValue);
        int timeTypeID = Convert.ToInt32(ddlTimeType.SelectedValue);

        // 1. 为栏目设置数据类型和时间类型  2. 为栏目设置预报要素和站点信息
        if (sbll.setCPDataTypeAndTimeType(programID, dataTypeID, timeTypeID) 
            && sbll.addElementAndStationSetting(programID, dataTypeReqStr, stationReqStr))
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('栏目：{0} 配置成功！');</script>", tvProgram.SelectedNode.Text));
        }
        else
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('栏目：{0} 配置失败！');</script>", tvProgram.SelectedNode.Text));
        }

        // 重新绑定数据
        trProgram_SelectedNodeChanged(sender, e);
    }

}
