using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class PollViewModels
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string QuestionPerPage { get; set; }
        public bool NotAnonymousIsChecked { get; set; }
        public string DateTimePoll { get; set; }
        public List<QuestionListViewModels> QuestionList { get; set; }
    }

    public class QuestionListViewModels
    {
        public int? Id { get; set; }
        public string Question { get; set; }
        public bool QuestionTypeVer { get; set; }
        public string QuestionSelectedDropDown { get; set; }
        public List<QuestionInputListViewModels> QuestionInputList { get; set; }
    }

    public class QuestionInputListViewModels
    {
        public string Value { get; set; }
    }

    public class UserAuthentication
    {
        public string Email { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string FNumber { get; set; }

    }


    public class SentEmailViewModel
    {
        public string Email { get; set; }
        public string Date { get; set; }
    }

    public class AnswerPieViewModel
    {
        public string Answer { get; set; }
        public double Percent { get; set; }
    }
}