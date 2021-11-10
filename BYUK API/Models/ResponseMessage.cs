using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BYUK_API.Models
{
    public class ResponseMsgLogin
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string ID { get; set; }
        public string Role { get; set; }
    }

    public class ResponseMsg
    {
        public string Status { get; set; }
        public string Description { get; set; }
    }


}