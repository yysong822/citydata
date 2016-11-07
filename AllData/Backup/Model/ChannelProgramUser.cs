/**************************************************************************************************
  Copyright (C), 2013, 中国气象局华风气象影视信息集团技术部软件开发组
  文件名: ChannelProgramUser.cs
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
    public class ChannelProgramUser
    {
        public ChannelProgramUser() { }

        private string _UserID;             // 工号ID
        public string UserID
        {
            set { _UserID = value; }
            get { return _UserID; }
        }

        private string _UserName;           // 用户名称
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }

        private string _UserRole;           // 用户名称
        public string UserRole
        {
            set { _UserRole = value; }
            get { return _UserRole; }
        }

        private string _UserState;           // 用户名称
        public string UserState
        {
            set { _UserState = value; }
            get { return _UserState; }
        }

        private int? _ChannelID;              // 频道ID
        public int? ChannelID
        {
            set { _ChannelID = value; }
            get { return _ChannelID; }
        }

        private string _ChannelName;           // 频道名称
        public string ChannelName
        {
            set { _ChannelName = value; }
            get { return _ChannelName; }
        }

        private int? _ProgramID;              // 栏目ID
        public int? ProgramID
        {
            set { _ProgramID = value; }
            get { return _ProgramID; }
        }

        private string _ProgramName;              // 栏目名称
        public string ProgramName
        {
            set { _ProgramName = value; }
            get { return _ProgramName; }
        }

        private int _TimeTypeID;                // 时次类型ID
        public int TimeTypeID
        {
            set { _TimeTypeID = value; }
            get { return _TimeTypeID; }
        }

        private int _DataTypeID;                // 数据类型ID
        public int DataTypeID
        {
            set { _DataTypeID = value; }
            get { return _DataTypeID; }
        }

    }
}
