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
using Newtonsoft.Json;

namespace BYUK_API.Controllers
{

    public class CourseController : ApiController
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
        [Route("api/course/upload")]
        public IHttpActionResult CourseUpload([FromBody] PostCourse trx)
        {
            cu.Logging("INPUT", JsonConvert.SerializeObject(trx));
            ResponseMsg res = new ResponseMsg();
            

            if (val.validationInputCourse(trx))
            {
                DataTable cekCourse = dbu.getCoursebyTitle(trx.title);

                if (cekCourse == null || cekCourse.Rows.Count == 0)
                {
                    try
                    {
                        int indata = dbu.insertNewCourse(trx.title, trx.subject, trx.description, trx.urlVideo, trx.file);

                        if (indata == 1)
                        {
                            status = Constant.rc00;
                            desc = Constant.actionsucces;
                        }
                        else
                        {
                            status = Constant.rc01;
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
                    desc = "Course sudah terdaftar";
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
        [Route("api/course/getall")]
        public IHttpActionResult GetDataCourseAll()
        {
            ResponseMsgCourseDataAll res = new ResponseMsgCourseDataAll();
            List<PostCourse> resdata = new List<PostCourse>();

            DataTable cekCourse = new DataTable();
            cekCourse = dbu.geAllCourseData();


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
        [Route("api/course/search")]
        public IHttpActionResult GetSearchDataCourseAll(string title)
        {
            ResponseMsgCourseDataAll res = new ResponseMsgCourseDataAll();
            List<PostCourse> resdata = new List<PostCourse>();

            DataTable cekCourse = dbu.geAllCourseDatawithParam(title);

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
        [Route("api/course/detail/{id}")]
        public IHttpActionResult GetDataCourse(string id)
        {
            ResponseMsgCourseData res = new ResponseMsgCourseData();
            PostCourse resdata = new PostCourse();

            DataTable cekCourse = dbu.geDetailCourseData(id);

            if (cekCourse.Rows.Count > 0)
            {
                try
                {
                    resdata.id = cekCourse.Rows[0]["id"].ToString();
                    resdata.title = cekCourse.Rows[0]["title"].ToString();
                    resdata.description = cekCourse.Rows[0]["description"].ToString();
                    resdata.subject = cekCourse.Rows[0]["subject"].ToString();
                    resdata.file = cekCourse.Rows[0]["file"].ToString();
                    resdata.urlVideo = cekCourse.Rows[0]["videourl"].ToString();

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

            res = new ResponseMsgCourseData
            {
                Status = status,
                Description = desc,
                Data = resdata
            };

            return Ok(res);
        }

        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/course/update")]
        public IHttpActionResult CourseUpdate([FromBody] PostCourse trx)
        {
            cu.Logging("INPUT", JsonConvert.SerializeObject(trx));
            ResponseMsg res = new ResponseMsg();


            if (val.validationInputCourse(trx))
            {
                DataTable cekCourse = dbu.getCoursebyTitle(trx.title);

                if (cekCourse != null || cekCourse.Rows.Count > 0)
                {
                    try
                    {
                        int indata = dbu.updateCourse(trx.id, trx.title, trx.subject, trx.description, trx.urlVideo, trx.file, trx.status);

                        if (indata == 1)
                        {
                            status = Constant.rc00;
                            desc = Constant.actionsucces;
                        }
                        else
                        {
                            status = Constant.rc01;
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
                    desc = "Course tidak ditemukan";
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


    }

    
}