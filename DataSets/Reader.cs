using CsvHelper;
using CsvHelper.Configuration;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regressors.DataSets
{
    /// <summary>
    /// reader fast and easy with huge data csv  look for CsvHelper and more in github repos
    /// </summary>
    public static class Reader
    {


        /// <summary>
        /// 
        /// </summary>
        private static int getMaxAllowCount = (int)Math.Sqrt(int.MaxValue) - 100;


        /// <summary>
        /// easy method to read 
        /// <para>slow reader effect using linq with cached on memory </para>
        /// </summary>
        /// <param name="csvpath"></param>
        /// <param name="country"></param>
        /// <param name="saveMemory">data will burn your laptop</param>
        /// <returns></returns>

        public static List<USModel> ReadToEndTheCases(this string csvpath, string country, bool saveMemory = false)
        {
            if (!csvpath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                throw new Exception("should be csv file");

            Console.WriteLine("please waiting... until csvpath read");
            const int saveCount = 200;
            List<USModel> data;

            using (var reader = new StreamReader(csvpath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                data = csv.GetRecords<USModel>().
                      Where(x => x.Country == country).
                      OrderBy(x => x.Date). // order from begining
                      SkipWhile(x => x.Cases == 0).// Skip all data which covid does not effect state 
                      Take(saveMemory ? saveCount : getMaxAllowCount).
                      ToList();
                //Georgia  ,Pike  GA   
            }



            const int longof = 100;
            int miniCount = (int)Math.Sqrt(int.MaxValue) - longof;
            int _count = data.Count;

            //...DenseMatrix
            if (_count > miniCount)
            {
                //data will burn your laptop
                data.RemoveRange(miniCount, data.Count - miniCount);

                Console.WriteLine("cut data for DenseMatrix count*count over int.MaxValue  {0}*{0}  to be {1}*{1} ", _count, miniCount);
            }
            return data;
        }

    }
}
