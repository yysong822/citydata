using System;
using System.Collections.Generic;
using System.Text;

namespace data2db
{
    class Tmax2List
    {
        string _ReportTime;

        public string ReportTime
        {
            get { return _ReportTime; }
            set { _ReportTime = value; }
        }

        string _Time;

        public string Time
        {
            get { return _Time; }
            set { _Time = value; }
        }

        string _TimeBegin;

        public string TimeBegin
        {
            get { return _TimeBegin; }
            set { _TimeBegin = value; }
        }

        string _TimeOver;

        public string TimeOver
        {
            get { return _TimeOver; }
            set { _TimeOver = value; }
        }
        string _TimeType;

        public string TimeType
        {
            get { return _TimeType; }
            set { _TimeType = value; }
        }

        string _DataType;

        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }

        List<Tmax2Element> _Tmax2EL;

        internal List<Tmax2Element> Tmax2EL
        {
            get { return _Tmax2EL; }
            set { _Tmax2EL = value; }
        }
        
    }

    class Tmax2Element
    {
        string _StationID;

        public string StationID
        {
            get { return _StationID; }
            set { _StationID = value; }
        }

        string _Lon;

        public string Lon
        {
            get { return _Lon; }
            set { _Lon = value; }
        }

        string _Lat;

        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        string _Height;

        public string Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        string _HighTemperature;

        public string HighTemperature
        {
            get { return _HighTemperature; }
            set { _HighTemperature = value; }
        }

        //string _Time;

        //public string Time
        //{
        //    get { return _Time; }
        //    set { _Time = value; }
        //}

        //string _TimeBegin;

        //public string TimeBegin
        //{
        //    get { return _TimeBegin; }
        //    set { _TimeBegin = value; }
        //}

        //string _TimeOver;

        //public string TimeOver
        //{
        //    get { return _TimeOver; }
        //    set { _TimeOver = value; }
        //}
    }

}
