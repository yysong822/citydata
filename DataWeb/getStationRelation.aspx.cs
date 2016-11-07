using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Data.SqlClient;

public partial class getStationRelation : System.Web.UI.Page
{
    SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["TEST_AllDataConnectionString"].ConnectionString);


    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void bt_getStation_Click(object sender, EventArgs e)
    {
        string strNew = "";
        string sqlStr = "SELECT [stationid],[name_cn] FROM tb_StationInfo order by [stationid]";
        cn.Open();
        SqlCommand cm = new SqlCommand(sqlStr, cn);
        SqlDataReader rd = cm.ExecuteReader();
        try
        {

            while (rd.Read())
            {
                strNew+=rd["stationid"].ToString() + " " + rd["name_cn"].ToString() + "\r\n";
            }

        }
        catch
        { }

        cm.Dispose();
        rd.Close();
        rd.Dispose();
        cn.Close();
        cn.Dispose();

        writeNewFile(strNew);
    }

    public void writeNewFile(string newstr)
    {
        string path = Server.MapPath("~/Temp/");
        FileInfo f = new FileInfo(path+"stationlist.txt");
        StreamWriter w = f.CreateText();
        w.WriteLine(newstr);
        w.Write(w.NewLine);
        w.Close();
    }

}
