using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        public byte[] Document { get; set; }

        public int TaskId { get; set; }

        public Task Task { get; set; }
    }
}
