/**************************************************************************************************
  Copyright (C), 2009, 中国气象局华风气象影视信息集团技术部软件开发组
  文件名: StationRelation.cs
  作者: 王晗         版本: 1.0.0          完成日期: 2009.06.01
  程序说明:  关于站点配置关联关系模型，数据类型定义    
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
    public class StationRelation
    {

        public StationRelation() { }


        private int _CP_ID;
        public int CP_ID
        {
            set { _CP_ID = value; }
            get { return _CP_ID; }
        }

        private int _Channel_ID;
        public int Channel_ID
        {
            set { _Channel_ID = value; }
            get { return _Channel_ID; }
        }

        private int _Program_ID;
        public int Program_ID
        {
            set { _Program_ID = value; }
            get { return _Program_ID; }
        }

        private int _StationID;//站点号
        public int StationID
        {
            set { _StationID = value; }
            get { return _StationID; }
        }

        private int _SelectID;//选择站点的序号
        public int SelectID
        {
            set { _SelectID = value; }
            get { return _SelectID; }
        }

        private int _StationOrder;
        public int StationOrder
        {
            set { _StationOrder = value; }
            get { return _StationOrder; }
        }

        private int _StationTableID;//station表的id字段
        public int StationTableID
        {
            set { _StationTableID = value; }
            get { return _StationTableID; }
        }

        private string _StationName;
        public string StationName
        {
            set { _StationName = value; }
            get { return _StationName; }
        }



    }
}
