using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;
using Subscriber = MatthewsGalaxy.Server.Models.Subscriber;

namespace MatthewsGalaxy.WorkerMicroservice
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<PostCategories> PostCategories { get; set; }
        public DbSet<PostLikes> PostLikes { get; set; }
        public DbSet<PostTags> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostCategories>()
                .HasKey(pc => new { pc.BlogPostId, pc.CategoryId });

            modelBuilder.Entity<PostLikes>()
                .HasKey(pl => new { pl.BlogPostId, pl.UserId });
            modelBuilder.Entity<PostTags>()
                .HasKey(pt => new { pt.BlogPostId, pt.TagId });
            ////

            modelBuilder.Entity<PostCategories>()
                .HasOne(pc => pc.BlogPost)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.BlogPostId);

            modelBuilder.Entity<PostCategories>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(pc => pc.CategoryId);
            /// 

            modelBuilder.Entity<PostLikes>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId);

            modelBuilder.Entity<PostLikes>()
                .HasOne(pl => pl.BlogPost)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.BlogPostId);

            /// 
            modelBuilder.Entity<PostTags>()
                .HasOne(pt => pt.BlogPost)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.BlogPostId);

            modelBuilder.Entity<PostTags>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);


        }
    }
}
