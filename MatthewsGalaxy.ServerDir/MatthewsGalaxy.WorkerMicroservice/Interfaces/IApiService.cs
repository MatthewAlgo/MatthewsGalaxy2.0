using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.WorkerMicroservice.Interfaces
{
    public interface IApiService
    {
        Task<HttpResponseMessage> PostAsync<T>(string url, T data);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
