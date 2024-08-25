using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.WorkerMicroservice.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<BlogPost> BlogPosts { get; }
        IGenericRepository<Subscriber> Subscribers { get; }
        Task<int> SaveAsync();
    }
}
