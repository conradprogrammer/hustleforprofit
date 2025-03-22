using System;
using System.IO;
using NAudio.Wave;
using Wisej.Web;

namespace WJ_HustleForProfit_003.Shared
{
    public static class AudioHelper
    {
        public static string AudioFileDuration(string filePath)
        {
            try
            {
                using (var reader = new NAudio.Wave.Mp3FileReader(filePath))
                {
                    TimeSpan duration = reader.TotalTime;
                    return $"{duration.Minutes:D2}:{duration.Seconds:D2}";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, invalid format)
                Console.WriteLine($"Error: {ex.Message}");
                return "00:00";
            }
        }
        public static void ConcatenateMp3Files(ListView.ListViewItemCollection items, string outputFilePath)
        {
            using (var writer = new FileStream(outputFilePath, FileMode.Create))
            {
                foreach (ListViewItem item in items)
                {
                    string filePath = Path.Combine(clsGlobals.audioFolderPath, item.SubItems[4].Text); // Filename column
                    using (var reader = new Mp3FileReader(filePath))
                    {
                        Mp3Frame frame;
                        while ((frame = reader.ReadNextFrame()) != null)
                        {
                            writer.Write(frame.RawData, 0, frame.RawData.Length);
                        }
                    }
                }
            }
        }
    }
}
