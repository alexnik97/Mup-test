using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MUPTest.Models
{
    public class RequestContext : DbContext
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestMovement> RequestMovements { get; set; }
    }
}