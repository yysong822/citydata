using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace data2db
{
    class Program
    {
        //时次（TimeType；库表中TT_id字段）
        int[] cfTimeType = new int[] { 6, 8, 12, 16, 20 };
        int[] d7TimeType = new int[] { 8, 20 };
        int[] h3TimeType = new int[] { 8, 20 };
        int[] indexTimeType = new int[] { 8, 20 };

        //实况数据3小时一次，不是每小时都有，使用时次和3小时数据一样
        int[] skTimeType = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        int[] URPTimeType = new int[] { 12, 17 };
        int[] hqTimeType = new int[] { 8, 14, 20 };

        //20120829 syy 增加h6TimeType
        int[] h6TimeType = new int[] { 8, 20 };

        //时效（文件命中“.”后面的数字；库表中Time字段）
        static int[] cfTime = new int[] { 24, 36, 48, 72, 96, 120, 144, 168 };
        //重复了24和36时效，为了拼接出一个36时效的数据
        static int[] china_SEVP_Time = new int[] { 12, 24, 24, 36, 36, 48, 60, 72, 84, 96, 108, 120, 132, 144, 156, 168 };


        static int[] d7Time = new int[] { 24, 48, 72, 96, 120, 144, 168 };
        static int[] h3Time = new int[] { 3, 6, 9, 12, 15, 18, 21, 24 };
        static int[] indexTime = new int[] { 24, 48 };
        static int[] skTime = new int[] { 3 };//实况数据的时效没有意义
        static int[] skTimeSetting = new int[] { 3 };//实况数据的时效没有意义
        static int[] URPTime = new int[] { 24 };
        static int[] hqTime = new int[] { 24, 48, 72 };

        //20120829 syy 增加h6Time
        static int[] h6Time = new int[] { 6, 12, 18, 24 };

        static String connectionString = "Data Source=.;Initial Catalog=weatherdata;Integrated Security=SSPI;";
        static String CurrentDateTime = DateTime.Now.ToString("yyMMdd");//20131028 syy Tmax2也使用该变量
        static String CurrentDateTimeSK = DateTime.Now.ToString("MMddHH");
        static String CurrentDateTimeSKsetting = DateTime.Now.ToString("MMdd");
        //20120830 syy 增加CurrentDateTime6H“20120830”
        static String CurrentDateTime6H = DateTime.Now.ToString("yyyyMMdd");

        static String CurrentDateTimeSEVP = DateTime.Now.ToString("yyyyMMdd");

        static void DBPro(string parameter)
        {
            string flg = "";//数据类型（cf\sk\3h\7d\index）
            string file = "";//要处理的长文件名

            string[] newParameter = parameter.Split('-');
            flg = newParameter[1].Trim();
            file = newParameter[2].Trim();
            

            switch (flg)
            {

                case "cfSEVPChina":

                    runCFSEVPChina("1", file);
                    //runCF("1", cfTime, file);
                    break;

                case "cfSEVPWorld":

                    runCFSEVPWorld("1", file);
                    //runCF("1", cfTime, file);
                    break;
                
                case "cf":

                    runCF("1", cfTime, file);
                    break;

                case "3h":

                    runCF("2", h3Time, file);
                    break;

                case "7d":

                    runCF("3", d7Time, file);
                    break;

                case "7dSEVP":

                    runCFSEVP7Days("3", file);
                    break;

                case "sk":

                    runSKsetting("4", file);
                    break;

                case "index":

                    runIndex("5", indexTime, file);
                    break;

                case "urp":

                    runURP("6", URPTime, file);
                    break;

                case "hq":

                    runHQ("7", hqTime, file);
                    break;

                case "6h":

                    string reportTimePara = newParameter[3].Trim();
                    run6H("8", h6Time, file, reportTimePara);
                    break;

                case "tmax2":

                    runTmax2("9", file);
                    break;

                default:
                    Console.WriteLine("数据出错了！！\n");
                    Console.WriteLine("数据出错了！！\n");
                    break;
            }
        }

        //cf\3h\7d数据格式相同，时效各有不同，但是程序处理相同
        public static void runCF(string dtype, int[] times, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            int sx;//时效
            string timeBegin = "";//开始时间
            string timeOver = "";//结束时间

            string stLine;
            string sql_citydata;
            string sql_delNewCityData;

            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            //数据类型，区分cf、sk等类型
            //string dtype = "1";
            string wBegin, wOver;//天气现象
            string wdBegin, wdOver;//风向
            string wpBegin, wpOver;//风力
            string tpLow, tpHigh;//最低、最高温度

            newFile = newFile.Replace("######", CurrentDateTime);
            string tempFileName = newFile;


            for (int i = 0; i < times.Length; i++)
            {
                newFile = tempFileName;
                newFile = newFile.Replace("???", times[i].ToString("D3"));
                if (File.Exists(newFile))
                {
                    string[] newFileName = newFile.Split('.');//newFileName[0]文件名，newFileName[1]扩展名

                    //时间类型
                    string timeType = newFileName[0].Substring(newFileName[0].Length - 2, 2);//

                    //发布时间
                    reportTime = DateTime.Now.ToShortDateString() + " " + timeType + ":00:00";

                    //时效
                    sx = times[i];

                    if (dtype == "2")
                    {
                        timeBegin = reportTime;
                        //TimeOver
                        timeOver = Convert.ToDateTime(reportTime).AddHours(sx).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    try
                    {
                        StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                        Console.WriteLine("正在读取数据：" + newFile);
                        Console.WriteLine("\n");

                        stLine = srReader.ReadLine();

                        string[] newString = stLine.Split(' ');

                        if (dtype != "2")
                        {
                            //TimeBegin
                            timeBegin = Convert.ToDateTime(reportTime).AddHours(sx - 24).ToString("yyyy") + "-" + newString[1].Substring(1, 2) + "-" + newString[1].Substring(3, 2) + " " + newString[3].Substring(1, 2) + ":00:00";
                            //TimeOver
                            timeOver = Convert.ToDateTime(reportTime).AddHours(sx).ToString("yyyy") + "-" + newString[2].Substring(1, 2) + "-" + newString[2].Substring(3, 2) + " " + newString[3].Substring(3, 2) + ":00:00";
                        }

                        stLine = srReader.ReadLine();

                        sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reportTime + "' AND Time='" + sx + "' and DataType=" + dtype + " and TimeType=" + timeType;
                        MyCmd.CommandText = sql_delNewCityData;
                        MyCmd.ExecuteNonQuery();
                        Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);

                        int count = 1;
                        while (stLine != null && stLine != "")
                        {
                            string[] newCF = stLine.Split(' ');
                            //天气现象
                            wBegin = newCF[1].Substring(1, 2);
                            wOver = newCF[1].Substring(3, 2);
                            //风向
                            wdBegin = newCF[2].Substring(1, 1);
                            wdOver = newCF[2].Substring(2, 1);
                            //风力
                            wpBegin = newCF[2].Substring(3, 1);
                            wpOver = newCF[2].Substring(4, 1);
                            //高低气温
                            tpLow = newCF[3].Substring(1, 2);//低
                            tpHigh = newCF[3].Substring(3, 2);//高

                            sql_citydata = "INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, Element01, Element02, Element03, Element04, Element05, Element06, Element07, Element08) VALUES (" + newCF[0].Trim() + ",'" + sx.ToString() + "','" + reportTime + "','" + timeBegin + "','" + timeOver + "','" + timeType + "','" + dtype + "','" + wBegin + "','" + wOver + "','" + wdBegin + "','" + wdOver + "','" + wpBegin + "','" + wpOver + "','" + tpLow + "','" + tpHigh + "')";
                            MyCmd.CommandText = sql_citydata;
                            MyCmd.ExecuteNonQuery();
                            Console.WriteLine("插入数据库记录：" + count);
                            count++;

                            stLine = srReader.ReadLine();
                        }
                        srReader.Close();

                    }
                    catch (Exception Exc)
                    {
                        Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                    }
                }
            }
        }


        /// <summary>
        /// CF SEVP China
        /// </summary>
        /// <param name="dtype"></param>
        /// <param name="newFile"></param>
        public static void runCFSEVPChina(string dtype, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            string timeType = "";//从参数得到。06.08.12.16.20

            string stLine;


            newFile = newFile.Replace("########", CurrentDateTimeSEVP);
            

            //SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_20130707160016812.TXT
            if (File.Exists(newFile))
            {
                //制作一个大的list后一次性提交处理数据库
                List<CFClass> cfChina = new List<CFClass>();

                int underscodeIndex = newFile.LastIndexOf("_");
                string date_file = newFile.Substring(underscodeIndex + 1, 12) + "00";
                reportTime = date_file.Substring(0, 4) + "-" + date_file.Substring(4, 2) + "-" + date_file.Substring(6, 2) + " "
                    + date_file.Substring(8, 2) + ":" + date_file.Substring(10, 2) + ":" + date_file.Substring(12, 2);

                //06,08,12,16,20
                timeType = date_file.Substring(8, 2);

                try
                {
                    StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                    Console.WriteLine("正在读取数据：" + newFile);
                    Console.WriteLine("\n");

                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();

                    //syy 第四行取timebegin，确定是从20点启报还是，8点的启报
                    //00 标示08,12标示20，格林威治时间+8
                    string[] newString = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string timeBeginFlag = newString[1].Substring(newString[1].Length - 2, 2);
                    if (timeBeginFlag == "00")
                    {
                        timeBeginFlag = "08";
                    }
                    if (timeBeginFlag == "12")
                    {
                        timeBeginFlag = "20";
                    }

                    string timeBO = reportTime.Substring(0, 10) + " " + timeBeginFlag + ":00:00";

                    stLine = srReader.ReadLine();
                    //syy 第六行开始，为站点号，并以此为界开始15行记录的循环
                    stLine = srReader.ReadLine();

                    CFClass cfOne = new CFClass();

                    while (stLine != null && stLine != "")
                    {
                        newString = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (newString.Length == 1 && newString[0] == "NNNN")
                        {
                            Console.WriteLine("文件读取结束");
                            break;
                        }

                        if (newString.Length == 6)
                        {
                            cfOne = new CFClass();

                            cfOne.StationID = newString[0];
                            cfOne.ReportTime = reportTime;
                            cfOne.TimeType = timeType;
                            cfOne.DataType = dtype;

                            cfOne.CFList = new List<CFElement>();
                        }

                        //syy 第七行开始(循环的第二行)，为各个时次的信息，12、24、36、48、60、72、84、96、108、120、132、144、156、168
                        //时效（文件命中“.”后面的数字；库表中Time字段）
                        //static int[] cfTime = new int[] { 24, 36, 48, 72, 96, 120, 144, 168 };
                        string[] newCF01;
                        string[] newCF02;

                        stLine = srReader.ReadLine();
                        while (stLine != null && stLine != "")
                        {
                            newCF01 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (newCF01.Length > 6)
                            {
                                stLine = srReader.ReadLine();

                                newCF02 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                
                                CFElement cf = new CFElement();
                                cf = formatCF(newCF01, newCF02, timeBO);
                                cfOne.CFList.Add(cf);

                                stLine = srReader.ReadLine();
                            }
                            else
                            {

                                //拼接出36时效
                                CFElement cf24 = cfOne.CFList[0];
                                CFElement cf48 = cfOne.CFList[1];

                                CFElement cf36 = formCF36(cf24, cf48, timeBeginFlag, timeBO);

                                cfOne.CFList.Add(cf36);

                                //制作一个大的list后一次性提交处理数据库
                                cfChina.Add(cfOne);
                                
                                break;
                            }
                        }
                        
                    }
                    if (cfChina != null && cfChina.Count > 0)
                    {
                        operateSQL(cfChina, "0");
                    }
                    else
                    {
                        Console.WriteLine("数据源异常");
                    }
                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                }
            }
            else
            {
                Console.WriteLine("数据源文件不存在");
            }
        }

        /// <summary>
        /// CF SEVP World
        /// </summary>
        /// <param name="dtype"></param>
        /// <param name="newFile"></param>
        public static void runCFSEVPWorld(string dtype, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            //string timeType = "";//从文件名得到。08.20

            string stLine;

            string timeType = newFile.Substring(newFile.LastIndexOf("\\") - 2, 2);

            if (timeType == "06")
            {
                string yesterday = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                newFile = newFile.Replace("########", yesterday);
            }
            else
            {
                newFile = newFile.Replace("########", CurrentDateTimeSEVP);
            }

            //string dtype = "1";

            //SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_20130722000016812.TXT
            if (File.Exists(newFile))
            {

                //制作一个大的list后一次性提交处理数据库
                List<CFClass> cfWorld = new List<CFClass>();

                reportTime = CurrentDateTimeSEVP.Substring(0, 4) + "-" + CurrentDateTimeSEVP.Substring(4, 2) + "-" + CurrentDateTimeSEVP.Substring(6, 2) + " " + timeType + ":00:00";

                try
                {
                    StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                    Console.WriteLine("正在读取数据：" + newFile);
                    Console.WriteLine("\n");

                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();

                    //syy 第四行取timebegin，确定是从20点启报还是，8点的启报
                    //00 标示08,12标示20，格林威治时间+8
                    string[] newString = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string timeBeginFlag = newString[1].Substring(newString[1].Length - 2, 2);
                    if (timeBeginFlag == "00")
                    {
                        timeBeginFlag = "08";
                    }
                    if (timeBeginFlag == "12")
                    {
                        timeBeginFlag = "20";
                    }

                    string timeBO = reportTime.Substring(0, 10) + " " + timeBeginFlag + ":00:00";

                    stLine = srReader.ReadLine();
                    //syy 第六行开始，为站点号，并以此为界开始15行记录的循环
                    stLine = srReader.ReadLine();

                    CFClass cfOne = new CFClass();
                    while (stLine != null && stLine != "")
                    
                    {
                        newString = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (newString.Length == 1 && newString[0] == "NNNN")
                        {
                            Console.WriteLine("文件读取结束");
                            break;
                        }


                        if (newString.Length == 6)
                        {
                            cfOne = new CFClass();
                            cfOne.StationID = newString[0];
                            if (timeType == "08" || timeType == "12" || timeType == "20")
                            {
                                if (cfOne.StationID == "59554")
                                {
                                    for (int i = 0; i < 28; i++)
                                    {
                                        stLine = srReader.ReadLine();
                                    }

                                    stLine = srReader.ReadLine();
                                    continue;
                                }
                            }

                            cfOne.ReportTime = reportTime;
                            cfOne.TimeType = timeType;
                            cfOne.DataType = dtype;

                            cfOne.CFList = new List<CFElement>();
                        }

                        //syy 读取12、24、36、48、60、72、84、96、108、120、132、144、156、168行的数据，组成入库数据
                        //时效（文件命中“.”后面的数字；库表中Time字段）
                        //static int[] cfTime = new int[] { 24, 36, 48, 72, 96, 120, 144, 168 };

                        stLine = srReader.ReadLine();//3
                        stLine = srReader.ReadLine();//6
                        stLine = srReader.ReadLine();//9
                        stLine = srReader.ReadLine();//12
                        string[] newCF12 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        stLine = srReader.ReadLine();//15
                        stLine = srReader.ReadLine();//18
                        stLine = srReader.ReadLine();//21
                        stLine = srReader.ReadLine();//24
                        string[] newCF24 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        CFElement cf24 = new CFElement();
                        cf24 = formatCF(newCF12, newCF24, timeBO);
                        cfOne.CFList.Add(cf24);

                        stLine = srReader.ReadLine();//27
                        stLine = srReader.ReadLine();//30
                        stLine = srReader.ReadLine();//33
                        stLine = srReader.ReadLine();//36
                        string[] newCF36 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        stLine = srReader.ReadLine();//39
                        stLine = srReader.ReadLine();//42
                        stLine = srReader.ReadLine();//45
                        stLine = srReader.ReadLine();//48
                        string[] newCF48 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        CFElement cf48 = new CFElement();
                        cf48 = formatCF(newCF36, newCF48, timeBO);
                        cfOne.CFList.Add(cf48);

                        stLine = srReader.ReadLine();//54
                        stLine = srReader.ReadLine();//60
                        string[] newCF60 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        stLine = srReader.ReadLine();//66
                        stLine = srReader.ReadLine();//72
                        string[] newCF72 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        CFElement cf72 = new CFElement();
                        cf72 = formatCF(newCF60, newCF72, timeBO);
                        cfOne.CFList.Add(cf72);

                        string[] newCF01;
                        string[] newCF02;

                        stLine = srReader.ReadLine();
                        while (stLine != null && stLine != "")
                        {
                            newCF01 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (newCF01.Length > 6)
                            {
                                stLine = srReader.ReadLine();

                                newCF02 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                CFElement cf = new CFElement();
                                cf = formatCF(newCF01, newCF02, timeBO);
                                cfOne.CFList.Add(cf);

                                stLine = srReader.ReadLine();
                            }
                            else
                            {
                                //拼接出36时效

                                CFElement cf36 = formCF36(cf24, cf48, timeBeginFlag, timeBO);

                                cfOne.CFList.Add(cf36);

                                //制作一个大的list后一次性提交处理数据库
                                cfWorld.Add(cfOne);

                                break;
                            }
                        }
                    }
                    if (cfWorld != null && cfWorld.Count > 0)
                    {
                        operateSQL(cfWorld, "1");
                    }
                    else
                    {
                        Console.WriteLine("数据源异常");
                    }

                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                }
            }
            else
            {
                Console.WriteLine("数据源文件不存在");
            }
        }

        /// <summary>
        /// CF SEVP 7Days
        /// </summary>
        /// <param name="dtype"></param>
        /// <param name="newFile"></param>
        public static void runCFSEVP7Days(string dtype, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            string timeType = "";//从文件名得到。08.20

            string stLine;

            newFile = newFile.Replace("########", CurrentDateTimeSEVP);

            //string dtype = "1";

            //SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_20130722000016812.TXT
            if (File.Exists(newFile))
            {

                //制作一个大的list后一次性提交处理数据库
                List<CFClass> cf7Days = new List<CFClass>();

                try
                {
                    StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                    Console.WriteLine("正在读取数据：" + newFile);
                    Console.WriteLine("\n");

                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();

                    //syy 第四行取timebegin，确定是从20点启报还是，8点的启报
                    //00 标示08,12标示20，格林威治时间+8
                    string[] newString = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string timeBeginFlag = newString[1].Substring(newString[1].Length - 2, 2);
                    if (timeBeginFlag == "00")
                    {
                        timeBeginFlag = "08";
                    }
                    if (timeBeginFlag == "12")
                    {
                        timeBeginFlag = "20";
                    }

                    reportTime = CurrentDateTimeSEVP.Substring(0, 4) + "-" + CurrentDateTimeSEVP.Substring(4, 2) + "-" + CurrentDateTimeSEVP.Substring(6, 2) + " " + timeBeginFlag + ":00:00";
                    string timeBO = reportTime;

                    timeType = timeBeginFlag;


                    stLine = srReader.ReadLine();
                    //syy 第六行开始，为站点号，并以此为界开始15行记录的循环
                    stLine = srReader.ReadLine();

                    CFClass cfOne = new CFClass();
                    while (stLine != null && stLine != "")
                    {
                        newString = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (newString.Length == 1 && newString[0] == "NNNN")
                        {
                            Console.WriteLine("文件读取结束");
                            break;
                        }


                        if (newString.Length == 6)
                        {
                            cfOne = new CFClass();
                            cfOne.StationID = newString[0];
                            cfOne.ReportTime = reportTime;
                            cfOne.TimeType = timeType;
                            cfOne.DataType = dtype;

                            cfOne.CFList = new List<CFElement>();
                        }

                        //syy 读取12、24、36、48、60、72、84、96、108、120、132、144、156、168行的数据，组成入库数据
                        //时效（文件命中“.”后面的数字；库表中Time字段）
                        //static int[] cfTime = new int[] { 24, 36, 48, 72, 96, 120, 144, 168 };

                        stLine = srReader.ReadLine();//3
                        stLine = srReader.ReadLine();//6
                        stLine = srReader.ReadLine();//9
                        stLine = srReader.ReadLine();//12
                        string[] newCF12 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        stLine = srReader.ReadLine();//15
                        stLine = srReader.ReadLine();//18
                        stLine = srReader.ReadLine();//21
                        stLine = srReader.ReadLine();//24
                        string[] newCF24 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        CFElement cf24 = new CFElement();
                        cf24 = formatCF(newCF12, newCF24, timeBO);
                        cfOne.CFList.Add(cf24);

                        stLine = srReader.ReadLine();//27
                        stLine = srReader.ReadLine();//30
                        stLine = srReader.ReadLine();//33
                        stLine = srReader.ReadLine();//36
                        string[] newCF36 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        stLine = srReader.ReadLine();//39
                        stLine = srReader.ReadLine();//42
                        stLine = srReader.ReadLine();//45
                        stLine = srReader.ReadLine();//48
                        string[] newCF48 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        CFElement cf48 = new CFElement();
                        cf48 = formatCF(newCF36, newCF48, timeBO);
                        cfOne.CFList.Add(cf48);

                        stLine = srReader.ReadLine();//54
                        stLine = srReader.ReadLine();//60
                        string[] newCF60 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        stLine = srReader.ReadLine();//66
                        stLine = srReader.ReadLine();//72
                        string[] newCF72 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        CFElement cf72 = new CFElement();
                        cf72 = formatCF(newCF60, newCF72, timeBO);
                        cfOne.CFList.Add(cf72);

                        string[] newCF01;
                        string[] newCF02;
                        stLine = srReader.ReadLine();
                        while (stLine != null && stLine != "")
                        {
                            newCF01 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (newCF01.Length > 6)
                            {
                                stLine = srReader.ReadLine();

                                newCF02 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                CFElement cf = new CFElement();
                                cf = formatCF(newCF01, newCF02, timeBO);
                                cfOne.CFList.Add(cf);

                                stLine = srReader.ReadLine();
                            }
                            else
                            {

                                //制作一个大的list后一次性提交处理数据库
                                cf7Days.Add(cfOne);

                                break;
                            }

                        }
                    }

                    if (cf7Days != null && cf7Days.Count > 0)
                    {
                        operateSQL(cf7Days, "2");
                    }
                    else
                    {
                        Console.WriteLine("数据源异常");
                    }
                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                }
            }
            else
            {
                Console.WriteLine("数据源文件不存在");
            }
        }

        

        /// <summary>
        /// 999,转为99
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string format999(string data)
        {
            string returnStr = data;
            if (data == "999")
            {
                returnStr = "99";
            }
            return returnStr;
        }

        /// <summary>
        /// 用两行数据拼接成一个CF数据。
        /// </summary>
        /// <param name="newCF01">天气、风向、风力</param>
        /// <param name="newCF02">转天气、转风向、转风力、高温、低温、时效</param>
        /// <param name="timeBO"></param>
        /// <returns></returns>
        private static CFElement formatCF(string[] newCF01, string[] newCF02, string timeBO)
        {
            CFElement cf = new CFElement();
            //从newCF01中取值
            //天气现象//19
            //cf.WBegin = format999(newCF01[19].Substring(0, newCF01[19].Length - 2));
            cf.WBegin = formatW(newCF01[19], "0");

            //风向//20
            cf.WdBegin = formatW(newCF01[20], "1");
            //风力//21
            cf.WpBegin = formatW(newCF01[21], "2");

            //从newCF02中取值
            //时效
            cf.Time = newCF02[0];

            //天气现象//19
            cf.WOver = formatW(newCF02[19], "0");

            //风向//20
            cf.WdOver = formatW(newCF02[20], "1");
            //风力//21
            cf.WpOver = formatW(newCF02[21], "2");

            //温度//11//高温
            cf.TpHigh = formatTp(newCF02[11]);
            //温度//12//低温
            cf.TpLow = formatTp(newCF02[12]);


            //TimeBegin
            cf.TimeBegin = Convert.ToDateTime(timeBO).AddHours(Convert.ToInt32(cf.Time) - 24).ToString("yyyy-MM-dd HH:mm:ss");
            //TimeOver
            cf.TimeOver = Convert.ToDateTime(timeBO).AddHours(Convert.ToInt32(cf.Time)).ToString("yyyy-MM-dd HH:mm:ss");

            return cf;
        }
        /// <summary>
        /// 处理温度。999.9缺测，录入数据库99。小数四舍五入。负数用50-。
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        private static string formatTp(string tp)
        {
            string returnTp = "";
            if (tp == "999.9")
            {
                returnTp = "99";
                return returnTp;
            }
            try
            {
                int tp_int = Convert.ToInt32(Convert.ToDouble(tp));
                if (tp_int < 0)
                {
                    tp_int = 50 - (tp_int);
                }
                returnTp = tp_int + "";
            }
            catch
            {
                returnTp = format999(tp.Substring(0, tp.Length - 2));
            }

            return returnTp;
        }

        /// <summary>
        /// 处理天气现象、风力、风向。“0” 天气现象；“1” 风力；“2” 风向；
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private static string formatW(string w,string flg)
        {
            string returnW = "";
            if (w == "999.9")
            {
                returnW = "99";
                return returnW;
            }
            try
            {
                int w_int = Convert.ToInt32(Convert.ToDouble(w));
                switch (flg)
                {
                    case "0":

                        //21-28天气现象为W转W。将其采用后一种天气现象代替。
                        //21-25 雨类天气现象
                        if (w_int >= 21 && w_int <= 25)
                        {
                            w_int = w_int - 13; //-13对应处理成雨类天气现象
                        }
                        //26-28 雪类天气现象
                        if (w_int >= 26 && w_int <= 28)
                        {
                            w_int = w_int - 11; //-11对应处理成雪类天气现象
                        }    
                    
                        //天气现象入库为两位，不足两位的补足两位
                        string w_s = w_int.ToString();

                        if (w_s.Length == 1)
                        {
                            w_s = "0" + w_s;
                        }

                        returnW = w_s;

                        break;

                    case "1":

                        returnW = w_int.ToString();
                        break;

                    case "2":

                        returnW = w_int.ToString();
                        break;

                    default:
                        returnW = w_int.ToString();
                        break;
                }
            }
            catch
            {
                returnW = format999(w.Substring(0, w.Length - 2));
            }

            return returnW;
        }

        /// <summary>
        /// 用24和48拼接成36
        /// </summary>
        /// <param name="cf24"></param>
        /// <param name="cf48"></param>
        /// <param name="timeBeginFlag"></param>
        /// <param name="timeBO"></param>
        /// <returns></returns>
        private static CFElement formCF36(CFElement cf24, CFElement cf48, string timeBeginFlag, string timeBO)
        {
            CFElement cf36 = new CFElement();

            cf36.Time = "36";

            //天气现象//24转48
            cf36.WBegin = cf24.WOver;
            cf36.WOver = cf48.WBegin;
            //风向//24转48
            cf36.WdBegin = cf24.WdOver;
            cf36.WdOver = cf48.WdBegin;
            //风力//24转48
            cf36.WpBegin = cf24.WpOver;
            cf36.WpOver = cf48.WpBegin;

            cf36.TimeBegin = Convert.ToDateTime(timeBO).AddHours(12).ToString("yyyy-MM-dd HH:mm:ss");
            cf36.TimeOver = Convert.ToDateTime(timeBO).AddHours(36).ToString("yyyy-MM-dd HH:mm:ss");

            if (timeBeginFlag == "08")
            {
                //温度//24低温，48高温

                string tem01 = cf24.TpLow;
                string tem02 = cf48.TpHigh;

                try
                {
                    int tem01_i = Convert.ToInt32(tem01);
                    int tem02_i = Convert.ToInt32(tem02);
                    if (tem01_i <= tem02_i)
                    {
                        cf36.TpLow = tem01_i.ToString();

                        cf36.TpHigh = tem02_i.ToString();
                    }
                    else
                    {
                        cf36.TpLow = tem02_i.ToString();

                        cf36.TpHigh = tem01_i.ToString();
                    }
                }
                catch
                {
                    //温度////24低温
                    cf36.TpLow = cf24.TpLow;

                    //温度////48高温
                    cf36.TpHigh = cf48.TpHigh;
                }
            }
            else
            {

                //温度////24高温，48低

                string tem01 = cf24.TpHigh;
                string tem02 = cf48.TpLow;

                try
                {
                    int tem01_i = Convert.ToInt32(tem01);
                    int tem02_i = Convert.ToInt32(tem02);
                    if (tem01_i <= tem02_i)
                    {
                        cf36.TpLow = tem01_i.ToString();

                        cf36.TpHigh = tem02_i.ToString();
                    }
                    else
                    {
                        cf36.TpLow = tem02_i.ToString();

                        cf36.TpHigh = tem01_i.ToString();
                    }
                }
                catch
                {
                    //温度////48低温
                    cf36.TpLow = cf48.TpLow;

                    //温度////24高温
                    cf36.TpHigh = cf24.TpHigh;
                }
            }

            return cf36;
        }

        public static string formatLen(string temp)
        {
            if (temp.Length == 1)
            {
                temp =　"0" + temp;
            }
            return temp;
        }

        /// <summary>
        /// 数据库操作
        /// </summary>
        /// <param name="cfAll"></param>
        /// <param name="dstype"></param>
        public static  void operateSQL(List<CFClass> cfAll, string dstype)
        {
            string reporttime = cfAll[0].ReportTime;
            string datatype = cfAll[0].DataType;
            string timetype = cfAll[0].TimeType;
            
            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;


            string sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reporttime + "' AND DataType=" + datatype + " AND TimeType=" + timetype + " AND DataSourceType=" + dstype;
            MyCmd.CommandText = sql_delNewCityData;
            int deleteCount= MyCmd.ExecuteNonQuery();
            Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);
            Console.WriteLine("已清除数据库记录：" + deleteCount);

            SqlDataAdapter sd = new SqlDataAdapter();
            sd.SelectCommand = new SqlCommand("SELECT  StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType,"
                + " Element01, Element02, Element03, Element04, Element05, Element06, Element07, Element08, DataSourceType FROM tb_AllDataRecorde WHERE 1=0", MyConn);

            DataSet dataset = new DataSet();
            sd.Fill(dataset);


            sd.InsertCommand = new SqlCommand("INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, "
                + "Element01, Element02, Element03, Element04, Element05, Element06, Element07, Element08, DataSourceType) "
                + " values (@StationID, @Time, @ReportTime, @TimeBegin, @TimeOver, @TimeType, @DataType, "
                + "@Element01, @Element02, @Element03, @Element04, @Element05, @Element06, @Element07, @Element08, @DataSourceType);", MyConn);
            sd.InsertCommand.Parameters.Add("@StationID", SqlDbType.Int, 8, "StationID");
            sd.InsertCommand.Parameters.Add("@Time", SqlDbType.Int, 8, "Time");
            sd.InsertCommand.Parameters.Add("@ReportTime", SqlDbType.DateTime, 20, "ReportTime");
            sd.InsertCommand.Parameters.Add("@TimeBegin", SqlDbType.DateTime, 20, "TimeBegin");
            sd.InsertCommand.Parameters.Add("@TimeOver", SqlDbType.DateTime, 20, "TimeOver");
            sd.InsertCommand.Parameters.Add("@TimeType", SqlDbType.Int, 8, "TimeType");
            sd.InsertCommand.Parameters.Add("@DataType", SqlDbType.Int, 8, "DataType");
            sd.InsertCommand.Parameters.Add("@Element01", SqlDbType.NVarChar, 10, "Element01");
            sd.InsertCommand.Parameters.Add("@Element02", SqlDbType.NVarChar, 10, "Element02");
            sd.InsertCommand.Parameters.Add("@Element03", SqlDbType.NVarChar, 10, "Element03");
            sd.InsertCommand.Parameters.Add("@Element04", SqlDbType.NVarChar, 10, "Element04");
            sd.InsertCommand.Parameters.Add("@Element05", SqlDbType.NVarChar, 10, "Element05");
            sd.InsertCommand.Parameters.Add("@Element06", SqlDbType.NVarChar, 10, "Element06");
            sd.InsertCommand.Parameters.Add("@Element07", SqlDbType.NVarChar, 10, "Element07");
            sd.InsertCommand.Parameters.Add("@Element08", SqlDbType.NVarChar, 10, "Element08");
            sd.InsertCommand.Parameters.Add("@DataSourceType", SqlDbType.Int, 8, "DataSourceType");
            sd.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            sd.UpdateBatchSize = 0;

            for (int i = 0; i < cfAll.Count; i++)
            {
                CFClass cfOne = cfAll[i];

                for (int j = 0; j < cfOne.CFList.Count; j++)
                {
                    CFElement cfEl = cfOne.CFList[j];
                    object[] row = { cfOne.StationID, cfEl.Time, cfOne.ReportTime, cfEl.TimeBegin, cfEl.TimeOver, cfOne.TimeType, cfOne.DataType,
                                       cfEl.WBegin, cfEl.WOver, cfEl.WdBegin, cfEl.WdOver, cfEl.WpBegin, cfEl.WpOver, cfEl.TpLow, cfEl.TpHigh, dstype};


                    dataset.Tables[0].Rows.Add(row);
                    if (i % 300 == 0)
                    {
                        sd.Update(dataset.Tables[0]);
                        dataset.Tables[0].Clear();
                        Console.WriteLine("插入数据库记录300条");
                    }
                }
            }

            sd.Update(dataset.Tables[0]);
            dataset.Tables[0].Clear();
            Console.WriteLine("余下记录插入数据库");
            sd.Dispose();
            dataset.Dispose();
            MyConn.Close();
        }


        public static void runSK(string dtype, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            int sx=0;//时效，写入数据库的Time字段为0
            string timeBegin = "";//开始时间
            string timeOver = "";//结束时间

            string stLine;
            string sql_citydata;
            string sql_delNewCityData;

            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            //天气现象
            string wSK;
            //风向
            string wdSK;
            //风速
            string wpSK;
            //摄氏温度
            string tSK;
            //湿度
            string hSK;
            //海里/小时
            string wsSK;

            newFile = newFile.Replace("######", CurrentDateTimeSK);

            if (File.Exists(newFile))
            {
                string[] newFileName = newFile.Split('.');//newFileName[0]文件名，newFileName[1]扩展名

                //时间类型
                string timeType = newFileName[0].Substring(newFileName[0].Length - 2, 2);//

                //发布时间
                reportTime = DateTime.Now.ToShortDateString() + " " + timeType + ":00:00";

                timeBegin = reportTime;
                timeOver = reportTime;

                try
                {
                    StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                    Console.WriteLine("正在读取数据：" + newFile);
                    Console.WriteLine("\n");

                    stLine = srReader.ReadLine();

                    stLine = srReader.ReadLine();

                    sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reportTime + "' AND Time='" + sx + "' and DataType=" + dtype + " and TimeType=" + timeType;
                    MyCmd.CommandText = sql_delNewCityData;
                    MyCmd.ExecuteNonQuery();
                    Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);

                    int count = 1;
                    while (stLine != null && stLine != "")
                    {
                        string[] newCF = stLine.Split(' ');
                        //天气现象
                        //wBegin = newCF[1].Substring(1, 2);
                        wSK = newCF[1].Substring(3, 2);

                        //风向
                        wdSK = Convert.ToInt32(newCF[2].Substring(1, 2)).ToString();
                        //风速
                        wpSK = Convert.ToInt32(newCF[2].Substring(3, 2)).ToString();

                        //摄氏温度
                        tSK = Convert.ToInt32(newCF[3].Substring(3, 2)).ToString();

                        //湿度
                        hSK = Convert.ToInt32(newCF[4].Substring(1, 4)).ToString();

                        //海里/小时
                        wsSK = Convert.ToInt32(newCF[5].Substring(1, 4)).ToString();



                        sql_citydata = "INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, Element01, Element02, Element03, Element04, Element05, Element06) VALUES (" + newCF[0].Trim() + ",'" + sx.ToString() + "','" + reportTime + "','" + timeBegin + "','" + timeOver + "','" + timeType + "','" + dtype + "','" + wSK + "','" + wdSK + "','" + wpSK + "','" + tSK + "','" + hSK + "','" + wsSK + "')";
                        MyCmd.CommandText = sql_citydata;
                        MyCmd.ExecuteNonQuery();
                        Console.WriteLine("插入数据库记录：" + count);
                        count++;

                        stLine = srReader.ReadLine();
                    }
                    srReader.Close();

                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                }
            }

        }

        public static void runSKsetting(string dtype, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            int sx = 0;//时效，写入数据库的Time字段为0
            string timeBegin = "";//开始时间
            string timeOver = "";//结束时间

            string stLine;
            string sql_citydata;
            string sql_delNewCityData;

            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            //天气现象
            string wSK;
            //风向
            string wdSK;
            //风速
            string wpSK;
            //摄氏温度
            string tSK;
            //湿度
            string hSK;
            //海里/小时
            string wsSK;


            string[] newFileName = newFile.Split('.');//newFileName[0]文件名，newFileName[1]扩展名

            //时间类型
            string timeType = newFileName[0].Substring(newFileName[0].Length - 2, 2);//
            //发布时间
            reportTime = DateTime.Now.ToShortDateString() + " " + timeType + ":00:00";
            string replacetime = CurrentDateTimeSKsetting;


            if (timeType == "23")
            {
                reportTime = DateTime.Now.AddDays(-1).ToShortDateString() + " " + timeType + ":00:00";
                replacetime = DateTime.Now.AddDays(-1).ToString("MMdd");
            }





            timeBegin = reportTime;
            timeOver = reportTime;




            newFile = newFile.Replace("####", replacetime);

            if (File.Exists(newFile))
            {


                try
                {
                    StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                    Console.WriteLine("正在读取数据：" + newFile);
                    Console.WriteLine("\n");

                    stLine = srReader.ReadLine();

                    stLine = srReader.ReadLine();

                    sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reportTime + "' AND Time='" + sx + "' and DataType=" + dtype + " and TimeType=" + timeType;
                    MyCmd.CommandText = sql_delNewCityData;
                    MyCmd.ExecuteNonQuery();
                    Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);

                    int count = 1;
                    while (stLine != null && stLine != "")
                    {
                        string[] newCF = stLine.Split(' ');
                        //天气现象
                        //wBegin = newCF[1].Substring(1, 2);
                        wSK = newCF[1].Substring(3, 2);

                        //风向
                        wdSK = Convert.ToInt32(newCF[2].Substring(1, 2)).ToString();
                        //风速
                        wpSK = Convert.ToInt32(newCF[2].Substring(3, 2)).ToString();

                        //摄氏温度
                        tSK = Convert.ToInt32(newCF[3].Substring(3, 2)).ToString();

                        //湿度
                        hSK = Convert.ToInt32(newCF[4].Substring(1, 4)).ToString();

                        //海里/小时
                        wsSK = Convert.ToInt32(newCF[5].Substring(1, 4)).ToString();



                        sql_citydata = "INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, Element01, Element02, Element03, Element04, Element05, Element06) VALUES (" + newCF[0].Trim() + ",'" + sx.ToString() + "','" + reportTime + "','" + timeBegin + "','" + timeOver + "','" + timeType + "','" + dtype + "','" + wSK + "','" + wdSK + "','" + wpSK + "','" + tSK + "','" + hSK + "','" + wsSK + "')";
                        MyCmd.CommandText = sql_citydata;
                        MyCmd.ExecuteNonQuery();
                        Console.WriteLine("插入数据库记录：" + count);
                        count++;

                        stLine = srReader.ReadLine();
                    }
                    srReader.Close();

                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                }
            }

        }

        //指数zu文件
        public static void runIndex(string dtype, int[] times, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            int sx;//时效
            string timeBegin = "";//开始时间
            string timeOver = "";//结束时间

            string stLine;
            string sql_citydata;
            string sql_delNewCityData;

            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            string elm1, elm2, elm3, elm4, elm5, elm6, elm7, elm8;

            newFile = newFile.Replace("######", CurrentDateTime);
            string tempFileName = newFile;

            for (int i = 0; i < times.Length; i++)
            {
                newFile = tempFileName;
                newFile = newFile.Replace("???", times[i].ToString("D3"));
                if (File.Exists(newFile))
                {
                    string[] newFileName = newFile.Split('.');//newFileName[0]文件名，newFileName[1]扩展名

                    //时间类型
                    string timeType = newFileName[0].Substring(newFileName[0].Length - 2, 2);//

                    //发布时间
                    reportTime = DateTime.Now.ToShortDateString() + " " + timeType + ":00:00";

                    //时效
                    sx = times[i];

                    try
                    {
                        StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                        Console.WriteLine("正在读取数据：" + newFile);
                        Console.WriteLine("\n");

                        stLine = srReader.ReadLine();

                        string[] newString = stLine.Split(' ');

                            //TimeBegin
                            timeBegin = Convert.ToDateTime(reportTime).AddHours(sx - 24).ToString("yyyy") + "-" + newString[1].Substring(1, 2) + "-" + newString[1].Substring(3, 2) + " " + newString[3].Substring(1, 2) + ":00:00";
                            //TimeOver
                            timeOver = Convert.ToDateTime(reportTime).AddHours(sx).ToString("yyyy") + "-" + newString[2].Substring(1, 2) + "-" + newString[2].Substring(3, 2) + " " + newString[3].Substring(3, 2) + ":00:00";


                        stLine = srReader.ReadLine();

                        sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reportTime + "' AND Time='" + sx + "' and DataType=" + dtype + " and TimeType=" + timeType;
                        MyCmd.CommandText = sql_delNewCityData;
                        MyCmd.ExecuteNonQuery();
                        Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);

                        int count = 1;
                        while (stLine != null && stLine != "")
                        {
                            string[] newCF = stLine.Split(' ');

                            elm1 = Convert.ToInt32(newCF[1].Substring(1, 4)).ToString();
                            elm2 = Convert.ToInt32(newCF[2].Substring(1, 4)).ToString();
                            elm3 = Convert.ToInt32(newCF[3].Substring(1, 4)).ToString();
                            elm4 = Convert.ToInt32(newCF[4].Substring(1, 4)).ToString();
                            elm5 = Convert.ToInt32(newCF[5].Substring(1, 4)).ToString();
                            elm6 = Convert.ToInt32(newCF[6].Substring(1, 4)).ToString();
                            elm7 = Convert.ToInt32(newCF[7].Substring(1, 4)).ToString();
                            elm8 = Convert.ToInt32(newCF[8].Substring(1, 4)).ToString();

                            sql_citydata = "INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, Element01, Element02, Element03, Element04, Element05, Element06, Element07, Element08) VALUES (" + newCF[0].Trim() + ",'" + sx.ToString() + "','" + reportTime + "','" + timeBegin + "','" + timeOver + "','" + timeType + "','" + dtype + "','" + elm1 + "','" + elm2 + "','" + elm3 + "','" + elm4 + "','" + elm5 + "','" + elm6 + "','" + elm7 + "','" + elm8 + "')";
                            MyCmd.CommandText = sql_citydata;
                            MyCmd.ExecuteNonQuery();
                            Console.WriteLine("插入数据库记录：" + count);
                            count++;

                            stLine = srReader.ReadLine();
                        }
                        srReader.Close();
                    }
                    catch (Exception Exc)
                    {
                        Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                    }
                }


            }
        }

        //单独紫外线指数，BJ053109.URP
        public static void runURP(string dtype, int[] times, string newFile)//"6", 24, 文件名
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            int sx;//时效
            string timeBegin = "";//开始时间
            string timeOver = "";//结束时间

            string stLine;
            string sql_citydata;
            string sql_delNewCityData;

            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            string elm1;

            newFile = newFile.Replace("####", CurrentDateTimeSKsetting);
            for (int i = 0; i < times.Length; i++)
            {
                if (File.Exists(newFile))
                {
                    string[] newFileName = newFile.Split('.');//newFileName[0]文件名，newFileName[1]扩展名

                    //时间类型
                    string timeType = (Convert.ToInt32(newFileName[0].Substring(newFileName[0].Length - 2, 2))+8).ToString();//

                    //发布时间
                    reportTime = DateTime.Now.ToShortDateString() + " " + timeType + ":00:00";

                    //时效
                    sx = times[i];

                    try
                    {
                        StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                        Console.WriteLine("正在读取数据：" + newFile);
                        Console.WriteLine("\n");

                        stLine = srReader.ReadLine();

                        string[] newString = stLine.Split(' ');

                        //TimeBegin
                        timeBegin = reportTime;
                        //TimeOver
                        timeOver = Convert.ToDateTime(reportTime).AddHours(sx).ToString("yyyy-MM-dd HH:00:00");

                        stLine = srReader.ReadLine();

                        sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reportTime + "' AND Time='" + sx + "' and DataType=" + dtype + " and TimeType=" + timeType;
                        MyCmd.CommandText = sql_delNewCityData;
                        MyCmd.ExecuteNonQuery();
                        Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);

                        int count = 1;
                        while (stLine != null && stLine != "")
                        {
                            string[] newCF = stLine.Split(' ');

                            elm1 = newCF[1].Substring(1, 1);

                            sql_citydata = "INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, Element01) VALUES (" + newCF[0].Trim() + ",'" + sx.ToString() + "','" + reportTime + "','" + timeBegin + "','" + timeOver + "','" + timeType + "','" + dtype + "','" + elm1 + "')";
                            MyCmd.CommandText = sql_citydata;
                            MyCmd.ExecuteNonQuery();
                            Console.WriteLine("插入数据库记录：" + count);
                            count++;

                            stLine = srReader.ReadLine();
                        }
                        srReader.Close();
                    }
                    catch (Exception Exc)
                    {
                        Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                    }
                }
            }
        }

        //海区，hq011714.024
        public static void runHQ(string dtype, int[] times, string newFile)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            int sx;//时效
            string timeBegin = "";//开始时间
            string timeOver = "";//结束时间

            string stLine;
            string sql_citydata;
            string sql_delNewCityData;

            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            newFile = newFile.Replace("####", CurrentDateTimeSKsetting);
            string tempFileName = newFile;
            for (int i = 0; i < times.Length; i++)
            {


                newFile = tempFileName;
                newFile = newFile.Replace("???", times[i].ToString("D3"));


                if (File.Exists(newFile))
                {
                    string[] newFileName = newFile.Split('.');//newFileName[0]文件名，newFileName[1]扩展名

                    //时间类型
                    string timeType = (Convert.ToInt32(newFileName[0].Substring(newFileName[0].Length - 2, 2))).ToString();//

                    //发布时间
                    reportTime = DateTime.Now.ToShortDateString() + " " + timeType + ":00:00";

                    //时效
                    sx = times[i];

                    try
                    {
                        StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                        Console.WriteLine("正在读取数据：" + newFile);
                        Console.WriteLine("\n");

                        stLine = srReader.ReadLine();

                        string[] newString = stLine.Split(' ');

                        //TimeBegin
                        timeBegin = reportTime;
                        //TimeOver
                        timeOver = Convert.ToDateTime(reportTime).AddHours(sx).ToString("yyyy-MM-dd HH:00:00");

                        stLine = srReader.ReadLine();

                        sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reportTime + "' AND Time='" + sx + "' and DataType=" + dtype + " and TimeType=" + timeType;
                        MyCmd.CommandText = sql_delNewCityData;
                        MyCmd.ExecuteNonQuery();
                        Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);

                        int count = 1;
                        while (stLine != null && stLine != "")
                        {
                            string elm1, elm2, elm3, elm4, elm5, elm6;

                            string[] newCF = stLine.Split(' ');

                            elm1 = newCF[1].Substring(1, 2);
                            elm2 = newCF[1].Substring(3, 2);
                            elm3 = newCF[2].Substring(2, 1);
                            elm4 = newCF[2].Substring(4, 1);
                            elm5 = newCF[3].Substring(2, 1);
                            elm6 = newCF[3].Substring(4, 1);

                            sql_citydata = "INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, Element01, Element02, Element03, Element04, Element05, Element06) VALUES (" + newCF[0].Trim() + ",'" + sx.ToString() + "','" + reportTime + "','" + timeBegin + "','" + timeOver + "','" + timeType + "','" + dtype + "','" + elm1 + "','" + elm2 + "','" + elm3 + "','" + elm4 + "','" + elm5 + "','" + elm6 + "')";
                            MyCmd.CommandText = sql_citydata;
                            MyCmd.ExecuteNonQuery();
                            Console.WriteLine("插入数据库记录：" + count);
                            count++;

                            stLine = srReader.ReadLine();
                        }
                        srReader.Close();
                    }
                    catch (Exception Exc)
                    {
                        Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                    }
                }
            }
            //MyConn.Close();

        }

        //1.    6H与CF数据有相同，也有不同。为避免修改runCF引起原程序出现问题，故特增加特定函数run6H
        //2.	与标准CF不同，天气现象、风向、风力只有一个，不存在转的问题，则天气现象用后两位表示，风向、风力用后一位表示。
        //3.	添加标识“9”以表示降水量。0110，表示降水量11.0mm
        //  newFile SEVP_NMC_RFFC_SNWFD6H_EME_ACHN_L88_P9_20120829200002406.txt
        //  newFile SEVP_NMC_RFFC_SNWFD6H_EME_ACHN_L88_P9_########200002406.txt"8个"
        public static void run6H(string dtype, int[] times, string newFile, string reportTimePara)
        {
            string reportTime = "";//发布时间，传给我们的时间，从文件名得到
            string timeBegin = "";//开始时间////syy 20120830启报时间，分08、20//修改08-14，14-20,20-02,02-08
            string timeOver = "";//结束时间
            string timeFlag = ""; //标记是08点数据还是20点数据。0645、1030的是08点数据，1630的是20点数据，读取第4行数据最后两位+8

            //syy 20120830 站点ID
            string sationID = "";

            string stLine;
            string sql_citydata;
            string sql_delNewCityData;

            SqlConnection MyConn = new SqlConnection(connectionString);
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            string weather;//天气现象
            string winddir;//风向
            string windpower;//风力
            string tpLow, tpHigh;//最低、最高温度
            string precipitation;//降水量


            int indexCharacter = newFile.LastIndexOf('#');
            newFile = newFile.Replace("########", CurrentDateTime6H);
            string tempFileName = newFile;

            newFile = tempFileName;
            if (File.Exists(newFile))
            {
                //时间类型//参数传入产生
                string timeType = reportTimePara.Substring(0, 2);//

                string currentDate6H = DateTime.Now.ToString("yyyy-MM-dd");

                //发布时间
                reportTime = currentDate6H + " " + reportTimePara.Substring(0, 2) + ":00:00";

                

                try
                {
                    StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                    Console.WriteLine("正在读取数据：" + newFile);
                    Console.WriteLine("\n");

                    //前7行没有有效数据，从第8行开始读取
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();

                    //syy 更改起始时间。读取第四行数据
                    if (stLine != null)
                    {
                        string[] tempStartTimeLine = stLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (tempStartTimeLine != null && tempStartTimeLine.Length == 2)
                        {
                            //TimeBegin//格林威治时间+8
                            if (tempStartTimeLine[1].Substring(8, 2) == "00")
                            {
                                timeFlag = currentDate6H + " " + "08:00:00";
                            }
                            else
                            {
                                timeFlag = currentDate6H + " " + "20:00:00";
                            }
                        }
                    }


                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();
                    stLine = srReader.ReadLine();

                    //一个时次的只插入一次，删除以避免重复插入
                    //删除语句中where将sx属性去掉，因为与原CF不同，每个有四个时效
                    sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + reportTime + "' AND DataType=" + dtype + " and TimeType=" + timeType;
                    MyCmd.CommandText = sql_delNewCityData;
                    MyCmd.ExecuteNonQuery();
                    Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);

                    int count = 1;
                    int counter = 1;
                    while (stLine != null && stLine != "")
                    {
                        if (counter == 6)
                        {
                            counter = 1;
                        }

                        if (counter == 1)
                        {
                            string[] tempLine = stLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (tempLine != null)
                            {
                                if (tempLine[0] == "NNNN")
                                {
                                    break;
                                }
                                sationID = tempLine[0];
                            }
                        }
                        else
                        {
                            string[] tempLine = stLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (tempLine != null)
                            {
                                string sxStr = tempLine[0];
                                int sx = Convert.ToInt32(sxStr);  //时效

                                //syy 增加修改起始时间，分为08-14，14-20,20-02,02-08
                                switch (sx)
                                {
                                    case 6:
                                        timeBegin = timeFlag;
                                        break;

                                    default:
                                        timeBegin = Convert.ToDateTime(timeFlag).AddHours(sx - 6).ToString("yyyy-MM-dd HH:mm:ss");
                                        break;

                                }

                                timeOver = Convert.ToDateTime(timeFlag).AddHours(sx).ToString("yyyy-MM-dd HH:mm:ss");

                                weather = tempLine[1];//天气现象
                                if (weather.Length == 1)
                                {
                                    weather = "0" + weather;
                                }
                                tpHigh = tempLine[2];//最高温度
                                tpLow = tempLine[3];//最低、
                                winddir = tempLine[4]; ;//风向
                                windpower = tempLine[5]; //风力
                                precipitation = tempLine[6]; ;//降水量

                                sql_citydata = "INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, Element01, Element02, Element03, Element04, Element05, Element06) VALUES ("
                                    + sationID + ",'" + sxStr + "','" + reportTime + "','" + timeBegin + "','" + timeOver + "','" + timeType + "','" + dtype + "','"
                                    + weather + "','" + tpHigh + "','" + tpLow + "','" + winddir + "','" + windpower + "','" + precipitation + "')";
                                MyCmd.CommandText = sql_citydata;
                                MyCmd.ExecuteNonQuery();

                                Console.WriteLine("插入数据库记录：" + count + "----" + sationID + "----" + sxStr + "----" + reportTime);
                            }
                        }

                        count++;
                        counter++;

                        stLine = srReader.ReadLine();
                    }
                    srReader.Close();
                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                }
            }

        }



       /// <summary>
        /// 02时最高温度，output13082602.000
        /// 时效,开始时间,结束时间,为空
       /// </summary>
       /// <param name="dtype"></param>
       /// <param name="times"></param>
       /// <param name="newFile"></param>
        public static void runTmax2(string dtype, string newFile)
        {
            
            newFile = newFile.Replace("######", CurrentDateTime);
            string tempFileName = newFile;

            //时效为空
            if (File.Exists(newFile))
            {
                Tmax2List Tmax2All = new Tmax2List();

                string[] newFileName = newFile.Split('.');//newFileName[0]文件名，newFileName[1]扩展名

                int index = newFileName[0].Length - 8;//8=13082602
                //时间类型
                Tmax2All.TimeType = newFileName[0].Substring(index + 6, 2);
                Tmax2All.Time = "0";

                //发布时间
                //output13082602.000
                //发布时间，传给我们的时间，从文件名得到
                Tmax2All.ReportTime = "20" + newFileName[0].Substring(index, 2) + "-" + newFileName[0].Substring(index + 2, 2) + "-" + newFileName[0].Substring(index + 4, 2)
                    + " " + Tmax2All.TimeType + ":00:00";

                Tmax2All.TimeBegin = Tmax2All.ReportTime;
                Tmax2All.TimeOver = Tmax2All.ReportTime;

                Tmax2All.DataType = dtype;

                string stLine;
                try
                {
                    StreamReader srReader = new StreamReader(newFile, System.Text.Encoding.GetEncoding("gb2312"));
                    Console.WriteLine("正在读取数据：" + newFile);
                    Console.WriteLine("\n");

                    stLine = srReader.ReadLine(); //Weather Central 001d0300 Surface Data TimeStamp=2013.08.26.0200
                    stLine = srReader.ReadLine();//5
                    stLine = srReader.ReadLine();// 5 Station
                    stLine = srReader.ReadLine();// 8 Lon
                    stLine = srReader.ReadLine();// 8 Lat
                    stLine = srReader.ReadLine();//8 Height
                    stLine = srReader.ReadLine();// 8 High Temperature

                    stLine = srReader.ReadLine();//信息开始
                    Tmax2All.Tmax2EL = new List<Tmax2Element>();

                    while (stLine != null && stLine != "")
                    {
                        Tmax2Element tmax2El = new Tmax2Element();

                        string[] newTmax2 = stLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        tmax2El.StationID = newTmax2[0];
                        tmax2El.Lon = newTmax2[1];
                        tmax2El.Lat = newTmax2[2];
                        tmax2El.Height = newTmax2[3];
                        tmax2El.HighTemperature = newTmax2[4];

                        Tmax2All.Tmax2EL.Add(tmax2El);

                        stLine = srReader.ReadLine();
                    }
                    srReader.Close();
                }
                catch (Exception Exc)
                {
                    Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                }

                //操作数据库
                SqlConnection MyConn = new SqlConnection(connectionString);
                MyConn.Open();
                SqlCommand MyCmd = new SqlCommand();
                MyCmd.Connection = MyConn;

                string sql_delNewCityData = "DELETE FROM tb_AllDataRecorde WHERE ReportTime = '" + Tmax2All.ReportTime + "' AND DataType=" + Tmax2All.DataType + " AND TimeType=" + Tmax2All.TimeType;
                MyCmd.CommandText = sql_delNewCityData;
                int deleteCount = MyCmd.ExecuteNonQuery();
                Console.WriteLine("已清除数据库记录：" + sql_delNewCityData);
                Console.WriteLine("已清除数据库记录：" + deleteCount);


                SqlDataAdapter sd = new SqlDataAdapter();
                //sd.SelectCommand = new SqlCommand("SELECT  StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType,"
                //    + " Element01, Element02, Element03, Element04, Element05, Element06, Element07, Element08, DataSourceType FROM tb_AllDataRecorde WHERE 1=0", MyConn);

                //时效,开始时间,结束时间,为空
                sd.SelectCommand = new SqlCommand("SELECT  StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType,"
                    + " Element01, Element02, Element03, Element04 FROM tb_AllDataRecorde WHERE 1=0", MyConn);

                DataSet dataset = new DataSet();
                sd.Fill(dataset);

                sd.InsertCommand = new SqlCommand("INSERT INTO tb_AllDataRecorde (StationID, Time, ReportTime, TimeBegin, TimeOver, TimeType, DataType, "
                + "Element01, Element02, Element03, Element04) "
                + " values (@StationID, @Time, @ReportTime, @TimeBegin, @TimeOver, @TimeType, @DataType, "
                + "@Element01, @Element02, @Element03, @Element04);", MyConn);
                sd.InsertCommand.Parameters.Add("@StationID", SqlDbType.Int, 8, "StationID");
                sd.InsertCommand.Parameters.Add("@Time", SqlDbType.Int, 8, "Time");
                sd.InsertCommand.Parameters.Add("@ReportTime", SqlDbType.DateTime, 20, "ReportTime");
                sd.InsertCommand.Parameters.Add("@TimeBegin", SqlDbType.DateTime, 20, "TimeBegin");
                sd.InsertCommand.Parameters.Add("@TimeOver", SqlDbType.DateTime, 20, "TimeOver");
                sd.InsertCommand.Parameters.Add("@TimeType", SqlDbType.Int, 8, "TimeType");
                sd.InsertCommand.Parameters.Add("@DataType", SqlDbType.Int, 8, "DataType");
                sd.InsertCommand.Parameters.Add("@Element01", SqlDbType.NVarChar, 10, "Element01");
                sd.InsertCommand.Parameters.Add("@Element02", SqlDbType.NVarChar, 10, "Element02");
                sd.InsertCommand.Parameters.Add("@Element03", SqlDbType.NVarChar, 10, "Element03");
                sd.InsertCommand.Parameters.Add("@Element04", SqlDbType.NVarChar, 10, "Element04");

                sd.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                sd.UpdateBatchSize = 0;

                for (int i = 0; i < Tmax2All.Tmax2EL.Count; i++)
                {

                    object[] row = { Tmax2All.Tmax2EL[i].StationID, Tmax2All.Time, Tmax2All.ReportTime, Tmax2All.TimeBegin, Tmax2All.TimeOver, Tmax2All.TimeType, Tmax2All.DataType,
                                       Tmax2All.Tmax2EL[i].Lon, Tmax2All.Tmax2EL[i].Lat, Tmax2All.Tmax2EL[i].Height, Tmax2All.Tmax2EL[i].HighTemperature};


                    dataset.Tables[0].Rows.Add(row);
                    if (i % 300 == 0)
                    {
                        sd.Update(dataset.Tables[0]);
                        dataset.Tables[0].Clear();
                        Console.WriteLine("插入数据库记录300条");
                    }
                }

                sd.Update(dataset.Tables[0]);
                dataset.Tables[0].Clear();
                Console.WriteLine("余下记录插入数据库");
                sd.Dispose();
                dataset.Dispose();
                MyConn.Close();
            }
            else
            {
                Console.WriteLine("文件不存在！");
            }

        }





        static int Main(string[] args)
        {
            //if (args.Length != 0)
            //{
                //try
                {
                    /*DBPro("c:\\cf\\cf08122906.024");
                    DBPro("c:\\cf\\cf08122906.036");
                    DBPro("c:\\cf\\cf08122906.048");
                    DBPro("c:\\cf\\cf08122906.072");

                    DBPro("c:\\cf\\cf08122916.024");
                    DBPro("c:\\cf\\cf08122916.036");
                    DBPro("c:\\cf\\cf08122916.048");
                    DBPro("c:\\cf\\cf08122916.072");
                    DBPro("c:\\cf\\cf08122916.096");
                    DBPro("c:\\cf\\cf08122916.120");
                    DBPro("c:\\cf\\cf08121006.024");
                    DBPro("c:\\cf\\cf08121008.024");
                    DBPro("c:\\cf\\cf08121012.024");*/
                    //DBPro("-7d -cf######08.???");//cf09030308.096
                    //DBPro("-cf -cf######08.???");//
                    //DBPro("-3h -cf######02.???");//
                    //DBPro("-sk-D:\\work\\sk\\sk####23.003");//sk071602.003
                    //DBPro("-index -zu######08.???");
                    //DBPro("-urp-c:\\data\\BJ####04.URP");//BJ053009.URP
                    //DBPro("-hq-c:\\data\\hq####14.???");//hq011714.024
                    //-6h-C:/metodata/citydata/6hours/1630/SEVP_NMC_RFFC_SNWFD6H_EME_ACHN_L88_P9_########200002406.txt-1630


                    //runCFSEVPChina("1", @"D:\SEVP DATA\data\sdata\china\0500\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########060007212.TXT");
                    //runCFSEVPChina("1", @"D:\SEVP DATA\data\sdata\china\0645\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########080016812.TXT");
                    //runCFSEVPChina("1", @"D:\SEVP DATA\data\sdata\china\1030\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########120016812.TXT");
                    //runCFSEVPChina("1", @"D:\SEVP DATA\data\sdata\china\1530\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########160016812.TXT");
                    //runCFSEVPChina("1", @"D:\SEVP DATA\data\sdata\china\1630\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########200016812.TXT");
                    //runCFSEVPChina("1", @"D:\SEVP DATA\data\sdata\china\1710\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########200016812.TXT");

                    //runCFSEVPWorld("1", "06", @"D:\SEVP DATA\data\sdata\world\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########120016812.TXT");
                    //runCFSEVPWorld("1", "08", @"D:\SEVP DATA\data\sdata\world\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########000016812.TXT");
                    //runCFSEVPWorld("1", "12", @"D:\SEVP DATA\data\ddata\world\12\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########000016812.TXT");
                    //runCFSEVPWorld("1", "16", @"D:\SEVP DATA\data\sdata\world\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########000016812.TXT");
                    //runCFSEVPWorld("1", "20", @"D:\SEVP DATA\data\sdata\world\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########120016812.TXT");

                    //runCFSEVP7Days("3", @"D:\SEVP DATA\data\sdata\7days\SEVP_NMC_RFFC_SCMOC_EME_ACHN_L88_P9_########000016812.TXT");
                    //runCFSEVP7Days("3", @"D:\SEVP DATA\data\sdata\7days\SEVP_NMC_RFFC_SCMOC_EME_ACHN_L88_P9_########120016812.TXT");


                    //DBPro("-cfSEVPChina-D:\\SEVP DATA\\data\\ddata\\china\\0500\\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########060007212.TXT");//
                    //DBPro("-cfSEVPChina-D:\\SEVP DATA\\data\\ddata\\china\\0645\\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########080016812.TXT");
                    //DBPro("-cfSEVPChina-D:\\SEVP DATA\\data\\ddata\\china\\1030\\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########120016812.TXT");
                    //DBPro("-cfSEVPChina-D:\\SEVP DATA\\data\\ddata\\china\\1530\\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########160016812.TXT");
                    //DBPro("-cfSEVPChina-D:\\SEVP DATA\\data\\ddata\\china\\1630\\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########200016812.TXT");
                    //DBPro("-cfSEVPChina-D:\\SEVP DATA\\data\\ddata\\china\\1710\\SEVP_NMC_RFFC_SNWFD_EME_ACHN_L88_P9_########200016812.TXT");

                    //DBPro("-cfSEVPWorld-D:\\SEVP DATA\\data\\ddata\\world\\06\\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########120016812.TXT");
                    //DBPro("-cfSEVPWorld-D:\\SEVP DATA\\data\\ddata\\world\\08\\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########000016812.TXT");
                    //DBPro("-cfSEVPWorld-D:\\SEVP DATA\\data\\ddata\\world\\12\\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########000016812.TXT");
                    //DBPro("-cfSEVPWorld-D:\\SEVP DATA\\data\\ddata\\world\\16\\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########000016812.TXT");
                    //DBPro("-cfSEVPWorld-D:\\SEVP DATA\\data\\ddata\\world\\20\\SEVP_NMC_RFFC_SFER_EME_AGLB_LNO_P9_########120016812.TXT");

                    //DBPro("-7dSEVP-D:\\SEVP DATA\\data\\sdata\\7days\\SEVP_NMC_RFFC_SCMOC_EME_ACHN_L88_P9_########000016812.TXT");
                    //DBPro("-7dSEVP-D:\\SEVP DATA\\data\\sdata\\7days\\SEVP_NMC_RFFC_SCMOC_EME_ACHN_L88_P9_########120016812.TXT");

                    //DBPro("-tmax2-D:\\tmax2data\\output######02.000");

                    DBPro(args[0]);
                }
                //catch
                { }
            //}
            //else
            //{/
            //    Console.WriteLine("显示帮助信息\n");
            //}

            return 0;
        }
    }
}
