﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bilibili_batch_rename
{
    class Common
    {
        /// <summary>
        /// 迭代获取目录。
        /// </summary>
        /// <param name="RootDir">根目录。</param>
        /// <param name="DirList">获取的目录列表。</param>
        public static void GetAllDir(string RootDir, ref List<string> DirList)
        {
            if (!Directory.Exists(RootDir))
                throw new DirectoryNotFoundException("Directory doesn't exist.");

            DirectoryInfo directoryInfo = new DirectoryInfo(RootDir);
            foreach(DirectoryInfo d in directoryInfo.GetDirectories())
            {
                DirList.Add(d.FullName);
                GetAllDir(d.FullName, ref DirList);
            }
        }

        /// <summary>
        /// 重命名方法。
        /// </summary>
        /// <param name="dir">视频文件存放的文件夹。</param>
        /// <returns>重命名操作是否成功。</returns>
        public static bool Rename(string dir)
        {
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException("Directory doesn't exist.");
            string jsonName = Path.Combine(dir, "info.json");
            string videoName = Path.Combine(dir, "000.flv");
            if (!(File.Exists(jsonName) && File.Exists(videoName)))
            {
                Console.Write("Rename {0} failed，there is no .flv file of .json file in the directory.", videoName);
                return false;
            }

            string newVidName = GetVidName(jsonName) + ".flv";
            if (newVidName == string.Empty)
            {
                Console.Write("Rename {0} failed，empty title in JSON。", videoName);
                return false;
            }
            FileInfo f = new FileInfo(videoName);
            f.MoveTo(Path.Combine(dir,newVidName));
            return true;
        }

        /// <summary>
        /// 从JSON获取视频名称。
        /// </summary>
        /// <param name="JsonPath">JSON文件的完整路径。</param>
        /// <returns>获取的文件名。若获取失败返回空。</returns>
        private static string GetVidName(string JsonPath)
        {
            try
            {
                using (StreamReader stream = File.OpenText(JsonPath))
                {
                    using (JsonTextReader reader = new JsonTextReader(stream))
                    {
                        JObject o = (JObject)JToken.ReadFrom(reader);
                        return(o["title"].ToString());
                    }
                }
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                return string.Empty;
            }
        }
    }
}
