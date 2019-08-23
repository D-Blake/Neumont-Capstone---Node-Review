using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.ReviewTypes
{
    class Movie
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public List<string> Reviews { get; set; }

    }
}
