using System;
using System.IO;
using Wisej.Web;

namespace WJ_HustleForProfit_003.Shared
{
    public static class clsGlobals
    {
        // Global variable for file folder location
        public static string FileFolderRoot { get; set; } = Application.StartupPath;
        public static string audioFolderPath = Path.Combine(FileFolderRoot, "UsersAudioFiles");
        public static string strategyJSONFolderPath = Path.Combine(FileFolderRoot, "UsersStrategyJSON");
        
        // Add other global variables as needed
    }
}
