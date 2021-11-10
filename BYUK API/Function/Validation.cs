using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BYUK_API.Models;

namespace BYUK_API.Function
{
    public class Validation
    {

        public bool validationInputRegis(Regis trx)
        {
            bool result = false;


            if (trx.UserName != "" && trx.Password != "" && trx.NoHP != "" && trx.UserName != null && trx.Password != null && trx.NoHP != null && IsValidEmail(trx.UserName))
            {
                result = true;
            }

            return result;

        }

        private bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool validationInputLogin(Login trx)
        {
            bool result = false;


            if (trx.UserName != "" && trx.Password != "" && trx.UserName != null && trx.Password != null && IsValidEmail(trx.UserName))
            {
                result = true;
            }

            return result;

        }

        public bool validationInputLogout(Login trx)
        {
            bool result = false;


            if (trx.UserName != "" && trx.UserName != null && IsValidEmail(trx.UserName))
            {
                result = true;
            }

            return result;

        }


        public bool validationInputCourse(PostCourse trx)
        {
            bool result = false;


            if (trx.title != "" && trx.description != "" && trx.urlVideo != "" && trx.subject != "" && trx.file != "" && trx.title != null && trx.description != null && trx.urlVideo != null && trx.subject != null && trx.file != null)
            {
                result = true;
            }

            return result;

        }

        public bool validationEnrollCourse(EnrollCourse trx)
        {
            bool result = false;


            if (trx.idCourse != "" && trx.usrid != "" && trx.flag != "" && trx.idCourse != null && trx.usrid != null && trx.flag != null)
            {
                result = true;
            }

            return result;

        }
    }
}