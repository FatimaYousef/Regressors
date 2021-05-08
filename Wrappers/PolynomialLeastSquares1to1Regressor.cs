using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Models.Regression.Linear;
using Accord.Math.Optimization.Losses;
using Regressors.Entities;
using Regressors.Exceptions;
using Regressors.DataSets;
using System.IO;
using System;

namespace Regressors.Wrappers
{
    public class PolynomialLeastSquares1to1Regressor
    {
        private int _degree;
        private bool _isRobust;
        private PolynomialRegression _polynomialRegression;
        List<USModel> mylist = new List<USModel>();

        public PolynomialLeastSquares1to1Regressor(List<USModel> list, int degree, bool isRobust = false)
        {
            _degree = degree;
            _isRobust = isRobust;
            mylist = list;
        }

        public double[] Weights
        {
            get
            {
                AssertAlreadyLearned();
                return _polynomialRegression.Weights;
            }
        }

        public double Intercept
        {
            get
            {
                AssertAlreadyLearned();
                return _polynomialRegression.Intercept;
            }
        }
        private void AssertAlreadyLearned()
        {
            if (_polynomialRegression == null)
                throw new NotTrainedException();
        }

        public string StringfyLearnedPolynomial(string format = "e")
        {
            if (_polynomialRegression == null)
                return string.Empty;

            return _polynomialRegression.ToString(format, CultureInfo.InvariantCulture);
        }

        public void Learn(IList<XtoY> dsLearn)
        {

            List<double> data = new List<double>(mylist.Count);

            for (int i = 0; i < mylist.Count; i++)
            {
                data.Add(mylist[i].Cases);
            }
            
            double[] inputs = dsLearn.Select(i => i.X).ToArray();
            var pls = new PolynomialLeastSquares() { Degree = _degree, IsRobust = _isRobust };
            _polynomialRegression = pls.Learn(inputs, data.ToArray());
        }

        public IEnumerable<XtoY> Predict(IEnumerable<double> xvalues)
        {
            double[] xvaluesArray = xvalues.ToArray();
            double[] yvaluesArray = _polynomialRegression.Transform(xvaluesArray);
            for (int i = 0; i < xvaluesArray.Length; ++i)
            {
                yield return new XtoY() { X = xvaluesArray[i], Y = yvaluesArray[i] };
            }
        }

        public double ComputeError(IEnumerable<XtoY> ds, IEnumerable<XtoY> predicted)
        {
            List<double> data = new List<double>(mylist.Count);


            for (int i = 0; i < mylist.Count; i++)
            {
                data.Add(mylist[i].Cases);
            }
            double[] outputs = data.ToArray();
            double[] preds = predicted.Select(i => i.Y).ToArray();
            double error = new SquareLoss(outputs).Loss(preds);
            return error;
        }
    }
}