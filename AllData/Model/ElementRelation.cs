/**************************************************************************************************
  Copyright (C), 2009, 中国气象局华风气象影视信息集团技术部软件开发组
  文件名: ElementRelation.cs
  作者: 王晗         版本: 1.0.0          完成日期: 2009.06.01
  程序说明:  关于天气元素配置关联关系的数据模型，数据类型定义    
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
    public class ElementRelation
    {

        public ElementRelation() { }

        private int _CP_ID;
        public int CP_ID
        {
            set { _CP_ID = value; }
            get { return _CP_ID; }
        }

        private int _DataTypeID;
        public int DataTypeID
        {
            set { _DataTypeID = value; }
            get { return _DataTypeID; }
        }

        private int _ElementID;//元素号
        public int ElementID
        {
            set { _ElementID = value; }
            get { return _ElementID; }
        }

        private int _ElementOrder;
        public int ElementOrder
        {
            set { _ElementOrder = value; }
            get { return _ElementOrder; }
        }


        private string _ElementName;//元素名，如果：Element01
        public string ElementName
        {
            set { _ElementName = value; }
            get { return _ElementName; }
        }

        private string _ElementNameCN;//元素中文名称，如：天气现象
        public string ElementNameCN
        {
            set { _ElementNameCN = value; }
            get { return _ElementNameCN; }
        }

        private int _ElementCodeType;//transCode表里面的type字段
        public int ElementCodeType
        {
            set { _ElementCodeType = value; }
            get { return _ElementCodeType; }
        }

        private string _ElementCode;
        public string ElementCode
        {
            set { _ElementCode = value; }
            get { return _ElementCode; }
        }


        private string _TransCodeName;//transCode表里面的Name字段
        public string TransCodeName
        {
            set { _TransCodeName = value; }
            get { return _TransCodeName; }
        }


        private string _CodeTypeName;//CodeType表里面的TypeName字段
        public string CodeTypeName
        {
            set { _CodeTypeName = value; }
            get { return _CodeTypeName; }
        }
    }
}
