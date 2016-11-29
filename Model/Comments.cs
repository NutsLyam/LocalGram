using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalGram.Model
{
    public class Comments
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string[] Hashtag { get; set; }
        public Guid Mention { get; set; }

    }
}
