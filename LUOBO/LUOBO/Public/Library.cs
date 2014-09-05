using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUOBO.Public
{
    public class Library
    {
        private static Library instance = null;
        private BLL.BLL_SYS_DICT dicBll = new BLL.BLL_SYS_DICT();
        private List<Entity.SYS_DICT> dicitems;
        public static Library Instance()
        {
            if (instance == null)
                instance = new Library();
            return instance;
        }

        public Library()
        {
            getDicList();
        }
        public void getDicList()
        {
            dicitems = dicBll.Select();
        }


        public List<Entity.SYS_DICT> DicList
        {
            get
            {
                return dicitems;
            }
        }

        public List<Entity.SYS_DICT> GetDicByCategory(string category)
        {
            return dicitems.Where(c => c.CATEGORY == category).ToList();
        }

        public string CheckTrueFileName(string path)
        {
            //System.IO.BinaryReader r = new System.IO.BinaryReader(s);
            //string bx = " ";
            //byte buffer;
            //try
            //{
            //    buffer = r.ReadByte();
            //    bx = buffer.ToString();
            //    buffer = r.ReadByte();
            //    bx += buffer.ToString();
            //    //return (CustomEnum.FileExtension)Convert.ToInt32(bx);
            //}
            //catch (Exception exc)
            //{
            //    Console.WriteLine(exc.Message);
            //}
            //return CustomEnum.FileExtension.UNKNOW;
            return System.IO.Path.GetExtension(path);
            ////真实的文件类型
            //Console.WriteLine(bx);
            ////文件名，包括格式
            //Console.WriteLine(System.IO.Path.GetFileName(path));
            ////文件名， 不包括格式
            //Console.WriteLine(System.IO.Path.GetFileNameWithoutExtension(path));
            ////文件格式
            //Console.WriteLine(System.IO.Path.GetExtension(path));
            //Console.ReadLine();
        }
    }
}