using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerInvoice.Classes
{
    class FileWriter
    {
        Processing processing;

        public FileWriter(Processing processing)
        {
            this.processing = processing;
        }
        public void WriteFileTxt()
        {
            string pathDir = Directory.GetCurrentDirectory();
            string fileName = GetFreeFilename(pathDir);

            using (TextWriter tw = new StreamWriter(fileName))
            {
                foreach (string line in processing.Invoice)
                {
                    tw.WriteLine(line);
                }
            }
        }

        private string GetFreeFilename(string pathDir)
        {
            string fileName = "Invoice.txt";
            int fileCounter = 1;
            while (File.Exists(pathDir + "\\" + fileName))
            {
                fileName = "Invoice (" + fileCounter + ").txt";
                fileCounter++;
            }
            return fileName;
        }
    }
}
