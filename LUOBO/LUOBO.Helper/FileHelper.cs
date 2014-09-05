using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace LUOBO.Helper
{
    public class FileHelper
    {
        private ArrayList dirs = new ArrayList();
        public ArrayList GetAllFileName(string rootPath)
        {
            dirs.Clear();
            dirs.Add(rootPath);
            GetDirs(rootPath);
            object[] allDir = dirs.ToArray();
            ArrayList list = new ArrayList();
            foreach (object o in allDir)
            {
                list.AddRange(GetFileName(o.ToString()));
            }
            return list;
        }

        private void GetDirs(string dirPath)
        {
            if (Directory.GetDirectories(dirPath).Length > 0)
            {
                foreach (string path in Directory.GetDirectories(dirPath))
                {
                    dirs.Add(path);
                    GetDirs(path);
                }
            }
        }

        private ArrayList GetFileName(string dirPath)
        {
            ArrayList list = new ArrayList();
            if (Directory.Exists(dirPath))
            {
                list.AddRange(Directory.GetFiles(dirPath));
            }
            return list;
        }
    }
}
