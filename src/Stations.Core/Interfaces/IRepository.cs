using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stations.Core.SharedKernel;

namespace Stations.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Save(T entity);
        Task<T> GetById(Guid id);
        Task<IList<T>> List();
    }
}
