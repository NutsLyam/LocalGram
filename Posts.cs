using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalGram.Model
{
   public class Posts
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PhotoId { get; set; }
        public DateTime Date { get; set; }

    }
}
