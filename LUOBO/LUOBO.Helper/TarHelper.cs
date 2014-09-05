using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace LUOBO.Helper
{
    public class TarHelper
    {
        public void TarFile(string fileToTar, string zipedFile)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(fileToTar))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToTar + " 不存在!");
            }

            using (FileStream zipFile = System.IO.File.Create(zipedFile))
            {
                using (GZipOutputStream gzipStream = new GZipOutputStream(zipFile))
                {
                    using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzipStream, TarBuffer.DefaultBlockFactor))
                    {
                        string fileName = fileToTar.Substring(fileToTar.LastIndexOf("\\") + 1);
                        TarEntry tarEntry = TarEntry.CreateEntryFromFile(fileToTar);
                        tarArchive.WriteEntry(tarEntry, true);
                        tarArchive.Close();
                    }
                    gzipStream.Finish();
                    gzipStream.Close();
                }
                zipFile.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipedFolderPath">待压缩的文件夹路径</param>
        /// <param name="zipToFolderPath">生成压缩文件的路径</param>
        /// <param name="fileName">生成压缩文件的名称</param>
        /// <returns></returns>
        public bool TarFolder(string zipedFolderPath, string zipToFolderPath, string fileName)
        {
            if (string.IsNullOrEmpty(zipedFolderPath)
                || string.IsNullOrEmpty(zipToFolderPath)
                || string.IsNullOrEmpty(fileName)
                || !System.IO.Directory.Exists(zipedFolderPath)
                || !System.IO.Directory.Exists(zipToFolderPath))
            {
                return false;
            }

            bool flag = false;

            Stream zipFile = null;
            Stream gzipStream = null;
            TarArchive archive = null;
            try
            {

                Environment.CurrentDirectory = zipedFolderPath;
                zipFile = new FileStream(Path.Combine(zipToFolderPath, fileName + ".tar.gz"), FileMode.OpenOrCreate);
                gzipStream = new GZipOutputStream(zipFile);
                archive = TarArchive.CreateOutputTarArchive(gzipStream, TarBuffer.DefaultBlockFactor);
                TarEntry entry = TarEntry.CreateEntryFromFile(zipedFolderPath);

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
