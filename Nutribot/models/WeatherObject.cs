using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nutribot.models
{
    public class Nutriobject
    {
        public class Fields
        {

            public string item_name { get; set; }
            public string brand_name { get; set; }
            public double nf_calories { get; set; }
            public double nf_total_fat { get; set; }
            public double nf_total_carbohydrate { get; set; }
            public double nf_dietary_fiber { get; set; }
            public double nf_sugars { get; set; }
            public double nf_protein { get; set; }
            public double nf_vitamin_c_dv { get; set; }
            public double nf_calcium_dv { get; set; }
            public double nf_iron_dv { get; set; }
            public int nf_serving_size_qty { get; set; }
            public string nf_serving_size_unit { get; set; }

        }


        public class Hit
        {
            public string _index { get; set; }
            public string _type { get; set; }
            public string _id { get; set; }
            public double _score { get; set; }
            public Fields fields { get; set; }
        }

        public class RootObject
        {
            public int total_hits { get; set; }
            public double max_score { get; set; }
            public List<Hit> hits { get; set; }
        }
    }
}
