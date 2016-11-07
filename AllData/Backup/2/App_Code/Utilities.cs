using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Utilities
/// </summary>
public class Utilities
{
    public Utilities()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static String[] RemoveDup(String[] arr)
    {
        String[] ret = null;
        if (arr.Length > 0)
        {
            Array.Sort(arr);    // 数组排序
            int size = 1;
            for (int i = 1; i < arr.Length; i++)    // 计算新数组大小
                if (arr[i] != arr[i - 1])
                    size++;
            ret = new String[size];
            int j = 0;
            ret[j++] = arr[0];
            for (int i = 1; i < arr.Length; i++)
                if (arr[i] != arr[i - 1])
                    ret[j++] = arr[i];
        }
        
        return ret;
    }

}
