using Infrastructure.Models;

namespace Infrastructure
{
    public class Category: BaseNamedEntity
    {
        public int Id { get; set; }
        public Category()
        {
            
        }
        public Category(string name):base(name)
        {
            
        }
    }
}
