using Infrastructure.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SearchFixResult(List<FixDto> fixes, long count)
    {
        public List<FixDto> Fixes { get; set; } = fixes;
        public long Count { get; set; } = count;
    }
}
