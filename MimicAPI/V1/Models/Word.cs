using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Models
{
    public class Word
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Score { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Att { get; set; }
    }
}
