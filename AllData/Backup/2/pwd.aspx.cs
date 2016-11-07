using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pwd : System.Web.UI.Page
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
            // do nothing.
        }
    }

    protected void btnModPwd_Click(object sender, EventArgs e)
    {
        string id = Session["UserID"].ToString();

        Model.User user = new Model.User();
        user.UserID = id;
        user.Password = tbOldPwd.Text;

        Model.User[] mur = dbll.getUsers(user);
        if (mur.Length > 0)
        {
            Model.User mu = new Model.User();
            mu.UserID = id;
            mu.Password = tbNewPwd.Text;

            if (sbll.updatePwd(mu))
            {
                Response.Write("<script>alert('密码修改成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('密码修改失败！');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('旧密码不正确！');</script>");
        }

        tbOldPwd.Text = "";
        tbNewPwd.Text = "";
        tbCfmPwd.Text = "";
    }
}
