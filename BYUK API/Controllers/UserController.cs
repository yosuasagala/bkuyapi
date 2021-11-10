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
    //[EnableCors(origins: "https://belajaryuk.netlify.app/", headers: "*", methods: "*")]
    public class UserController : ApiController
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
        [Route("api/auth/login")]
        public IHttpActionResult RequestLogin([FromBody] Login trx)
        {
            string role = null;
            string email = null;
            string id = null;
            ResponseMsgLogin res = new ResponseMsgLogin();
            DataTable cekUser = new DataTable();
            
            try
            {
                if (val.validationInputLogin(trx))
                {
                    cekUser = dbu.getUserAuth(trx.UserName, trx.Password);

                    if (cekUser.Rows.Count > 0)
                    {
                        if (cekUser.Rows[0]["login_stat"].ToString() != "Y")
                        {
                            int result = dbu.UpdateLoginStat(trx.UserName, "Y");

                            if (result > 0)
                            {
                                status = Constant.rc00;
                                desc = "Login Berhasil";
                                role = cekUser.Rows[0]["role"].ToString();
                                email = cekUser.Rows[0]["email"].ToString();
                                id = cekUser.Rows[0]["id"].ToString();
                            }
                            else
                            {
                                status = Constant.rc01;
                                desc = "Login Gagal";
                            }
                        }
                        else
                        {
                            status = Constant.rc01;
                            desc = "User sudah login";
                        }
                    }
                    else
                    {
                        status = Constant.rc01;
                        desc = "UserId/Password Salah";
                    }
                }
                else
                {
                    status = Constant.rc01;
                    desc = "Invalid Input";
                }

            }
            catch (Exception ex)
            {
                status = Constant.rc01;
                desc = ex.Message;
                cu.Logging("Exception", ex.ToString());
            }

            res = new ResponseMsgLogin
            {
                Status = status,
                Description = desc,
                Role = role,
                Email = email,
                ID = id
            };
            return Ok(res);
        }

        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/auth/logout")]
        public IHttpActionResult RequestLogout([FromBody] Login trx)
        {
            String userid = "", nama = "", role = "", project = "";
            ResponseMsgLogin res = new ResponseMsgLogin();
            
            try
            {
                if (val.validationInputLogout(trx))
                {
                    DataTable cekUser = dbu.getUserAuth2(trx.UserName);

                    if (cekUser.Rows.Count > 0)
                    {
                        if (cekUser.Rows[0]["login_stat"].ToString() != "N")
                        {
                            int result = dbu.UpdateLoginStat(trx.UserName, "N");

                            if (result > 0)
                            {
                                status = Constant.rc00;
                                desc = "Logout Berhasil";
                            }
                            else
                            {
                                status = Constant.rc01;
                                desc = "Logout Gagal";
                            }
                        }
                        else
                        {
                            status = Constant.rc01;
                            desc = "User belum login";
                        }
                    }
                    else
                    {
                        status = Constant.rc01;
                        desc = "UserId/Password Salah";
                    }
                }
                else
                {
                    status = Constant.rc01;
                    desc = "Invalid Input";
                }

            }
            catch (Exception ex)
            {
                status = Constant.rc01;
                desc = ex.Message;
                cu.Logging("Exception", ex.ToString());
            }

            res = new ResponseMsgLogin
            {
                Status = status,
                Description = desc
            };
            return Ok(res);
        }

        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/auth/regis")]
        public IHttpActionResult RegisterUser([FromBody] Regis trx)
        {
            ResponseMsg res = new ResponseMsg();

            if (val.validationInputRegis(trx))
            {
                DataTable cekUser = dbu.getUserbyName(trx.UserName);

                if (cekUser == null || cekUser.Rows.Count == 0)
                {
                    try
                    {
                        int indata = dbu.insertNewUser(trx.UserName, cu.HashPassword(trx.UserName, trx.Password), trx.NoHP);

                        status = Constant.rc00;
                        desc = Constant.actionsucces;

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
                    desc = "User sudah terdaftar";
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