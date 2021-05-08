using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regressors.DataSets
{
    /// <summary>
    ///  custome select on data
    /// </summary>
    internal static class  FilterExtensions
    {
        /// <summary>
        /// Difference between tow rows ordering
        /// </summary>
        /// <param name="list"></param>
        /// <returns>new list of <see cref="USModel"/></returns>
        public static List<USModel> Difference(this List<USModel> list)
        {
            List<USModel> data = new List<USModel>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    data.Add(list[i]);
                    continue;
                }
                var pervious = list[i - 1];
                var current = list[i].Copy();

                current.Cases -= pervious.Cases;
                current.Deaths -= pervious.Deaths;
                data.Add(current);
            }
            list.Clear();
            return data;
        }

        public static (double[] x, double[] y) ToDateAndCases(this List<USModel> list)
        {
            double[] x = Enumerable.Range(1, list.Count).Select(Convert.ToDouble).ToArray();
            double[] y = list.Select(x => (double)x.Cases).ToArray();
            list.Clear();
            return (x, y);
        }

        public static (double[] x, double[] y) ToDateAndDeaths(this List<USModel> list)
        {
            double[] x = Enumerable.Range(1, list.Count).Select(Convert.ToDouble).ToArray();
            double[] y = list.Select(x => (double)x.Deaths).ToArray();
            list.Clear();
            return (x, y);
        }




    }
}
