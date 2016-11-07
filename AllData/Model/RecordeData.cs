/**************************************************************************************************
  Copyright (C), 2009, 中国气象局华风气象影视信息集团技术部软件开发组
  文件名: RecordeData.cs
  作者: 王晗         版本: 1.0.0          完成日期: 2009.06.01
  程序说明:  关于气象数据库模型，数据类型定义    
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
    public class RecordeData
    {

        public RecordeData() { }




        private string _Element01;
        public string Element01
        {
            set { _Element01 = value; }
            get { return _Element01; }
        }

        private string _Element02;
        public string Element02
        {
            set { _Element02 = value; }
            get { return _Element02; }
        }

        private string _Element03;
        public string Element03
        {
            set { _Element03 = value; }
            get { return _Element03; }
        }

        private string _Element04;
        public string Element04
        {
            set { _Element04 = value; }
            get { return _Element04; }
        }

        private string _Element05;
        public string Element05
        {
            set { _Element05 = value; }
            get { return _Element05; }
        }

        private string _Element06;
        public string Element06
        {
            set { _Element06 = value; }
            get { return _Element06; }
        }

        private string _Element07;
        public string Element07
        {
            set { _Element07 = value; }
            get { return _Element07; }
        }

        private string _Element08;
        public string Element08
        {
            set { _Element08 = value; }
            get { return _Element08; }
        }

        private string _Element09;
        public string Element09
        {
            set { _Element09 = value; }
            get { return _Element09; }
        }

        private int _StationID;
        public int StationID
        {
            set { _StationID = value; }
            get { return _StationID; }
        }

        private int _Time;
        public int Time
        {
            set { _Time = value; }
            get { return _Time; }
        }

        private DateTime _ReportTime;
        public DateTime ReportTime
        {
            set { _ReportTime = value; }
            get { return _ReportTime; }
        }

        private DateTime _BeginTime;
        public DateTime BeginTime
        {
            set { _BeginTime = value; }
            get { return _BeginTime; }
        }

        private DateTime _OverTime;
        public DateTime OverTime
        {
            set { _OverTime = value; }
            get { return _OverTime; }
        }

        private int _TimeType;
        public int TimeType
        {
            set { _TimeType = value; }
            get { return _TimeType; }
        }

        private int _DataType;
        public int DataType
        {
            set { _DataType = value; }
            get { return _DataType; }
        }


        private string _DataTypeName;
        public string DataTypeName
        {
            set { _DataTypeName = value; }
            get { return _DataTypeName; }
        }

        private string _TimeTypeName;
        public string TimeTypeName
        {
            set { _TimeTypeName = value; }
            get { return _TimeTypeName; }
        }

        private int _TimeTypeOrder;
        public int TimeTypeOrder
        {
            set { _TimeTypeOrder = value; }
            get { return _TimeTypeOrder; }
        }

        private int _selectID;
        public int selectID
        {
            set { _selectID = value; }
            get { return _selectID; }
        }

        private int _stationOrder;
        public int stationOrder
        {
            set { _stationOrder = value; }
            get { return _stationOrder; }
        }

        





    }




}
