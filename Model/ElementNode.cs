using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewSpotCode.Model
{
    public class ElementNode
    {
       
        public int id { get; set; }
        public List<int> nodes { get; set; }
    }
}
