<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" Title="城市预报" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript" >
<!-- 
function   PrintNote() 
{ 
var PrintWin=window.open( 'about:blank ', 'Print'); 
PrintWin.document.write( "<style type='text/css'> TD{font-size: 12px;border-bottom: #AC8D9D 1px solid; text-align: center;} .Noprint{display:none;}</style><object id='WebBrowser' width=0 height=0 classid='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2'> </object> " + document.all("printdiv").innerHTML);
PrintWin.document.all.WebBrowser.ExecWB(7,1);
PrintWin.close(); 
} 
--> 
</script>

    <style type="text/css">
        .style2
        {
            width: 743px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
    <ContentTemplate>
    <table style="width: 100%" >
        <tr >  
        <td style=" width:35px"></td> 
            <td style=" text-align:left; width:140px">
                频道：
            </td>
            <td style=" text-align:left; min-width:100px">
                栏目：
            </td>
            <td style=" text-align:left ">
            <asp:UpdatePanel ID="up_sxtxt" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Label ID="l_sx" runat="server" Text="时效：" Visible ="false"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddl_program" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            </td>
            <td>
            </td>
        </tr>
        <tr>
        <td style=" width:35px"></td> 
            <td style=" text-align:left; width:140px">
                   <asp:DropDownList ID="ddl_channel" runat="server" AutoPostBack="True"  OnSelectedIndexChanged = "ddl_channel_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style=" text-align:left; min-width:100px">
            <asp:UpdatePanel ID="up_program" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList ID="ddl_program" runat="server" AutoPostBack="True"  OnSelectedIndexChanged = "ddl_program_SelectedIndexChanged">
                    </asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddl_channel" EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
            </td>
            <td  style=" text-align:left;width:300px" >
                <asp:UpdatePanel ID="up_sx" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:CheckBoxList ID="cbl_timeSX" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </asp:CheckBoxList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_program" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="text-align:right; width:70px">
                <asp:UpdatePanel ID="up_commit" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="b_commit" runat="server" Text="查询" onclick="b_commit_Click"  Visible="false"/>
                </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_program" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        
        </table>
      </ContentTemplate>
</asp:UpdatePanel>



            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:Panel ID="p_content" runat="server" Visible="False">
            <asp:Panel ID="p_print" runat="server" Visible="False">
            <div align="right">
            <table>
                <tr>
                    <td style="text-align:right">
                        <asp:Button OnPreRender="b_print_OnPreRender" ID="b_print" runat="server" Text="打印"  class="Noprint" />
                    </td>
                    <td style="text-align:right" width="50px">
                        <asp:Button ID="b_save" runat="server" class="Noprint" onclick="b_save_Click" 
                            Text="导出" />
                    </td>
                </tr>
            </table>
            </div>
            </asp:Panel>
            <!--打印区域-->
            <div id="printdiv">
            <table>
                <tr>
                <asp:Panel ID="p_tip" runat="server" Visible="False">
                    <td style="text-align:left;width:8%">
                    <li>
                    <b>时间：</b>
                    </li>
                    </td>
                        <td style="text-align:left">
                                <asp:Label ID="lb_tipRT" runat="server" Text="当前时间" Font-Bold="True" 
                                    Font-Size="13px"></asp:Label>
                                <asp:Label ID="lb_tipCP" runat="server" Text="全部频道栏目" Font-Bold="True" 
                                    Font-Size="13px"></asp:Label>
                                <asp:Label ID="lb_tipSX" runat="server" Text="全部时效" Font-Bold="True" 
                                    Font-Size="13px"></asp:Label>
                                    <asp:Label ID="lb_tipDT" runat="server" Text="" Font-Bold="True" 
                                    Font-Size="13px"></asp:Label>
                            
                            <div style="text-align:left">
                                <asp:Panel ID="p_timeInterval" runat="server" Visible="False">
                                    <asp:Label ID="lb_titleTimeZoneBegin" runat="server" Font-Bold="True" 
                                        Font-Size="13px"></asp:Label>
                                    <asp:Label ID="lb_titleTimeZoneOver" runat="server" Font-Bold="True" 
                                        Font-Size="13px"></asp:Label>
                                </asp:Panel>
                            </div>
                        </td>
                    </asp:Panel>
                </tr>
                <tr>
                    <asp:Panel ID="p_noExistStation" runat="server" Visible="False">
                        <td style="text-align:left;width:8%">
                            <li>
                                <b>数据：</b>
                            </li>
                        </td>
                        <td style="text-align:left">                   
                            <asp:Label ID="lb_noExistStation" runat="server" Font-Bold="True" 
                                Font-Size="13px" ForeColor="Red"></asp:Label>
                        </td>
                    </asp:Panel>
                </tr>
                
                <tr>
                <asp:Panel ID="p_TepExpTip" runat="server" Visible="False">
                    <td style="text-align:left;width:8%" >
                    <li>
                        <b>温度：</b>
                    </li>
                    </td>
                    <td style="text-align:left">
                        <table>
                            <tr>
                                <td style="text-align:left">
                                    <asp:Label ID="lb_equalMaxMinTemTip" runat="server" Font-Bold="True" 
                                    Font-Size="13px" ForeColor="Red" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left">
                                    <asp:Label ID="lb_hlTemTip" runat="server" Font-Bold="True" 
                                    Font-Size="13px" ForeColor="Red" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left">
                                    <asp:Label ID="lb_subTemTip" runat="server" Font-Bold="True" 
                                    Font-Size="13px" ForeColor="Red" ></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    </asp:Panel>
                </tr>
                
                <tr>
                <asp:Panel ID="p_WethExp" runat="server" Visible="False">
                    <td style="text-align:left;width:8%" >
                    <li>
                        <b>天气：</b>
                    </li>
                    </td>
                    <td style="text-align:left">
                        <table>
                            <tr>
                                <td style="text-align:left">
                                    <asp:Label ID="lb_LTempWethExpTip" runat="server" Font-Bold="True" 
                                Font-Size="13px" ForeColor="Red" ></asp:Label>    
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left">
                                    <asp:Label ID="lb_WinNTip" runat="server" Font-Bold="True" 
                                    Font-Size="13px" ForeColor="Red" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left">
                                    <asp:Label ID="lb_WethTip" runat="server" Font-Bold="True" 
                                    Font-Size="13px" ForeColor="Red" ></asp:Label>
                                </td>
                            </tr>
                        </table>
                                           
                    </td>
                    </asp:Panel>
                </tr>
            </table>

            <div align="center">        
                <asp:GridView ID="gv_citydata" runat="server" 
                    OnRowDataBound = "gv_citydata_RowDataBound"
                     CellPadding="4" ForeColor="#333333" 
                    GridLines="None" Width="100%"  >
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </div>
            </div>
            <!-- <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>-->
            </asp:Panel>
            </ContentTemplate>
            <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="b_commit" EventName="Click" />
                    <asp:PostBackTrigger ControlID="b_save" />
                </Triggers>
            </asp:UpdatePanel> 

<asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
    <ContentTemplate>
     <table>
        <tr>
            <td align = "center" colspan="2">
                <asp:Panel ID="p_illustration" runat="server" Visible="true">
                    <div>
                    <ul>
			                <li>
				                <b>城市预报</b>
				                <div>用户可以根据频道、栏目、时效检索需要的城市预报信息</div>
			                </li>
                            <br />
			                <li>
				                <b>后台管理</b>
				                <div>对城市预报显示进行管理，自由更改频道栏目中的相关内容</div>
			                </li>
                            <br />
			                <li>
				                <b>信息打印</b>
				                <div>按照栏目可以方便的查找数据信息
                                </div>
			                </li>
			                <!--这里增加Li-->
		                </ul>
                    </div>
                </asp:Panel>
            </td>
        </tr>  
     </table>
     </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddl_channel" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="b_commit" EventName="Click" />
    </Triggers>
</asp:UpdatePanel> 

</asp:Content>

