using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regressors.DataSets

{
    public class USModel
    {
        [Name("fips")]
        public string FipsCode { get; set; }
        [Name("county")]
        public string Country { get; set; }
        [Name("state")]
        public string State { get; set; }
        [Name("lat")]
        public decimal Lat { get; set; }
        [Name("long")]
        public decimal Long { get; set; }
        [Name("date")]
        public DateTime Date { get; set; }
        [Name("cases")]
        public int Cases { get; set; }
        [Name("state_code")]
        public string StateCode { get; set; }
        [Name("deaths")]
        public int Deaths { get; set; }
        

        public USModel Copy()
         =>  (USModel)this.MemberwiseClone();

    }
}
