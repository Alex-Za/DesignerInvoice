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
            using (TextWriter tw = new StreamWriter("Invoice.txt"))
            {
                foreach (string line in processing.Invoice)
                {
                    tw.WriteLine(line);
                }
            }
        }
    }
}
