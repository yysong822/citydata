<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Default, App_Web_default.aspx.cdcab7d2" title="城市预报" %>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table>
            <tr>
                <td>
                    <div align="center">

                    频道：<asp:DropDownList ID="ddl_channel" runat="server" AutoPostBack="True"  OnSelectedIndexChanged = "ddl_channel_SelectedIndexChanged">
                    </asp:DropDownList>
                    
                &nbsp;&nbsp;&nbsp;&nbsp;
                    
                    栏目：<asp:DropDownList ID="ddl_program" runat="server" AutoPostBack="True"  OnSelectedIndexChanged = "ddl_program_SelectedIndexChanged">
                    </asp:DropDownList>
                    
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    
                    时效：<asp:DropDownList ID="ddl_timeSX" runat="server" AutoPostBack="True" OnSelectedIndexChanged = "ddl_timeSX_SelectedIndexChanged">

                    </asp:DropDownList>

                </div>
                </td>
            </tr>


</table>




<div id="printdiv">
<table style="width:100%;">
            <tr>
            <asp:Panel ID="p_tip" runat="server" Visible="False">
                <td style="text-align:left;width:8%">
                <li>
                <b>时间：</b>
                </li>
                </td>
                    <td style="text-align:left">
                            <asp:Label ID="lb_titleTime" runat="server" Text="当前时间" Font-Bold="True" 
                                Font-Size="13px"></asp:Label>
                            <asp:Label ID="lb_titleName" runat="server" Text="全部频道栏目" Font-Bold="True" 
                                Font-Size="13px"></asp:Label>
                            <asp:Label ID="lb_titleTimeSX" runat="server" Text="全部时效" Font-Bold="True" 
                                Font-Size="13px"></asp:Label>
                                <asp:Label ID="lb_dataType" runat="server" Text="" Font-Bold="True" 
                                Font-Size="13px"></asp:Label>
                            <asp:Label ID="lb_cityWeather" runat="server" Text="城市天气预报" Font-Bold="True" 
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
                            Font-Size="13px"></asp:Label>
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
            <tr>
                <td colspan="2">
                    <table width="100%">
                    <tr>
                    <td style="text-align:right">
                    <asp:Panel  ID = "p_print" runat="server"  Visible ="false">
                    <input onclick="javascript:PrintNote(); " type= "button" value= "打印" class="Noprint"/>
                    </asp:Panel>
                    </td>
                    <td style="text-align:right" width="50px">
                 <asp:Panel  ID = "p_down" runat="server"  Visible ="false">
                    <asp:Button ID="bTxt" runat="server" Text="导出" onclick="bTxt_Click" class="Noprint" />
                 </asp:Panel>
                </td>
                    </tr>
                    </table>
                
                </td>
                
                 
            </tr>
            <tr>
                <td style="width:100%;" colspan = "2">
                    <asp:Panel ID="p_gv" runat="server" >
                        <div align="center">        
                            <asp:GridView ID="gv_citydata" runat="server" 
                                OnRowCreated = "gv_citydata_RowCreated"  
                                OnRowDataBound = "gv_citydata_RowDataBound" CellPadding="4" ForeColor="#333333" 
                                GridLines="None" Width="100%" >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
            
            <tr>
                <td align = "center" colspan="2">
                    <asp:Panel ID="p_text" runat="server" Visible="true">
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
        <table>
        <tr>
        <td>
            <asp:ListBox ID="lbGVCN" runat="server" Visible ="false"></asp:ListBox>
        </td>
        <td>
            <asp:ListBox ID="lbLastLine" runat="server" Visible ="false"></asp:ListBox>
        </td>
        </tr>
        </table>
 </div>       
        
</asp:Content>

