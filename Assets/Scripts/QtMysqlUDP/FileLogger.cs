using System;
using System.IO;
using UnityEngine;

namespace ZTools
{
    internal class FileLogger
    {
        private StreamWriter fileWriter;
        private string filename = "";
        private string filePath = "";
        private static string CurrentDay { get { return DateTime.Now.ToString("yyyyMMdd"); } }
        private string CurrentLogDay = "";
        private string PreSavePath;
        /// <summary>
        /// Start print
        /// </summary>
        public FileLogger()
        {
            filename = ZLog.LogFileName;
            filePath = ZLog.LogPath.TrimEnd('/') + "/";
            StartNewFile();
            DeleteOldFile();
            CurrentLogDay = CurrentDay;
            this.Write("*********************************************************\nSystem Start at "+ Application.productName +"-"+ DateTime.Now.ToString("HH:mm:ss") + "\n*********************************************************\n");
        }
        /// <summary>
        /// New file
        /// </summary>
        private void StartNewFile()
        {
            if (fileWriter != null)
            {
                fileWriter.Close();
                fileWriter.Dispose();
            }

            fileWriter = null;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            fileWriter = File.AppendText(filePath + string.Format(this.filename, DateTime.Now));
        }

        public void Write(string content)
        {
            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();
            if (CurrentLogDay != CurrentDay)
            {
                this.filename = ZLog.LogFileName;
                StartNewFile();
                DeleteOldFile();
                CurrentLogDay = CurrentDay;
            }

            if (fileWriter != null && fileWriter.BaseStream != null && fileWriter.BaseStream.CanWrite)
                fileWriter.WriteLine(content);
        }
        void DeleteOldFile()
        {
            string[] fileList = Directory.GetFiles(filePath);
            if (fileList.Length > ZLog.SaveDays)
            {
                PreSavePath = filePath + string.Format(this.filename, DateTime.Now.AddDays(-ZLog.SaveDays + 1));
                for (int i = 0; i < fileList.Length; i++)
                {
                    if (string.Compare(PreSavePath, fileList[i]) == 1)
                    {
                        File.Delete(fileList[i]);
                    }
                }
            }
        }
        public void OnDestroy()
        {
            if (fileWriter == null)
                return;

            this.Write("*********************************************************\nSystem Shutdown at " + DateTime.Now.ToString("HH:mm:ss")+ "\n*********************************************************\n");

            fileWriter.Flush();
            fileWriter.Close();
            fileWriter = null;
        }
    }
}