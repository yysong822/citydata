using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    //private static Model.Admin mAdmin = new Model.Admin();
    BLL.DataBLL dbll = new BLL.DataBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["UserName"] == null
            || Session["UserRole"] == null)
        {
            plLogin.Visible = true;
            plLoginDone.Visible = false;

            Page.Form.DefaultFocus = tbUserID.ClientID;     // 输入焦点
            Page.Form.DefaultButton = btnLogin.UniqueID;    // 默认按钮
        }
        else
        {
            plLogin.Visible = false;
            plLoginDone.Visible = true;

            string userName = Session["UserName"].ToString();
            lblMessage.Text = userName + " 您已经成功登录！";
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string id = tbUserID.Text.Trim();
        string psw = tbUserPwd.Text.Trim();

        Model.User user = new Model.User();
        user.UserID = id;
        user.Password = psw;

        Model.User[] mur = dbll.getUsers(user);
        if (mur.Length > 0)
        {
            // 判断用户状态
            if (mur[0].UserState == "0")        // 启用：0；禁用：1
            {
                // 保存Session
                Session["UserID"] = mur[0].UserID;
                Session["UserName"] = mur[0].UserName;
                Session["UserRole"] = mur[0].UserRole;      // 管理员：0；制片人：1

                tbUserID.Text = "";
                tbUserPwd.Text = "";
                plLogin.Visible = false;
                plLoginDone.Visible = true;
                lblMessage.Text = "欢迎您：" + mur[0].UserName + " <br/>您已经成功登录！";

                Response.Redirect(Request.Url.ToString());      // 刷新当前页
            }
            else
            {
                Response.Write("<script>alert('用户已被管理员禁用！');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('用户名或密码错误！');</script>");
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();

        Response.Redirect(Request.Url.ToString());      // 刷新当前页
    }

}
