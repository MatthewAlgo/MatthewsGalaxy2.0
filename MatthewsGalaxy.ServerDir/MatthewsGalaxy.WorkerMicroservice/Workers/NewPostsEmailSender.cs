using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using MatthewsGalaxy.WorkerMicroservice.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MatthewsGalaxy.WorkerMicroservice.Workers
{
    public class NewPostsEmailSender : BackgroundService
    {
        private readonly ILogger<NewPostsEmailSender> _logger;
        private readonly IServiceProvider _serviceProvider;
        private DateTime _lastChecked = DateTime.MinValue;

        public NewPostsEmailSender(ILogger<NewPostsEmailSender> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                        var newBlogPosts = (await unitOfWork.BlogPosts.GetAllAsync())
                            .Where(a => a.PublishedDate > _lastChecked)
                            .OrderBy(a => a.PublishedDate)
                            .ToList();

                        if (newBlogPosts.Any())
                        {
                            // Get all subscribers that are verified
                            var subscribers = (await unitOfWork.Subscribers.GetAllAsync())
                                .Where(s => s.Verified)
                                .ToList();
                            foreach (var article in newBlogPosts)
                            {
                                var tasks = subscribers.Select(subscriber =>
                                    emailService.SendNewPostEmailAsync(
                                        subscriber.Email,
                                        article.Title,
                                        "matthewsgalaxy.com/article/" + article.Id.GetHashCode().ToString()
                                        )
                                );
                                await Task.WhenAll(tasks);
                            }
                            _lastChecked = newBlogPosts.Max(a => a.PublishedDate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending new post emails.");
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken); // Check every 30 minutes
            }
        }

    }
}
