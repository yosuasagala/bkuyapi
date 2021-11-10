using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BYUK_API.Models
{
    public class PostCourse
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string subject { get; set; }
        public string file { get; set; }
        public string urlVideo { get; set; }
        public string status { get; set; }
    }

    public class EnrollCourse
    {
        public string idCourse { get; set; }
        public string usrid { get; set; }
        public string flag { get; set; }
    }

    public class ResponseMsgCourseDataAll
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public List<PostCourse> Data { get; set; }
    }

    public class ResponseMsgCourseData
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public PostCourse Data { get; set; }
    }
}