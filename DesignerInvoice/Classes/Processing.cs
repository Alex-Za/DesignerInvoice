using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerInvoice.Classes
{
    class Processing
    {
        FileReader fileReader;
        Action<int> changeProgress;
        public Processing(FileReader fileReader, Action<int> ChangeProgress)
        {
            this.fileReader = fileReader;
            changeProgress = ChangeProgress;
        }

        private List<string> invoice;
        public List<string> Invoice
        {
            get
            {
                if (invoice == null)
                    invoice = GenerateInvoice();

                return invoice;
            }
        }
        private List<string> GenerateInvoice()
        {
            List<string> invoice = new List<string>();
            string typeOfWork = "";
            string dIndex = "";
            HashSet<string> categorys = new HashSet<string>();
            HashSet<string> brands = new HashSet<string>();
            int mediaCount = 0;
            int fileLength = fileReader.DIndexFile.Count();
            int step = fileReader.DIndexFile.Count() / 100;
            int currentStep = step;
            int currentLine = 0;
            int i = 0;

            foreach (string[] line in fileReader.DIndexFile.Skip(4))
            {
                if (currentLine == currentStep)
                {
                    i++;
                    changeProgress(i);
                    currentStep += step;
                }

                if (currentLine == fileLength - 5)
                    changeProgress(100);

                currentLine++;

                if (line[0] != "")
                {
                    if (typeOfWork != "")
                    {
                        string work = GenerateTypeOfWork(typeOfWork, categorys, brands, mediaCount, dIndex);
                        invoice.Add(work);
                        categorys = new HashSet<string>();
                        brands = new HashSet<string>();
                        mediaCount = 0;
                    }
                    typeOfWork = "";
                    invoice.Add(line[0] + ":");
                    continue;
                }

                if (line[1] != "")
                {
                    if (typeOfWork != "")
                    {
                        string work = GenerateTypeOfWork(typeOfWork, categorys, brands, mediaCount, dIndex);
                        invoice.Add(work);
                        categorys = new HashSet<string>();
                        brands = new HashSet<string>();
                        mediaCount = 0;
                    }
                    typeOfWork = line[1];
                    dIndex = line.Last();
                    continue;
                }

                if (line[4] != "" && line[4] != "-")
                    categorys.Add(line[4]);

                if (line[5] != "" && line[5] != "-")
                {
                    if (line[5].Contains(", "))
                    {
                        string[] brandsSplit = line[5].Split(new string[] { ", " }, StringSplitOptions.None);
                        foreach (string brand in brandsSplit)
                        {
                            brands.Add(brand);
                        }
                    } else
                    {
                        brands.Add(line[5]);
                    }
                }

                int dotPos = line[6].LastIndexOf('.');
                string mediaNumber = "";
                if (dotPos > -1)
                    mediaNumber = line[6].Substring(0, dotPos);

                if (int.TryParse(mediaNumber, out int number))
                    mediaCount += number;

            }
            string lastWork = GenerateTypeOfWork(typeOfWork, categorys, brands, mediaCount, dIndex);
            invoice.Add(lastWork);

            return invoice;
        }
        private string GenerateTypeOfWork(string typeOfWork, HashSet<string> categorys, HashSet<string> brands, int mediaCount, string dIndex)
        {
            StringBuilder sb = new StringBuilder();
            switch (typeOfWork)
            {
                case "Created  new  icons for  categories":
                    if (categorys.Count > 0)
                    {
                        sb.Append("- Created new " + mediaCount + " icons for " + categorys.Count + " categories (");
                        sb.Append(string.Join(", ", categorys) + ") (" + dIndex + ")");
                    } else
                    {
                        sb.Append("- Created new " + mediaCount + " icons (" + dIndex + ")");
                    }
                    break;
                case "Created  new  promo banners for brands":
                    if (brands.Count > 0)
                    {
                        sb.Append("- Created new " + mediaCount + " promo banners for " + brands.Count + " brands (");
                        sb.Append(string.Join(", ", brands) + ") (" + dIndex + ")");
                    } else
                    {
                        sb.Append("- Created new " + mediaCount + " promo banners (" + dIndex + ")");
                    }
                    break;
                case "Created  new collages for categories":
                    if (categorys.Count > 0)
                    {
                        sb.Append("- Created new " + mediaCount + " collages for " + categorys.Count + " categories (");
                        sb.Append(string.Join(", ", categorys) + ") (" + dIndex + ")");
                    } else
                    {
                        sb.Append("- Created new " + mediaCount + " collages (" + dIndex + ")");
                    }
                    break;
                case "Created new collages and other graphics for brands":
                    if (brands.Count > 0)
                    {
                        sb.Append("- Created new " + mediaCount + " collages and other graphics for " + brands.Count + " brands (");
                        sb.Append(string.Join(", ", brands) + ") (" + dIndex + ")");
                    } else
                    {
                        sb.Append("- Created new " + mediaCount + " collages and other graphics (" + dIndex + ")");
                    }
                    break;
                case "Customized/Developed videos for new media  for  brands/categories":
                    sb.Append("- Customized/Developed " + mediaCount + " videos for new media");
                    if (categorys.Count > 0 || brands.Count > 0)
                    {
                        int count = categorys.Count + brands.Count;
                        sb.Append(" for " + count + " brands/categories (");
                        categorys.UnionWith(brands);
                        sb.Append(string.Join(", ", categorys) + ") (" + dIndex + ")");
                    } else
                    {
                        sb.Append(" (" + dIndex + ")");
                    }
                    break;
                case "Data Mining for Quality Images & Videos with  new media for  brand /categories":
                    sb.Append("- Data Mining for Quality Images & Videos with new " + mediaCount + " videos for new media");
                    if (categorys.Count > 0 || brands.Count > 0)
                    {
                        int count = categorys.Count + brands.Count;
                        sb.Append(" for " + count + " brands/categories (");
                        categorys.UnionWith(brands);
                        sb.Append(string.Join(", ", categorys) + ") (" + dIndex + ")");
                    } else
                    {
                        sb.Append(" (" + dIndex + ")");
                    }
                    break;
                case "Design team management (planning, work flows control, working with newcomers, testing, improving efficiency)":
                    sb.Append("- Design team management (planning, work flows control, working with newcomers, testing, improving efficiency)");
                    sb.Append(" (" + dIndex + ")");
                    break;
                case "Designing of images/videos for new images for brands":
                    if (brands.Count > 0)
                    {
                        sb.Append("- Designing of images/videos for new " + mediaCount + " images for " + brands.Count + " brands (");
                        sb.Append(string.Join(", ", brands) + ") (" + dIndex + ")");
                    } else
                    {
                        sb.Append("- Designing of images/videos for new " + mediaCount + " images (" + dIndex + ")");
                    }
                    break;
                case "Pre-relised design works Quality Assure":
                    sb.Append("- Pre-relised design works Quality Assure (" + dIndex + ")");
                    break;
            }

            return sb.ToString();
        }

    }
}
