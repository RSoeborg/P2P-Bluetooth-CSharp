using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using InTheHand.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using InTheHand.Net.Bluetooth;

namespace ProjectGreenEnvironment
{
    public class GreenEnvironment
    {
        public string PeerName { get; private set; }
        
        public string GetFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GreenEnvironment");
        }

        public void Setup(string name)
        {
            var path = GetFolderPath();
            Directory.CreateDirectory(path);

            using (var sw = new StreamWriter(TranslateFile("env.txt")))
            {
                sw.Write(name);
            }

            File.Create(TranslateFile($"{name}.txt"));
        }
        public void Load()
        {
            using (var sr = new StreamReader(TranslateFile("env.txt")))
            {
                PeerName = sr.ReadToEnd();
            }
        }

        public string TranslateFile(string file)
        { 
            return Path.Combine(GetFolderPath(), file.Replace("\0", ""));
        }
         
        public void SaveFileData(string Data)
        {
            Data = Data.Replace("\0", "");
            var SenderName = Data.Split(' ').First();
            Data = Data.Substring(SenderName.Length + 1, Data.Length - (SenderName.Length + 1));
            Data = Data.Replace("$END", string.Empty);

            if (SenderName.ToLower().Contains(PeerName.ToLower()))
            {
                return;//skip myself.
            }

            using (var sw = new StreamWriter(TranslateFile($"{SenderName}.txt")))
            {
                sw.Write(Data);
            }      
        }
        public string MyFile()
        {
            // Read my file
            string Content = string.Empty;
            using (var sr = new StreamReader(TranslateFile($"{PeerName}.txt")))
            {
                Content = sr.ReadToEnd();
            }

            // Construct file
            return $"{PeerName} {Content}$END"; 
        }

        public string[] AllFiles()
        {

            List<string> filesOut = new List<string>();
            var files = Directory.GetFiles(GetFolderPath());
            foreach (var file in files)
            {
                if (!file.Contains("env.txt"))
                {
                    filesOut.Add(ReadFile(file));
                }
            }
            return filesOut.ToArray();
        }

        public string ReadFile(string path)
        {
            // Read my file
            string Content = string.Empty;
            using (var sr = new StreamReader(path))
            {
                Content = sr.ReadToEnd();
            }

            // Construct file
            var thisPeerName = Path.GetFileNameWithoutExtension(path).Replace("\0", string.Empty);
            return $"{thisPeerName} {Content}$END";
        }

    }

}
