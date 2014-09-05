using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using System.Threading;
using LUOBO.Model;
using System.Configuration;
using LUOBO.Entity;

namespace LUOBO.BLL
{
    public class BLL_ZipQueue
    {
        private static BLL_ZipQueue instance = null;
        private static Queue queue = null;
        private static Thread thread = null;
        private bool isRun = true;
        private string root = ConfigurationSettings.AppSettings["UserFile_Path"];

        public static BLL_ZipQueue Instance()
        {
            if (instance == null)
                instance = new BLL_ZipQueue();
            return instance;
        }
        public BLL_ZipQueue()
        {
            queue = new Queue();
            thread = new Thread(ExcuteTar) { IsBackground = true };
            thread.Start();
        }

        public void Push(string ssid_path)
        {
            string zipedPath = ssid_path.Substring(0, ssid_path.LastIndexOf('/', ssid_path.Length - 2, ssid_path.Length - 2)+1);
           
            M_ZipItem zipItem = new M_ZipItem();
            zipItem.zipedPath = root + ssid_path;
            zipItem.toPath = root + zipedPath.Replace("Pub", "Download");

            queue.Enqueue(zipItem);
        }

        private void ExcuteTar()
        {
            M_ZipItem zipItem = null;
            while (isRun)
            {
                zipItem = null;
                zipItem = Pop() as M_ZipItem;
                if (zipItem != null)
                {
                    if (!TarFolder(zipItem.zipedPath, zipItem.toPath))
                        if (!TarFolder(zipItem.zipedPath, zipItem.toPath))
                            if (!TarFolder(zipItem.zipedPath, zipItem.toPath))
                                if (!TarFolder(zipItem.zipedPath, zipItem.toPath))
                                    if (!TarFolder(zipItem.zipedPath, zipItem.toPath))
                                        ;
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }

        private object Pop()
        {
            try
            {
                return queue.Dequeue();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string TarFolderForControl(string ssid_path)
        {
            string fileName = ssid_path.Substring(ssid_path.LastIndexOf('/', ssid_path.Length - 2, ssid_path.Length - 1) + 1);
            fileName = fileName.Substring(0, fileName.Length - 1);

            string zipedPath = ssid_path.Substring(0, ssid_path.LastIndexOf('/', ssid_path.Length - 2, ssid_path.Length - 2) + 1);

            if (TarFolder(root + ssid_path, root + zipedPath.Replace("Pub", "Download")))
                return zipedPath.Replace("Pub", "Download") + fileName + ".tar.gz";
            else
                return null;
        }

        /// <summary>
        /// 生成压缩包
        /// </summary>
        /// <param name="zipedFolderPath">待压缩路径，完整路径</param>
        /// <param name="zipToFolderPath">压缩到路径，完整路径</param>
        /// <returns></returns>
        public bool TarFolder(string zipedFolderPath, string zipToFolderPath)
        {
            if (string.IsNullOrEmpty(zipedFolderPath)
                || string.IsNullOrEmpty(zipToFolderPath)
                || !System.IO.Directory.Exists(zipedFolderPath))
            {
                return false;
            }

            string fileName = "";

            if (!System.IO.Directory.Exists(zipToFolderPath))
                System.IO.Directory.CreateDirectory(zipToFolderPath);
            else
            {
                fileName = zipedFolderPath.Substring(zipedFolderPath.LastIndexOf('/', zipedFolderPath.Length - 2, zipedFolderPath.Length - 1) + 1);
                fileName = fileName.Substring(0, fileName.Length - 1);
                if (File.Exists(zipToFolderPath  + fileName + ".tar.gz"))
                    return true;
            }


            bool flag = false;
            //zipedFolderPath = zipedFolderPath.Substring(0, zipedFolderPath.Length - 1);
            //fileName = zipedFolderPath.Substring(zipedFolderPath.LastIndexOf('/') + 1);

            Stream zipFile = null;
            Stream gzipStream = null;
            TarArchive archive = null;
            try
            {
                zipFile = new FileStream(Path.Combine(zipToFolderPath, fileName + ".tar.gz"), FileMode.OpenOrCreate);
                gzipStream = new GZipOutputStream(zipFile);
                archive = TarArchive.CreateOutputTarArchive(gzipStream, TarBuffer.DefaultBlockFactor);
                TarEntry entry = TarEntry.CreateEntryFromFile(zipedFolderPath);                
                archive.RootPath = root;
                archive.WriteEntry(entry, true);

                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
                if (archive != null)
                {
                    archive.Close();
                }
                gzipStream.Close();
                zipFile.Close();
            }
            return flag;
        }
    }
}
