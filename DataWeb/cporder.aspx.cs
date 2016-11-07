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

public partial class cporder : System.Web.UI.Page
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
            initlboxChannel();
        }
    }


    protected void initlboxChannel()
    {
        lboxChannel.Items.Clear();
        mChannelInfo = dbll.getCP("0");
        for (int i = 0; i < mChannelInfo.Length; i++)
        {
            ListItem liChannel = new ListItem();
            liChannel.Value = mChannelInfo[i].CP_ID.ToString();
            liChannel.Text = mChannelInfo[i].CP_Name;
            liChannel.Attributes.Add("title", liChannel.Text);
            lboxChannel.Items.Add(liChannel);
        }
    }

    protected void lboxChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        lboxProgram.Items.Clear();
        string channelID = lboxChannel.SelectedItem.Value;
        Model.ChannelProgram[] mProgramInfo = dbll.getCP(channelID);
        for (int i = 0; i < mProgramInfo.Length; i++)
        {
            ListItem liProgram = new ListItem();
            liProgram.Value = mProgramInfo[i].CP_ID.ToString();
            liProgram.Text = mProgramInfo[i].CP_Name;
            liProgram.Attributes.Add("title", liProgram.Text);
            lboxProgram.Items.Add(liProgram);
        }
    }

    
    protected void bOrder_Click(object sender, EventArgs e)
    {

    }

    protected void bPUp_Click(object sender, EventArgs e)
    {
        int index = -1;
        moveDisplayItem(index, lboxProgram);
    }

    protected void bPDown_Click(object sender, EventArgs e)
    {
        int index = 1;
        moveDisplayItem(index, lboxProgram);
    }

    //改变listbox中的item的显示位置---内容显示设置
    public void moveDisplayItem(int index, ListBox lboxSelected)
    {
        if (lboxSelected.SelectedIndex != -1)
        {
            if ((lboxSelected.SelectedIndex != 0 && index < 0) || (lboxSelected.SelectedIndex != (lboxSelected.Items.Count - 1) && index > 0))
            {
                //将当前条目的文本以及值都保存到一个临时变量里面
                ListItem lt = new ListItem(lboxSelected.SelectedItem.Text, lboxSelected.SelectedValue);

                for (int i = 0; i < Math.Abs(index); i++)
                {
                    if (index > 0)
                    {
                        //依次替换上一条的值
                        lboxSelected.Items[lboxSelected.SelectedIndex + i].Text = lboxSelected.Items[lboxSelected.SelectedIndex + (i + 1)].Text;
                        lboxSelected.Items[lboxSelected.SelectedIndex + i].Value = lboxSelected.Items[lboxSelected.SelectedIndex + (i + 1)].Value;
                    }
                    else
                    {
                        //依次替换下一条的值
                        lboxSelected.Items[lboxSelected.SelectedIndex - i].Text = lboxSelected.Items[lboxSelected.SelectedIndex - (i + 1)].Text;
                        lboxSelected.Items[lboxSelected.SelectedIndex - i].Value = lboxSelected.Items[lboxSelected.SelectedIndex - (i + 1)].Value;
                    }

                }

                //把被选中项的前一条或下一条的值用临时变量中的取代
                lboxSelected.Items[lboxSelected.SelectedIndex + index].Text = lt.Text;

                //把被选中项的前一条或下一条的值用临时变量中的取代
                lboxSelected.Items[lboxSelected.SelectedIndex + index].Value = lt.Value;

                //把鼠标指针放到移动后的那项上
                lboxSelected.SelectedIndex = lboxSelected.SelectedIndex + index;
            }
        }
    }

    protected void bCUp_Click(object sender, EventArgs e)
    {
        int index = -1;
        moveDisplayItem(index, lboxChannel);
    }

    protected void bCDown_Click(object sender, EventArgs e)
    {
        int index = 1;
        moveDisplayItem(index, lboxChannel);
    }

    protected void bCOreder_Click(object sender, EventArgs e)
    {
        int lengthChannel = lboxChannel.Items.Count;
        int sucCount = 0;
        if (lengthChannel > 0)
        {
            for (int i = 0; i < lengthChannel; i++)
            {
                Model.ChannelProgram mcoSingle = new Model.ChannelProgram();
                mcoSingle.CP_ID = Convert.ToInt32(lboxChannel.Items[i].Value);
                mcoSingle.CP_Order = i;
                if (sdbll.setCPnewOrder(mcoSingle) == 0)
                {
                    sucCount++;
                }
                else
                {
                    Response.Write("<script>alert('设置频道顺序失败！');</script>");
                }
            }
            if (sucCount == lengthChannel)
            {
                Response.Write("<script>alert('设置频道顺序成功！');</script>");
            }
        }
    }

    protected void bPOrder_Click(object sender, EventArgs e)
    {
        int lengthProgram = lboxProgram.Items.Count;
        int sucCount = 0;
        if (lengthProgram > 0)
        {
            for (int i = 0; i < lengthProgram; i++)
            {
                Model.ChannelProgram mpoSingle = new Model.ChannelProgram();
                mpoSingle.CP_ID = Convert.ToInt32(lboxProgram.Items[i].Value);
                mpoSingle.CP_Order = i;
                if (sdbll.setCPnewOrder(mpoSingle) == 0)
                {
                    sucCount++;
                }
                else
                {
                    Response.Write("<script>alert('设置栏目顺序失败！');</script>");
                }
            }
            if (sucCount == lengthProgram)
            {
                Response.Write("<script>alert('设置栏目顺序成功！');</script>");
 
            }
           
        }
    }
}
