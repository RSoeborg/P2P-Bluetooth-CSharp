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
    class GreenEnvironment
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
            return Path.Combine(GetFolderPath(), file);
        }
        
        public void SaveFileData(string Data)
        { 
            var SenderName = Data.Split(' ').First();
            Data = Data.Substring(SenderName.Length + 1, Data.Length - (SenderName.Length + 1));
            Data = Data.Replace("$END", string.Empty);

            var aes = new StandardAES();
            Data = aes.DecryptString(Data);

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

            var aes = new StandardAES();
            Content = aes.EncryptToString(Content);

            // Construct file
            return $"{PeerName} {Content}$END"; 
        }
    }
}
