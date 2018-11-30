using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUPTest.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
    }
}