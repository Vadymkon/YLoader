using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLoader.Classes.FileHandling
{
    public static class SPathCheck
    {
        public static void checkAndCreateFile (string filePath)
        {
            // Check
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "");
        }
        
        public static void checkAndCreateDir (string filePath)
        {
            // Check
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        public static void checkAllPaths()
        {
            checkAndCreateFile(SystemPaths.getSEOFilePath());
            checkAndCreateFile(SystemPaths.getSEOTemplateFile());
            checkAndCreateDir(SystemPaths.getUserFilesPath());
            checkAndCreateDir(SystemPaths.getSEOPath());
            checkAndCreateDir(SystemPaths.getHistoryPath());
        }
    }
}
