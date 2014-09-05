using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LUOBO.Helper
{
    public class AD_Templet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempletPath"></param>
        /// <param name="TempletURL"></param>
        /// <param name="PortalFile"></param>
        /// <returns></returns>
        public static List<M_ADTempletFile> ReadTemplet(String TempletPath, String TempletURL, String PortalFile)
        {
            if (!System.IO.Directory.Exists(TempletPath))
            {
                return null;
            }

            List<M_ADTempletFile> filelist = new List<M_ADTempletFile>();
            M_ADTempletFile ftile;

            String[] files = System.IO.Directory.GetFiles(TempletPath);

            foreach (String fi in files)
            {
                String f = fi.Replace("\\", "/").ToLower();
                f = f.Substring(f.LastIndexOf("/") + 1);
                ftile = new M_ADTempletFile();
                ftile.File_Name = f;
                ftile.File_Url = TempletURL + "/" + f;
                GetTempletContent(fi, ftile);
                if (f.Equals(PortalFile.ToLower()))
                {
                    ftile.isPortal = true;
                    filelist.Insert(0, ftile);
                }
                else
                {
                    ftile.isPortal = false;
                    filelist.Add(ftile);
                }
            }
            return filelist;
        }

        /// <summary>
        /// 解析模版可编辑内容
        /// </summary>
        /// <param name="tfile">模版文件路径</param>
        /// <param name="fobj">文件对象</param>
        private static void GetTempletContent(String tfile,M_ADTempletFile fobj)
        {
            if (!System.IO.File.Exists(tfile))
            {
                return;
            }
            String allcontent = System.IO.File.ReadAllText(tfile);
            fobj.File_Note = getMidTxt(allcontent, "<!--Note:", "-->");
            String[] tmplist = Regex.Split(allcontent, "<!--WifiTempS ");
            List<M_ADTempletUnit> UList = new List<M_ADTempletUnit>();
            List<M_ADContentItem> CList = new List<M_ADContentItem>();

            if (tmplist.Length > 1)
            {
                String tmpS;
                M_ADTempletUnit unit;
                M_ADContentItem uitem;

                for (int i = 1; i < tmplist.Length; ++i)
                {
                    tmpS = tmplist[i];
                    if (tmpS.IndexOf("<!--WifiTempE-->") > 0)
                    {
                        unit = GetTUnit(tmpS.Substring(0, tmpS.IndexOf("-->")));
                        UList.Add(unit);

                        String content = getMidTxt(tmplist[i], "-->", "<!--WifiTempE");
                        switch (unit.Unit_Type)
                        {
                            case "txt":
                                if (unit.Unit_Link == "true")
                                {
                                    uitem = new M_ADContentItem();
                                    uitem.TKey = "Templet_" + (i - 1);
                                    uitem.TValue = getMidTxt(content, ">", "</a");
                                    CList.Add(uitem);

                                    uitem = new M_ADContentItem();
                                    uitem.TKey = "Templet_" + (i - 1) + "_link";
                                    uitem.TValue = getMidTxt(content, "href=\"", "\"");
                                    CList.Add(uitem);
                                }
                                else
                                {
                                    uitem = new M_ADContentItem();
                                    uitem.TKey = "Templet_" + (i - 1);
                                    uitem.TValue = content;
                                    CList.Add(uitem);
                                }
                                break;
                            case "pic":
                                uitem = new M_ADContentItem();
                                uitem.TKey = "Templet_" + (i - 1);
                                uitem.TValue = getMidTxt(content, "src=\"", "\"");
                                CList.Add(uitem);
                                if (unit.Unit_Link == "true")
                                {
                                    uitem = new M_ADContentItem();
                                    uitem.TKey = "Templet_" + (i - 1) + "_link";
                                    uitem.TValue = getMidTxt(content, "href=\"", "\"");
                                    CList.Add(uitem);
                                }
                                break;
                            case "parameter":
                                uitem = new M_ADContentItem();
                                uitem.TKey = "Templet_" + (i - 1);
                                uitem.TValue = getMidTxt(content, "\"", "\"");
                                CList.Add(uitem);
                                break;
                            case "sysparam":
                                uitem = new M_ADContentItem();
                                uitem.TKey = "Templet_" + (i - 1);
                                uitem.TValue = getMidTxt(content, "\"", "\"");
                                CList.Add(uitem);
                                break;
                            case "richtxt":
                                uitem = new M_ADContentItem();
                                uitem.TKey = "Templet_" + (i - 1);
                                uitem.TValue = content;
                                CList.Add(uitem);
                                break;
                        }
                    }
                }
            }
            fobj.File_Templet = UList;
            fobj.File_Content = CList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theStr"></param>
        /// <returns></returns>
        private static M_ADTempletUnit GetTUnit(String theStr)
        {
            M_ADTempletUnit tunit = new M_ADTempletUnit();

            Dictionary<String, String> UnitDict = new Dictionary<string, string>();
            ReadTempletUint(UnitDict, theStr);
            if (UnitDict.Keys.Contains("name"))
            {
                tunit.Unit_Name = UnitDict["name"];
            }
            if (UnitDict.Keys.Contains("type"))
            {
                tunit.Unit_Type = UnitDict["type"];
            }
            if (UnitDict.Keys.Contains("link"))
            {
                tunit.Unit_Link = UnitDict["link"];
            }
            if (UnitDict.Keys.Contains("width"))
            {
                tunit.Unit_Width = UnitDict["width"];
            }
            if (UnitDict.Keys.Contains("height"))
            {
                tunit.Unit_Height = UnitDict["height"];
            }
            if (UnitDict.Keys.Contains("key"))
            {
                tunit.Unit_Key = UnitDict["key"];
            }
            if (UnitDict.Keys.Contains("param"))
            {
                tunit.Unit_Param = UnitDict["param"];
            }
            if (UnitDict.Keys.Contains("dict"))
            {
                tunit.Unit_ValueDict = UnitDict["dict"].Trim();
            }
            
            return tunit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="UnitStr"></param>
        private static void ReadTempletUint(Dictionary<String, String> result, String UnitStr)
        {
            if (UnitStr.StartsWith("<!--"))
            {
                UnitStr = UnitStr.Substring(4);
            }
            if (UnitStr.IndexOf("-->") > 0)
            {
                UnitStr = UnitStr.Substring(0, UnitStr.IndexOf("-->"));
            }
            if (UnitStr.IndexOf("=") > 0)
            {
                String tkey = UnitStr.Substring(0, UnitStr.IndexOf("=")).Trim();
                if (tkey.IndexOf(" ") > 0)
                {
                    tkey = tkey.Substring(tkey.LastIndexOf(" ")).Trim().ToLower();
                }
                String tValue = UnitStr.Substring(UnitStr.IndexOf("=") + 1).Trim();
                String str = UnitStr.Substring(UnitStr.IndexOf("=") + 1).Trim();
                if (tValue.StartsWith("\""))
                {
                    tValue = tValue.Substring(1);
                    if (tValue.IndexOf("\"") > 0)
                    {
                        tValue = tValue.Substring(0, tValue.IndexOf("\""));
                    }
                    if (str.Length > tValue.Length + 3)
                    {
                        str = str.Substring(tValue.Length + 2).Trim();
                    }
                }
                else if (tValue.StartsWith("'"))
                {
                    tValue = tValue.Substring(1);
                    if (tValue.IndexOf("'") > 0)
                    {
                        tValue = tValue.Substring(0, tValue.IndexOf("'"));
                    }
                    if (str.Length > tValue.Length + 3)
                    {
                        str = str.Substring(tValue.Length + 2).Trim();
                    }
                }
                else
                {
                    if (tValue.IndexOf(" ") > 0)
                    {
                        tValue = tValue.Substring(0, tValue.IndexOf(" "));
                    }
                    if (str.Length > tValue.Length + 1)
                    {
                        str = str.Substring(tValue.Length).Trim();
                    }
                }
                if (result.Keys.Contains(tkey))
                {
                    result[tkey] = tValue;
                }
                else
                {
                    result.Add(tkey, tValue);
                }
                if (str.Length > 1)
                {
                    ReadTempletUint(result, str);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="adfile"></param>
        /// <param name="templetitems"></param>
        /// <returns></returns>
        public static Boolean WriteADFile(String adfile, List<M_ADContentItem> templetitems, String adtitle)
        {
            return WriteADFile(adfile, templetitems, "../UserPic/", adtitle);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adfile"></param>
        /// <param name="templetitems"></param>
        /// <param name="userpicdir"></param>
        /// <returns></returns>
        private static Boolean WriteADFile(String adfile, List<M_ADContentItem> templetitems, String userpicdir, String adtitle)
        {
            String allcontent = System.IO.File.ReadAllText(adfile);
            String[] tmplist = Regex.Split(allcontent, "<!--WifiTempS ");

            String NewContent = tmplist[0];
            String unitS = "";
            String tmpS = "";
            if (tmplist.Length > 1)
            {
                M_ADTempletUnit unit;
                for (int i = 1; i < tmplist.Length; ++i)
                {
                    NewContent += "<!--WifiTempS ";
                    unitS = tmplist[i];
                    if (tmplist[i].IndexOf("<!--WifiTempE-->") > 0)
                    {
                        unit = GetTUnit(tmplist[i].Substring(0, tmplist[i].IndexOf("-->")));
                        tmpS = "";
                        switch (unit.Unit_Type)
                        {
                            case "richtxt":
                                for (int j = 0; j < templetitems.Count; ++j)
                                {
                                    if (templetitems[j].TKey == "Templet_" + (i - 1))
                                    {
                                        tmpS = templetitems[j].TValue;
                                        break;
                                    }
                                }
                                unitS = getNewContent(tmplist[i], tmpS);
                                break;
                            case "txt":
                                for (int j = 0; j < templetitems.Count; ++j)
                                {
                                    if (templetitems[j].TKey == "Templet_" + (i - 1))
                                    {
                                        tmpS = templetitems[j].TValue;
                                        break;
                                    }
                                }
                                if (unit.Unit_Link == "true")
                                {
                                    for (int j = 0; j < templetitems.Count; ++j)
                                    {
                                        if (templetitems[j].TKey == "Templet_" + (i - 1) + "_link")
                                        {
                                            if (templetitems[j].TValue.Trim().Length > 0)
                                            {
                                                tmpS = "<a href=\"" + templetitems[j].TValue + "\" >" + tmpS + "</a>";
                                            }
                                            break;
                                        }
                                    }
                                }
                                unitS = getNewContent(tmplist[i], tmpS);
                                break;
                            case "pic":
                                tmpS = "<img " + getMidTxt(tmplist[i], "<img ", ">") + ">";
                                for (int j = 0; j < templetitems.Count; ++j)
                                {
                                    if (templetitems[j].TKey == "Templet_" + (i - 1))
                                    {
                                        if (templetitems[j].TValue.Trim().Length > 0)
                                        {
                                            tmpS = "<img src=\"" + userpicdir + templetitems[j].TValue + "\"";
                                            if (unit.Unit_Width != null && unit.Unit_Width.Trim().Length > 0)
                                            {
                                                tmpS += " width=\"" + unit.Unit_Width + "\"";
                                            }
                                            if (unit.Unit_Height != null && unit.Unit_Height.Trim().Length > 0)
                                            {
                                                tmpS += " height=\"" + unit.Unit_Height + "\"";
                                            }
                                            if (unit.Unit_Param != null && unit.Unit_Param.Trim().Length > 0)
                                            {
                                                tmpS += " " + unit.Unit_Param + " ";
                                            }
                                            tmpS += "/>";
                                        }
                                        break;
                                    }
                                }
                                if (unit.Unit_Link == "true")
                                {
                                    for (int j = 0; j < templetitems.Count; ++j)
                                    {
                                        if (templetitems[j].TKey == "Templet_" + (i - 1) + "_link")
                                        {
                                            if (templetitems[j].TValue.Trim().Length > 0)
                                            {
                                                tmpS = "<a href=\"" + templetitems[j].TValue + "\" >" + tmpS + "</a>";
                                            }
                                            break;
                                        }
                                    }
                                }
                                unitS = getNewContent(tmplist[i], tmpS);
                                break;
                            case "parameter":
                                for (int j = 0; j < templetitems.Count; ++j)
                                {
                                    if (templetitems[j].TKey == "Templet_" + (i - 1))
                                    {
                                        tmpS = "<script>" + unit.Unit_Key + " = \"" + templetitems[j].TValue + "\";</script>";
                                        break;
                                    }
                                }
                                unitS = getNewContent(tmplist[i], tmpS);
                                break;
                            case "sysparam":
                                for (int j = 0; j < templetitems.Count; ++j)
                                {
                                    if (templetitems[j].TKey == "Templet_" + (i - 1))
                                    {
                                        tmpS = "<script>" + unit.Unit_Key + " = \"" + templetitems[j].TValue + "\";</script>";
                                        break;
                                    }
                                }
                                unitS = getNewContent(tmplist[i], tmpS);
                                break;
                        }
                    }
                    NewContent += unitS;
                }
            }
            //if (adtitle.Length > 0)
            //{
            //    int titlestart = NewContent.ToLower().IndexOf("<title>");
            //    int titleend = NewContent.ToLower().IndexOf("</title>") + 8;
            //    NewContent = NewContent.Remove(titlestart, titleend - titlestart);
            //    NewContent = NewContent.Insert(titlestart, "<title>" + adtitle + "</title>");
            //}
            System.IO.File.WriteAllText(adfile, NewContent);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="theStr"></param>
        /// <returns></returns>
        private static String getNewContent(String Content, String theStr)
        {
            return Content.Substring(0, Content.IndexOf("-->") + 3) + theStr + Content.Substring(Content.IndexOf("<!--WifiTempE"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="BeginStr"></param>
        /// <param name="EndStr"></param>
        /// <returns></returns>
        private static String getMidTxt(String Content, String BeginStr, String EndStr)
        {
            String outStr = "";
            String tmpStr = Content;
            if (tmpStr.IndexOf(BeginStr) > 0)
            {
                tmpStr = Regex.Split(tmpStr,BeginStr)[1];
                if (tmpStr.IndexOf(EndStr) > 0)
                {
                    tmpStr = Regex.Split(tmpStr, EndStr)[0];
                }
                outStr = tmpStr.Trim();
            }
            return outStr;
        }

        /// <summary>
        /// 生成广告正式发布文件
        /// </summary>
        /// <param name="UserPath"></param>
        /// <param name="AD_ID"></param>
        /// <param name="PubDir"></param>
        /// <returns></returns>
        public static String CreatePubAD(String UserPath, String AD_ID, String PubDir)
        {
            String SourcePath = UserPath + AD_ID + "/";
            String DirName = AD_ID + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            String PubPath = UserPath + PubDir + "/" + DirName + "/";

            if (!System.IO.Directory.Exists(SourcePath))
            {
                return "";
            }

            if (System.IO.Directory.Exists(PubPath))
            {
                System.IO.Directory.Move(PubPath, PubPath + "_bak");
            }

            PubFun.CopyDirectory(SourcePath, PubPath);
            if (!System.IO.Directory.Exists(PubPath + "UserPic/"))
            {
                System.IO.Directory.CreateDirectory(PubPath + "UserPic/");
            }
            String tmppic = "";
            Boolean isre = false;
            List<M_ADTempletFile> adfiles = ReadTemplet(PubPath, "", "");
            for (int i = 0; i < adfiles.Count; ++i)
            {
                isre = false;
                for (int j = 0; j < adfiles[i].File_Templet.Count; ++j)
                {
                    if (adfiles[i].File_Templet[j].Unit_Type == "pic")
                    {
                        for (int m = 0; m < adfiles[i].File_Content.Count; ++m)
                        {
                            if (adfiles[i].File_Content[m].TKey == "Templet_" + j && adfiles[i].File_Content[m].TValue.StartsWith("../UserPic/"))
                            {
                                tmppic = adfiles[i].File_Content[m].TValue.Substring(adfiles[i].File_Content[m].TValue.LastIndexOf("/")+1);
                                if (System.IO.File.Exists(UserPath + "UserPic/" + tmppic))
                                {
                                    System.IO.File.Copy(UserPath + "UserPic/" + tmppic, PubPath + "UserPic/" + tmppic);
                                }
                                adfiles[i].File_Content[m].TValue = adfiles[i].File_Content[m].TValue.Substring(3);
                                isre = true;
                            }
                        }
                    }
                    if (adfiles[i].File_Templet[j].Unit_Type == "richtxt")
                    {
                        String tmpUrl = "";
                        if (UserPath.EndsWith("/"))
                        {
                            tmpUrl = UserPath.Substring(0, UserPath.Length - 1);
                        }
                        string[] tmpU = Regex.Split(tmpUrl, "/");
                        tmpUrl = "/" + tmpU[tmpU.Length - 2] + "/" + tmpU[tmpU.Length - 1] + "/UserPic/";

                        for (int m = 0; m < adfiles[i].File_Content.Count; ++m)
                        {
                            if (adfiles[i].File_Content[m].TKey == "Templet_" + j && adfiles[i].File_Content[m].TValue.IndexOf(tmpUrl) > 0)
                            {
                                String[] tmpl = Regex.Split(adfiles[i].File_Content[m].TValue, tmpUrl);
                                for (int y = 1; y < tmpl.Length; ++y)
                                {
                                    tmppic = tmpl[y].Trim();
                                    if (tmpl[y].Trim().IndexOf(" ") > 0 || tmpl[y].Trim().IndexOf("'") > 0 || tmpl[y].Trim().IndexOf("\"") > 0)
                                    {
                                        tmppic = tmpl[y].Trim().Split(new char[3] { ' ', '\'', '"' })[0];
                                    }
                                    if (System.IO.File.Exists(UserPath + "UserPic/" + tmppic))
                                    {
                                        System.IO.File.Copy(UserPath + "UserPic/" + tmppic, PubPath + "UserPic/" + tmppic);
                                    }
                                }
                                adfiles[i].File_Content[m].TValue = adfiles[i].File_Content[m].TValue.Replace(tmpUrl, "UserPic/");
                                isre = true;
                            }
                        }
                    }
                }
                if (isre)
                {
                    WriteADFile(PubPath + "/" + adfiles[i].File_Name, adfiles[i].File_Content, "", "");
                }
            }
            return DirName;
        }
    }
}
