using System;
using System.Collections.Generic;
using System.IO;
using Regressors.DataSets;
using Regressors.Testers;
namespace Regressors
{
    class Program
    {
        private static string dataset_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "مشروع التخرج", "dataset", "covid_us_county.csv");
        List<USModel> mylist = new List<USModel>();
        static int Main(string[] args)
        {
             
            try
            {
                Program pro = new Program();
                pro. ExecutePolynomialLeastSquaresRegressor1to1Tester();
                ///pro.ExecuteLinearOrdinaryLeastSquaresRegressor1to1Tester();
                Draw dr = new Draw();
                dr.DrawFunction();
                return 0;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().FullName + " " + ex.Message);
                return -1;
            }
        }
       
        private  void ExecutePolynomialLeastSquaresRegressor1to1Tester()
        {
            mylist = Reader.ReadToEndTheCases(dataset_path, "Gwinnett").Difference();
            PolynomialLeastSquares1to1RegressorTester tester = new PolynomialLeastSquares1to1RegressorTester(mylist );
            tester.RunTest();
        }

        private  void ExecuteLinearOrdinaryLeastSquaresRegressor1to1Tester()
        {
            mylist = Reader.ReadToEndTheCases(dataset_path, "Gwinnett").Difference();
            SimpleLinearOrdinaryLeastSquares1to1RegressorTester tester = new SimpleLinearOrdinaryLeastSquares1to1RegressorTester(mylist);
            tester.RunTest();
        }
    }
}
