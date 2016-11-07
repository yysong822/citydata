<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="use, App_Web_use.aspx.cdcab7d2" title="使用说明" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            font-size: large;
            font-weight: bold;
        }
        .style2
        {
            text-align: left;
        }
        .style3
        {
            font-size: medium;
            font-weight: bold;
        }
        .style4
        {
            border: 2px solid #89c3e9;
        }
        .style5
        {
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<p class="style1">城市预报使用说明（第二版）</p>

<p class="style3">● 请使用Internet Explorer 6.0以上版本和1024x768以上分辨率浏览本站。</p>

<ul class="style2">
    <li><a href="#C1">城市预报</a>
    <ul>
        <li><a href="#C1-0">关于浏览器兼容性的说明</a></li>
        <li><a href="#C1-1">城市预报、总表数据查询</a></li>
        <li><a href="#C1-2">打印、导出查询结果</a></li>
        <li><a href="#C1-3">用户登录与退出</a></li>
        <li><a href="#C1-4">用户修改密码</a></li>
    </ul>
    </li>

    <li><a href="#C2">系统管理</a>
    <ul>
        <li><a href="#C2-1">用户管理</a></li>
        <li><a href="#C2-2">用户频道配置</a></li>
    </ul>
    </li>
    
    <li><a href="#C3">站点管理</a>
    <ul>
        <li><a href="#C3-1">添加站点并关联栏目</a></li>
        <li><a href="#C3-2">查询、管理站点信息</a></li>
        <li><a href="#C3-3">导出站点查询结果</a></li>
    </ul>
    </li>
    
    <li><a href="#C4">栏目配置</a>
    <ul>
        <li><a href="#C4-1">浏览现有的栏目配置</a></li>
        <li><a href="#C4-2">添加、删除和调整项目</a></li>
        <li><a href="#C4-3">动态过滤可选城市</a></li>
        <li><a href="#C4-4">批量导入已选城市</a></li>
        <li><a href="#C4-5">提交栏目配置</a></li>
    </ul>
    </li>
</ul>

<p class="style3"><a name="C1">● 城市预报</a></p>

<h3><a name="C1-0"><span class="style5">● 关于浏览器兼容性的说明</span></a></h3>
<p>1. 请使用最新版本的Internet Explorer浏览本站，如果使用Firefox或Chrome浏览器，可能导致页面显示和功能不全的问题。<br />
    2. Windows XP平台可以升级的最高版本为IE8，如果使用IE8浏览某些页面时出现如下警告，请下载安装<a href="file/MicrosoftFixit50403.msi">微软修复包程序</a>。</p>
<img alt="" src="images/help/c1-0.png" class="style4"/>

<h3><a name="C1-1">● 城市预报、总表数据查询</a></h3>
<p>1. 在导航菜单中选择城市预报，并在下拉列表中选择频道、栏目和预报时效。</p>
<img alt="" src="images/help/c1-1.png" class="style4"/>

<p>2. 在导航菜单中选择总表数据，并在下拉列表中选择数据类型、时间类型和预报时效。</p>
<img alt="" src="images/help/c1-2.png" class="style4"/>

<h3><a name="C1-2">● 打印、导出查询结果</a></h3>
<p>1. 点击工具-〉Internet选项，打开Internet选项对话框。</p>
<img alt="" src="images/help/c1-3.png" class="style4"/>

<p>2. 选择安全选项卡-〉受信任的站点-〉自定义级别，将“对未标记为可安全执行脚本的ActiveX控件初始化并执行脚本”选项修改为“提示”。</p>
<img alt="" src="images/help/c1-4.png" class="style4"/>

<p>3. 选择安全选项卡-〉受信任的站点-〉站点，取消“对该区域中的所有站点要求服务器验证(https:)”复选框，并将“http://10.16.36.45”添加到可信站点。</p>
<img alt="" src="images/help/c1-5.png" class="style4"/>

<p>4. 正确修改Internet选项后，浏览器右下角会有可信站点的提示。</p>
<img alt="" src="images/help/c1-6.png" class="style4"/>

<p>5. 点击查询结果右上角的“打印”按钮即可打印查询结果，点击“导出”按钮即可导出查询结果为文本文件。</p>
<img alt="" src="images/help/c1-7.png" class="style4"/>

<p>6. 点击“打印”按钮，并在IE弹出的安全对话框中选择“是”。</p>
<img alt="" src="images/help/c1-8.png" class="style4"/>

<p>7. IE会自动打开“打印预览”窗口，点击左上角的“打印”按钮，配置打印机后即可打印。</p>
<img alt="" src="images/help/c1-9.png" class="style4"/>

<p>8. 点击“导出”按钮，并保存文本文件到本地磁盘，文件内容如下图所示。</p>
<img alt="" src="images/help/c1-10.png" class="style4"/>

<h3><a name="C1-3">● 用户登录与退出</a></h3>
<p>1. 点击导航菜单上方的“登录”按钮进入登录界面，点击“退出”按钮退出当前用户。<br />
    <span class="style5">2. 新用户的初始密码为6个连续的数字0，请用户登录后及时修改密码。</span></p>
<img alt="" src="images/help/c1-11.png" class="style4"/>

<h3><a name="C1-3">● 用户修改密码</a></h3>
<p>1. 成功登录系统后，点击导航菜单上方的<span class="style5">用户名</span>进入修改密码界面。</p>
<img alt="" src="images/help/c1-12.png" class="style4"/>


<p class="style3"><a name="C2">● 系统管理</a></p>

<h3><a name="C2-1">● 用户管理</a></h3>
<p>1. 以管理员用户登录，在导航菜单中选择用户管理，即可进入下图界面。<br />
    2. 系统默认显示所有用户，其中已禁用的用户显示为灰色，任意输入四个查询条件可检索符合条件的用户。<br />
    3. 点击编辑按钮可以修改用户信息，点击删除按钮将删除该用户，点击重置按钮会重置该用户的密码为默认。</p>
<img alt="" src="images/help/c2-1.png" class="style4"/>
<p>4. 点击“点击此处添加用户”按钮将显示添加用户界面，输入相关信息后点击添加按钮完成添加。</p>
<img alt="" src="images/help/c2-2.png" class="style4"/>

<h3><a name="C2-2">● 用户频道配置</a></h3>
<p>1. 以管理员用户登录，在导航菜单中选择频道配置，即可进入下图界面。<br />
    2. 树形菜单中显示了系统已配置的用户频道设置，点击用户名可查看该用户可管理的频道和已管理的频道。</p>
<img alt="" src="images/help/c2-3.png" class="style4"/>
<p>3. 点击频道名可查看该频道可管理的用户和已管理的用户，一个频道可以由多个用户管理，同时一个用户也可以管理多个频道。</p>
<img alt="" src="images/help/c2-4.png" class="style4"/>


<p class="style3"><a name="C3">● 站点管理</a></p>

<h3><a name="C3-1">● 添加站点并关联栏目</a></h3>
<p>1. 在导航菜单中选择内容管理下的站点管理，点击“点击此处添加站点”按钮即可进入下图界面。<br />
    2. 输入相关信息后点击添加按钮完成添加站点并关联栏目。
    <span class="style5">提示：在一个栏目中可以多次关联同一站点。<br />
    3. 注意：如果用户输入的站点在系统中已存在，系统会直接关联该站点和栏目。制片人用户仅可以对自己管理频道下的栏目添加站点。</span>
</p>
<img alt="" src="images/help/c3-1.png" class="style4"/>

<h3><a name="C3-2">● 查询、管理站点信息</a></h3>
<p>1. 在导航菜单中选择内容管理下的站点管理，即可进入下图界面。<br />
    2. 任意输入四个查询条件可检索符合条件的站点。提示：已选栏目的站点将自动显示，无需再次点击查询按钮。<br /></p>
<img alt="" src="images/help/c3-2.png" class="style4"/>

<p>3. 点击编辑按钮可以修改站点信息，点击删除按钮将删除该站点。
    <span class="style5">警告：站点信息关联到城市预报的查询结果，请勿随意修改站点信息！<br />
    4. 注意：删除一个站点时，与该站点关联的栏目配置也将同时被删除，因此，制片人用户仅可以删除自己管理频道下的栏目所关联的站点。</span>
</p>
<img alt="" src="images/help/c3-3.png" class="style4"/>

<h3><a name="C3-3">● 导出站点查询结果</a></h3>
<p>1. 选择频道和栏目后，点击导出按钮，将station.txt文件保存到本地即可。</p>
<img alt="" src="images/help/c3-4.png" class="style4"/>


<p class="style3"><a name="C4">● 栏目配置</a></p>

<h3><a name="C4-1">● 浏览现有的栏目配置</a></h3>
<p>1. 在导航菜单中选择内容管理下的栏目配置，并选择左侧树形菜单下的一个栏目，即可进入下图界面。<br />
    2. 在右侧的配置界面中可浏览该栏目的数据类型、时间类型、项目要素和预报城市的配置。</p>
<img alt="" src="images/help/c4-1.png" class="style4"/>

<h3><a name="C4-2">● 添加、删除和调整项目</a></h3>
<p>1. 点击选择框按钮可以实现添加已选项、移除已选项、全部添加、全部移除、上移、下移、上移到顶部、下移到底部的功能。<br />
    2. 提示：如果要调整一个城市下移10项，无需点击10次下移按钮，只需将该城市的后10个城市选中，点击1次上移按钮即可。</p>
<img alt="" src="images/help/c4-2.png" class="style4"/>

<p>
    <span class="style5">3. 提示：一个栏目可以多次添加同一城市，并可以任意调整列表顺序，列表顺序影响城市预报数据查询结果。</span>
</p>
<img alt="" src="images/help/c4-3.png" class="style4"/>

<h3><a name="C4-3">● 动态过滤可选城市</a></h3>
<p>1. 在可选城市列表上方的文本框中输入<span class="style5">汉字简拼、全拼、英文、汉字或站点号的全部或一部分</span>，均可动态过滤可选城市列表。<br />
    2. 提示：清空可选城市列表上方的文本框内容即可恢复显示所有城市。</p>
<img alt="" src="images/help/c4-4.png" class="style4"/>

<h3><a name="C4-4">● 批量导入已选城市</a></h3>
<p>1. 从Internet Explorer的工具菜单或控制面板中打开Internet选项对话框。</p>
<img alt="" src="images/help/c4-5.png" class="style4"/>

<p>2. 选择安全选项卡-〉受信任的站点-〉自定义级别，进入安全设置对话框。</p>
<img alt="" src="images/help/c4-6.png" class="style4"/>

<p>3. 将“对未标记为可安全执行脚本的ActiveX控件初始化并执行脚本”选项修改为“提示”，点击确定-〉是。</p>
<img alt="" src="images/help/c4-7.png" class="style4"/>

<p>4. 返回Internet选项对话框，选择安全选项卡-〉受信任的站点-〉站点，进入受信任的站点对话框。</p>
<img alt="" src="images/help/c4-8.png" class="style4"/>

<p>5. 在文本框中输入：http://10.16.36.45，点击添加按钮，并取消“对该区域中的所有站点要求服务器验证(https:)”复选框。</p>
<img alt="" src="images/help/c4-9.png" class="style4"/>

<p>6. 点击此处下载：<a href="file/example.sta">站点示例.sta</a>，按文件格式编辑需要导入的站点：
    <span class="style5">每行一个站点，站点号和站点名称之间用空格分隔</span>。</p>
<img alt="" src="images/help/c4-10.png" class="style4"/>

<p>7. 进入栏目配置，选择一个栏目，点击已选城市列表上方的浏览按钮，选择一个sta文件，在弹出的安全提示对话框点“是”。<br />
    <span class="style5">注意：sta文件中的城市会覆盖已选城市列表中的城市，但只要不保存栏目配置，就不会影响城市预报数据查询结果。</span>
</p>
<img alt="" src="images/help/c4-11.png" class="style4"/>

<h3><a name="C4-5">● 提交栏目配置</a></h3>
<p>1. 完成所有栏目配置后，点击“设置”按钮，再次确认配置无误，点击确定保存配置。</p>
<img alt="" src="images/help/c4-12.png" class="style4"/>


</asp:Content>

