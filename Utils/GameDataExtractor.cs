using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MAI2.Util;
using Manager;
using Manager.MaiStudio;
using MelonLoader;
using SinmaiAssist.Utils;

namespace SinmaiAssist.Utils
{
    public class GameDataExtractor
    {
        public static void ExtractAllCollectionIds()
        {
            try
            {
                var sb = new StringBuilder();
                
                sb.AppendLine("=== Icon ===");
                foreach (KeyValuePair<int, IconData> icon in Singleton<DataManager>.Instance.GetIcons())
                {
                    sb.AppendLine($"ID: {icon.Value.GetID()}, Name: {icon.Value.name.str}");
                }
                sb.AppendLine();
                
                sb.AppendLine("=== Frame ===");
                foreach (KeyValuePair<int, FrameData> frame in Singleton<DataManager>.Instance.GetFrames())
                {
                    sb.AppendLine($"ID: {frame.Value.GetID()}, Name: {frame.Value.name.str}");
                }
                sb.AppendLine();
                
                sb.AppendLine("=== Title ===");
                foreach (var title in Singleton<DataManager>.Instance.GetTitles())
                {
                    sb.AppendLine($"ID: {title.Value.GetID()}, Name: {title.Value.name.str}");
                }
                sb.AppendLine();
                
                sb.AppendLine("=== Plate ===");
                foreach (var plate in Singleton<DataManager>.Instance.GetPlates())
                {
                    sb.AppendLine($"ID: {plate.Value.GetID()}, Name: {plate.Value.name.str}");
                }
                sb.AppendLine();
                
                sb.AppendLine("=== Partner ===");
                foreach (var partner in Singleton<DataManager>.Instance.GetPartners())
                {
                    sb.AppendLine($"ID: {partner.Value.GetID()}, Name: {partner.Value.name.str}");
                }
                
                var filePath = $"{BuildInfo.Name}/AllCollectionIds.txt";
                EnsureDirectoryExists(BuildInfo.Name);
                File.WriteAllText(filePath, sb.ToString());
                
                MelonLogger.Msg($"Extract all collection ids to file: {filePath}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to extract all collection ids: {ex.Message}");
            }
        }
        
        public static void ExtractAllMusicInfo()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("=== SongsInfo ===");
                
                foreach (var music in Singleton<DataManager>.Instance.GetMusics())
                {
                    var musicData = music.Value;
                    sb.AppendLine($"ID: {musicData.GetID()}, Name: {musicData.name.str}");
                    
                    sb.AppendLine($"  Diff: Basic={musicData.notesData[0].level}, Advanced={musicData.notesData[1].level}, Expert={musicData.notesData[2].level}, Master={musicData.notesData[3].level}, Re:Master={musicData.notesData[4].level}");
                    
                    sb.AppendLine($"  Genre: {musicData.genreName}");
                    
                    sb.AppendLine($"  Artist: {musicData.artistName.str}");
                    sb.AppendLine($"  BPM: {musicData.bpm}");
                    sb.AppendLine();
                }
                
                var filePath = $"{BuildInfo.Name}/AllMusicInfo.txt";
                EnsureDirectoryExists(BuildInfo.Name);
                File.WriteAllText(filePath, sb.ToString());
                
                MelonLogger.Msg($"Extract all music info to file: {filePath}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to extract all music info: {ex.Message}");
            }
        }
        
        private static void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
} 