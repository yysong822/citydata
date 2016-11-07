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


public partial class program : System.Web.UI.Page
{
    BLL.DataBLL dbll = new BLL.DataBLL();
    BLL.SettingBLL sdbll = new BLL.SettingBLL();
    private static Model.ChannelProgram[] mChannelInfo;
    private static Model.ChannelProgram[] mProgramInfo;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["UserName"] == null
            || Session["UserRole"] == null)
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您还没有登录！');window.location.href='login.aspx';</script>");
            return;
        }

        // 判断是否具有管理员权限
        if (Session["UserRole"] != null && Session["UserRole"].ToString() != "0")
        {
            // 管理员角色：0
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您没有管理员权限！');window.location.href='login.aspx';</script>");
            return;
        }

        if (!IsPostBack)
        {
            initTrProgram();
            initDDLProgram();
            bAddProgram.Enabled = false;
            bDelete.Enabled = false;
            bModifyName.Enabled = false;
            bMove.Enabled = false;
            ddlProgramList.Enabled = false;
        }
        
    }

    protected void initTrProgram()
    {
        if (trProgram.Nodes.Count > 0)
        {
            trProgram.Nodes.Clear();
        }
        mChannelInfo = dbll.getCP("0");
        for (int i = 0; i < mChannelInfo.Length; i++)
        {
            TreeNode nodeChannel = new TreeNode();
            nodeChannel.Value = mChannelInfo[i].CP_ID.ToString();
            nodeChannel.Text = mChannelInfo[i].CP_Name;
            trProgram.Nodes.Add(nodeChannel);
            addChileNode(nodeChannel);
        }
        trProgram.CollapseAll();
    }

    protected void addChileNode(TreeNode nodeChannel)
    {
        string channelID = nodeChannel.Value;
        mProgramInfo = dbll.getCP(channelID);
        for (int i = 0; i < mProgramInfo.Length; i++)
        {
            TreeNode nodeProgram = new TreeNode();
            nodeProgram.Value = mProgramInfo[i].CP_ID.ToString();
            nodeProgram.Text = mProgramInfo[i].CP_Name;
            nodeChannel.ChildNodes.Add(nodeProgram);
        }
    }

    private void flushPage()
    {
        initTrProgram();
        tbAddProgram.Text = "";
        tbExistProgram.Text = "";
        lbSelected.Text = "空！";
        ddlProgramList.SelectedIndex = 0;
        bAddProgram.Enabled = false;
        bDelete.Enabled = false;
        bModifyName.Enabled = false;
        bMove.Enabled = false;
        ddlProgramList.Enabled = false;
    }

    protected void initDDLProgram()
    {
        ddlProgramList.Items.Clear();
        ListItem li = new ListItem();
        li.Value = "选择频道";
        li.Text = "选择频道";
        ddlProgramList.Items.Add(li);
        for (int i = 0; i < mChannelInfo.Length; i++)
        {
            ListItem liChannel = new ListItem();
            liChannel.Value = mChannelInfo[i].CP_ID.ToString();
            liChannel.Text = mChannelInfo[i].CP_Name;
            ddlProgramList.Items.Add(liChannel);
        }
    }

    protected void trProgram_SelectedNodeChanged(object sender, EventArgs e)
    {
        string oldName = trProgram.SelectedNode.Text.Trim();
        string oldNameID = trProgram.SelectedNode.Value.Trim();
        if (trProgram.SelectedNode != null && trProgram.SelectedNode.Parent != null)    /*二级节点*/
        {
            lbTip.Text = "已选栏目：";
            lbSelected.Text = oldName;
            tbExistProgram.Text = oldName;
            ddlProgramList.Enabled = true;
            bMove.Enabled = true;
            bModifyName.Enabled = true;
            bDelete.Enabled = true;
            bAddProgram.Enabled = true;
        }
        else
        {
            trProgram.CollapseAll();
            trProgram.SelectedNode.ExpandAll();
            lbTip.Text = "已选频道：";
            lbSelected.Text = oldName;
            tbExistProgram.Text = "";
            ddlProgramList.Enabled = false;
            bMove.Enabled = false;
            bDelete.Enabled = false;
            bModifyName.Enabled = false;
            bAddProgram.Enabled = true;
        }
    }

    protected void bModifyName_Click(object sender, EventArgs e)
    {
        if (tbExistProgram.Text.Trim() != "")
        {
            Model.ChannelProgram mChannel = new Model.ChannelProgram();
            mChannel.CP_ID = Convert.ToInt32(trProgram.SelectedNode.Value.Trim());
            mChannel.CP_Name = tbExistProgram.Text.Trim();
            if (sdbll.setCP(mChannel, "update") == 0)
            {
                flushPage();
                Response.Write("<script>alert('修改栏目成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('修改栏目失败！');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('不允许为空！');</script>");
        }
        
    }

    protected void bDelete_Click(object sender, EventArgs e)
    {
        Model.ChannelProgram mChannel = new Model.ChannelProgram();
        mChannel.CP_ID = Convert.ToInt32(trProgram.SelectedNode.Value.Trim());
        if (sdbll.setCP(mChannel, "delete") == 0)
        {
            flushPage();
            Response.Write("<script>alert('删除栏目成功！');</script>");
        }
        else
        {
            Response.Write("<script>alert('删除栏目失败！');</script>");
        }
    }


    protected void bAddProgram_Click(object sender, EventArgs e)
    {
        if (trProgram.SelectedNode != null && tbAddProgram.Text.Trim() != "")
        {
            Model.ChannelProgram mChannel = new Model.ChannelProgram();
            mChannel.CP_Name = tbAddProgram.Text.Trim();
            if (trProgram.SelectedNode.Parent != null)    /*二级节点*/
            {
                mChannel.FatherID = Convert.ToInt32(trProgram.SelectedNode.Parent.Value.Trim());
            }
            else
            {
                mChannel.FatherID = Convert.ToInt32(trProgram.SelectedNode.Value.Trim());
            }

            if (sdbll.setCP(mChannel, "insert") == 0)
            {
                flushPage();
                Response.Write("<script>alert('增加栏目成功！');</script>");

            }
            else
            {
                Response.Write("<script>alert('增加栏目失败！');</script>");
            }
        }
        else
        {
            if (trProgram.SelectedNode == null)
            {
                Response.Write("<script>alert('请正确选择频道或者栏目！');</script>");

            }
            else
            {
                Response.Write("<script>alert('不允许为空！');</script>");
            }
        }
        
    }

    protected void bMove_Click(object sender, EventArgs e)
    {
        if (trProgram.SelectedNode != null)
        {
            Model.ChannelProgram mChannel = new Model.ChannelProgram();
            mChannel.CP_Name = trProgram.SelectedNode.Text.Trim();
            mChannel.CP_ID = Convert.ToInt32(trProgram.SelectedNode.Value.Trim());
            if (ddlProgramList.SelectedValue.Trim() != "选择频道")
            {
                mChannel.FatherID = Convert.ToInt32(ddlProgramList.SelectedValue.Trim());
            }
            else
            {
                Response.Write("<script>alert('请选择频道！');</script>");
            }
            if (sdbll.setCPnewFather(mChannel) == 0)
            {
                flushPage();
                Response.Write("<script>alert('转移栏目成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('转移栏目失败！');</script>");
            }
        }
        
    }
}
