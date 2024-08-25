using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.WorkerMicroservice.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<BlogPost> _blogPosts;
        private IGenericRepository<Subscriber> _subscribers;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<BlogPost> BlogPosts =>
            _blogPosts ??= new GenericRepository<BlogPost>(_context);

        public IGenericRepository<Subscriber> Subscribers =>
            _subscribers ??= new GenericRepository<Subscriber>(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
