using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VkInfrastructure.Profilers
{
    public class ProfilerListReader
    {
        private readonly string _workingDirectory;

        public ProfilerListReader(string workingDirectory = null)
        {
            if (workingDirectory != null)
                _workingDirectory = workingDirectory;
            else
                _workingDirectory = this.AssemblyDirectory;
        }

        public List<string> GetWordList(string filePath)
        {
            var fullPath = Path.Combine(_workingDirectory, filePath);

            // reads file into list
            List<string> lines = new List<string>();

            using (StreamReader r = new StreamReader(fullPath))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
