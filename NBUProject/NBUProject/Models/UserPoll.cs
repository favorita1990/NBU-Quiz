using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class UserPoll
    {

        public UserPoll()
        {
            this.UserPollQuestions = new HashSet<UserPollQuestion>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime TimeBegins { get; set; }

        public DateTime TimeEnd { get; set; }

        public int PollId { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        public virtual ICollection<UserPollQuestion> UserPollQuestions { get; set; }

    }
}