using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUPTest.Models
{
    public class RequestMovement
    {
        public int Id { get; set; }
        public int OldStatusId { get; set; }
        public int NewStatusId { get; set; }
        public int RequestId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public Status OldStatus { get; set; }
        public Status NewStatus { get; set; }
        public Request Request { get; set; }
    }
}