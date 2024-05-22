using Infrastructure.Models;

namespace Infrastructure
{
    public class Tag:BaseNamedEntity
    {
        public int Id { get; set; }
        public Tag()
        {
            
        }

        public Tag(string name):base(name) 
        { 
        
        }
    }
}
