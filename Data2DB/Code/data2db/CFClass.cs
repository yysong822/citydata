using System;
using System.Collections.Generic;
using System.Text;

namespace data2db
{
    class CFClass
    {
       
        string _StationID;

        public string StationID
        {
            get { return _StationID; }
            set { _StationID = value; }
        }

        string _ReportTime;

        public string ReportTime
        {
            get { return _ReportTime; }
            set { _ReportTime = value; }
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

        List<CFElement> _CFList;

        internal List<CFElement> CFList
        {
            get { return _CFList; }
            set { _CFList = value; }
        }

    }

    class CFElement
    {
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

        

        

        string _WBegin;

        public string WBegin
        {
            get { return _WBegin; }
            set { _WBegin = value; }
        }

        string _WOver;

        public string WOver
        {
            get { return _WOver; }
            set { _WOver = value; }
        }

        string _WdBegin;

        public string WdBegin
        {
            get { return _WdBegin; }
            set { _WdBegin = value; }
        }

        string _WdOver;

        public string WdOver
        {
            get { return _WdOver; }
            set { _WdOver = value; }
        }

        string _WpBegin;

        public string WpBegin
        {
            get { return _WpBegin; }
            set { _WpBegin = value; }
        }

        string _WpOver;

        public string WpOver
        {
            get { return _WpOver; }
            set { _WpOver = value; }
        }

        string _TpLow;

        public string TpLow
        {
            get { return _TpLow; }
            set { _TpLow = value; }
        }

        string _TpHigh;

        public string TpHigh
        {
            get { return _TpHigh; }
            set { _TpHigh = value; }
        }
    }
}
