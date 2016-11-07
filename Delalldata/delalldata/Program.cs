using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace delalldata
{
    class Program
    {
        static void DelData(string delDays)
        {
            string sql_delCityData;
            int days = 0;

            if (delDays == "?")
            {
                Console.WriteLine("请正确输入1-9的数字！数字代表天数。");
                Console.WriteLine("Example: delcitydata 2");
                Console.WriteLine("Example: delcitydata 5");
                return;
            }
            else
            {
                try
                {
                    days = Convert.ToInt32(delDays);
                }
                catch
                {
                    Console.WriteLine("请正确输入1-9的数字！数字代表删除n天前的数据。");
                    return;
                }

                if (days > 9 || days < 1)
                {
                    Console.WriteLine("请正确输入1-9的数字！数字代表删除n天前的数据。");
                    return;
                }
            }

            SqlConnection MyConn = new SqlConnection("Data Source=(local);Initial Catalog=weatherdata;Integrated Security=SSPI;");
            MyConn.Open();
            SqlCommand MyCmd = new SqlCommand();
            MyCmd.Connection = MyConn;

            try
            {
                string oldDataTime = DateTime.Today.AddDays(-days).ToString();
                sql_delCityData = "DELETE FROM weatherdata.dbo.tb_AllDataRecorde WHERE ReportTime < '" + oldDataTime + "'";
                MyCmd.CommandText = sql_delCityData;
                MyCmd.ExecuteNonQuery();
                Console.WriteLine("已清除数据库记录" + oldDataTime + "以前的所以记录");
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.Message + "\n" + Exc.Data);
                Console.WriteLine("数据库错误，请与管理员联系");
                return;
            }
            finally
            {
                MyConn.Close();
            }
        }




        static int Main(string[] args)
        {
            try
            {
                DelData(args[0]);
            }
            catch
            {
                Console.WriteLine("Using the parameter,please. Using '?' for help");
            }
            return 0;
        }
    }
}
