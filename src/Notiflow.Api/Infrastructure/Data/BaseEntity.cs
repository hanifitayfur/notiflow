using System;

namespace Notiflow.Api.Infrastructure.Data
{
    public class BaseEntity
    {
        public DateTime CratedDate { get; set; }
        public bool IsActive { get; set; }
    }
}