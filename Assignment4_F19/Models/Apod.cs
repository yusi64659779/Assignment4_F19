using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment4_F19.Models
{
     public class Apod
        {
            public string date { get; set; }
            public string explanation { get; set; }
            public string hdurl { get; set; }
            public string media_type { get; set; }
            public string service_version { get; set; }
            public string title { get; set; }
            public string url { get; set; }
        }

    public class Apods
    {
        public List<Apod> data { get; set; }
    }
    
}
