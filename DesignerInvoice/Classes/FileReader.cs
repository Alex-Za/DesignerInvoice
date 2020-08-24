using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerInvoice.Classes
{
    class FileReader
    {
        private string filePath;
        public FileReader(string filePath)
        {
            this.filePath = filePath;
        }

        private IEnumerable<string[]> dIndexFile;
        public IEnumerable<string[]> DIndexFile
        {
            get
            {
                if (dIndexFile == null)
                    dIndexFile = GetLineFromCSVFile();
                
                return dIndexFile;
            }
        }
        private IEnumerable<string[]> GetLineFromCSVFile()
        {
            using (StreamReader reader = new StreamReader(filePath))
                while (!reader.EndOfStream)
                    yield return reader.ReadLine().Split(';');
        }

    }
}
