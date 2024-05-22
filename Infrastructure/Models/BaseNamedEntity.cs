using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class BaseNamedEntity : INamedEntity,IEquatable<BaseNamedEntity>
    {
        public string Name { get; set;}


        public BaseNamedEntity()
        {
            
        }
        public BaseNamedEntity(string name)
        {
            Name = name;
        }

        public bool Equals(BaseNamedEntity other)
        {
            return this.Equals(other.Name);
        }
    }
}
