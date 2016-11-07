using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
//using System.IO;

public partial class station : System.Web.UI.Page
{
    BLL.DataBLL dbll = new BLL.DataBLL();
    BLL.SettingBLL sdbll = new BLL.SettingBLL();

    private static Model.StationRelation[] msrs = null;
    private static Dictionary<string, string> stas = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["UserName"] == null
            || Session["UserRole"] == null)
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您还没有登录！');window.location.href='login.aspx';</script>");
            return;
        }


        if (!IsPostBack)    // 页面第一次加载
        {
            fillUserChannel();

            plView.Visible = true;
            plAdd.Visible = false;
            lblMessage.Text = "";
            headDynRow.Visible = false;        // 隐藏导出按钮


            // 加载制片人可管理的城市列表
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
            {
                stas = dbll.getUserStations(Session["UserID"].ToString());
            }
        }

        // 判断是否选中栏目，设置导出选项状态
        if (ddlUserCP.SelectedValue != "-1")
        {
            rbExportAsSetting.Disabled = false;
        }
        else
        {
            rbExportAsSetting.Checked = false;
            rbExportAsSetting.Disabled = true;
        }

        rbExportAsResult.Checked = true;

    }

    private void fillUserChannel()
    {
        ddlUserChannel.Items.Clear();

        Model.ChannelProgram[] mcps = null;

        if (Session["UserRole"] != null && Session["UserRole"].ToString() == "0")   
        {
            // 管理员加载所有频道栏目列表
            mcps = dbll.getCP("0");        // 所有频道
        }
        else if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")  
        {
            // 制片人仅加载所属频道栏目列表
            if (Session["UserID"] != null && Session["UserID"].ToString() != "")
            {
                string userID = Session["UserID"].ToString();
                mcps = dbll.getUsedChannelByUserID(userID);
            }
        }

        if (mcps != null && mcps.Length > 0)
        {
            for (int i = 0; i < mcps.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = mcps[i].CP_Name;
                li.Value = mcps[i].CP_ID.ToString();
                ddlUserChannel.Items.Add(li);
            }
        }

        ddlUserChannel.Items.Insert(0, new ListItem("-- 所有频道 --", "-1"));
    }

    private void fillUserCP(string channelID)
    {
        ddlUserCP.Items.Clear();

        Model.ChannelProgram[] mcps = dbll.getCP(channelID);
        if (mcps.Length > 0)
        {
            for (int i = 0; i < mcps.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = mcps[i].CP_Name;
                li.Value = mcps[i].CP_ID.ToString();
                ddlUserCP.Items.Add(li);
            }
        }

        ddlUserCP.Items.Insert(0, new ListItem("-- 所有栏目 --", "-1"));
    }

    protected void gvStationInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvStationInfo.PageIndex = e.NewPageIndex;

        btnQuery_Click(sender, e);
    }

    protected void gvStationInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvStationInfo.EditIndex = -1;

        btnQuery_Click(sender, e);
    }

    protected void gvStationInfo_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        gvStationInfo.EditIndex = e.NewEditIndex;

        btnQuery_Click(sender, e);
    }

    protected void gvStationInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标停留时更改背景色
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#E2DED6'");
            //当鼠标移开时还原背景色
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

            if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
            {
                ((LinkButton)e.Row.Cells[4].Controls[0]).Attributes.Add("onclick",
                    "javascript:return confirm('您确定要删除城市：" + e.Row.Cells[3].Text + " 吗？警告：该城市所有的栏目配置都将被删除！')");

                // 如果该制片人管理的城市也关联了其他制片人管理的栏目，则该城市不允许删除操作
                if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
                {
                    string tblID = gvStationInfo.DataKeys[e.Row.RowIndex].Values[0].ToString();
                    string count = String.Empty;
                    //if (stas != null && stas.ContainsKey(tblID) && stas[tblID] == "1")
                    if (stas != null && stas.TryGetValue(tblID, out count) && count == "1")
                    {
                        // do nothing.
                    }
                    else
                    {
                        // 禁用删除按钮
                        ((LinkButton)e.Row.Cells[4].Controls[0]).Enabled = false;
                        ((LinkButton)e.Row.Cells[4].Controls[0]).ForeColor = System.Drawing.Color.Gray;
                    }
                }

            }
        }
    }

    protected void gvStationInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //e.Cancel = true;

        string tblID = gvStationInfo.DataKeys[e.RowIndex].Values[0].ToString();
        string staID = ((TextBox)gvStationInfo.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();
        string staName = ((TextBox)gvStationInfo.Rows[e.RowIndex].Cells[3].Controls[0]).Text.Trim();
        
        // 检查输入
        if (staID.Equals("") || staName.Equals("") || !isValidInt(staID))
        {
            Response.Write("<script>alert('您的输入有误！');</script>");
            return;
        }

        Model.StationRelation msr = new Model.StationRelation();
        msr.StationTableID = Convert.ToInt32(tblID);
        msr.StationID = Convert.ToInt32(staID);
        msr.StationName = staName;

        if (sdbll.setStationsInfo(msr, "update") == 0)
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('城市：{0} 更新成功！');</script>", msr.StationName));
        }
        else
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('城市：{0} 更新失败！');</script>", msr.StationName));
        }

        gvStationInfo.EditIndex = -1;

        btnQuery_Click(sender, e);
    }

    protected void gvStationInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string staName = gvStationInfo.Rows[e.RowIndex].Cells[3].Text;

        Model.StationRelation msr = new Model.StationRelation();
        msr.StationTableID = Convert.ToInt32(gvStationInfo.DataKeys[e.RowIndex].Values[0].ToString());

        if (sdbll.setStationsInfo(msr, "delete") == 0)
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('城市：{0} 删除成功！');</script>", staName));
        }
        else
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('城市：{0} 删除失败！');</script>", staName));
        }

        btnQuery_Click(sender, e);
    }


    private bool isValidInt(string str)
    {
        return Regex.IsMatch(str, @"^[0-9]+$");
    }


    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Model.StationRelation msr = new Model.StationRelation();
        msr.Channel_ID = Convert.ToInt32(ddlUserChannel.SelectedValue);
        msr.Program_ID = Convert.ToInt32(ddlUserCP.SelectedValue);
        msr.StationID = tbStaID.Text.Trim() == "" ? -1 : Convert.ToInt32(tbStaID.Text.Trim());
        msr.StationName = tbStaName.Text.Trim() == "" ? null : tbStaName.Text.Trim();

        msrs = null;

        if (Session["UserRole"] != null && Session["UserRole"].ToString() == "0")
        {
            // 管理员可查询所有栏目的城市列表
            msrs = dbll.getStations(null, msr);
        }
        else if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
        {
            // 制片人仅查询所属栏目的城市列表
            string userID = Session["UserID"].ToString();
            msrs = dbll.getStations(userID, msr);
        }

        if (msrs != null && msrs.Length > 0)
        {
            lblMessage.Text = "";
            headDynRow.Visible = true;
        }
        else
        {
            lblMessage.Text = "没有查询到任何符合条件的城市！";
            headDynRow.Visible = false;
        }

        gvStationInfo.DataSource = msrs;
        gvStationInfo.DataBind();
    }

    protected void lbtnShowAdd_Click(object sender, EventArgs e)
    {
        plView.Visible = false;
        plAdd.Visible = true;
        lblMessage.Text = "";

        // 加载频道列表
        fillUserChannelAdd();

        ddlUserCP_Add.Items.Clear();
        ddlUserCP_Add.Items.Insert(0, new ListItem("-- 请选择栏目 --", "-1"));
    }

    private void fillUserChannelAdd()
    {
        ddlUserChannel_Add.Items.Clear();

        Model.ChannelProgram[] mcps = null;

        if (Session["UserRole"] != null && Session["UserRole"].ToString() == "0")
        {
            // 管理员加载所有频道栏目列表
            mcps = dbll.getCP("0");        // 所有频道
        }
        else if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
        {
            // 制片人仅加载所属频道栏目列表
            if (Session["UserID"] != null && Session["UserID"].ToString() != "")
            {
                string userID = Session["UserID"].ToString();
                mcps = dbll.getUsedChannelByUserID(userID);
            }
        }

        if (mcps != null && mcps.Length > 0)
        {
            for (int i = 0; i < mcps.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = mcps[i].CP_Name;
                li.Value = mcps[i].CP_ID.ToString();
                ddlUserChannel_Add.Items.Add(li);
            }
        }

        ddlUserChannel_Add.Items.Insert(0, new ListItem("-- 请选择频道 --", "-1"));
    }

    private void fillUserCPAdd(string channelID)
    {
        ddlUserCP_Add.Items.Clear();

        Model.ChannelProgram[] mcps = dbll.getCP(channelID);
        if (mcps.Length > 0)
        {
            for (int i = 0; i < mcps.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = mcps[i].CP_Name;
                li.Value = mcps[i].CP_ID.ToString();
                ddlUserCP_Add.Items.Add(li);
            }
        }

        ddlUserCP_Add.Items.Insert(0, new ListItem("-- 请选择栏目 --", "-1"));
    }


    protected void btnAddOper_Click(object sender, EventArgs e)
    {
        int cpID = Convert.ToInt32(ddlUserCP_Add.SelectedValue);
        string cpName = ddlUserCP_Add.SelectedItem.Text;
        string staID = tbStaID_Add.Text.Trim();
        string staName = tbStaName_Add.Text.Trim();

        // 检查输入
        if (!isValidInt(staID))
        {
            Response.Write("<script>alert('站点编号必须为数字！');</script>");
            return;
        }

        // 配置操作
        Model.StationRelation msr = new Model.StationRelation();
        msr.StationID = Convert.ToInt32(staID);
        msr.StationName = staName;
        msr.CP_ID = Convert.ToInt32(cpID);

        int selectID = -1;      // 用来接收新添加的站点索引

        if (sdbll.addStationInfoWithSetting(msr, out selectID))
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('栏目：{0} 城市：{1} 配置成功！');</script>", cpName, staName));

            // 定位最新添加的城市
            findAddedStation(ddlUserChannel_Add.SelectedValue, ddlUserCP_Add.SelectedValue, selectID);

            btnAddCancel_Click(sender, e);  // 返回到城市列表
        }
        else
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('栏目：{0} 城市：{1} 配置失败！');</script>", cpName, staName));
        }

    }

    private void findAddedStation(string channelID, string programID, int selectID)
    {
        // 重新绑定CP下拉列表并定位条目
        fillUserChannel();
        fillUserCP(channelID);

        // 手动设置DropDownList选中项之前必须先清除原选择项
        // 否则会报错误“不能在 DropDownList 中选择多个项”
        //ddlUserChannel.ClearSelection();
        //ddlUserCP.ClearSelection();

        // 避免错误：不能在DropDownList中选择多个项，不推荐使用.Selected属性设置DropDownList选择项
        //ddlUserChannel.Items.FindByValue(channelID).Selected = true;
        //ddlUserCP.Items.FindByValue(programID).Selected = true;

        ddlUserChannel.SelectedIndex = -1;
        ListItem chlItem = ddlUserChannel.Items.FindByValue(channelID);
        ddlUserChannel.SelectedIndex = ddlUserChannel.Items.IndexOf(chlItem);

        ddlUserCP.SelectedIndex = -1;
        ListItem prgItem = ddlUserCP.Items.FindByValue(programID);
        ddlUserCP.SelectedIndex = ddlUserCP.Items.IndexOf(prgItem);

        btnQuery_Click(null, null);     // 重新查询

        /*
        Model.StationRelation msr = new Model.StationRelation();
        msr.Channel_ID = Convert.ToInt32(ddlUserChannel.SelectedValue);
        msr.Program_ID = Convert.ToInt32(ddlUserCP.SelectedValue);
        msr.StationID = -1;
        msr.StationName = null;
        Model.StationRelation[] msrs = dbll.getStations(Session["UserRole"].ToString() == "0" ? null : Session["UserID"].ToString(), msr);
        */
 
        // 找到该项在GridView中的位置
        int idx = 0;
        for (int i = 0; i < msrs.Length; i++)
        {
            // 遍历主键，查找符合已添加主键的序号
            //if (gvUsers.DataKeys[i].Value.ToString() == userID)
            if (msrs[i].StationTableID == selectID)
                break;
            else
                idx++;
        }

        int pageSize = gvStationInfo.PageSize;
        int pageIdx = idx / pageSize;
        int rowIdx = idx % pageSize;

        gvStationInfo.PageIndex = pageIdx;    // 设置页数后必须重新绑定数据才能正确显示

        // 重新绑定用户数据
        gvStationInfo.DataSource = msrs;
        gvStationInfo.DataBind();

        gvStationInfo.Rows[rowIdx].BackColor = System.Drawing.Color.FromName("#B9CDFB");

    }

    protected void btnAddCancel_Click(object sender, EventArgs e)
    {
        plView.Visible = true;
        plAdd.Visible = false;
        lblMessage.Text = "";

        // 清空已添加的站点信息
        tbStaID_Add.Text = "";
        tbStaName_Add.Text = "";
    }

    protected void ddlUserChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 加载该频道下的栏目列表
        fillUserCP(ddlUserChannel.SelectedValue);
    }

    protected void ddlUserCP_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 加载该栏目下的城市列表
        btnQuery_Click(sender, e);
    }

    protected void ddlUserChannel_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 加载该频道下的栏目列表
        fillUserCPAdd(ddlUserChannel_Add.SelectedValue);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        // 导出当前查询的所有城市信息为station.txt文件
        /*
        string fileName = "station.txt";
        string filePath = Server.MapPath("~/Temp/" + fileName);
        StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default);
        sw.NewLine = "\r\n";

        for (int i = 0; i < msrs.Length; i++)
        {
            sw.WriteLine(msrs[i].StationID.ToString() + " " + msrs[i].StationName);
        }

        sw.Flush();
        sw.Close();

        // 向浏览器发送station.txt文件
        // 以字符流的形式下载文件
        FileStream fs = new FileStream(filePath, FileMode.Open);
        byte[] bytes = new byte[(int)fs.Length];
        
        fs.Read(bytes, 0, bytes.Length);
        fs.Close();

        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
        */

        // 不生成临时文件，直接向浏览器写数据
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (rbExportAsSetting.Disabled == false && rbExportAsSetting.Checked == true)
        {
            // 查询该栏目的站点配置
            Model.StationRelation[] sets = dbll.getUsedStationsByCPID(ddlUserCP.SelectedValue);
            for (int i = 0; i < sets.Length; i++)
            {
                sb.AppendLine(sets[i].StationID.ToString() + " " + sets[i].StationName);
            }
        }
        else
        {
            for (int i = 0; i < msrs.Length; i++)
            {
                sb.AppendLine(msrs[i].StationID.ToString() + " " + msrs[i].StationName);
            }
        }

        byte[] bytes = System.Text.Encoding.Default.GetBytes(sb.ToString());

        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("station.txt", System.Text.Encoding.UTF8));
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();

    }

}
