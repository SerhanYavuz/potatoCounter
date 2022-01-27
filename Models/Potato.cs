using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotatoCounter.Models
{
    public class Potato
    {
        public int Count { get; set; }
        public string Origin { get; set; }
        public string Name { get; set; }


        public override string ToString()
        {
            return Origin + " kaynağından " + Name + " sisteme " + Count.ToString() + " patates ekledi";
        }
    }
}