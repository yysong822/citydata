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

public partial class channel : System.Web.UI.Page
{
    BLL.DataBLL dbll = new BLL.DataBLL();
    BLL.SettingBLL sdbll = new BLL.SettingBLL();
    private static Model.ChannelProgram[] mChannelInfo;


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
            initTrChannel();
        }

        bModifyName.Enabled = false;
        bDelete.Enabled = false;
        bAddChannel.Enabled = false;     
    }


    protected void initTrChannel()
    {
        if (trChannel.Nodes.Count > 0)
        {
            trChannel.Nodes.Clear();
        }
        TreeNode nodeChannelList = new TreeNode();
        nodeChannelList.Value = "频道列表";
        nodeChannelList.Text = "频道列表";
        trChannel.Nodes.Add(nodeChannelList);
        mChannelInfo = dbll.getCP("0");
        for (int i = 0; i < mChannelInfo.Length; i++)
        {
            TreeNode nodeChannel = new TreeNode();
            nodeChannel.Value = mChannelInfo[i].CP_ID.ToString();
            nodeChannel.Text = mChannelInfo[i].CP_Name;
            nodeChannelList.ChildNodes.Add(nodeChannel);
        }
        trChannel.ExpandAll();
    }

    /*
    * 根据选择更改页面显示
    */
    protected void trChannel_SelectedNodeChanged(object sender, EventArgs e)
    {
        string oldName = trChannel.SelectedNode.Text.Trim();
        string oldNameID = trChannel.SelectedNode.Value.Trim();
        if (trChannel.SelectedNode.Parent != null)    /*二级节点*/
        {
            lbTip.Text = "已选频道：";
            lbSelected.Text = oldName;
            tbExistChannel.Text = oldName;
            bModifyName.Enabled = true;
            bDelete.Enabled = true;
            bAddChannel.Enabled = true;
        }
        else
        {
            lbTip.Text = "选择为：";
            lbSelected.Text = "频道列表！";
            tbExistChannel.Text = "";
            bModifyName.Enabled = false;
            bDelete.Enabled = false;
            bAddChannel.Enabled = true;
        }
    }

    protected void flushPage()
    {
        initTrChannel();
        tbAddChannel.Text = "";
        tbExistChannel.Text = "";
        lbSelected.Text = "空！";
    }

    protected void bModifyName_Click(object sender, EventArgs e)
    {
        if (tbExistChannel.Text.Trim() != "")
        {
            Model.ChannelProgram mChannel = new Model.ChannelProgram();
            mChannel.CP_ID = Convert.ToInt32(trChannel.SelectedNode.Value.Trim());
            mChannel.CP_Name = tbExistChannel.Text.Trim();
            if (sdbll.setCP(mChannel, "update") == 0)
            {
                flushPage();
                Response.Write("<script>alert('修改频道成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('修改频道失败！');</script>");
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
        mChannel.CP_ID = Convert.ToInt32(trChannel.SelectedNode.Value.Trim());
        Model.ChannelProgram[] mProgram;
        try
        {
            mProgram = dbll.getCP(trChannel.SelectedNode.Value.Trim());
        }
        catch
        {
            mProgram = null;
        }
        if (mProgram != null && mProgram.Length == 0)
        {
            if (sdbll.setCP(mChannel, "delete") == 0)
            {
                flushPage();
                Response.Write("<script>alert('删除频道成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('删除频道失败！');</script>");
            }
            
        }
        else
        {
            flushPage();
            Response.Write("<script>alert('不允许删除有栏目的频道！');</script>");
        }
        
    }

    protected void bAddChannel_Click(object sender, EventArgs e)
    {
        if (tbAddChannel.Text.Trim() != "")
        {
            Model.ChannelProgram mChannel = new Model.ChannelProgram();
            mChannel.CP_Name = tbAddChannel.Text.Trim();
            if (sdbll.setCP(mChannel, "insert") == 0)
            {
                flushPage();
                
                Response.Write("<script>alert('增加频道成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('增加频道失败！');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('不允许为空！');</script>");
 
        }
        
    }
}
