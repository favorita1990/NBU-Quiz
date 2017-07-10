using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class Poll
    {

        public Poll()
        {
            this.PollQuastions = new HashSet<PollQuastion>();
            this.SentEmails = new HashSet<SentEmail>();
        }

        [Key]
        public int Id { get; set; }

        public byte Type { get; set; }

        public string Name { get; set; }

        public DateTime ActivationDate { get; set; }

        public byte QuationsPerPage { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual UserModel User { get; set; }


        public virtual ICollection<PollQuastion> PollQuastions { get; set; }
        public virtual ICollection<SentEmail> SentEmails { get; set; }
    }
}