<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="setting2.aspx.cs" Inherits="setting2" Title="��Ŀ����" %>

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

    /* ˫ѡ�����Ŀ���� */
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
        /* ���Ƴ�����ӵ���Ŀ */
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
        /* ���Ƴ�����ӵ���Ŀ */
    }


    /* ѡ����ƶ���Ŀ���� */
    function moveUp(sel) {
        var obj = document.getElementById(sel);
        for (var i = 1; i < obj.length; i++) {//�������һ������Ҫ�ƶ�������ֱ�Ӵ�i=1��ʼ
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
        for (var i = obj.length - 2; i >= 0; i--) {//�����ƶ������һ������Ҫ��������ֱ�Ӵӵ����ڶ�����ʼ
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
    
    
    /* �б��ؼ��ֹ��� */
    /*
    var filter;     // ����������
    window.onload = function() {
        filter = new filterlist(document.getElementById("<%=lbStationUsable.ClientID%>"));  // ҳ�������Ϻ�Ա������и�ֵ
    }
    */
    
    function filteroption(root) {  //��ʼ���б�����Ϊ�б�id
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

    function resetOption(keys, root) {  //ɸѡ���ϵ��б���
        var duplul = $(window[root]);
        if (keys.length <= 0) {
            $("#" + root).children().remove();
            duplul.children().each(function() {
                $("#" + root).append($(this).clone(true).removeAttr("mka").removeAttr("mkb").removeAttr("mkc"));
            });
            return;
        }
        var pattern = /^([\u4e00-\u9fa5]|\w)+$/; //ֻ��������ĸ���ֻ���
        if (!pattern.test(keys)) {
            alert("������Ч�ַ���");
            return;
        }
        keys = keys.toLowerCase();
        $("#" + root).children().remove();
        duplul.children('[mka*="' + keys + '"],[mkb*="' + keys + '"],[mkc*="' + keys + '"]').each(function() {
            $("#" + root).append($(this).clone(true).removeAttr("mka").removeAttr("mkb").removeAttr("mkc"));
        });
    }

    $(document).ready(function() {
        filteroption("<%=lbStationUsable.ClientID%>"); //��ʼ���б�,����Ϊul����select��id
    });
    
    
    /* File API��Ҫ֧��HTML5���������IE8��֧�֣�IE9����������֧��
    window.onload = function() {
        // Check for the various File API support.
        if (window.File && window.FileReader && window.FileList && window.Blob) {
            //do your stuff!
        } else {
            alert('The File APIs are not fully supported by your browser.');
        }
    }
    */


    /* ��ȡ.staվ���б��ļ� */
    function readStaFile(path) {
        try {
            var fso = new ActiveXObject("Scripting.FileSystemObject");
        }
        catch (e) {
            alert("��������ò���ȷ���޷���ȡվ���ļ�������ղ���˵���޸ģ�");
            window.location.href = 'use.aspx#C4-4';
            return;
        }
        
        var pattern = /.*\.sta$/;   // ֻ��ѡ��.sta�ļ�
        if (!pattern.test(path)) {
            alert("��֧�ֶ�ȡ��չ��Ϊ.sta��վ���ļ���������ѡ��վ���ļ���");
            return;
        }

        var obj = document.getElementById("<%=lbStationUsed.ClientID%>");
        obj.options.length = 0;     // �������option
        
        var f = fso.OpenTextFile(path, 1);      // 1 ��ʾ��ֻ����ʽ��
        while (!f.AtEndOfStream) {
            // [0]:վ���; [1]:վ������
            var arr = f.ReadLine().split(' ');   // ��ȡһ�е����������з�
            // value:վ���#����(��:-1)
            obj.options.add(new Option(arr[1] + " ( " + arr[0] + " ) ", arr[0] + "#-1"));
        }
        f.close();
        
        alert("վ���ļ���" + path.substring(path.lastIndexOf('\\') + 1) + " ������ɣ�");
    }


    function prepSettingOper() {
        // �����Ŀ����
        var tips = document.getElementById("<%=lbTips.ClientID%>").innerHTML;
        //var tips = document.getElementById("<%=lbTips.ClientID%>").innerText;
        if (tips.indexOf("��ѡ��Ŀ��") == -1) {
            alert("����û��ѡ����Ŀ��");
            return false;
        }
        var obj = document.getElementById("<%=ddlDataType.ClientID%>");
        if (obj.options[obj.selectedIndex].value == "-1") {
            alert("����û��ѡ���������ͣ�");
            return false;
        }
        obj = document.getElementById("<%=ddlTimeType.ClientID%>");
        if (obj.options[obj.selectedIndex].value == "-1") {
            alert("����û��ѡ��ʱ�����ͣ�");
            return false;
        }
        if (document.getElementById("<%=lbDataTypeUsed.ClientID%>").options.length < 1) {
            alert("����ָ��һ����ѡ��Ŀ��");
            return false;
        }
        if (document.getElementById("<%=lbStationUsed.ClientID%>").options.length < 1) {
            alert("����ָ��һ����ѡ���У�");
            return false;
        }

        // ׼���ط�����
        var src = document.getElementById("<%=lbDataTypeUsed.ClientID%>");
        var dst = document.getElementById("<%=hfDataTypeUsed.ClientID%>");

        // datatype bundle example: 1|����;2|ת����;...
        dst.value = "";
        for (var i = 0; i < src.options.length; i++) {
            //dst.value += src.options[i].value + "|" + src.options[i].text + ";";
            dst.value += src.options[i].value + ";";
        }
        
        src = document.getElementById("<%=lbStationUsed.ClientID%>");
        dst = document.getElementById("<%=hfStationUsed.ClientID%>");
        
        // station bundle example: 54511#-1|����;59287#-1|����;...
        dst.value = "";
        for (var i = 0; i < src.options.length; i++) {
            dst.value += src.options[i].value + "|";
            var pos = src.options[i].text.indexOf('(');
            dst.value += src.options[i].text.substring(0, pos - 1) + ";";
        }
        
        return confirm("��ȷ��Ҫִ����Ŀ������");
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
                            ��Ŀ����</td>
                    </tr>
                    <tr>
                        <td class="style21">
                            ������ʾ��<br />
                            1. չ��������νṹ�鿴��ǰ�����õ�Ƶ����Ŀ�б�<br />
                            2. ѡ����Ŀ����������/ʱ�����͡�Ԥ��Ҫ�غ�Ԥ������<br />
                            3. ��ѡ��Ŀ/���пɵ���˳�������ڳ���Ԥ���Ĳ�ѯ�����</td>
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
                                        �������ͣ�</td>
                                    <td class="style8">
                                        <asp:DropDownList ID="ddlDataType" runat="server" 
                                            OnSelectedIndexChanged="ddlDataType_SelectedIndexChanged" AutoPostBack="True" 
                                            Width="92px">
                                            <asp:ListItem Value="-1">-- ��ѡ�� --</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style9">
                                        ʱ�����ͣ�</td>
                                    <td class="style21">
                                        <asp:DropDownList ID="ddlTimeType" runat="server" Width="92px">
                                            <asp:ListItem Value="-1"  Text="-- ��ѡ�� --">
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
                                <td class="style3">��ѡ��Ŀ��</td>
                                <td class="style4">&nbsp;</td>
                                <td class="style3">��ѡ��Ŀ��</td>
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
                                    <input id="btnMoveUp_1" type="button" value="��" onclick='moveUp("<%=lbDataTypeUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveDown_1" type="button" value="��" onclick='moveDown("<%=lbDataTypeUsed.ClientID%>");' />
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
                                <td class="style5">��ѡ���У�</td>
                                <td class="style6">&nbsp;</td>
                                <td class="style5">��ѡ���У�</td>
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
                                    <input id="btnMoveTop_2" type="button" value="��" onclick='moveTop("<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveUp_2" type="button" value="��" onclick='moveUp("<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveDown_2" type="button" value="��" onclick='moveDown("<%=lbStationUsed.ClientID%>");' />
                                    <br />
                                    <br />
                                    <input id="btnMoveBottom_2" type="button" value="��" onclick='moveBottom("<%=lbStationUsed.ClientID%>");' />
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    ��ʾ�����ı�����������м�ƴ��վ��ſɶ�̬���˳��С�</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    ��ʾ��ѡ��վ���ļ�������������ѡ���У�<a href="use.aspx#C4-4">�������ʹ�ã�</a>
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
                            <asp:Button ID="btnSettingOper" runat="server" Text="����" 
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

