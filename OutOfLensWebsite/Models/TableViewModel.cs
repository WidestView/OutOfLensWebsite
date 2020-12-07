using System.Collections;
using System.Collections.Generic;

namespace OutOfLensWebsite.Models
{
    public class TableViewModel
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<Dictionary<string, object>> Data { get; set; }
    }
}