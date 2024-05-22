using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Author:BaseNamedEntity
    {
        public int Id { get; set; }
        public Author()
        {

        }
        public Author(string name) : base(name)
        {

        }
    }
}
