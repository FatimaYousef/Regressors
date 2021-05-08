using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using System.Linq;
using CsvHelper;
using Regressors.Entities;
using Regressors.DataSets;
using Regressors.Wrappers;
using Microsoft.Office.Interop.Excel;




namespace Regressors.DataSets
{
    public  class Draw
    {
        private static string dataset_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "مشروع التخرج", "dataset", "covid_us_county.csv");
        private static string excel_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "مشروع التخرج", "dataset","new");

        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook worKbooK;
        Microsoft.Office.Interop.Excel.Worksheet worKsheeT;
        public void DrawFunction()
        {
            List<USModel> mylist = new List<USModel>();
            mylist = Reader.ReadToEndTheCases(dataset_path, "Gwinnett").Difference();

            List<int> data = new List<int>(mylist.Count);
            int[] date = new int[mylist.Count];

            for (int i = 0; i < mylist.Count; i++)
            {
                data.Add(mylist[i].Cases);
                date[i] = i + 1;
            }
            data.ToArray();

            excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = false;
            excel.DisplayAlerts = false;
            worKbooK = excel.Workbooks.Add(Type.Missing);
            worKsheeT = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;
            worKsheeT.Name = "DataForDraw";
         
            for (var i = 0; i < data.Count; i++)
            {
                worKsheeT.Cells[ i + 1,1] = data[i];
                worKsheeT.Cells[ i + 1,2] = date[i];
            }
            worKsheeT.SaveAs(excel_path);
            excel.Quit();
            Console.WriteLine(excel.Name);

        }
       
    }
}
