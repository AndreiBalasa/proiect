using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using TrackerLibrary.DataAccess;


namespace TrackerLibrary
{
   public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType DB)
        {


            if (DB == DatabaseType.Sql)
            {
                //create sql connection
                //TODO - seteaza connectorul calumea

                SqlConnector sql = new SqlConnector();
                Connection = sql;
            }

            else if (DB == DatabaseType.TextFile)
            {
                //create file connection

                TextConnector text = new TextConnector();
                Connection = text;
            }




        }

        public static string CnnString(string name)
        {
           return  ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

    }
}
