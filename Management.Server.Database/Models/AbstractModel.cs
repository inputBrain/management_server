using System;

namespace Management.Server.Database.Models
{
    public abstract class AbstractModel
    {
        public DateTimeOffset CreatedAt { get; set; }
    }
}