using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.Managers.Supports
{
    public class PathProfilerSupportClass
    {
        public static List<string> GetProjectPaths()
        {
            string PrevPath = "../../../";

            string[] allFolders = Directory.GetDirectories(PrevPath, "*", SearchOption.TopDirectoryOnly);

            List<string> AllowPaths = new List<string>();

            foreach (string folder in allFolders)
            {
                if (!folder.Contains("ServerCore") && 
                    !folder.Contains("PacketDesignTool") && 
                    (folder.Contains("Server") || folder.Contains("Client") || folder.Contains("Tool")))
                {
                    AllowPaths.Add(folder);
                }
            }

            return AllowPaths;
        }

        public static string GetKeywordContainsFormPath(string KeyWord)
        {
            string PrevPath = "../../../";

            string[] allFolders = Directory.GetDirectories(PrevPath, "*", SearchOption.TopDirectoryOnly);

            List<string> AllowPaths = new List<string>();

            foreach (string folder in allFolders)
            {
                if (!folder.Contains("ServerCore") && (folder.Contains("Server") || folder.Contains("Client")))
                {
                    AllowPaths.Add(folder);
                }
            }

            return AllowPaths.Find(x=>x.Contains(KeyWord));
        }

        public static string GetKeywordFormPath(string KeyWord)
        {
            string PrevPath = "../../../";

            string[] allFolders = Directory.GetDirectories(PrevPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string folder in allFolders)
            {
                if (folder == KeyWord)
                    return folder;
            }

            return "Error-Path-None";
        }

        public static string GetInKeywordFrompath(string KeyWord)
        {
            string PrevPath = "../../../";

            string[] allFolders = Directory.GetDirectories(PrevPath, "*", SearchOption.TopDirectoryOnly);

            List<string> AllowPaths = new List<string>();

            foreach (string folder in allFolders)
            {
                string SearchPath = folder.Replace("../../../", "");

                if (!folder.Contains("ServerCore") && SearchPath == KeyWord)
                {
                    AllowPaths.Add(folder);
                }
            }

            return AllowPaths.Find(x => x.Contains(KeyWord));
        }
    }
}
