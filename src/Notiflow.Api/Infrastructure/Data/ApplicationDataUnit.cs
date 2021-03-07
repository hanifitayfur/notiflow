using System;
using System.Collections.Generic;
using System.Linq;
using Notiflow.Api.Infrastructure.Data.Entities;

namespace Notiflow.Api.Infrastructure.Data
{
    public interface IApplicationDataUnit
    {
        Application GetApplication(string token);
    }

    public class ApplicationDataUnit : IApplicationDataUnit
    {
        public Application GetApplication(string token)
        {
            return Applications.FirstOrDefault(x => x.Token.Equals(token) && x.IsActive);
        }


        // db'den alınacak
        private IList<Application> Applications =>
            new List<Application>()
            {
                new()
                {
                    Token = "e6723973-c033-4325-bd5c-844666a88349",
                    Name= "VW",
                    SenderId = "senderId",
                    ServerKey = "server key",
                    CratedDate = DateTime.Now,
                    IsActive = true
                }
            };
    }
}