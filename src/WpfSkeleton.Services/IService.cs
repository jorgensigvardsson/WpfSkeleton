using System.Collections.Generic;

namespace WpfSkeleton.Services
{
    public interface IService
    {
        IEnumerable<string> GetData();
    }
}
