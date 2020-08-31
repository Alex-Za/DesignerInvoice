using DesignerInvoice.AdditionalClasses;
using DesignerInvoice.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DesignerInvoice
{
    class InvoiceViewModel : INotifyPropertyChanged
    {
        private string filePath;
        BackgroundWorker worker;
        public InvoiceViewModel()
        {
            chooseFileCheck = false;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += work;
            worker.ProgressChanged += workProgressChanged;
            
        }

        private void workProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }
        private void ChangeProgress(int count)
        {
            worker.ReportProgress(count);
        }
        private void work(object sender, DoWorkEventArgs e)
        {
            try
            {
                FileReader reader = new FileReader(filePath);
                Processing processing = new Processing(reader, ChangeProgress);

                FileWriter writer = new FileWriter(processing);
                writer.WriteFileTxt();
                ConsoleText = "Done";
            } catch (Exception ex)
            {
                WriteError(ex.ToString());
                ConsoleText = "Error!";
            }
            
        }

        private RelayCommand chooseFile;
        private RelayCommand run;
        private bool chooseFileCheck;
        private int progress;
        private string consoleText;
        public string ConsoleText
        {
            get { return consoleText; }
            set
            {
                consoleText = value;
                OnPropertyChanged("ConsoleText");
            }
        }
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }
        public bool CooseFileCheck
        {
            get { return chooseFileCheck; }
            set
            {
                chooseFileCheck = value;
                OnPropertyChanged("CooseFileCheck");
            }
        }
        public RelayCommand ChooseFile
        {
            get
            {
                return chooseFile ??
                  (chooseFile = new RelayCommand(obj =>
                  {
                      OpenFileDialog openFileDialog = new OpenFileDialog();
                      if (openFileDialog.ShowDialog() == true)
                      {
                          filePath = openFileDialog.FileName;
                          CooseFileCheck = true;
                          Progress = 0;
                          ConsoleText = "";
                      }
                  }));
            }
        }
        public RelayCommand Run
        {
            get
            {
                return run ??
                  (run = new RelayCommand(obj =>
                  {
                      worker.RunWorkerAsync();
                  }));
            }
        }

        private void WriteError(string error)
        {
            using (TextWriter tw = new StreamWriter("Error.txt"))
            {
                tw.WriteLine(error);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
