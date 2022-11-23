using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtility
{
    public static class Common
    {
        public static string SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        public static string SQLCONNSTR = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
    }
}
