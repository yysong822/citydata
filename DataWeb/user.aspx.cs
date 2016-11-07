using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user : System.Web.UI.Page
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

        // 判断是否具有管理员权限
        if (Session["UserRole"] != null && Session["UserRole"].ToString() != "0")
        {
            // 管理员角色：0
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您没有管理员权限！');window.location.href='login.aspx';</script>");
            //Session.Clear();
            return;
        }
        
        
        if (!IsPostBack)    // 页面第一次加载
        {
            fillAllUsers();

            plView.Visible = true;
            plAdd.Visible = false;
            lblMessage.Text = "";

            // 是否接受查询参数id={0}
            /*
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
            {
                findAddedUser(Request.QueryString["id"].ToString());
            }
            */

        }


    }


    private void fillAllUsers()
    {
        // 查询所有用户信息
        gvUsers.DataSource = dbll.getUsers(null);
        gvUsers.DataBind();
    }

    private void findAddedUser(string userID)
    {
        // 找到该项在GridView中的位置
        Model.User[] murs = dbll.getUsers(null);
        if (murs.Length == 0)   // 无记录
            return;

        int idx = 0;
        for (int i = 0; i < murs.Length; i++)
        {
            // 遍历主键，查找符合已添加主键的序号
            //if (gvUsers.DataKeys[i].Value.ToString() == userID)
            if (murs[i].UserID == userID)
                break;
            else
                idx++;
        }

        int pageSize = gvUsers.PageSize;
        int pageIdx = idx / pageSize;
        //int rowIdx = idx % pageSize == 0 ? idx : idx % pageSize;
        int rowIdx = idx % pageSize;

        gvUsers.PageIndex = pageIdx;    // 设置页数后必须重新绑定数据才能正确显示

        // 重新绑定用户数据
        gvUsers.DataSource = murs;
        gvUsers.DataBind();

        gvUsers.Rows[rowIdx].BackColor = System.Drawing.Color.FromName("#B9CDFB");  // 必须放在绑定数据后，否则无显示

    }


    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string userID = tbUserID_Query.Text.Trim();
        string userName = tbUserName_Query.Text.Trim();
        string userRole = ddlUserRole_Query.SelectedValue;
        string userState = ddlUserState_Query.SelectedValue;

        Model.User mur = new Model.User();
        mur.UserID =  userID != String.Empty ? userID : null;
        mur.UserName = userName != String.Empty ? userName : null;
        mur.UserRole = userRole != "-1" ? userRole : null;
        mur.UserState = userState != "-1" ? userState : null;

        Model.User[] murs = dbll.getUsers(mur);
        if (murs.Length > 0)
        {
            lblMessage.Text = "";
        }
        else
        {
            lblMessage.Text = "没有查询到任何符合条件的用户！";
        }

        gvUsers.DataSource = murs;
        gvUsers.DataBind();

    }


    protected void btnAddOper_Click(object sender, EventArgs e)
    {
        Model.User mur = new Model.User();
        mur.UserID =  tbUserID_Add.Text.Trim();
        mur.Password = "000000";    // 初始密码
        mur.UserName = tbUserName_Add.Text.Trim();
        mur.UserRole = rblUserRole_Add.SelectedValue;
        mur.UserState = rblUserState_Add.SelectedValue;
        mur.UserComment = tbUserComm_Add.Text;

        // 判断用户是否已存在
        if (dbll.isUserExist(mur.UserID))
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 已存在，请检查后重新输入！');</script>", mur.UserName));
            return;
        }

        if (sbll.addUser(mur))
        {
            //Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 添加成功！');window.location.href='user.aspx?id={1}';</script>", mur.UserName, mur.UserID));
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 添加成功！');</script>", mur.UserName));

            btnAddCancel_Click(sender, e);  // 返回到用户列表

            findAddedUser(mur.UserID);      // 定位最新添加的用户

        }
        else
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 添加失败！');</script>", mur.UserName));
        }

    }


    protected void lbtnShowAdd_Click(object sender, EventArgs e)
    {
        plView.Visible = false;
        plAdd.Visible = true;
        lblMessage.Text = "";
    }

    protected void btnAddCancel_Click(object sender, EventArgs e)
    {
        plView.Visible = true;
        plAdd.Visible = false;
        lblMessage.Text = "";

        // 清空已添加的用户信息
        tbUserID_Add.Text = "";
        tbUserName_Add.Text = "";
        rblUserRole_Add.SelectedIndex = 0;
        rblUserState_Add.SelectedIndex = 0;
        tbUserComm_Add.Text = "";

    }

    protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string userID = gvUsers.DataKeys[e.RowIndex].Value.ToString();
        //string userName = gvUsers.Rows[e.RowIndex].Cells[2].Text.Trim();
        string userName = ((Label)gvUsers.Rows[e.RowIndex].Cells[2].FindControl("lbUserName_Item")).Text;

        if (sbll.deleteUser(userID))
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 删除成功！');</script>", userName));
        }
        else
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 删除失败！');</script>", userName));
        }

        btnQuery_Click(sender, e);
    }

    protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        gvUsers.EditIndex = e.NewEditIndex;

        // 隐藏密码操作列
        gvUsers.Columns[7].Visible = false;

        btnQuery_Click(sender, e);
    }

    protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        e.Cancel = true;
        gvUsers.EditIndex = -1;

        // 恢复密码操作列
        gvUsers.Columns[7].Visible = true;

        btnQuery_Click(sender, e);
    }

    protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //e.Cancel = true;

        Model.User mur = new Model.User();
        mur.UserID = gvUsers.DataKeys[e.RowIndex].Value.ToString();
        mur.UserName = ((TextBox)gvUsers.Rows[e.RowIndex].Cells[2].FindControl("tbUserName_Edit")).Text;
        mur.UserRole = ((DropDownList)gvUsers.Rows[e.RowIndex].Cells[4].FindControl("ddlUserRole_Edit")).SelectedValue;
        mur.UserState = ((DropDownList)gvUsers.Rows[e.RowIndex].Cells[5].FindControl("ddlUserState_Edit")).SelectedValue;
        //mur.UserComment = // 暂无

        if (sbll.updateUser(mur))
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 更新成功！');</script>", mur.UserName));
        }
        else
        {
            Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 更新失败！');</script>", mur.UserName));
        }

        gvUsers.EditIndex = -1;

        // 恢复密码操作列
        gvUsers.Columns[7].Visible = true;

        btnQuery_Click(sender, e);
    }

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Reset")   // 重置该用户密码
        {
            string userID = e.CommandArgument.ToString();

            GridViewRow gvr = ((GridViewRow)(((LinkButton)(e.CommandSource)).Parent.Parent));
            string userName = (((Label)gvUsers.Rows[gvr.RowIndex].Cells[2].FindControl("lbUserName_Item")).Text);

            Model.User mu = new Model.User();
            mu.UserID = userID;
            mu.Password = "000000";     // 默认密码

            if (sbll.updatePwd(mu))
            {
                Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 重置成功！');</script>", userName));
            }
            else
            {
                Response.Write(String.Format("<script language=\"javascript\" type=\"text/javascript\">alert('用户：{0} 重置失败！');</script>", userName));
            }

        }

    }

    protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUsers.PageIndex = e.NewPageIndex;

        btnQuery_Click(sender, e);
    }

    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标停留时更改背景色
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#E2DED6'");
            //当鼠标移开时还原背景色
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

            if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
            {
                /* 对于模版列，Controls[1]才是用户定义的第一个控件
                 * 对于普通列，可直接用Cells[索引].Text取到单元格内容
                 */ 
                // 绑定删除用户提示内容
                ((LinkButton)e.Row.Cells[8].Controls[0]).Attributes.Add("onclick",
                    "javascript:return confirm('您确定要删除用户：" + ((Label)e.Row.Cells[2].Controls[1]).Text + " 吗？警告：该用户所有的频道配置都将被删除！')");

                // 绑定重置密码提示内容
                ((LinkButton)e.Row.Cells[7].Controls[1]).Attributes.Add("onclick", 
                    "javascript:return confirm('您确定要重置：" + ((Label)e.Row.Cells[2].Controls[1]).Text + " 的密码吗？')");

                // 状态为禁用的用户显示为灰色
                if (((Label)e.Row.Cells[5].Controls[1]).Text == "禁用")
                    //e.Row.ForeColor = System.Drawing.Color.Gray;
                    e.Row.Attributes.Add("style", "color:gray");
            }
        }

    }




}
