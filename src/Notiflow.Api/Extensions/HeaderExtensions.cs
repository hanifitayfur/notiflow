using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Notiflow.Api.Extensions
{
    public static class HeaderExtensions
    {
        public static string GetApplicationToken(this IHeaderDictionary headerDictionary)
        {
            var applicationToken = headerDictionary.FirstOrDefault(x => x.Key.Equals("ApplicationToken")).Value;
            if (string.IsNullOrEmpty(applicationToken))
            {
                throw new ArgumentNullException("ApplicationToken zorunludur");
            }

            return applicationToken;
        }
    }
}