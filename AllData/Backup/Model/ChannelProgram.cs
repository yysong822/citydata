/**************************************************************************************************
  Copyright (C), 2009, 中国气象局华风气象影视信息集团技术部软件开发组
  文件名: ChannelProgram.cs
  作者: 王晗         版本: 1.0.0          完成日期: 2009.06.01
  程序说明:  关于频道栏目配置关联关系的数据模型，数据类型定义    
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
    public class ChannelProgram
    {

        public ChannelProgram() { }

        private int _CP_ID;
        public int CP_ID
        {
            set { _CP_ID = value; }
            get { return _CP_ID; }
        }

        private int _FatherID;
        public int FatherID
        {
            set { _FatherID = value; }
            get { return _FatherID; }
        }


        private string _CP_Name;
        public string CP_Name
        {
            set { _CP_Name = value; }
            get { return _CP_Name; }
        }

        private int _TimeTypeID;
        public int TimeTypeID
        {
            set { _TimeTypeID = value; }
            get { return _TimeTypeID; }
        }

        private int _DataTypeID;
        public int DataTypeID
        {
            set { _DataTypeID = value; }
            get { return _DataTypeID; }
        }

        private int _CP_Order;
        public int CP_Order
        {
            set { _CP_Order = value; }
            get { return _CP_Order; }
        }

    }



}
