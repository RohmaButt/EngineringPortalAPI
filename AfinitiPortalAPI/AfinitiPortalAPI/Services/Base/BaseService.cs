using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Shared.Shared;
using AfinitiPortalAPI.Shared.Shared.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Services.Base
{
    public class BaseService
    {
        protected readonly PortalDBContext _dbContext;
        protected readonly WebApiContext _webApiContext;
        protected readonly IMemoryCache _cacheProvider;
        protected readonly AppSettings _appSettings;

        protected readonly int JiraCacheDurationInHours;

        public BaseService(PortalDBContext dbContext, IMemoryCache cacheProvider, WebApiContext webApiContext, IOptions<AppSettings> configuration)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _webApiContext = webApiContext;
            _appSettings = configuration.Value;

            this.JiraCacheDurationInHours = this._appSettings?.Cache?.Jira?.LifetimeInHours ?? 2;
        }
    }
}