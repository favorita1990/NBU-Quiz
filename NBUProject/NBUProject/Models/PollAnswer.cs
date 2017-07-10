using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class PollAnswer
    {

        [Key]
        public int Id { get; set; }

        public string Answer { get; set; }

        [ForeignKey("PollQuastion")]
        public int PollQuastionId { get; set; }
        public virtual PollQuastion PollQuastion { get; set; }
    }
}