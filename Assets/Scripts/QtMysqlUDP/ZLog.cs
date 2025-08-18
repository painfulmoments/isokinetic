using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZTools
{
    internal class ZLog : MonoBehaviour
    {
        /// <summary>
        /// File name format
        /// </summary>
        public static string LogFileName => "Log{0:_yyyy_MM_dd}.txt";

        /// <summary>
        /// Log file path
        /// </summary>
        public static string LogPath => "logUnity";

        /// <summary>
        /// Log retention days
        /// </summary>
        public static int SaveDays = 30;

        /// <summary>
        /// Print the contents of each line
        /// </summary>
        public string LogContent => "--------------------------" + Time + "--------------------------\n{0}\n{1}";

        public FileLogger FileLogger;
        private static ZLog instance;

        static string Time => DateTime.Now.ToString("[HH:mm:ss.ffffff]");

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            FileLogger = new FileLogger();
            Application.logMessageReceivedThreaded += LogMessage;
        }

        void LogMessage(string condition, string stackTrace, LogType type)
        {
            if (!type.Equals(LogType.Warning))
            {
                FileLogger?.Write(string.Format(LogContent, condition, stackTrace));
            }
        }

        private void OnDestroy()
        {
            FileLogger?.OnDestroy();
            FileLogger = null;
            Application.logMessageReceivedThreaded -= LogMessage;
        }

        // Static facade for easy logging calls
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void Warn(string message)
        {
            Debug.LogWarning(message);
        }

        public static void Error(string message)
        {
            Debug.LogError(message);
        }
    }
}