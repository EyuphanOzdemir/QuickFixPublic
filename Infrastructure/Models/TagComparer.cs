using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class TagComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Category obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
