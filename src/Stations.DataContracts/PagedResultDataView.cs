using System.Collections.Generic;

namespace Stations.DataContracts
{
    public class PagedResultDataView<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}