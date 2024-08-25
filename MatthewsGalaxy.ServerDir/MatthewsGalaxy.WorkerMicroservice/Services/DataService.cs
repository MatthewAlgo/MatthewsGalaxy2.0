using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.WorkerMicroservice.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _context;

        public DataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            return await _context.Subscribers.ToListAsync();
        }

        public async Task<List<BlogPost>> GetNewBlogPostsAsync(DateTime lastChecked)
        {
            return await _context.BlogPosts.Where
                (a => a.PublishedDate > lastChecked).ToListAsync();
        }
    }
}
