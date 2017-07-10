using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class SentEmail
    {

        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Poll")]
        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }
    }
}