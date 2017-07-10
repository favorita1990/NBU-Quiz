using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class User
    {
        public User()
        {
            this.UserPolls = new HashSet<UserPoll>();
        }

        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FacultyNumber { get; set; }
        public string IPAddress { get; set; }
        public string HashCode { get; set; }


        [ForeignKey("Poll")]
        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }

        public virtual ICollection<UserPoll> UserPolls { get; set; }
    }
}