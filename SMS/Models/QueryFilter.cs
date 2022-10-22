//----------------Coperights----------------
//Developer Name          :               Azam Aftab
//Development Date        :               October 23, 2013
//Class Name              :               Query Filter
//Company Name            :               

//Purpose of class:

//                This class is use for query filtering, if any query contains extra apostrophy(') it
//converts into (&#39) via EncodeQuery method so that the query couldn't break.

//All Rights Reserved
//No part of this website or any of its contents may be reproduced, 
//copied, modified or adapted, without the prior written consent of the author, 
//unless otherwise indicated for stand-alone materials.




//All require namespaces
#region Namespaces
using System;
using System.Data;
using System.Configuration;
#endregion


public class QueryFilter
{
    //Properties and fields
    #region Properties
    public string StrText { get; set; }
    #endregion

    //Class Constructor
    #region Constructor
    public QueryFilter()
    {

    }
    #endregion
    

    public string EncodeQuery(string query)
    {
        
        if (query.Contains("'"))
        {
            query = query.Replace("'", "&#39;");
        }
        StrText = query;
        return StrText;
    }

    public string DecodeQuery(string query)
    {
        if (query.Contains("&#39;"))
        {
            query = query.Replace("&#39;", "'");
        }
        StrText = query;
        return StrText;
    }
}
