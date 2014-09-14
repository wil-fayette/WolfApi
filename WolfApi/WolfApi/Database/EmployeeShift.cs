using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolfTheatres.Database
{
    public class EmployeeShift
    {
        public Guid EmployeeShiftId { get; set; }
        public string StartOfShift { get; set; }
        public string EndOfShift { get; set; }
        public Guid EmployeeId { get; set; }
    }
}