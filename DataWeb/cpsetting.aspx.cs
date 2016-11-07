using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cpsetting : System.Web.UI.Page
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
            return;
        }


        if (!IsPostBack)
        {
            tvCPUsersDataBind();
            ddlChannelDataBind();

            showCtrlTab("CP", false);
            showCtrlTab("User", true);

            lbTips.Text = "请您选择一个操作！";
        }

    }

    private void tvCPUsersDataBind()
    {
        Model.ChannelProgramUser[] mcpus = dbll.getCPUsers();

        if (mcpus.Length == 0)
            return;

        /*
        // 移除数组中的重复元素
        string[] userNameDistArray = Utilities.RemoveDup(userNameArray);
        */

        if (tvCPUsers.Nodes.Count > 0)
        {
            tvCPUsers.Nodes.Clear();
        }


        /*********************************
         * 前提条件：SQL中已对结果进行排序
         * 分别对UserID，ChannelID，ProgramID
         * 相邻字段进行判断，不同内容绑定不同
         * 的节点，相同内容绑定到兄弟节点
         * *******************************/

        // User第一项
        TreeNode rootNode = new TreeNode(mcpus[0].UserName, mcpus[0].UserID);   // text, value
        tvCPUsers.Nodes.Add(rootNode);

        TreeNode childNode = null;

        // bug fix. 如果第一个用户也没有频道设置
        if (mcpus[0].ChannelID != null && mcpus[0].ProgramID != null)
        {
            // Channel第一项
            childNode = new TreeNode(mcpus[0].ChannelName, mcpus[0].ChannelID.ToString());
            rootNode.ChildNodes.Add(childNode);

            // Program第一项
            TreeNode leafNode = new TreeNode(mcpus[0].ProgramName, mcpus[0].ProgramID.ToString());
            childNode.ChildNodes.Add(leafNode);
        }
        else
        {
            childNode = new TreeNode("该用户无频道栏目设置！", "-1");
            childNode.SelectAction = TreeNodeSelectAction.None;  // 节点不可选
            rootNode.ChildNodes.Add(childNode);
        }

        TreeNode userNode = rootNode;
        TreeNode channelNode = childNode;
        
        for (int i = 1; i < mcpus.Length; i++)
        {
            userNode = addUserNode(userNode, mcpus[i - 1], mcpus[i]);
            channelNode = addCPNode(userNode, channelNode, mcpus[i - 1], mcpus[i]);

            channelNode.CollapseAll();      // 隐藏栏目节点显示
        }

        //tvCPUsers.CollapseAll();      // 折叠所有节点

    }


    private TreeNode addUserNode(TreeNode prevNode, Model.ChannelProgramUser prevItem, Model.ChannelProgramUser currItem)
    {
        TreeNode currNode = prevNode;

        if (prevItem.UserID != currItem.UserID)
        {
            currNode = new TreeNode();
            currNode.Value = currItem.UserID;
            currNode.Text = currItem.UserName;
            tvCPUsers.Nodes.Add(currNode);
        }

        return currNode;
    }

    private TreeNode addCPNode(TreeNode parentNode, TreeNode prevNode, Model.ChannelProgramUser prevItem, Model.ChannelProgramUser currItem)
    {
        if (parentNode == null)
            throw new Exception("参数：父节点为空！");

        TreeNode currNode = prevNode;

        if ((prevItem.UserID != currItem.UserID && currItem.ChannelID != null) 
            || (currItem.ChannelID != null && prevItem.ChannelID != currItem.ChannelID))   // bug fix.
        {
            // 1、相邻User不相同，频道设置可能相同，可能不同，也可能为空
            currNode = new TreeNode();
            currNode.Value = currItem.ChannelID.ToString();
            currNode.Text = currItem.ChannelName;
            parentNode.ChildNodes.Add(currNode);
        }

        if (currItem.ProgramID != null)
        {
            // 2、User的频道设置不同，栏目设置一定不同
            TreeNode programNode = new TreeNode();
            programNode.Value = currItem.ProgramID.ToString();
            programNode.Text = currItem.ProgramName;
            currNode.ChildNodes.Add(programNode);
        }

        // 该用户的频道栏目设置为空
        if (currItem.ChannelID == null && parentNode.ChildNodes.Count == 0)     // bug fix.
        {
            TreeNode nullNode = new TreeNode("该用户无频道栏目设置！", "-1");
            nullNode.SelectAction = TreeNodeSelectAction.None;  // 节点不可选
            parentNode.ChildNodes.Add(nullNode);
        }

        return currNode;
    }


    protected void tvCPUsers_SelectedNodeChanged(object sender, EventArgs e)
    {
        lbUsableCP.Items.Clear();
        lbUsedCP.Items.Clear();
        lbMessage.Text = "";

        if (tvCPUsers.SelectedNode != null && tvCPUsers.SelectedNode.Parent == null)
        {
            // 选中的是根节点：用户
            string userID = tvCPUsers.SelectedNode.Value;
            string userName = tvCPUsers.SelectedNode.Text;

            lbTips.Text = "您已选择用户：" + userName;
            lbMessage.Text = "";
            lbMessage2.Text = "";

            // 显示或隐藏
            showCtrlTab("CP", true);
            showCtrlTab("User", false);


            // 加载该用户未设置的频道列表和已设置的频道列表
            lbUsableCPDataBind(userID);
            lbUsedCPDataBind(userID);
        }

        else if (tvCPUsers.SelectedNode != null && tvCPUsers.SelectedNode.Parent != null)
        {
            // 选中的是子节点：频道或栏目
            int cpID = Convert.ToInt32(tvCPUsers.SelectedNode.Value);
            string cpName = tvCPUsers.SelectedNode.Text;

            lbTips.Text = "您已选择频道栏目：" + cpName;
            lbMessage.Text = "";
            lbMessage2.Text = "";

            if (tvCPUsers.SelectedNode.Depth == 1)      // 父节点：频道
            {
                showCtrlTab("User", true);
                showCtrlTab("CP", false);

                // 设置下拉列表项
                int index = ddlChannel.Items.IndexOf(ddlChannel.Items.FindByValue(cpID.ToString()));
                ddlChannel.SelectedIndex = index;

                // 加载该频道未设置的用户列表和已设置的用户列表
                lbUsableUserDataBind(cpID);
                lbUsedUserDataBind(cpID);
            }
            else
            {
                // 叶子节点：栏目
                showCtrlTab("User", false);
                showCtrlTab("CP", false);

            }

        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="area"></param>
    /// <param name="show"></param>
    private void showCtrlTab(string area, bool show)
    {
        if (area == "CP")
        {
            ctrlTab.Rows[7].Style["display"] = show ? "" : "none";       // 说明文字行
            ctrlTab.Rows[8].Style["display"] = show ? "" : "none";       // listbox行
            ctrlTab.Rows[9].Style["display"] = show ? "" : "none";       // button行
        }
        else if (area == "User")
        {
            ctrlTab.Rows[10].Style["display"] = show ? "" : "none";       // dropdownlist行
            ctrlTab.Rows[11].Style["display"] = show ? "" : "none";       // 说明文字行
            ctrlTab.Rows[12].Style["display"] = show ? "" : "none";       // listbox行
            ctrlTab.Rows[13].Style["display"] = show ? "" : "none";       // button行
        }
    }


    private void lbUsableCPDataBind(string userID)
    {
        lbUsableCP.Items.Clear();

        Model.ChannelProgram[] mcp = dbll.getUsableChannelByUserID(userID);
        if (mcp.Length == 0)
        {
            // 该用户已经选择了所有频道
            lbMessage.Text = "该用户已经没有可供选择的频道！";
        }
        else
        {
            for (int i = 0; i < mcp.Length; i++)
            {
                ListItem li = new ListItem();
                li.Value = mcp[i].CP_ID.ToString();
                li.Text = mcp[i].CP_Name.ToString();

                lbUsableCP.Items.Add(li);
            }

            lbMessage.Text += "可选频道数：" + mcp.Length;
        }

    }


    private void lbUsedCPDataBind(string userID)
    {
        lbUsedCP.Items.Clear();

        Model.ChannelProgram[] mcp = dbll.getUsedChannelByUserID(userID);
        if (mcp.Length == 0)
        {
            // 该用户还没有选择频道
            int count = lbUsableCP.Items.Count;
            lbMessage.Text = "共有 " + count + " 个频道可供选择。";
        }
        else
        {
            for (int i = 0; i < mcp.Length; i++)
            {
                ListItem li = new ListItem();
                li.Value = mcp[i].CP_ID.ToString();
                li.Text = mcp[i].CP_Name.ToString();

                lbUsedCP.Items.Add(li);
            }

            lbMessage.Text += " ；已选频道数：" + mcp.Length;
        }

    }


    private void ddlChannelDataBind()
    {
        ddlChannel.Items.Clear();

        Model.ChannelProgram[] mcp = dbll.getCP("0");    // 获取所有栏目
        if (mcp.Length == 0)
            return;

        for (int i = 0; i < mcp.Length; i++)
        {
            ListItem li = new ListItem();
            li.Value = mcp[i].CP_ID.ToString();
            li.Text = mcp[i].CP_Name.ToString();

            ddlChannel.Items.Add(li);
        }

        ListItem nli = new ListItem();
        nli.Value = "-1";
        nli.Text = "--- 请选择频道 ---";
        ddlChannel.Items.Insert(0, nli);

    }


    private void lbUsableUserDataBind(int cpID)
    {
        lbUsableUser.Items.Clear();

        Model.User[] mu = dbll.getUsableUserByCPID(cpID);
        if (mu.Length == 0)
        {
            // 该频道已经选择了所有用户
            lbMessage2.Text = "该频道已经没有可供选择的用户！";
        }
        else
        {
            for (int i = 0; i < mu.Length; i++)
            {
                ListItem li = new ListItem();
                li.Value = mu[i].UserID;
                li.Text = String.Format("{0} ( {1} )", mu[i].UserName, mu[i].UserID);

                lbUsableUser.Items.Add(li);
            }

            lbMessage2.Text += "可选用户数：" + mu.Length;
        }

    }

    private void lbUsedUserDataBind(int cpID)
    {
        lbUsedUser.Items.Clear();

        Model.User[] mu = dbll.getUsedUserByCPID(cpID);
        if (mu.Length == 0)
        {
            // 该频道还没有选择用户
            int count = lbUsableUser.Items.Count;
            lbMessage2.Text = "共有 " + count + " 个用户可供选择。";
        }
        else
        {
            for (int i = 0; i < mu.Length; i++)
            {
                ListItem li = new ListItem();
                li.Value = mu[i].UserID;
                li.Text = String.Format("{0} ( {1} )", mu[i].UserName, mu[i].UserID);

                lbUsedUser.Items.Add(li);
            }

            lbMessage2.Text += " ；已选用户数：" + mu.Length;
        }

    }


    protected void btnUserCPSet_Click(object sender, EventArgs e)
    {
        // 为已选择用户设置频道
        //string uCPlst = Request.Form[hfUserCP.UniqueID].ToString();
        string uCPlst = hfUserCP.Value;
        string[] currUserCPs = uCPlst.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        string userName = tvCPUsers.SelectedNode.Text;
        string userID = tvCPUsers.SelectedValue;
        Model.ChannelProgram[] mcps = dbll.getUsedChannelByUserID(userID);

        string[] prevUserCPs = new string[mcps.Length];
        for (int i = 0; i < mcps.Length; i++)
        {
            prevUserCPs[i] = mcps[i].CP_ID.ToString();
        }

        // 找不同的频道ID
        string[] addArr = findDistinct(prevUserCPs, currUserCPs);   // 
        string[] delArr = findDistinct(currUserCPs, prevUserCPs);   // 

        if (addArr.Length == 0 && delArr.Length == 0)
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('已选频道没有任何改变！');</script>");
            return;
        }


        lbMessage.Text = "";
        lbWarning.Text = "";

        for (int i = 0; i < addArr.Length; i++)
        {
            if (sbll.addCPUser(userID, Convert.ToInt32(addArr[i])))
            {
                lbMessage.Text += String.Format("频道：{0} 用户：{1} 设置成功。<br/>", addArr[i], userName);
            }
            else
            {
                lbWarning.Text += String.Format("频道：{0} 用户：{1} 设置失败！<br/>", addArr[i], userName);
            }
        }

        for (int j = 0; j < delArr.Length; j++)
        {
            if (sbll.delCPUser(userID, Convert.ToInt32(delArr[j])))
            {
                lbMessage.Text += String.Format("频道：{0} 用户：{1} 解除成功。<br/>", delArr[j], userName);
            }
            else
            {
                lbWarning.Text += String.Format("频道：{0} 用户：{1} 解除失败！<br/>", delArr[j], userName);
            }
        }

        // 加载该频道未设置的用户列表和已设置的用户列表
        lbUsableCPDataBind(userID);
        lbUsedCPDataBind(userID);
        tvCPUsersDataBind();

        // 设置TreeView的选择状态
        TreeNode lastNode = null;
        foreach (TreeNode node in tvCPUsers.Nodes)
        {
            lastNode = GetNode(node, userID);
            if (lastNode != null)
                break;
        }

        // 选中并展开子节点
        if (lastNode != null)
        {
            lastNode.Selected = true;
            lastNode.Expand();
        }

    }


    protected void btnCPUserSet_Click(object sender, EventArgs e)
    {
        // 为已选择频道设置用户
        //string cpUlst = Request.Form[hfCPUser.UniqueID].ToString();
        string cpUlst = hfCPUser.Value;
        string[] currCPUsers = cpUlst.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        string cpName = ddlChannel.SelectedItem.Text;
        int cpID = Convert.ToInt32(ddlChannel.SelectedValue);
        Model.User[] mus = dbll.getUsedUserByCPID(cpID);

        string[] prevCPUsers = new string[mus.Length];
        for (int i = 0; i < mus.Length; i++)
        {
            prevCPUsers[i] = mus[i].UserID;
        }

        // 找不同的用户ID
        string[] addArr = findDistinct(prevCPUsers, currCPUsers);   // 
        string[] delArr = findDistinct(currCPUsers, prevCPUsers);   // 

        if (addArr.Length == 0 && delArr.Length == 0)
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('已选用户没有任何改变！');</script>");
            return;
        }


        lbMessage2.Text = "";
        lbWarning.Text = "";

        for (int i = 0; i < addArr.Length; i++)
        {
            if (sbll.addCPUser(addArr[i], cpID))
            {
                lbMessage.Text += String.Format("工号：{0} 频道：{1} 设置成功。<br/>", addArr[i], cpName);
            }
            else
            {
                lbWarning.Text += String.Format("工号：{0} 频道：{1} 设置失败！<br/>", addArr[i], cpName);
            }
        }

        for (int j = 0; j < delArr.Length; j++)
        {
            if (sbll.delCPUser(delArr[j], cpID))
            {
                lbMessage.Text += String.Format("工号：{0} 频道：{1} 解除成功。<br/>", delArr[j], cpName);
            }
            else
            {
                lbWarning.Text += String.Format("工号：{0} 频道：{1} 解除失败！<br/>", delArr[j], cpName);
            }
        }


        // 加载该频道未设置的用户列表和已设置的用户列表
        lbUsableUserDataBind(cpID);
        lbUsedUserDataBind(cpID);
        tvCPUsersDataBind();
        
        // 展开所有用户节点
        foreach (TreeNode node in tvCPUsers.Nodes)
        {
            node.Expand();
        }

        /*
        for (int i = 0; i < cpUsers.Length; i++)
        {
            if (latest.Exists(value => value == cpUsers[i]))    // .NET 3.5编译
            {
                latest.Remove(cpUsers[i]);
            }
        }
        */
        /*
        string[] a1 = { "001", "002", "005", "009", "008" };
        string[] a2 = { "002", "009", "005", "003", "000" };

        string[] a3 = findDistinct(a1, a2);
        string[] a4 = findDistinct(a2, a1);
        */

    }


    /// <summary>
    /// 查找TreeNode及子节点中值是strValue的节点
    /// </summary>
    /// <param name="node">父节点</param>
    /// <param name="strValue">字符串</param>
    /// <returns>当前节点</returns>
    private TreeNode GetNode(TreeNode node, string strValue)
    {
        //if (node.Text == strValue)
        if (node.Value == strValue)
        {
            return node;
        }

        TreeNode targetNode = null;

        foreach (TreeNode subNode in node.ChildNodes)
        {
            targetNode = GetNode(subNode, strValue);
            if (targetNode != null)
            {
                break;
            }
        }
        return targetNode;
    }


    /// <summary>
    /// 在两个字符串数组中找到不同的字符串
    /// </summary>
    /// <param name="refe">参照字符串数组</param>
    /// <param name="comp">对比字符串数组</param>
    /// <returns>不同的字符串数组</returns>
    private static string[] findDistinct(string[] refe, string[] comp)
    {
        string[] retArr = null;

        if (refe == null || refe.Length == 0)
            retArr = comp;

        if (comp == null || comp.Length == 0)
            retArr = new string[0];

        List<string> retlst = new List<string>();

        for (int i = 0; i < comp.Length; i++)
        {
            int idx = -1;
            while (++idx < refe.Length)
            {
                if (comp[i] == refe[idx])
                    break;
            }

            if (idx == refe.Length)  // 参照中没有找到相同
            {
                retlst.Add(comp[i]);
            }
        }

        retArr = retlst.ToArray();

        return retArr;
    }


    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 加载该频道已设置和未设置的用户列表
        int cpID = Convert.ToInt32(ddlChannel.SelectedValue);
        string cpName = ddlChannel.SelectedItem.Text;

        lbMessage2.Text = "";

        if (ddlChannel.SelectedValue == "-1")
        {
            lbTips.Text = "请您选择一个操作！";

            lbUsedUser.Items.Clear();
            lbUsableUser.Items.Clear();
        }
        else
        {
            lbTips.Text = "您已选择频道栏目：" + cpName;

            // 加载该频道未设置的用户列表和已设置的用户列表
            lbUsableUserDataBind(cpID);
            lbUsedUserDataBind(cpID);
        }

    }

}
