using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class UserPollAnswer
    {

        [Key]
        public int Id { get; set; }

        public string Answer { get; set; }

        public int PollQuastionId { get; set; }

        [ForeignKey("UserPollQuastion")]
        public int UserPollQuastionId { get; set; }
        public virtual UserPollQuestion UserPollQuastion { get; set; }

    }
}