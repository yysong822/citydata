using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null && Session["UserName"].ToString() != "")
        {
            lblLoginUser.Text = "欢迎：<a href=\"pwd.aspx\">" + Session["UserName"].ToString() + "</a>";
            lbtnLoginOper.Text = "退出";
        }
        else
        {
            lbtnLoginOper.Text = "登录";
        }

        loadNaviMenu();         // 按权限加载导航

    }

    private void loadNaviMenu()
    {
        if (Session["UserRole"] != null && Session["UserRole"].ToString() != "")
        {
            naviTab.Style["display"] = "";

            // 制片人：1；管理员：0
            if (Session["UserRole"].ToString() == "1")
            {
                naviTab.Rows[0].Style["display"] = "none";
                naviTab.Rows[1].Style["display"] = "none";
                naviTab.Rows[2].Style["display"] = "none";
            }
            else if (Session["UserRole"].ToString() == "0")
            {
                // 全部显示
                naviTab.Rows[0].Style["display"] = "";
                naviTab.Rows[1].Style["display"] = "";
                naviTab.Rows[2].Style["display"] = "";
            }
        }
        else
        {
            // 全部隐藏
            naviTab.Style["display"] = "none";
        }
    }

    protected void lbtnLoginOper_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] != null && Session["UserID"].ToString() != "")
        {
            // 用户已登录
            Session.Clear();
            Session.Abandon();
        }
        else
        {
            // 用户未登录
        }

        Response.Redirect("login.aspx");
    }

}
