using Project.CrossCutting.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Helper
{
    public class RequestDetail : IRequestDetail
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestDetail(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetUser()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
