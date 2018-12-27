using System.Collections.Generic;

namespace  AspNetCoreVueJs.Web.Features.Filters
{
    public class FilterListViewModel
    {
        public List<string> Brands { get; set; }
        public List<int> Storage { get; set; }
        public List<string> Colours { get; set; }
        public List<string> OS { get; set; }
        public List<string> Features { get; set; }
    }
}
