/**************************************************************************************************
  Copyright (C), 2009, 中国气象局华风气象影视信息集团技术部软件开发组
  文件名: Admin.cs
  作者: 王晗         版本: 1.0.0          完成日期: 2009.06.08
  程序说明:  关于管理员登陆类数据模型，数据类型定义    
  其它:         
  主要函数列表:  
    1. ....
  修改历史记录: 
    1. 日期:
       作者:
       更改内容:
**************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Admin
    {
        public Admin() { }

        private string _UserName;
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }


        private string _UserPassword;
        public string UserPassword
        {
            set { _UserPassword = value; }
            get { return _UserPassword; }
        }
}

}
