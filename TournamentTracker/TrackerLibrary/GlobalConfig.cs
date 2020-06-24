using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
   public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections(bool database , bool textFiles)
        {

            if(database)
            {
                //create sql connection
                //TODO - seteaza connectorul calumea

                SqlConnector sql = new SqlConnector();
                Connections.Add(sql);
            }

            if(textFiles)
            {
                //create file connection

                TextConnector text = new TextConnector();
                Connections.Add(text);
            }

        }

    }
}
