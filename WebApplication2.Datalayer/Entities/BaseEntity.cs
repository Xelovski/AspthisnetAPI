using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Datalayer.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
    }
}
