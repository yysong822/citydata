/**************************************************************************************************
  Copyright (C), 2013, 中国气象局华风气象影视信息集团技术部软件开发组
  文件名: User.cs
  作者: 陈默涵         版本: 1.0.0          完成日期: 2013.06.28
  程序说明:  关于用户类数据模型，数据类型定义    
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
    public class User
    {
        public User() { }

        private string _UserID;         // 工号ID
        public string UserID
        {
            set { _UserID = value; }
            get { return _UserID; }
        }

        private string _UserName;       // 用户名称
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }

        private string _Password;       // 密码
        public string Password
        {
            set { _Password = value; }
            get { return _Password; }
        }

        private string _UserRole;       // 用户角色
        public string UserRole
        {
            set { _UserRole = value; }
            get { return _UserRole; }
        }

        private string _UserState;      // 用户状态
        public string UserState
        {
            set { _UserState = value; }
            get { return _UserState; }
        }

        private string _UserComment;    // 用户注释
        public string UserComment
        {
            set { _UserComment = value; }
            get { return _UserComment; }
        }

    }
}
