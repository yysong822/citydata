<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="setting2.aspx.cs" Inherits="setting2" Title="栏目配置" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            width: 126px;
            text-align: left;
        }
        .style4
        {
            width: 60px;
            text-align: left;
        }
        .style5
        {
            width: 156px;
            text-align: left;
        }
        .style6
        {
            width: 56px;
            text-align: left;
        }
        .style8
        {
            width: 128px;
            text-align: left;
        }
        .style9
        {
            width: 80px;
            text-align: left;
        }
        .style20
        {
            font-size: large;
            font-weight: bold;
        }
        .style21
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript" language="javascript" >

    /* 双选择框项目操作 */
    function singleSelect(s1, s2) {
        var objList1 = document.getElementById(s1);
        var objList2 = document.getElementById(s2);

        for (var i = 0; i < objList1.options.length; i++) {
            var optionItem = objList1.options[i];
            if (optionItem.selected == true) {
                var option = new Option(optionItem.text, optionItem.value);
                objList2.options.add(option);
            }
        }
        for (var j = 0; j < objList2.options.length; j++) {
            var optionItem1 = objList2.options[j];
            var index = ExistsCheck(objList1, optionItem1.value)
            if (index >= 0)
                objList1.options.remove(index);
        }
    }
    
    function ExistsCheck(objList, objItemValue) {
        var index = -1;
        for (var i = 0; i < objList.options.length; i++) {
            if (objList.options[i].value == objItemValue) {
                index = i;
                break;
            }
            else {
                index = -1;
                continue;
            }
        }
        return index;
    }
    
    function allSelect(s1, s2) {
        var objList1 = document.getElementById(s1);
        var objList2 = document.getElementById(s2);

        if (objList2.options.length == 0) {
            objList2.options.length = 0;
            for (var i = 0; i < objList1.options.length; i++) {
                var optionItem = objList1.options[i];
                var option = new Option(optionItem.text, optionItem.value);
                objList2.options.add(option);
            }
            objList1.options.length = 0;
        }
        else {
            for (var i = 0; i < objList1.options.length; i++) {
                var optionItem = objList1.options[i];
                var index = ExistsCheck(objList2, optionItem.value)
                if (index == -1) {
                    var option = new Option(optionItem.text, optionItem.value);
                    objList2.options.add(option);
                }
            }
            for (var j = 0; j < objList2.options.length; j++) {
                var optionItem1 = objList2.options[j];
                var index = ExistsCheck(objList1, optionItem1.value)
                if (index >= 0)
                    objList1.options.remove(index);
            }
        }
    }
    
    function singleAdd(sfr, sto) {
        var objLeft = document.getElementById(sfr);
        var objRight = document.getElementById(sto);
        for (var i = 0; i < objLeft.options.length; i++) {
            var optionItem = objLeft.options[i];
            if (optionItem.selected == true) {
                var option = new Option(optionItem.text, optionItem.value);
                objRight.options.add(option);
            }
        }
        /* 不移除已添加的项目 */
    }

    function singleRemove(s) {
        var objList = document.getElementById(s);
        for (var i = 0; i < objList.options.length; i++) {
            if (objList.options[i].selected == true) {
                objList.options.remove(i);
            }
        }
    }

    function allAdd(sfr, sto) {
        var objLeft = document.getElementById(sfr);
        var objRight = document.getElementById(sto);
        for (var i = 0; i < objLeft.options.length; i++) {
            var optionItem = objLeft.options[i];
            var option = new Option(optionItem.text, optionItem.value);
            objRight.options.add(option);
        }
        /* 不移除已添加的项目 */
    }


    /* 选择框移动项目操作 */
    function moveUp(sel) {
        var obj = document.getElementById(sel);
        for (var i = 1; i < obj.length; i++) {//最上面的一个不需要移动，所以直接从i=1开始
            if (obj.options[i].selected) {
                if (!obj.options.item(i - 1).selected) {
                    var selText = obj.options[i].text;
                    var selValue = obj.options[i].value;
                    obj.options[i].text = obj.options[i - 1].text;
                    obj.options[i].value = obj.options[i - 1].value;
                    obj.options[i].selected = false;
                    obj.options[i - 1].text = selText;
                    obj.options[i - 1].value = selValue;
                    obj.options[i - 1].selected = true;
                }
            }
        }
    }

    function moveDown(sel) {
        var obj = document.getElementById(sel);
        for (var i = obj.length - 2; i >= 0; i--) {//向下移动，最后一个不需要处理，所以直接从倒数第二个开始
            if (obj.options[i].selected) {
                if (!obj.options[i + 1].selected) {
                    var selText = obj.options[i].text;
                    var selValue = obj.options[i].value;
                    obj.options[i].text = obj.options[i + 1].text;
                    obj.options[i].value = obj.options[i + 1].value;
                    obj.options[i].selected = false;
                    obj.options[i + 1].text = selText;
                    obj.options[i + 1].value = selValue;
                    obj.options[i + 1].selected = true;
                }
            }
        }
    }

    function moveTop(sel) {
        var obj = document.getElementById(sel);
        var opts = [];
        for (var i = obj.options.length - 1; i >= 0; i--) {
            if (obj.options[i].selected) {
                opts.push(obj.options[i]);
                obj.remove(i);
            }
        }
        var index = 0;
        for (var t = opts.length - 1; t >= 0; t--) {
            var opt = new Option(opts[t].text, opts[t].value);
            opt.selected = true;
            obj.options.add(opt, index++);
        }
    }

    function moveBottom(sel) {
        var obj = document.getElementById(sel);
        var opts = [];
        for (var i = obj.options.length - 1; i >= 0; i--) {
            if (obj.options[i].selected) {
                opts.push(obj.options[i]);
                obj.remove(i);
            }
        }
        for (var t = opts.length - 1; t >= 0; t--) {
            var opt = new Option(opts[t].text, opts[t].value);
            opt.selected = true;
            obj.options.add(opt);
        }
    }
    
    
    /* 列表框关键字过滤 */
    /*
    var filter;     // 先声明变量
    window.onload = function() {
        filter = new filterlist(document.getElementById("<%=lbStationUsable.ClientID%>"));  // 页面加载完毕后对变量进行赋值
    }
    */
    
    function filteroption(root) {  //初始化列表，参数为列表id
        var tempul;
        tempul = $("#" + root).clone(true);
        tempul.children().each(function() {
            var htmword = $(this).html();
            var pyword = $(this).toPinyin();
            var supperword = "";
            pyword.replace(/[A-Z]/g, function(word) { supperword += word });
            $(this).attr("mka", (htmword).toLowerCase());
            $(this).attr("mkb", (pyword).toLowerCase());
            $(this).attr("mkc", (supperword).toLowerCase());
        });
        window[root] = tempul;
    }

    function resetOption(keys, root) {  //筛选符合的列表项
        var duplul = $(window[root]);
        if (keys.length <= 0) {
            $("#" + root).children().remove();
            duplul.children().each(function() {
                $("#" + root).append($(this).clone(true).removeAttr("mka").removeAttr("mkb").removeAttr("mkc"));
            });
            return;
        }
        var pattern = /^([\u4e00-\u9fa5]|\w)+$/; //只能输入字母数字或汉字
        if (!pattern.test(keys)) {
            alert("输入无效字符！");
            return;
        }
        keys = keys.toLowerCase();
        $("#" + root).children().remove();
        duplul.children('[mka*="' + keys + '"],[mkb*="' + keys + '"],[mkc*="' + keys + '"]').each(function() {
            $("#" + root).append($(this).clone(true).removeAttr("mka").removeAttr("mkb").removeAttr("mkc"));
        });
    }

    $(document).ready(function() {
        filteroption("<%=lbStationUsable.ClientID%>"); //初始化列表,参数为ul或者select的id
    });
    
    
    /* File API需要支持HTML5的浏览器，IE8不支持，IE9（含）以上支持
    window.onload = function() {
        // Check for the various File API support.
        if (window.File && window.FileReader && window.FileList && window.Blob) {
            //do your stuff!
        } else {
            alert('The File APIs are not fully supported by your browser.');
        }
    }
    */


    /* 读取.sta站点列表文件 */
    function readStaFile(path) {
        try {
            var fso = new ActiveXObject("Scripting.FileSystemObject");
        }
        catch (e) {
            alert("浏览器设置不正确，无法读取站点文件，请参照操作说明修改！");
            window.location.href = 'use.aspx#C4-4';
            return;
        }
        
        var pattern = /.*\.sta$/;   // 只能选择.sta文件
        if (!pattern.test(path)) {
            alert("仅支持读取扩展名为.sta的站点文件，请重新选择站点文件！");
            return;
        }

        var obj = document.getElementById("<%=lbStationUsed.ClientID%>");
        obj.options.length = 0;     // 清空所有option
        
        var f = fso.OpenTextFile(path, 1);      // 1 表示以只读方式打开
        while (!f.AtEndOfStream) {
            // [0]:站点号; [1]:站点名称
            var arr = f.ReadLine().split(' ');   // 读取一行但不包含换行符
            // value:站点号#主键(无:-1)
            obj.options.add(new Option(arr[1] + " ( " + arr[0] + " ) ", arr[0] + "#-1"));
        }
        f.close();
        
        alert("站点文件：" + path.substring(path.lastIndexOf('\\') + 1) + " 解析完成！");
    }


    function prepSettingOper() {
        // 检查栏目设置
        var tips = document.getElementById("<%=lbTips.ClientID%>").innerHTML;
        //var tips = document.getElementById("<%=lbTips.ClientID%>").innerText;
        if (tips.indexOf("已选栏目：") == -1) {
            alert("您还没有选择栏目！");
            return false;
        }
        var obj = document.getElementById("<%=ddlDataType.ClientID%>");
        if (obj.options[obj.selectedIndex].value == "-1") {
            alert("您还没有选择数据类型！");
            return false;
        }
        obj = document.getElementById("<%=ddlTimeType.ClientID%>");
        if (obj.options[obj.selectedIndex].value == "-1") {
            alert("您还没有选择时间类型！");
            return false;
        }
        if (document.getElementById("<%=lbDataTypeUsed.ClientID%>").options.length < 1) {
            alert("至少指定一个已选项目！");
            return false;
        }
        if (document.getElementById("<%=lbStationUsed.ClientID%>").options.length < 1) {
            alert("至少指定一个已选城市！");
            return false;
        }

        // 准备回发数据
        var src = document.getElementById("<%=lbDataTypeUsed.ClientID%>");
        var dst = document.getElementById("<%=hfDataTypeUsed.ClientID%>");

        // datatype bundle example: 1|天气;2|转天气;...
        dst.value = "";
        for (var i = 0; i < src.options.length; i++) {
            //dst.value += src.options[i].value + "|" + src.options[i].text + ";";
            dst.value += src.options[i].value + ";";
        }
        
        src = document.getElementById("<%=lbStationUsed.ClientID%>");
        dst = document.getElementById("<%=hfStationUsed.ClientID%>");
        
        // station bundle example: 54511#-1|北京;59287#-1|广州;...
        dst.value = "";
        for (var i = 0; i < src.options.length; i++) {
            dst.value += src.options[i].value + "|";
            var pos = src.options[i].text.indexOf('(');
            dst.value += src.options[i].text.substring(0, pos - 1) + ";";
        }
        
        return confirm("您确认要执行栏目配置吗？");
    }
    

</script>

    <table style="width:100%;">
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align:left;width:250px;vertical-align:top;">
                <asp:TreeView ID="tvProgram" runat="server" OnSelectedNodeChanged="trProgram_SelectedNodeChanged">
                    <ParentNodeStyle Font-Bold="true" VerticalPadding="20px" />
                    <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" BackColor="#D4D4D4"/>
                    <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" 
                        HorizontalPadding="0px" VerticalPadding="0px" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" 
                        HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                </asp:TreeView>
                <br />
                <asp:Label ID="lblLeftMessage" runat="server" Font-Italic="True" Font-Size="Large"></asp:Label>
            </td>
            <td style="vertical-align:top;">
                <table style="width:100%;">
                    <tr>
                        <td class="style20">
                            栏目配置</td>
                    </tr>
                    <tr>
                        <td class="style21">
                            操作提示：<br />
                            1. 展开左侧树形结构查看当前可配置的频道栏目列表<br />
                            2. 选择栏目可配置数据/时间类型、预报要素和预报城市<br />
                            3. 已选项目/城市可调整顺序，体现在城市预报的查询结果中</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbTips" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbWarning" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width:100%;">
                                <tr>
                                    <td class="style9">
                                        数据类型：</td>
                                    <td class="style8">
                                        <asp:DropDownList ID="ddlDataType" runat="server" 
                                            OnSelectedIndexChanged="ddlDataType_SelectedIndexChanged" AutoPostBack="True" 
                                            Width="92px">
                                            <asp:ListItem Value="-1">-- 请选择 --</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style9">
                                        时间类型：</td>
                                    <td class="style21">
                                        <asp:DropDownList ID="ddlTimeType" runat="server" Width="92px">
                                            <asp:ListItem Value="-1"  Text="-- 请选择 --">
                                            </asp:ListItem>
                                        </asp:DropDownList>    
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width:100%;">
                            <tr>
                                <td class="style3">可选项目：</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style3">已选项目：</td>
                                <td class="style21">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    <select id="lbDataTypeUsable" name="D1" multiple="true" runat="server" style="height: 136px; width: 108px" >
                                    </select>
                                </td>
                                <td class="style4">
                                    <input id="btnMoveAllRight_1" type="button" value=">>" onclick='allSelect("<%=lbDataTypeUsable.ClientID%>", "<%=lbDataTypeUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveRight_1" type="button" value=" > " onclick='singleSelect("<%=lbDataTypeUsable.ClientID%>", "<%=lbDataTypeUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveLeft_1" type="button" value=" < " onclick='singleSelect("<%=lbDataTypeUsed.ClientID%>", "<%=lbDataTypeUsable.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveAllLeft_1" type="button" value="<<" onclick='allSelect("<%=lbDataTypeUsed.ClientID%>", "<%=lbDataTypeUsable.ClientID%>");' />
                                </td>
                                <td class="style3">
                                    <select id="lbDataTypeUsed" name="D2" multiple="true" runat="server" style="height: 136px; width: 108px" >
                                    </select>
                                </td>
                                <td class="style21">
                                    <input id="btnMoveUp_1" type="button" value="︿" onclick='moveUp("<%=lbDataTypeUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveDown_1" type="button" value="﹀" onclick='moveDown("<%=lbDataTypeUsed.ClientID%>");' />
                                </td>
                                <td class="style21">
                                    &nbsp;</td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width:100%;">
                            <tr>
                                <td class="style5">可选城市：</td>
                                <td class="style6">&nbsp;</td>
                                <td class="style5">已选城市：</td>
                                <td class="style21">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <input id="tbStationName" type="text" style="width: 136px" 
                                        onkeyup='resetOption($.trim($("#tbStationName").val()), "<%=lbStationUsable.ClientID%>");' />
                                </td>
                                <td class="style6">&nbsp;</td>
                                <td class="style5">
                                    <input id="fuStationFile" type="file" style="width: 140px" onchange="readStaFile(this.value);" />
                                </td>
                                <td class="style21">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <select id="lbStationUsable" name="D3" multiple="true" runat="server" style="height: 220px; width: 140px" >
                                    </select>
                                </td>
                                <td class="style6">
                                    <input id="btnMoveAllRight_2" type="button" value=">>" onclick='allAdd("<%=lbStationUsable.ClientID%>", "<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveRight_2" type="button" value=" > " onclick='singleAdd("<%=lbStationUsable.ClientID%>", "<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveLeft_2" type="button" value=" < " onclick='singleRemove("<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveAllLeft_2" type="button" value="<<" onclick='document.getElementById("<%=lbStationUsed.ClientID%>").options.length = 0;' />
                                </td>
                                <td class="style5">
                                    <select id="lbStationUsed" name="D4" multiple="true" runat="server" style="height: 220px; width: 140px" >
                                    </select>
                                </td>
                                <td class="style21">
                                    <input id="btnMoveTop_2" type="button" value="︽" onclick='moveTop("<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveUp_2" type="button" value="︿" onclick='moveUp("<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveDown_2" type="button" value="﹀" onclick='moveDown("<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveBottom_2" type="button" value="︾" onclick='moveBottom("<%=lbStationUsed.ClientID%>");' />
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    提示：在文本框中输入城市简拼或站点号可动态过滤城市。</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    提示：选择站点文件可批量导入已选城市：<a href="use.aspx#C4-4">如何配置使用？</a>
                                </td>
                                <td class="style21">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnSettingOper" runat="server" Text="设置" 
                                onclick="btnSettingOper_Click" onclientclick="return prepSettingOper();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="hfDataTypeUsed" type="hidden" value="" runat="server" />
                            <input id="hfStationUsed" type="hidden" value="" runat="server" />
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

