using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.Server.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        protected readonly DbContextOptions<ApplicationDbContext> configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> configuration) : base(configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<SiteAdmin> SiteAdmins { get; set; }
        public DbSet<PostLikes> PostLikes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<PostCategories> PostCategories { get; set; }
        public DbSet<PostTags> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.Comments)
                .WithOne(c => c.BlogPost)
                .HasForeignKey(c => c.BlogPostId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId);

            // Explicitly define the many-to-many relationship using PostCategories
            modelBuilder.Entity<PostCategories>()
                .HasOne(pc => pc.BlogPost)
                .WithMany(bp => bp.PostCategories)
                .HasForeignKey(pc => pc.BlogPostId);

            modelBuilder.Entity<PostCategories>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<PostTags>()
                .HasOne(pt => pt.BlogPost)
                .WithMany(bp => bp.PostTags)
                .HasForeignKey(pt => pt.BlogPostId);

            modelBuilder.Entity<BlogPost>()
                .HasOne(s => s.Author)
                .WithMany(a => a.BlogPostsPosted)
                .HasForeignKey(s => s.AuthorId);

            modelBuilder.Entity<SiteAdmin>()
                .HasMany(s => s.BlogPostsPosted)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Guest>()
                .HasMany(g => g.SavedBlogPosts)
                .WithMany(b => b.SavedBy)
                .UsingEntity(j => j.ToTable("SavedBlogPosts"));

            modelBuilder.Entity<PostLikes>()
                .HasKey(pl => new { pl.BlogPostId, pl.UserId });

            modelBuilder.Entity<PostLikes>()
                .HasOne(pl => pl.BlogPost)
                .WithMany(bp => bp.PostLikes)
                .HasForeignKey(pl => pl.BlogPostId);

            modelBuilder.Entity<PostLikes>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId);

            modelBuilder.Entity<PostCategories>()
                .HasKey(pc => new { pc.BlogPostId, pc.CategoryId });

            modelBuilder.Entity<PostTags>()
                .HasKey(pt => new { pt.BlogPostId, pt.TagId });

            base.OnModelCreating(modelBuilder);
        }

    }
}
