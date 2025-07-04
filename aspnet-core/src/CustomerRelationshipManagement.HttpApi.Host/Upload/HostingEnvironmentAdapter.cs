using Microsoft.AspNetCore.Hosting;

namespace CustomerRelationshipManagement.Upload
{
    public class HostingEnvironmentAdapter : IHostingEnvironment
    {
        private readonly IWebHostEnvironment _env;
        public HostingEnvironmentAdapter(IWebHostEnvironment env)
        {
            _env = env;
        }
        public string WebRootPath => _env.WebRootPath;
        
    }
}
