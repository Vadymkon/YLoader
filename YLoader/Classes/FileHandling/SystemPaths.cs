using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YLoader.Properties;

namespace YLoader.Classes
{
    public static class SystemPaths
    {
        public static string getActivePath()
        {
            return Settings.Default.active_path;
            //return System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
        
        // SEO
        public static string getSEOPath(string path)
        {
            return path + "/infoFiles/mainJSONs/";
        }
        public static string getSEOPath()
        {
            return getSEOPath(getActivePath());
        }
        public static string getSEOFilePath(string path)
        {
            return getSEOPath(path) + "SEO.json";
        }
        public static string getSEOFilePath()
        {
            return getSEOFilePath(getActivePath());
        }
        public static string getSEOFilePath_Shorts(string path)
        {
            return getSEOPath(path) + "SEOsh.json";
        }
        public static string getSEOFilePath_Shorts()
        {
            return getSEOFilePath_Shorts(getActivePath());
        }
        public static string getSEOTemplateFile(string path)
        {
            return getSEOPath(path) + "TemplateVideo.json";
        }
        public static string getSEOTemplateFile()
        {
            return getSEOTemplateFile(getActivePath());
        }

        // History
        public static string getHistoryPath(string path)
        {
            return path + $"/infoFiles/history/";
        }
        public static string getHistoryPath()
        {
            return getHistoryPath(getActivePath());
        }
        public static string getHistoryNewGRFilePath(string path)
        {
            // get GR with index-number
            return getHistoryPath(path) + $"GR_{Directory.GetFiles(getHistoryPath(path)).Where(x => x.EndsWith(".json")).ToList().Count}.json";
        }
        public static string getHistoryNewGRFilePath()
        {
            // get GR with index-number
            return getHistoryNewGRFilePath(getActivePath());
        }

        // UserFiles
        public static string getUserFilesPath(string path)
        {
            return path + $"/infoFiles/userFiles/";
        }
        public static string getUserFilesPath()
        {
            return getUserFilesPath(getActivePath());
        } 
        public static string getUserFilesReadableFilePath(string path)
        { 
            return getUserFilesPath(path) + "ContentShedule.txt";
        }
        public static string getUserFilesReadableFilePath()
        { 
            return getUserFilesReadableFilePath(getActivePath());
        }
        public static string getUserFilesReadableFilePath_Shorts(string path)
        { 
            return getUserFilesPath(path) + "sh_ContentShedule.txt";
        }
        public static string getUserFilesReadableFilePath_Shorts()
        { 
            return getUserFilesReadableFilePath_Shorts(getActivePath());
        }

    }
}
