using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class UserPollQuestion
    {

        public UserPollQuestion()
        {
            this.UserPollAnswers = new HashSet<UserPollAnswer>();
        }

        [Key]
        public int Id { get; set; }

        public string Quastion { get; set; }

        public int PollQuastionId { get; set; }

        public int PollId { get; set; }

        [ForeignKey("UserPoll")]
        public int UserPollId { get; set; }
        public virtual UserPoll UserPoll { get; set; }

        public virtual ICollection<UserPollAnswer> UserPollAnswers { get; set; }

    }
}