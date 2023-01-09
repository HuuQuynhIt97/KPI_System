using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.DTO
{

    public class MutiOrPersionKPIAcDto
    {
        public int KpiId { get; set; }
    }

    public class ListMutiOrPersionKPIAcDto
    {
        public List<MutiOrPersionKPIAcDto> personal { get; set; }
        public List<MutiOrPersionKPIAcDto> muti { get; set; }
    }
}
