using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public interface INamedEntity
    {
        string Name { get; set; }
    }
}
