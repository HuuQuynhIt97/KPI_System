using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    public class OptionInFunctionSystem
    {
        public int FunctionSystemID { get; set; }
        public int OptionID { get; set; }
        public virtual Option Option { get; set; }
        public virtual FunctionSystem FunctionSystem { get; set; }
    }
}
