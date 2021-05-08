using System;
using System.Collections.Generic;
using System.IO;
using Accord.Statistics.Models.Regression.Linear;
using Regressors.DataSets;
using Regressors.Entities;
using Regressors.Exceptions;

namespace Regressors.Wrappers
{
    public class LinearOrdinaryLeastSquares1to1Regressor
    {
        private bool _isRobust;
        private SimpleLinearRegression _simpleLinearRegression;
        List<USModel> mylist = new List<USModel>();
        public LinearOrdinaryLeastSquares1to1Regressor(List<USModel> list, bool isRobust = false)
        {
            _isRobust = isRobust;
             mylist = list;
        }

        private void EnsureAlreadyTrained()
        {
            if (_simpleLinearRegression == null)
                throw new NotTrainedException();
        }
       
       
        public void Learn(IList<XtoY> dsLearn)
        {
            List<double> data = new List<double>(mylist.Count);
            double[] date = new double[mylist.Count];

            for (int i = 0; i < mylist.Count; i++)
            {
                data.Add(mylist[i].Cases);
                date[i] = i + 1;
            }
            data.ToArray();
            var ols = new OrdinaryLeastSquares() { IsRobust = _isRobust };
            _simpleLinearRegression = ols.Learn(data.ToArray(), date);  
        }

        public IEnumerable<XtoY> Predict(IEnumerable<double> xvalues)
        {
            EnsureAlreadyTrained();
            double[] date = new double[mylist.Count];
            for (int i = 0; i < mylist.Count; i++)
            {
                date[i] = i + 1;
            }
            double[] xvaluesArray = date;
            double[] yvaluesArray = _simpleLinearRegression.Transform(xvaluesArray);
            for (int i = 0; i < xvaluesArray.Length; ++i)
            {
                yield return new XtoY() { X = xvaluesArray[i], Y = yvaluesArray[i] };
            }
        }
    }
}