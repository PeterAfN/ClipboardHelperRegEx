using Csv;
using System;
using System.IO;
using System.Text;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class FileData
    {
        private string AppDataFilePath { get; set; }
        public string ExeFilePath { get; private set; }

        public FileData()
        {
            CreateFilePaths();
        }

        //https://blog.stephencleary.com/2013/11/taskrun-etiquette-examples-dont-use.html
        /// <summary>
        ///     searches column1 in a file with two columns, If match, column2 is returned.
        /// </summary>
        /// <param name="filename">filename including file type</param>
        /// <param name="searchText">tex to search for</param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public string Read(string filename, string searchText, bool ignoreCase)
        {
            var csvOptions = new CsvOptions();

            File.SetAttributes(ExeFilePath, FileAttributes.Normal);
            var csv = File.ReadAllText(ExeFilePath + @"\" + filename, Encoding.Default);
            switch (ignoreCase)
            {
                case true:
                    foreach (var line in CsvReader.ReadFromText(csv, csvOptions))
                        if (string.Equals(line[0], searchText, StringComparison.InvariantCultureIgnoreCase))
                            return line[1];
                    break;
                case false:
                    foreach (var line in CsvReader.ReadFromText(csv, csvOptions))
                        if (line[0] == searchText)
                            return line[1];
                    break;
            }

            return string.Empty;
        }

        private void CreateFilePaths()
        {
            AppDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ExeFilePath = Path.Combine(AppDataFilePath, AssemblyInformation.AssemblyTitle);
        }
    }
}