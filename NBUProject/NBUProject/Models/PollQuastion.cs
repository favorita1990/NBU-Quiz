using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class PollQuastion
    {
        public PollQuastion()
        {
            this.PollAnswers = new HashSet<PollAnswer>();
        }

        [Key]
        public int Id { get; set; }

        public string Quastion { get; set; }

        public byte Type { get; set; }

        public byte AnswerOrientation { get; set; }

        [ForeignKey("Poll")]
        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }
        public virtual ICollection<PollAnswer> PollAnswers { get; set; }

    }
}