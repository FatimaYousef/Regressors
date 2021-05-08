using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using Regressors.Entities;
using Regressors.DataSets;
using Regressors.Wrappers;

namespace Regressors.Testers
{
    public class PolynomialLeastSquares1to1RegressorTester
    {
        List<USModel> mylist = new List<USModel>();
       public  PolynomialLeastSquares1to1RegressorTester(List<USModel> mylist)
        {
            this.mylist = mylist;
        }
        public void RunTest()
        {
           
            Run(OneVarRealFuncSynthDataSetGenerator.GeneratePolynomialDS,1, mylist.Count, 1, 1, 3, false, "GeneratePolynomialDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateSineDS, 1, mylist.Count, 1, 1, 10, false, "GenerateSineDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateExponentialDS, 1, mylist.Count, 1, 1, 10, false, "GenerateExponentialDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateSqrtAbsDS, 1, mylist.Count, 1, 1, 20, false, "GenerateSqrtAbsDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateLog1PlusAbsDS, 1, mylist.Count, 1, 1, 16, false, "GenerateLog1PlusAbsDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateArcTanDS, 1, mylist.Count, 1, 1, 10, false, "GenerateArcTanDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateExponentialSineDS, 1, mylist.Count, 1, 1, 14, false, "GenerateExponentialSineDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateTanhDS, 1, mylist.Count, 1, 1, 4, false, " GenerateTanhDS", mylist);
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateMuffleWavedDS, 1, mylist.Count, 1, 1, 40, false, "GenerateMuffleWavedDS", mylist);
        }

        private static void ExportToCsvFile(IEnumerable<XtoY> ds, string pathToFile)
        {
            using (var writer = new StreamWriter(pathToFile, false))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(ds);    
            }
        }

        private static void GenerateOctaveScript(string dstestFileName, string predictionFileName, string octaveScriptFilename)
        {
            string octaveScript = @$"testds=csvread('{Path.GetFileName(dstestFileName)}', 1, 0);
coltest1 = testds(:, 1);
coltest2 = testds(:, 2);

prediction=csvread('{Path.GetFileName(predictionFileName)}', 1, 0);
colpred1 = prediction(:, 1);
colpred2 = prediction(:, 2);

plot(coltest1, coltest2, 'linewidth', 2, colpred1, colpred2, 'linewidth', 2);
set(gca, 'linewidth', 1.5, 'fontsize', 20)
";

            File.WriteAllText(octaveScriptFilename, octaveScript);
        }

        private static void Run
        (
            Func<double, double, double, IEnumerable<XtoY>> dsGenerator,
            double begin, double end, double learnDSStep, double testDSStep,
            int degree, bool isRobust,
            string subFolder, List<USModel> list
        )
        {
            Console.Error.WriteLine($"Started test #{subFolder}");
            
            Console.Error.WriteLine("Generating learning dataset");
            IList<XtoY> dsLearn = dsGenerator(begin, end, learnDSStep).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XtoY> dsTest = dsGenerator(begin, end, testDSStep).ToList();

            PolynomialLeastSquares1to1Regressor r = new PolynomialLeastSquares1to1Regressor(list ,degree, isRobust);
            Console.Error.WriteLine("Training");
            r.Learn(dsLearn);
            Console.Out.WriteLine($"Learned polynomial {r.StringfyLearnedPolynomial()}");

            Console.Out.WriteLine("Predicting");
            IEnumerable<double> xvaluesTest = dsTest.Select(i => i.X);
            IEnumerable<XtoY> prediction = r.Predict(xvaluesTest);
            
            Console.Out.WriteLine($"Error: {r.ComputeError(dsTest, prediction)}");

            string targetDir = Path.Combine("out", subFolder);
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);
            Console.Out.WriteLine($"Saving");
            string dsLearnFileName = Path.Combine(targetDir, "learnds.csv");
            string dsTestFileName = Path.Combine(targetDir, "testds.csv");
            string predictionFileName = Path.Combine(targetDir, "prediction.csv");
            string octaveScriptFilename = Path.Combine(targetDir, "octave.m");
            ExportToCsvFile(dsLearn, dsLearnFileName);
            ExportToCsvFile(dsTest, dsTestFileName);
            ExportToCsvFile(prediction, predictionFileName);
            GenerateOctaveScript(dsTestFileName, predictionFileName, octaveScriptFilename);
            
            Console.Error.WriteLine($"Terminated test #{subFolder}");
            Console.Error.WriteLine();
        }
    }
}