using UnityEngine;

namespace SSS
{
    namespace UnityFileDebug
    {
        // short keynames are used to make json output small
        [System.Serializable]
        public class LogOutput
        {
            public string type;  //type
            public string time; //time
            public string log;  //log
            public string stack;  //stack
        }

        public enum FileType
        {
            CSV,
            JSON,
            TSV,
            TXT
        }

        [ExecuteInEditMode]
        public class UnityFileDebug : MonoBehaviour
        {
            public bool useAbsolutePath = false;
            [HideInInspector] public string fileName = "MyGame";
            public FileType fileType = FileType.CSV;

            public string absolutePath = "c:\\";

            public string filePath;
            public string filePathFull;
            public int count = 0;

            System.IO.StreamWriter fileWriter;

            string FileExtensionFromType(FileType type)
            {
                switch (type)
                {
                    case FileType.JSON: return ".json";
                    case FileType.CSV: return ".csv";
                    case FileType.TSV: return ".tsv";
                    case FileType.TXT:
                    default: return ".txt";
                }
            }

            void Start()
            {
                UpdateFilePath();
                if (Application.isPlaying)
                {
                    count = 0;
                    fileWriter = new System.IO.StreamWriter(filePathFull, false);
                    fileWriter.AutoFlush = true;
                    switch (fileType)
                    {
                        case FileType.CSV:
                            fileWriter.WriteLine("type,time,log,stack");
                            break;
                        case FileType.JSON:
                            fileWriter.WriteLine("[");
                            break;
                        case FileType.TSV:
                            fileWriter.WriteLine("type\ttime\tlog\tstack");
                            break;
                    }
                    Application.logMessageReceived += HandleLog;
                }
            }

            public void UpdateFilePath()
            {
                filePath = Application.persistentDataPath + @"\"; //Application.dataPath + "/Logs"; //useAbsolutePath ? absolutePath : Application.persistentDataPath;                
                filePathFull = System.IO.Path.Combine(filePath, GameManager.instance.nombreUsuario + "-" + System.DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + FileExtensionFromType(fileType));
            }

            void OnDisable()
            {
                if (Application.isPlaying)
                {
                    Application.logMessageReceived -= HandleLog;

                    switch (fileType)
                    {
                        case FileType.JSON:
                            fileWriter.WriteLine("\n]");
                            break;
                        case FileType.CSV:
                        case FileType.TSV:
                        default:
                            break;
                    }
                    fileWriter.Close();
                    GameManager.instance.POST(); ////////////////////////////////////////////////REST CLIENT
                }
            }

            void HandleLog(string logString, string stackTrace, LogType type)
            {
                LogOutput output = new LogOutput();
                if (type == LogType.Assert)
                {
                    output.type = "Assert";
                    output.log = logString;
                }
                else if (type == LogType.Exception)
                {
                    output.type = "Exception";
                    output.log = logString;
                }
                else
                {
                    int end = logString.IndexOf("]");
                    output.type = logString.Substring(1, end - 1);
                    output.log = logString.Substring(end + 2);
                }

                output.stack = stackTrace;
                output.time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                switch (fileType)
                {
                    case FileType.CSV:
                        fileWriter.WriteLine(output.type + "," + output.time + "," + output.log.Replace(",", " ").Replace("\n", "") + "," + output.stack.Replace(",", " ").Replace("\n", ""));
                        break;
                    case FileType.JSON:
                        fileWriter.Write((count == 0 ? "" : ",\n") + JsonUtility.ToJson(output));
                        break;
                    case FileType.TSV:
                        fileWriter.WriteLine(output.type + "\t" + output.time + "\t" + output.log.Replace("\t", " ").Replace("\n", "") + "\t" + output.stack.Replace("\t", " ").Replace("\n", ""));
                        break;
                    case FileType.TXT:
                        fileWriter.WriteLine("Type: " + output.type);
                        fileWriter.WriteLine("Time: " + output.time);
                        fileWriter.WriteLine("Log: " + output.log);
                        fileWriter.WriteLine("Stack: " + output.stack);
                        break;
                }

                count++;
            }
        }
    }
}
