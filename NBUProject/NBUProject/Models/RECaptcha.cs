using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBUProject.Models
{
    public class RECaptcha
    {
        public string Key = "6LeZ8SUUAAAAAGx1u0VcvCC33PcGHQw-HoyJQRbo";

        public string Secret = "6LeZ8SUUAAAAADtaN3WV1MICVMLAv-eaeOVcbzOa";
        public string Response { get; set; }
    }
}