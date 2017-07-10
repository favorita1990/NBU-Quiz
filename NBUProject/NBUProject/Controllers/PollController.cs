using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NBUProject.Contex;
using NBUProject.Models;

namespace NBUProject.Controllers
{
    [Authorize]
    public class PollController : Controller
    {
        private DBContext db = new DBContext();

        [HttpGet]
        public JsonResult GetAllPolls()
        {
            var pollsDb = db.Polls.ToList();
            List<PollViewModels> polls = new List<PollViewModels>();
            foreach (var item in pollsDb)
            {
                polls.Add(new PollViewModels
                {
                    Id = item.Id,
                    Name = item.Name,
                    DateTimePoll = item.ActivationDate.ToString()
                });
            }
            return Json(polls.OrderByDescending(o => o.Id).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllPolls(string sortBy)
        {
            var pollsDb = db.Polls.ToList();
            List<PollViewModels> polls = new List<PollViewModels>();
            foreach (var item in pollsDb)
            {
                polls.Add(new PollViewModels
                {
                    Id = item.Id,
                    Name = item.Name,
                    DateTimePoll = item.ActivationDate.ToString()
                });
            }

            switch (sortBy)
            {
                case "1":
                    polls = polls.OrderBy(o => o.Name).ToList(); break;
                case "-1":
                    polls = polls.OrderByDescending(o => o.Name).ToList(); break;
                case "2":
                    polls = polls.OrderBy(o => o.Id).ToList(); break;
                case "-2":
                    polls = polls.OrderByDescending(o => o.Id).ToList(); break;
                case "3":
                    polls = polls.OrderBy(o => o.DateTimePoll).ToList(); break;
                case "-3":
                    polls = polls.OrderByDescending(o => o.DateTimePoll).ToList(); break;
            }


            return Json(polls.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetAllPollAndQuestions(int id)
        {

            var pollDb = db.Polls.Where(w => w.Id == id).FirstOrDefault();
            var questionsDb = db.PollQuastions.Where(w => w.PollId == pollDb.Id);
            
            var questionList = new List<QuestionListViewModels>();
            
            foreach (var questionItem in questionsDb)
            {
                
                var question = new QuestionListViewModels()
                {
                    Id = questionItem.Id,
                    Question = questionItem.Quastion,
                    QuestionTypeVer = questionItem.AnswerOrientation == 0 ? true : false,
                    QuestionSelectedDropDown = questionItem.Type.ToString()
                };
                questionList.Add(question);
            }

            var poll = new PollViewModels()
            {
                Id = pollDb.Id,
                DateTimePoll = pollDb.ActivationDate.ToString(),
                Name = pollDb.Name,
                NotAnonymousIsChecked = pollDb.Type == 0 ? true : false,
                QuestionPerPage = pollDb.QuationsPerPage.ToString(),
                QuestionList = questionList
            };


            return Json(poll, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetAllPollAnswers(int id)
        {
        
            var answersDb = db.PollAnswers.Where(w => w.PollQuastionId == id).OrderBy(o => o.Id);

            var answersList = new List<QuestionInputListViewModels>();

            foreach (var answerItem in answersDb)
            {

                var answer = new QuestionInputListViewModels()
                {
                    Value = answerItem.Answer
                };
                answersList.Add(answer);
            }

            return Json(answersList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetAllUserAnswers(int questionId, string userId)
        {

            var user = db.UsersFromThePoll.Where(w => w.HashCode == userId).FirstOrDefault();
            var poll = db.UserPolls.Where(w => (w.UserId == user.Id) && (w.PollId == user.PollId)).FirstOrDefault();
            var Question = db.UserPollQuestions.Where(w => (w.PollId == poll.PollId) && (w.UserPollId == poll.Id)
            && (w.PollQuastionId == questionId)).OrderBy(o => o.Id).FirstOrDefault();

            var answers = db.UserPollAnswers.Where(w => (w.PollQuastionId == Question.PollQuastionId) 
            && (w.UserPollQuastionId == Question.Id)).OrderBy(o => o.Id);


            var answersList = new List<QuestionInputListViewModels>();

            foreach (var answerItem in answers)
            {

                var answer = new QuestionInputListViewModels()
                {
                    Value = answerItem.Answer
                };
                answersList.Add(answer);
            }

            return Json(answersList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckFNumber(string fNumber, int pollId)
        {

            var fnumber = db.UsersFromThePoll.Where(w => (w.PollId == pollId) && (w.FacultyNumber == fNumber)).FirstOrDefault();

            return Json(fnumber.FirstName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AjaxCaptcha(string response)
        {
            RECaptcha recaptcha = new RECaptcha();
            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + recaptcha.Secret + "&response=" + response;
            recaptcha.Response = (new WebClient()).DownloadString(url);
            return Json(recaptcha);
        }


        [HttpPost]
        public JsonResult getSentEmails(int id)
        {
            var emailsDb = db.SentEmails.Where(w => w.PollId == id);
            List<SentEmailViewModel> sentLinkToEmail = new List<SentEmailViewModel>();

            foreach (var emailItem in emailsDb.OrderByDescending(o => o.Date))
            {
                sentLinkToEmail.Add(new SentEmailViewModel()
                {
                    Date = emailItem.Date.ToString(),
                    Email = emailItem.Email
                });
            }

            return Json(sentLinkToEmail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getPieDiagram(int questionId)
        {
            
            var question = db.PollQuastions.Where(w => w.Id == questionId).FirstOrDefault();
            var answers = db.PollAnswers.Where(w => w.PollQuastionId == question.Id).ToList();
            var allAnswers = db.UserPollAnswers.Where(w => w.PollQuastionId == question.Id).ToList();

            List<Tuple<List<string>, double>> resultAnswers = new List<Tuple<List<string>, double>>();

            //var answerCount = answers.Count();
            var counting = 0;

            foreach (var answerItem2 in answers)
            {
                
                foreach (var allAnswerItem2 in allAnswers)
                {
                    if (allAnswerItem2.Answer == answerItem2.Answer)
                    {
                        counting++;
                    }
                }
            }

            foreach (var answerItem in answers)
            {
                var result = new List<string>();
                foreach (var allAnswerItem in allAnswers)
                {
                    if(allAnswerItem.Answer == answerItem.Answer)
                    {
                        result.Add(answerItem.Answer);
                    }
                }
                
                double number = Math.Round((double)(result.Count() * 100) / counting, 2);

                if (result.Count == 0)
                {
                    result.Add(answerItem.Answer);
                    number = 0;
                }

                resultAnswers.Add(new Tuple<List<string>, double>(result, number));
            }

            
            //double da = Math.Round((double)(resultAnswers[1].Count() * 100) / resultAnswers.Count(), 2);

            return Json(resultAnswers.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SentEmails(List<string> emails, int pollId)
        {
            var poll = db.Polls.Where(w => w.Id == pollId).FirstOrDefault();
            List<SentEmailViewModel> sentLinkToEmail = new List<SentEmailViewModel>();

            foreach (var emailItem in emails)
            {
                bool isvalid = new EmailAddressAttribute().IsValid(emailItem.Trim());
                if (isvalid && (emailItem.Trim() != ""))
                {
                    var email = new SentEmail()
                    {
                        Date = DateTime.Now,
                        Email = emailItem.Trim(),
                        PollId = poll.Id
                    };

                    sentLinkToEmail.Add(new SentEmailViewModel() 
                    { 
                        Date = DateTime.Now.ToString(),
                        Email = emailItem.Trim()
                    });
                    db.SentEmails.Add(email);

                   

                }
            }

            if(sentLinkToEmail.Count == 0)
            {
                return null;
            }

           

            string firstName = db.Users.Where(w => w.Email == User.Identity.Name).FirstOrDefault().FirstName;
            string lastName = db.Users.Where(w => w.Email == User.Identity.Name).FirstOrDefault().LastName;

            foreach (var emailItem in sentLinkToEmail)
            {
                var body = "<p>From: {0} {1} </p><p>Please fill in the poll. <a href=\"http://localhost:51687/{2}\">Click Here</a> :)</p>";
                MailMessage mailMessage = new MailMessage();
                MailAddress fromAddress = new MailAddress(User.Identity.GetUserName());
                mailMessage.From = fromAddress;
                mailMessage.To.Add(emailItem.Email);
                mailMessage.Body = string.Format(body, firstName, lastName, poll.Id.ToString());
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = poll.Name;

                SmtpClient mailSender = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("wearenbu@gmail.com", "wearenbu12")
                };

                mailSender.Send(mailMessage);
            }
           


            db.SaveChanges();

            return Json(sentLinkToEmail, JsonRequestBehavior.AllowGet);
        }

        // GET: Quiz
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Poll(string id)
        {
            int convertedId;
            bool parseId = Int32.TryParse(id, out convertedId);

            
            if(Request.IsAuthenticated)
            {

                if (id == "Manage")
                {
                    return RedirectToAction("Change", "Manage");
                }
                if (id == "Poll")
                {
                    return View("Index");
                }

                if (convertedId == 0)
                {
                    var user = db.UsersFromThePoll.Where(w => w.HashCode == id).FirstOrDefault();

                    if (user != null)
                    {
                        var poll = db.Polls.Where(w => w.Id == user.PollId).FirstOrDefault();
                        TempData["pollPerPage"] = poll.QuationsPerPage;
                        TempData["pollQuestionCount"] = poll.PollQuastions.Count;
                        TempData["pollId"] = poll.Id;
                        TempData["userId"] = user.HashCode;

                        return View("PollHashCode");
                    }
                    else
                    {
                        return View("NotFound");
                    }

                }
                else 
                {
                    return RedirectToAction("View", new { id = convertedId });
                }

                
            }
            else
            {

                if(convertedId == 0)
                {
                    var user = db.UsersFromThePoll.Where(w => w.HashCode == id).FirstOrDefault();
                   

                    if (user != null)
                    {
                        var poll = db.Polls.Where(w => w.Id == user.PollId).FirstOrDefault();
                        TempData["pollPerPage"] = poll.QuationsPerPage;
                        TempData["pollQuestionCount"] = poll.PollQuastions.Count;
                        TempData["pollId"] = poll.Id;
                        TempData["userId"] = user.HashCode;

                        return View("PollHashCode");
                    }
                    else
                    {
                        return View("PollNotFound");
                    }

                    
                }
                else
                {
                    var poll = db.Polls.Where(w => w.Id == convertedId).FirstOrDefault();

                    if (poll != null)
                    {
                        TempData["timeStart"] = DateTime.Now.ToString();
                        TempData["convertedId"] = convertedId;

                        TempData["pollPerPage"] = poll.QuationsPerPage;
                        TempData["pollQuestionCount"] = poll.PollQuastions.Count;
                        TempData["pollId"] = poll.Id;
                        TempData["pollType"] = poll.Type;

                        return View((new RECaptcha()));
                    }
                    else
                    {
                        TempData["convertedId"] = convertedId;
                        return View("PollNotFound");
                    }
                }
            }
        }

        [AllowAnonymous]
        [ActionName("PollPartial")]
        public ActionResult _PollPartial(int id)
        {

            TempData["timeStart"] = DateTime.Now.ToString();

            var poll = db.Polls.Where(w => w.Id == id).FirstOrDefault();

            TempData["pollPerPage"] = poll.QuationsPerPage;
            TempData["pollQuestionCount"] = poll.PollQuastions.Count;
            TempData["pollId"] = poll.Id;
            TempData["pollType"] = (int)poll.Type;

            return PartialView((new RECaptcha()));
        }

        [AllowAnonymous]
        [ActionName("PollWellSentPartial")]
        [HttpPost]
        public ActionResult _PollWellSentPartial(List<List<string>> model, int pollId, UserAuthentication userAuthentication)
        {
            var pollDb = db.Polls.Where(w => w.Id == pollId).FirstOrDefault();

            if (pollDb == null)
            {
                return RedirectToAction("PollPartial", "Poll");
            }

            var questionsDb = db.PollQuastions.Where(w => w.PollId == pollDb.Id);

            var answersDb = new List<IQueryable<PollAnswer>>();

            foreach (var questionItem in questionsDb)
            {

                answersDb.Add(db.PollAnswers.Where(w => w.PollQuastionId == questionItem.Id));

            }


            var answers = new List<List<string>>();

            int index = 0;
            foreach (var answerDbItem in answersDb)
            {
                var list = new List<string>();
                
                foreach (var answerItem in answerDbItem)
                {

                    for (int i = 0; i < model[index].Count; i++)
                    {
                        if (model[index][i] == answerItem.Answer)
                        {
                            list.Add(answerItem.Answer);
                        }
                    }
                }

                index += 1;

                answers.Add(list);
            }

            int breakModelCount = 0;
            for (int i = 0; i < answers.Count; i++)
            {
                for (int j = 0; j < answers[i].Count; j++)
                {
                    if(!string.IsNullOrEmpty(model[i][j]))
                    {
                        if (model[i][j] != answers[i][j])
                        {
                            breakModelCount += 1;
                            break;
                        }
                    }
                   
                }
                if(breakModelCount != 0)
                {
                    break;
                }
            }

            if (breakModelCount != 0)
            {
                return RedirectToAction("PollPartial", "Poll");
            }

            string userIpAddress = Request.UserHostAddress;

            if(pollDb.Type == 1)
            {
                bool isvalid = new EmailAddressAttribute().IsValid(userAuthentication.Email.Trim());
                if(isvalid == false)
                {
                    return null;
                }
                var fnumber = db.UsersFromThePoll.Where(w => (w.FacultyNumber == userAuthentication.FNumber) && (w.PollId == pollId)).FirstOrDefault();
                if(fnumber != null)
                {
                    return null;
                }
                if(userAuthentication.FName == "")
                {
                    return null;
                }
                if(userAuthentication.LName == "")
                {
                    return null;
                }


                var user = new User()
                {
                    HashCode = Guid.NewGuid().ToString(),
                    PollId = pollDb.Id,
                    Email = userAuthentication.Email,
                    FacultyNumber = userAuthentication.FNumber,
                    FirstName = userAuthentication.FName,
                    LastName = userAuthentication.LName,
                    IPAddress = userIpAddress
                };

                db.UsersFromThePoll.Add(user);
                //db.SaveChanges();


                var userPoll = new UserPoll()
                {
                    Name = pollDb.Name,
                    UserId = user.Id,
                    PollId = pollDb.Id,
                    TimeBegins = DateTime.Parse(TempData["timeStart"].ToString()),
                    TimeEnd = DateTime.Now
                };

                db.UserPolls.Add(userPoll);
                db.SaveChanges();

                foreach (var questionItem in questionsDb)
                {
                    var question = new UserPollQuestion()
                    {
                        UserPollId = userPoll.Id,
                        PollId = userPoll.PollId,
                        PollQuastionId = questionItem.Id,
                        Quastion = questionItem.Quastion
                    };

                    db.UserPollQuestions.Add(question);

                }
                db.SaveChanges();

                var userQuestionDb = db.UserPollQuestions.Where(w => (w.PollId == userPoll.PollId) && (w.UserPollId == userPoll.Id));


                int i = 0;
                foreach (var userQuestionItem in userQuestionDb)
                {


                    for (int j = 0; j < answers[i].Count; j++)
                    {
                        if (!string.IsNullOrEmpty(model[i][j]))
                        {
                            if (model[i][j] == answers[i][j])
                            {
                                var answer = new UserPollAnswer()
                                {
                                    Answer = model[i][j],
                                    PollQuastionId = userQuestionItem.PollQuastionId,
                                    UserPollQuastionId = userQuestionItem.Id
                                };

                                db.UserPollAnswers.Add(answer);
                            }
                        }
                    }
                    i++;

                }

                db.SaveChanges();

                var body = "<p>Thank you for your waste of time. This is the completed poll <a href=\"http://localhost:51687/{0}\">Click Here</a> :)</p>";
                MailMessage mailMessage = new MailMessage();
                MailAddress fromAddress = new MailAddress("wearenbu@gmail.com");
                mailMessage.From = fromAddress;
                mailMessage.To.Add(user.Email);
                mailMessage.Body = string.Format(body, user.HashCode);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = userPoll.Name;

                SmtpClient mailSender = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("wearenbu@gmail.com", "wearenbu12")
                };

                mailSender.Send(mailMessage);

            }
            else
            {

                var user = new User()
                {
                    HashCode = Guid.NewGuid().ToString(),
                    PollId = pollDb.Id
                };

                db.UsersFromThePoll.Add(user);
                //db.SaveChanges();


                var userPoll = new UserPoll()
                {
                    Name = pollDb.Name,
                    UserId = user.Id,
                    PollId = pollDb.Id,
                    TimeBegins = DateTime.Parse(TempData["timeStart"].ToString()),
                    TimeEnd = DateTime.Now
                };

                db.UserPolls.Add(userPoll);
                db.SaveChanges();

                foreach (var questionItem in questionsDb)
                {
                    var question = new UserPollQuestion()
                    {
                        UserPollId = userPoll.Id,
                        PollId = userPoll.PollId,
                        PollQuastionId = questionItem.Id,
                        Quastion = questionItem.Quastion
                    };

                    db.UserPollQuestions.Add(question);

                }
                db.SaveChanges();

                var userQuestionDb = db.UserPollQuestions.Where(w => (w.PollId == userPoll.PollId) && (w.UserPollId == userPoll.Id));


                int i = 0;
                foreach (var userQuestionItem in userQuestionDb)
                {


                    for (int j = 0; j < answers[i].Count; j++)
                    {
                        if (!string.IsNullOrEmpty(model[i][j]))
                        {
                            if (model[i][j] == answers[i][j])
                            {
                                var answer = new UserPollAnswer()
                                {
                                    Answer = model[i][j],
                                    PollQuastionId = userQuestionItem.PollQuastionId,
                                    UserPollQuastionId = userQuestionItem.Id
                                };

                                db.UserPollAnswers.Add(answer);
                            }
                        }
                    }
                    i++;

                }
              
                db.SaveChanges();
            }


            //TempData["pollIp"] = userAuthentication;
            //TempData["poll"] = answers;
            //ViewBag.Model = model;
            //TempData["poll"] = pollId;
            TempData["pollType"] = pollDb.Type;

            return PartialView("_PollWellSentPartial");
        }


        public ActionResult View(int? id)
        {
            var poll = db.Polls.Where(w => w.Id == id).FirstOrDefault();
            if (poll != null)
            {
                TempData["pollPerPage"] = poll.QuationsPerPage;
                TempData["pollQuestionCount"] = poll.PollQuastions.Count;
                TempData["pollId"] = poll.Id;

                return View();
            }
            else
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [ActionName("View")]
        public ActionResult _ViewPollPartial(int id)
        {
            var poll = db.Polls.Where(w => w.Id == id).FirstOrDefault();

            TempData["pollPerPage"] = poll.QuationsPerPage;
            TempData["pollQuestionCount"] = poll.PollQuastions.Count;
            TempData["pollId"] = poll.Id;

            return PartialView("_ViewPollPartial");
        }

        public ActionResult CreatePoll()
        {
            ViewBag.Message = "Create a Poll";

            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            var poll = db.Polls.Where(w => w.Id == id).FirstOrDefault();
            if(poll != null)
            {
                TempData["pollId"] = poll.Id;
                TempData["pollTitle"] = poll.Name;
                TempData["pollDate"] = poll.ActivationDate.ToString();
                TempData["pollType"] = poll.Type == 1 ? "Not Anonymous" : "Anonymous";
                return View();
            }
            else
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [ActionName("PollDetailsPartialById")]
        public ActionResult _PollDetailsPartial(int id)
        {
            var poll = db.Polls.Where(w => w.Id == id).FirstOrDefault();
         
            TempData["pollId"] = poll.Id;
            TempData["pollTitle"] = poll.Name;
            TempData["pollDate"] = poll.ActivationDate.ToString();
            TempData["pollType"] = poll.Type == 1 ? "Not Anonymous" : "Anonymous";
            return PartialView("_PollDetailsPartial");
        }


        [HttpPost]
        [ValidateInput(false)]
        [ActionName("PollDetailsPartial")]
        public ActionResult _PollDetailsPartial(PollViewModels pollViewModel)
        {
            var user = db.Users.Where(w => w.Email == User.Identity.Name).FirstOrDefault();
            byte quationsPerPage = 0;
            Byte.TryParse(pollViewModel.QuestionPerPage, out quationsPerPage);
            if(quationsPerPage == 0)
            {
                quationsPerPage = 5;
            }


            var poll = new Poll()
            {
                Id = 5,
                Name = pollViewModel.Name,
                Type = pollViewModel.NotAnonymousIsChecked == true ? (byte)0 : (byte)1,
                QuationsPerPage = quationsPerPage,
                ActivationDate = DateTime.Parse(pollViewModel.DateTimePoll),
                UserId = user.Id
            };

            db.Polls.Add(poll);
            db.SaveChanges();

            foreach (var question in pollViewModel.QuestionList)
            {
                byte questionTypeDropDown = 0;
                Byte.TryParse(question.QuestionSelectedDropDown, out questionTypeDropDown);
                var pollQuestion = new PollQuastion()
                {
                    AnswerOrientation = question.QuestionTypeVer == true ? (byte)0 : (byte)1,
                    Quastion = question.Question,
                    Type = questionTypeDropDown,
                    PollId = poll.Id
                };
                db.PollQuastions.Add(pollQuestion);
                db.SaveChanges();
            }
            

            var questionListDb = db.PollQuastions.Where(w => w.PollId == poll.Id).ToList();

            int number = 0;
            foreach (var question in questionListDb)
            {
                for (int i = 0; i < pollViewModel.QuestionList[number].QuestionInputList.Count; i++)
                {
                    var pollAnswer = new PollAnswer()
                    {
                        Answer = pollViewModel.QuestionList[number].QuestionInputList[i].Value,
                        PollQuastionId = question.Id
                    };

                    db.PollAnswers.Add(pollAnswer);
                    db.SaveChanges();
                }
                
                number += 1;
            }

            TempData["pollId"] = poll.Id;
            TempData["pollTitle"] = poll.Name;
            TempData["pollDate"] = poll.ActivationDate.ToString();
            TempData["pollType"] = poll.Type == 1 ? "Not Anonymous" : "Anonymous";
            //TempData["da2"] = pollDb.Name;
            //TempData["da3"] = poll.QuestionList[0].QuestionInputList.FirstOrDefault().Value;
            //TempData["da4"] = poll.AnonymousOrNot;
            return PartialView("_PollDetailsPartial");
        }


        // POST: User/UserModels/Delete/5
        [HttpPost, ActionName("DeletePoll")]
        public ActionResult DeleteConfirmed(int id)
        {
            var poll = db.Polls.Where(w => w.Id == id).FirstOrDefault();
            db.Polls.Remove(poll);
            db.SaveChanges();

            return PartialView("_IndexPollPartial");
        }


        public ActionResult Export(int? id)
        {
            var sb = new StringBuilder();

            var users = db.UsersFromThePoll.Where(w => w.PollId == id);
            var polls = db.UserPolls.Where(w => w.PollId == id);
            var questions = db.UserPollQuestions.Where(w => w.PollId == id);
            var answersList = new List<Tuple<int, string, int>>();

            sb.AppendFormat(
                    "{0} -> {1}", "Users", Environment.NewLine);
            sb.AppendFormat(
                    "{0} | {1} | {2} | {3} | {4} | {5}", "Email", "First Name", "Last Name", "Faculty Number", 
                    "Hash Code", Environment.NewLine);
            foreach (var item in users)
            {
                sb.AppendFormat(
                    "{0} | {1} | {2} | {3} | {4} | {5}", item.Email, item.FirstName, item.LastName, item.FacultyNumber, item.HashCode, Environment.NewLine);

            }

            sb.AppendFormat(Environment.NewLine, Environment.NewLine);
            sb.AppendFormat(
                    "{0} -> {1}", "Polls", Environment.NewLine);
            sb.AppendFormat(
                    "{0} | {1} | {2} | {3} | {4} | {5}", "Poll's Id", "Title", "Time Begins", "Time End", "Poll's Id", Environment.NewLine);

            foreach (var item in polls)
            {
                sb.AppendFormat(
                    "{0} | {1} | {2} | {3} | {4} | {5}", item.Id, item.Name, item.TimeBegins, item.TimeEnd, item.PollId, Environment.NewLine);
            }

            sb.AppendFormat(Environment.NewLine, Environment.NewLine);

            sb.AppendFormat(
                    "{0} -> {1}", "Questions", Environment.NewLine);
            sb.AppendFormat(
                    "{0} | {1} | {2} | {3}", "Question's Id", "Question", "Poll's Id", Environment.NewLine);

            foreach (var item in questions)
            {

                answersList.Add(new Tuple<int, string, int>(item.Id, item.Quastion, item.PollId));
                
            }

            foreach (var item in answersList)
            {
                sb.AppendFormat(
                    "{0} -> {1}", "Question", Environment.NewLine);

                sb.AppendFormat("{0} | {1} | {2} | {3}", item.Item1, item.Item2, item.Item3, Environment.NewLine);

                sb.AppendFormat(
                         "{0} -> {1}", "Answers", Environment.NewLine);
                sb.AppendFormat(
                       "{0} | {1} | {2} | {3}", "Answer's Id", "Answer", "Question's Id", Environment.NewLine);

                foreach (var answer in db.UserPollAnswers)
                {
                   if(item.Item1 == answer.UserPollQuastionId)
                   {
                        sb.AppendFormat("{0} | {1} | {2} | {3}", answer.Id, answer.Answer, item.Item1, Environment.NewLine);
                    }
                }
            }
            

            return this.File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", string.Format("Log-{0}.csv", 
            DateTime.Now.ToString("g").Replace("/", "-").Replace(":", "_").Replace(" ", "-")));
        }

    }
}
