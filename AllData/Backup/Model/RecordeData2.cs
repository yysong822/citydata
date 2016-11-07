using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class RecordeData2
    {
        public RecordeData2() { }


        private int _StationID;
        public int StationID
        {
            set { _StationID = value; }
            get { return _StationID; }
        }

        private string _StationName;
        public string StationName
        {
            set { _StationName = value; }
            get { return _StationName; }
        }

        private int _stationOrder;
        public int stationOrder
        {
            set { _stationOrder = value; }
            get { return _stationOrder; }
        }

        // 以下字段可空
        private int? _Time;
        public int? Time
        {
            set { _Time = value; }
            get { return _Time; }
        }

        private DateTime? _ReportTime;
        public DateTime? ReportTime
        {
            set { _ReportTime = value; }
            get { return _ReportTime; }
        }

        private DateTime? _BeginTime;
        public DateTime? BeginTime
        {
            set { _BeginTime = value; }
            get { return _BeginTime; }
        }

        private DateTime? _OverTime;
        public DateTime? OverTime
        {
            set { _OverTime = value; }
            get { return _OverTime; }
        }

        private int? _TimeType;
        public int? TimeType
        {
            set { _TimeType = value; }
            get { return _TimeType; }
        }

        private int? _DataType;
        public int? DataType
        {
            set { _DataType = value; }
            get { return _DataType; }
        }

        // 数据字段
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

    }
}
