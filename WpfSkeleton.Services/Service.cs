using System.Collections.Generic;

namespace WpfSkeleton.Services
{
    public class Service : IService
    {
        public IEnumerable<string> GetData()
        {
            return new[] { "A", "bunch", "of", "sample", "data" };
        }
    }
}