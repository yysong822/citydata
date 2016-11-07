using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;

public partial class setting : System.Web.UI.Page
{
    BLL.DataBLL dbll = new BLL.DataBLL();
    BLL.SettingBLL sdbll = new BLL.SettingBLL();

    private static Model.ChannelProgram[] mChannelInfo;
    private static Model.ChannelProgram[] mProgramInfo;
    private static Model.RecordeData[] mDataType;
    private static Model.RecordeData[] mTimeType;
    private static Model.ElementRelation[] mAllElement;
    private static Model.ElementRelation[] mSelectedElement;
    private static Model.StationRelation[] mAllStation;
    private static Model.StationRelation[] mSelectedStation;

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
            // Added by Edward Chan.
            // 如果登陆用户是管理员，显示所有用户的频道配置
            // 如果登陆用户是制片人，仅显示当前用户的频道配置
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "0")
            {
                // 管理员角色：0
                initTrProgram();
            }
            else if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
            {
                // 制片人角色：1
                initTrProgramByProducer(Session["UserID"].ToString());
            }
            else
            {
                Session.Clear();
                Session.Abandon();

                //Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('没有对应的用户权限！');window.location.href='login.aspx';</script>");
            }

            initDDLDataType();
            initlboxStationAll();
            bSet.Enabled = false;
        }
        
        lbShowError.Text = "";
    }


    #region 管理员

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
            nodeChannel.Value = mChannelInfo[i].CP_ID.ToString() ;
            nodeChannel.Text = mChannelInfo[i].CP_Name;
            trProgram.Nodes.Add(nodeChannel);
            addChileNode(nodeChannel);
        }
        trProgram.CollapseAll();
    }

    protected void addChileNode(TreeNode nodeChannel)
    {
        string channelValue = nodeChannel.Value;
        string channelID = channelValue.Split('#')[0];
        mProgramInfo = dbll.getCP(channelID);
        for (int i = 0; i < mProgramInfo.Length; i++)
        {
            TreeNode nodeProgram = new TreeNode();
            nodeProgram.Value = mProgramInfo[i].CP_ID.ToString() + "#" + mProgramInfo[i].TimeTypeID.ToString()+ "#" + mProgramInfo[i].DataTypeID.ToString();
            nodeProgram.Text = mProgramInfo[i].CP_Name;
            nodeChannel.ChildNodes.Add(nodeProgram);
        }
    }

    #endregion


    #region 制片人

    private void initTrProgramByProducer(string userID)
    {
        if (trProgram.Nodes.Count > 0)
        {
            trProgram.Nodes.Clear();
        }

        Model.ChannelProgramUser[] mcpus = dbll.getCPUsersByUserID(userID);

        if (mcpus.Length == 0)      // 该用户没有指定任何频道
        {
            lblMessage.Text = "没有可管理的频道栏目！<br/>请联系管理员！";
        }
        else
        {
            // Channel第一项
            TreeNode rootNode = new TreeNode(mcpus[0].ChannelName, mcpus[0].ChannelID.ToString());   // text, value
            trProgram.Nodes.Add(rootNode);

            // Program第一项
            rootNode.ChildNodes.Add(new TreeNode(mcpus[0].ProgramName, mcpus[0].ProgramID.ToString() 
                + "#" + mcpus[0].TimeTypeID.ToString() + "#" + mcpus[0].DataTypeID.ToString()));

            TreeNode leafNode = rootNode;
            for (int i = 1; i < mcpus.Length; i++)
            {
                leafNode = addCPNode(leafNode, mcpus[i - 1], mcpus[i]);
            }

            //trProgram.CollapseAll();

        }

    }


    private TreeNode addCPNode(TreeNode prevNode, Model.ChannelProgramUser prevItem, Model.ChannelProgramUser currItem)
    {
        TreeNode currNode = prevNode;

        if (currItem.ChannelID != null && prevItem.ChannelID != currItem.ChannelID)
        {
            currNode = new TreeNode();
            currNode.Value = currItem.ChannelID.ToString();
            currNode.Text = currItem.ChannelName;
            trProgram.Nodes.Add(currNode);
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


    #endregion


    #region 数据绑定

    protected void initDDLDataType()
    {
        ddlDataType.Items.Clear();
        ListItem li = new ListItem();
        li.Value = "选择数据类型";
        li.Text = "选择数据类型";
        ddlDataType.Items.Add(li);
        mDataType = dbll.getDataType(null);
        for (int i = 0; i < mDataType.Length; i++)
        {
            ListItem liDataType = new ListItem();
            liDataType.Value = mDataType[i].DataType.ToString();
            liDataType.Text = mDataType[i].DataTypeName;
            ddlDataType.Items.Add(liDataType);
        }
    }


    protected void initDDLTimeType(Model.RecordeData mrd)
    {
        ddlTimeType.Items.Clear();
        ListItem li = new ListItem();
        li.Value = "选择数据时段";
        li.Text = "选择数据时段";
        ddlTimeType.Items.Add(li);
        if (ddlDataType.SelectedValue == "选择数据类型")
        {
            return;
        }
        mTimeType = dbll.getTimeType(mrd);
        for (int i = 0; i < mTimeType.Length; i++)
        {
            ListItem liTimeType = new ListItem();
            liTimeType.Value = mTimeType[i].TimeType.ToString();
            liTimeType.Text = mTimeType[i].TimeTypeName;
            ddlTimeType.Items.Add(liTimeType);
        }
    }

    protected void initlboxDisplayAll(Model.ElementRelation mer)
    {
        lboxDisplayAll.Items.Clear();
        mAllElement = dbll.getAllElement(mer);
        for (int i = 0; i < mAllElement.Length; i++)
        {
            ListItem liAllElement = new ListItem();
            liAllElement.Value = mAllElement[i].ElementID.ToString();
            liAllElement.Text = mAllElement[i].ElementNameCN;
            lboxDisplayAll.Items.Add(liAllElement);
        }
    }

    protected void initlboxDisplaySelected(string programValue)
    {
        lboxDisplaySelected.Items.Clear();
        string programID = programValue.Split('#')[0];
        mSelectedElement = dbll.getElementsName(programID);
        if (mSelectedElement.Length != 0)
        {
            for (int i = 0; i < mSelectedElement.Length; i++)
            {
                ListItem liSelectedElement = new ListItem();
                liSelectedElement.Value = mSelectedElement[i].ElementID.ToString();
                liSelectedElement.Text = mSelectedElement[i].ElementNameCN;
                lboxDisplaySelected.Items.Add(liSelectedElement);
            }
        } 
    }


    protected void initlboxStationAll()
    {
        lboxStationAll.Items.Clear();
        if (mAllStation != null)
        {
            mAllStation = null;
        }
        mAllStation = dbll.getStationsInfoOrderByName();
        for (int i = 0; i < mAllStation.Length; i++)
        {
            if (mAllStation[i].StationID == 0)
            {
                continue;
            }
            ListItem liAllStation = new ListItem();
            liAllStation.Value = mAllStation[i].StationID.ToString() + "#" + mAllStation[i].StationTableID.ToString();
            liAllStation.Text = mAllStation[i].StationName + "（" +mAllStation[i].StationID.ToString() + "）";
            lboxStationAll.Items.Add(liAllStation);
        }
    }


    protected void initlboxStationAllByID()
    {
        lboxStationAll.Items.Clear();
        if (mAllStation != null)
        {
            mAllStation = null;
        }
        mAllStation = dbll.getStationsInfo();
        for (int i = 0; i < mAllStation.Length; i++)
        {
            if (mAllStation[i].StationID == 0)
            {
                continue;
            }
            ListItem liAllStation = new ListItem();
            liAllStation.Value = mAllStation[i].StationID.ToString() + "#" + mAllStation[i].StationTableID.ToString();
            liAllStation.Text = mAllStation[i].StationName + "（" + mAllStation[i].StationID.ToString() + "）";
            lboxStationAll.Items.Add(liAllStation);
        }
    }

    protected void initlboxStationReal(string programID)
    {
        lboxStationReal.Items.Clear();
        mSelectedStation = dbll.getAllStationsModel(programID);
        if (mSelectedStation.Length != 0)
        {
            for (int i = 0; i < mSelectedStation.Length; i++)
            {
                ListItem liSeStation = new ListItem();
                liSeStation.Value = mSelectedStation[i].StationID.ToString() + "#" + mSelectedStation[i].StationTableID.ToString();
                liSeStation.Text = mSelectedStation[i].StationName + "（" + mSelectedStation[i].StationID.ToString() + "）";
                lboxStationReal.Items.Add(liSeStation);
            }
        }
    }

    #endregion


    protected void initddlStep1(int selectedIndex)
    {
        ddlStep1.Items.Clear();
        ListItem li = new ListItem();
        li.Value = "上移格数";
        li.Text = "上移格数";
        ddlStep1.Items.Add(li);
        if (lboxStationReal.SelectedIndex != 0)
        {
            for (int i = 0; i < lboxStationReal.SelectedIndex; i++)
            {
                ListItem liSetp = new ListItem();
                liSetp.Value = (i + 1).ToString();
                liSetp.Text = (i + 1).ToString();
                ddlStep1.Items.Add(liSetp);
            }
        }
    }

    protected void initddlStep2(int selectedIndex)
    {
        ddlStep2.Items.Clear();
        ListItem li = new ListItem();
        li.Value = "下移格数";
        li.Text = "下移格数";
        ddlStep2.Items.Add(li);
        if (lboxStationReal.SelectedIndex != lboxStationReal.Items.Count)
        {
            for (int i = 0; i < lboxStationReal.Items.Count - lboxStationReal.SelectedIndex - 1; i++)
            {
                ListItem liSetp = new ListItem();
                liSetp.Value = (i + 1).ToString();
                liSetp.Text = (i + 1).ToString();
                ddlStep2.Items.Add(liSetp);
            }
        }
    }

    protected void ddlDataType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string dataTypeID = ddlDataType.SelectedValue;
        if (dataTypeID == "选择数据类型")
        {
            lboxDisplayAll.Items.Clear();
            return;
        }
        else
        {
            Model.RecordeData dataType = new Model.RecordeData();
            Model.ElementRelation eleDataType = new Model.ElementRelation();
            dataType.DataType = Convert.ToInt32(dataTypeID);
            eleDataType.DataTypeID = Convert.ToInt32(dataTypeID);
            initDDLTimeType(dataType);
            initlboxDisplayAll(eleDataType);
            lboxDisplaySelected.Items.Clear();
        }
    }

    protected void ddlTimeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void trProgram_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (trProgram.SelectedNode != null && trProgram.SelectedNode.Parent != null)
        {
            string oldName = trProgram.SelectedNode.Text.Trim();
            string oldNameID = trProgram.SelectedNode.Value.Trim();
            string[] temp = oldNameID.Split('#');
            string typeID = temp[1];
            string programID = temp[0];
            lbTip.Text = "已选栏目：";
            lbSelected.Text = oldName;
            initlboxDisplaySelected(programID);
            initlboxStationReal(programID);
            if (temp[2] != "-1")
            {
                ddlDataType.SelectedValue = temp[2];
                Model.RecordeData mrd = new Model.RecordeData();
                mrd.DataType = Convert.ToInt32(temp[2]);
                initDDLTimeType(mrd);
                ddlTimeType.SelectedValue = typeID;
                Model.ElementRelation eleDataType = new Model.ElementRelation();
                eleDataType.DataTypeID = Convert.ToInt32(temp[2]);
                initlboxDisplayAll(eleDataType);
                lbShowError.Text = "";
                lbSelectedCity.Text = "已选城市列表：共" + lboxStationReal.Items.Count + "个城市！";
            }
            else
            {

            }
            bSet.Enabled = true;
        }
        else
        {
            if (trProgram.SelectedNode != null && trProgram.SelectedNode.Parent == null)
            {
                trProgram.CollapseAll();
                trProgram.SelectedNode.ExpandAll();
                lbTip.Text = "已选频道：";
                lbSelected.Text = trProgram.SelectedNode.Text.Trim();
                ddlTimeType.SelectedValue = "选择数据时段";
                ddlDataType.SelectedValue = "选择数据类型";
                lboxDisplaySelected.Items.Clear();
                lboxStationReal.Items.Clear();
                lbShowError.Text = "请正确选择需要操作的*栏目*";
                bSet.Enabled = false;
            }
        }
        
    }


    protected void btDelFromDisplaySelected_Click(object sender, EventArgs e)
    {
        if (lboxDisplaySelected.Items.Count > 0)
        {
            lboxDisplaySelected.Items.Remove(lboxDisplaySelected.SelectedItem);
        }
    }


    protected void btAddDisplaySelected_Click(object sender, EventArgs e)
    {
        if (trProgram.SelectedNode != null && trProgram.SelectedNode.Parent != null)    /*二级节点*/
        {
            if (lboxDisplayAll.SelectedItem != null)
            {

                bool hasIt = true;
                foreach (ListItem li in lboxDisplaySelected.Items)
                {
                    if (li.Value == lboxDisplayAll.SelectedItem.Value)//表示某一项被选中了
                    {
                        hasIt = false;
                        lbShowError.Text = "* 已经选择了该项目";
                        lbShowError.Visible = true;
                        break;
                    }
                }
                if (hasIt)
                {
                    lboxDisplaySelected.Items.Add(new ListItem(lboxDisplayAll.SelectedItem.Text, lboxDisplayAll.SelectedItem.Value));
                }
            }
            else
            {
                lbShowError.Text = "* 请选择需要操作的*项目*";
                lbShowError.Visible = true;
            }
        }
        else
        {
            lbShowError.Text = "* 请正确选择需要操作的*栏目*";
            lbShowError.Visible = true;
        }
    }


    protected void btAddAllElement_Click(object sender, EventArgs e)
    {
        if (trProgram.SelectedNode != null && trProgram.SelectedNode.Parent != null)    /*二级节点*/
        {
            lboxDisplaySelected.Items.Clear();

            for (int i = 0; i < lboxDisplayAll.Items.Count; i++)
            {
                lboxDisplaySelected.Items.Add(lboxDisplayAll.Items[i]);
            }
        }
        else
        {
            lbShowError.Text = "* 请正确选择需要操作的*栏目*";
            lbShowError.Visible = true;
        }
    }


    protected void btDisplaySelectedUp_Click(object sender, EventArgs e)
    {
        moveDisplayItem(-1,lboxDisplaySelected);
    }


    protected void btDisplaySelectedDown_Click(object sender, EventArgs e)
    {
        moveDisplayItem(1, lboxDisplaySelected);
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

                initddlStep1(lboxSelected.SelectedIndex);
                initddlStep2(lboxSelected.SelectedIndex);
            }
        }
    }

    protected void btAdd2stationReal_Click(object sender, EventArgs e)
    {
        if (trProgram.SelectedNode != null && trProgram.SelectedNode.Parent != null)    /*二级节点*/
        {
            if (lboxStationAll.SelectedItem != null)
            {
                lboxStationReal.Items.Add(new ListItem(lboxStationAll.SelectedItem.Text, lboxStationAll.SelectedItem.Value));
                lbSelectedCity.Text = "已选城市列表：共" + lboxStationReal.Items.Count + "个城市！";
            }
            else
            {
                lbShowError.Text = "* 请选择需要操作的*站点*";
                lbShowError.Visible = true;
            }
        }
        else
        {
            lbShowError.Text = "* 请正确选择需要操作的*栏目*";
            lbShowError.Visible = true;
        }
    }


    protected void btDelStationReal_Click(object sender, EventArgs e)
    {
        if (lboxStationReal.Items.Count > 0)
        {
            lboxStationReal.Items.Remove(lboxStationReal.SelectedItem);
            lbSelectedCity.Text = "已选城市列表：共" + lboxStationReal.Items.Count + "个城市！";
        }
    }
    

    protected void bSet_Click(object sender, EventArgs e)
    {
        int elementCount = lboxDisplaySelected.Items.Count;
        int stationCount = lboxStationReal.Items.Count;
        if (trProgram.SelectedNode.Parent != null && elementCount != 0 && stationCount != 0 && ddlTimeType.SelectedValue != "选择数据时段" && ddlDataType.SelectedValue != "选择数据类型")
        {
            string[] cpInfo = trProgram.SelectedNode.Value.Split('#');
            Model.ChannelProgram mcp = new Model.ChannelProgram();
            mcp.CP_ID = Convert.ToInt32(cpInfo[0]);
            mcp.DataTypeID = Convert.ToInt32(ddlDataType.SelectedValue);
            mcp.TimeTypeID = Convert.ToInt32(ddlTimeType.SelectedValue);
            trProgram.SelectedNode.Value = cpInfo[0] + "#" + ddlTimeType.SelectedValue + "#" + ddlDataType.SelectedValue;
            Model.ElementRelation[] mer = new Model.ElementRelation[elementCount];
            Model.StationRelation[] msr = new Model.StationRelation[stationCount];
            for (int i = 0; i < elementCount; i++)
            {
                Model.ElementRelation merSingle = new Model.ElementRelation();
                merSingle.ElementID = Convert.ToInt32(lboxDisplaySelected.Items[i].Value);
                merSingle.ElementOrder = i;
                mer[i] = merSingle;
            }
            for (int i = 0; i < stationCount; i++)
            {
                Model.StationRelation msrSingle = new Model.StationRelation();
                string[] stationInfo = lboxStationReal.Items[i].Value.Split('#');
                msrSingle.StationID = Convert.ToInt32(stationInfo[0]);
                msrSingle.SelectID = Convert.ToInt32(stationInfo[1]);
                msrSingle.StationOrder = i;
                msr[i] = msrSingle;
            }
            string[] setStationResult = sdbll.setAllSetting(mcp, mer, msr);
            int resultCount = setStationResult.Length;
            int doneCount = 0;
            for(int i = 0; i< resultCount; i++)
            {
                if(setStationResult[i] == "done")
                {
                    doneCount ++;
                }
            }
            if(doneCount == resultCount)
            {
                Response.Write("<script>alert('设置栏目成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('设置栏目失败！');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('请检查信息是否填充完整！');</script>");
        }  
    }


    protected void btFromStaFile_Click(object sender, EventArgs e)
    {
        if (trProgram.SelectedNode != null && trProgram.SelectedNode.Parent != null)    /*二级节点*/
        {
            string path = Server.MapPath("~/Temp/");
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            string stLine;

            lboxStationReal.Items.Clear();

            if (fuSelectStaFile.HasFile)
            {
                string fileExt = System.IO.Path.GetExtension(fuSelectStaFile.FileName);
                if (fileExt == ".sta")
                {
                    string dt = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                            + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                    string filename = dt + fileExt;
                    try
                    {
                        fuSelectStaFile.SaveAs(path + filename);
                    }
                    catch (Exception ee)
                    {
                        lb_error.Text = ee.Message + " The file upload Default";
                    }
                    try
                    {
                        StreamReader srReader = new StreamReader(path + filename, System.Text.Encoding.GetEncoding("gb2312"));
                        stLine = srReader.ReadLine();
                        while (stLine != null)
                        {
                            if (stLine != "")
                            {
                                Regex regex = new Regex("\\s+");
                                stLine = regex.Replace(stLine, "#");

                                string[] newString = stLine.Trim().Split('#');
                                int stationID = Convert.ToInt32(newString[0].Trim());
                                string stationName = newString[1].Trim();
                                Model.StationRelation msr = new Model.StationRelation();
                                msr.StationName = stationName;
                                msr.StationID = stationID;
                                Model.StationRelation searchStationResult = dbll.searchStationInfo2(msr);
                                if (searchStationResult != null)
                                {
                                    ListItem stationImport = new ListItem();
                                    stationImport.Value = searchStationResult.StationID.ToString() + "#" + searchStationResult.StationTableID.ToString();
                                    stationImport.Text = searchStationResult.StationName + "（" + searchStationResult.StationID.ToString() + "）";
                                    lboxStationReal.Items.Add(stationImport);
                                }
                                else
                                {
                                    //如果不存在站点，则添加相应站点
                                    if (sdbll.setStationsInfo(msr, "insert") == 0)
                                    {
                                        Model.StationRelation searchStationResultNew = dbll.searchStationInfo2(msr);
                                        if (searchStationResultNew != null)
                                        {
                                            ListItem stationImportNew = new ListItem();
                                            stationImportNew.Value = searchStationResultNew.StationID.ToString() + "#" + searchStationResultNew.StationTableID.ToString();
                                            stationImportNew.Text = searchStationResultNew.StationName + "（" + searchStationResultNew.StationID.ToString() + "）";
                                            lboxStationReal.Items.Add(stationImportNew);
                                        }
                                        else
                                        {
                                            Response.Write("<script>alert('新站点成功插入，但检索失败！');</script>");
                                        }
                                        
                                    }
                                    else
                                    {
                                        Response.Write("<script>alert('新站点号或名称插入失败！');</script>");
                                    }
                                }
                            }
                            stLine = srReader.ReadLine();
                        }
                        srReader.Close();
                        lbPathname.Text = fuSelectStaFile.PostedFile.FileName;
                        lbSelectedCity.Text = "已选城市列表：共" + lboxStationReal.Items.Count + "个城市！";
                    }
                    catch (Exception Exc)
                    {
                        lb_error.Text = Exc.Message;
                    }

                }
                else
                {
                    lb_error.Text = "只允许导入.sta文件!";
                }
            }
            else
            {
                lb_error.Text = "请选择一个城市列表文件";
            }
        }
        else
        {
            lbShowError.Text = "* 请正确选择需要操作的*栏目*";
            lbShowError.Visible = true;
        }
        
    }

    protected void lboxStationReal_SelectedIndexChanged(object sender, EventArgs e)
    {
        initddlStep1(lboxStationReal.SelectedIndex);
        initddlStep2(lboxStationReal.SelectedIndex);
    }

    protected void ddlStep1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = 0;
        if (ddlStep1.SelectedValue != "上移格数")
        {
            index = Convert.ToInt32(ddlStep1.SelectedValue);
            index = -index;
        }
        moveDisplayItem(index, lboxStationReal);
    }

    protected void ddlStep2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = 0;
        if (ddlStep2.SelectedValue != "下移格数")
        {
            index = Convert.ToInt32(ddlStep2.SelectedValue);
        }
        moveDisplayItem(index, lboxStationReal);
    }

    protected void bOrder_Click(object sender, EventArgs e)
    {
        if (bOrder.Text == "按站点号排列")
        {
            initlboxStationAllByID();
            bOrder.Text = "按站点名称排列";
        }
        else
        {
            initlboxStationAll();
            bOrder.Text = "按站点号排列";
        }
    }

}
