using System.Threading.Tasks;

namespace Stations.Core.Interfaces.Queries
{
    public interface IQueryProcessor
    {
        Task<TResult> Process<TResult>(IQuery<TResult> query);
    }
}