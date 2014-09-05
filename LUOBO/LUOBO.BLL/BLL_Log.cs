using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace LUOBO.BLL
{
    public class BLL_Log
    {
        private static BLL_Log logBll = null;
        private static string logPath = ConfigurationSettings.AppSettings["LogPath"];
        StreamWriter logWriter = null;

        public static BLL_Log Instance()
        {
            if (logBll == null)
                logBll = new BLL_Log();

            return logBll;
        }

        public void WriterLog(string text)
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            string filePath = logPath + "OP_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            logWriter = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write));
            logWriter.WriteLine(text);
            logWriter.Close();
        }
    }
}
