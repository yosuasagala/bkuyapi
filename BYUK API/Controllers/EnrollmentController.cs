using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using BYUK_API.Function;
using BYUK_API.Models;

namespace BYUK_API.Controllers
{
    
    public class EnrollmentController : ApiController
    {
        String status = "", desc = "", id = "";
        int count_id = 0;
        Util cu = new Util();
        QueryUtil dbu = new QueryUtil();
        Validation val = new Validation();
        DataColumn dataColumn = new DataColumn();
        DataTable data = new DataTable();

        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/course/enroll")]
        public IHttpActionResult CourseUpload([FromBody] EnrollCourse trx)
        {
            int indata;
            ResponseMsg res = new ResponseMsg();
            

            if (val.validationEnrollCourse(trx))
            {
                DataTable cekCourse = dbu.getEnrollCourse(trx.idCourse, trx.usrid);

                try
                {
                    if (cekCourse == null || cekCourse.Rows.Count == 0)
                    {
                        indata = dbu.insertEnrollCourse(trx.usrid, trx.idCourse, trx.flag);
                    }
                    else
                    {
                         indata = dbu.updateEnrollCourse(trx.usrid, trx.idCourse, trx.flag);
                    }

                    if (indata == 1)
                    {
                        status = Constant.rc00;
                        desc = Constant.actionsucces;
                    }
                    else
                    {
                        status = Constant.rc00;
                        desc = Constant.actionfailed;
                    }
                }
                catch (Exception ex)
                {
                    status = Constant.rc01;
                    desc = ex.Message;
                    cu.Logging("Exception", ex.ToString());
                }
            }
            else
            {
                status = Constant.rc01;
                desc = "Invalid Input";
            }

            res = new ResponseMsg
            {
                Status = status,
                Description = desc
            };

            return Ok(res);
        }

        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        [HttpGet]
        [Route("api/course/allenrolled/{userId}")]
        public IHttpActionResult GetDataCourseEnroll(string userId)
        {
            ResponseMsgCourseDataAll res = new ResponseMsgCourseDataAll();
            List<PostCourse> resdata = new List<PostCourse>();
            

            if (userId != null && userId != "")
            {
                DataTable cekCourse = dbu.geAllCourseDataEnroll(userId);

                if (cekCourse.Rows.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < cekCourse.Rows.Count; i++)
                        {
                            resdata.Add(new PostCourse
                            {
                                id = cekCourse.Rows[i]["id"].ToString(),
                                title = cekCourse.Rows[i]["title"].ToString(),
                                description = cekCourse.Rows[i]["description"].ToString(),
                                subject = cekCourse.Rows[i]["subject"].ToString(),
                                file = cekCourse.Rows[i]["file"].ToString(),
                                urlVideo = cekCourse.Rows[i]["videourl"].ToString()

                            });
                        }

                        status = Constant.rc00;
                        desc = Constant.actionsucces;

                    }
                    catch (Exception ex)
                    {
                        status = Constant.rc01;
                        desc = ex.Message;
                    }
                }
                else
                {
                    status = Constant.rc01;
                    desc = "Data Course tidak ditemukan";
                }
            }
            else
            {
                status = Constant.rc01;
                desc = "Invalid Input";
            }

            res = new ResponseMsgCourseDataAll
            {
                Status = status,
                Description = desc,
                Data = resdata
            };

            return Ok(res);
        }

        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        [HttpGet]
        [Route("api/course/searchenrolled/{userId}")]
        public IHttpActionResult GetDataCourseEnrollSearch(string userId, string title)
        {
            ResponseMsgCourseDataAll res = new ResponseMsgCourseDataAll();
            List<PostCourse> resdata = new List<PostCourse>();


            if (userId != null && userId != "")
            {
                DataTable cekCourse = dbu.geAllCourseDataEnrollsearch(userId, title);

                if (cekCourse.Rows.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < cekCourse.Rows.Count; i++)
                        {
                            resdata.Add(new PostCourse
                            {
                                id = cekCourse.Rows[i]["id"].ToString(),
                                title = cekCourse.Rows[i]["title"].ToString(),
                                description = cekCourse.Rows[i]["description"].ToString(),
                                subject = cekCourse.Rows[i]["subject"].ToString(),
                                file = cekCourse.Rows[i]["file"].ToString(),
                                urlVideo = cekCourse.Rows[i]["videourl"].ToString()

                            });
                        }

                        status = Constant.rc00;
                        desc = Constant.actionsucces;

                    }
                    catch (Exception ex)
                    {
                        status = Constant.rc01;
                        desc = ex.Message;
                    }
                }
                else
                {
                    status = Constant.rc01;
                    desc = "Data Course tidak ditemukan";
                }
            }
            else
            {
                status = Constant.rc01;
                desc = "Invalid Input";
            }

            res = new ResponseMsgCourseDataAll
            {
                Status = status,
                Description = desc,
                Data = resdata
            };

            return Ok(res);
        }
    }
}