using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YLoader.Properties;

namespace YLoader.Classes
{
    public static class SystemPaths
    {
        public static string getSEOPath()
        {
            return getSEOPath(Settings.Default.active_path);
        }
        public static string getSEOPath(string path)
        {
            return path + "/infoFiles/mainJSONs/SEO.json";
        }
    }
}
