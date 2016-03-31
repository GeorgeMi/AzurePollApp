using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Messages;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class FormController : ApiController
    {
        FormModel formModel = new FormModel();

        [RequireToken]
        public IEnumerable<FormDTO> Get()
        {
            // get forms 
            int page_nr = 0;
            int per_page = 10;
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();

            try
            {
                //daca exista query si este valid se schimba valorile implicite ale paginii si al numarului elementelor de pe pagina
                var queryString = this.Request.GetQueryNameValuePairs();
                foreach (KeyValuePair<string, string> pair in queryString)
                {
                    if (pair.Key == "page")
                    {
                        page_nr = Int32.Parse(pair.Value);
                    }
                    if (pair.Key == "per_page")
                    {
                        per_page = Int32.Parse(pair.Value);
                    }
                }

                if (page_nr < 0 || per_page < 0)
                {
                    page_nr = 0;
                    per_page = 10;
                }
            }
            catch
            {
                page_nr = 0;
                per_page = 10;
            }

            List<FormDTO> list = new List<FormDTO>();
            list = formModel.GetAllForms(token, page_nr, per_page);
            return list;
        }

        [RequireToken]
        [HttpGet]
        [ActionName("user")]
        public IEnumerable<FormDTO> User(string id)
        {
            // get forms 
            int page_nr = 0;
            int per_page = 10;
           
            try
            {
                //daca exista query si este valid se schimba valorile implicite ale paginii si al numarului elementelor de pe pagina
                var queryString = this.Request.GetQueryNameValuePairs();
                foreach (KeyValuePair<string, string> pair in queryString)
                {
                    if (pair.Key == "page")
                    {
                        page_nr = Int32.Parse(pair.Value);
                    }
                    if (pair.Key == "per_page")
                    {
                        per_page = Int32.Parse(pair.Value);
                    }
                }

                if (page_nr < 0 || per_page < 0)
                {
                    page_nr = 0;
                    per_page = 10;
                }
            }
            catch
            {
                page_nr = 0;
                per_page = 10;
            }

            List<FormDTO> list = new List<FormDTO>();
            list = formModel.GetUserForms(id, page_nr, per_page);
            return list;
        }

        [RequireToken]
        [HttpGet]
        [ActionName("result")]
        public VoteResultDetailDTO Result(int id) //returneaza rezultatul unui sondaj propriu
        {
            VoteResultDetailDTO voteResult = formModel.GetDetailResultForm(id);
            return voteResult;
        }

        [RequireToken]
        [HttpGet]
        [ActionName("getForm")]
        public FormDetailDTO GetForm(int id)
        {
            return formModel.GetContentForm(id);
        }

        [RequireToken]
        [HttpGet]
        [ActionName("voted")]
        public IEnumerable<FormDTO> Voted(string id) //returneaza lista sondajelor votate de catre un user
        {
            // get forms 
            int page_nr = 0;
            int per_page = 10;
          
            try
            {
                //daca exista query si este valid se schimba valorile implicite ale paginii si al numarului elementelor de pe pagina
                var queryString = this.Request.GetQueryNameValuePairs();
                foreach (KeyValuePair<string, string> pair in queryString)
                {
                    if (pair.Key == "page")
                    {
                        page_nr = Int32.Parse(pair.Value);
                    }
                    if (pair.Key == "per_page")
                    {
                        per_page = Int32.Parse(pair.Value);
                    }
                }

                if (page_nr < 0 || per_page < 0)
                {
                    page_nr = 0;
                    per_page = 10;
                }
            }
            catch
            {
                page_nr = 0;
                per_page = 10;
            }

            List<FormDTO> list = new List<FormDTO>();
            list = formModel.GetVotedForms(id, page_nr, per_page);
            return list;
        }

        [RequireToken]
        [HttpGet]
        [ActionName("category")]
        public IEnumerable<FormDTO> Category(int id) //returneaza lista sondajelor dintr-o categorie
        {
            // get forms 
            int page_nr = 0;
            int per_page = 10;
          
            try
            {
                //daca exista query si este valid se schimba valorile implicite ale paginii si al numarului elementelor de pe pagina
                var queryString = this.Request.GetQueryNameValuePairs();

                foreach (KeyValuePair<string, string> pair in queryString)
                {
                    if (pair.Key == "page")
                    {
                        page_nr = Int32.Parse(pair.Value);
                    }
                    if (pair.Key == "per_page")
                    {
                        per_page = Int32.Parse(pair.Value);
                    }
                }

                if (page_nr < 0 || per_page < 0)
                {
                    page_nr = 0;
                    per_page = 10;
                }
            }
            catch
            {
                page_nr = 0;
                per_page = 10;
            }


            List<FormDTO> list = new List<FormDTO>();
            string token = Request.Headers.SingleOrDefault(x => x.Key == "token").Value.First();

            list = formModel.GetCategoryForms(id, token, page_nr, per_page);
            return list;
        }

        [RequireToken]
        public HttpResponseMessage Post([FromBody] FormDetailDTO formDTO)
        {
            HttpResponseMessage responseMessage;

            bool response = formModel.AddForm(formDTO);
            if (response)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }

            return responseMessage;
        }

        [RequireAdminTokenOrUsername]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage responseMessage;

            bool response = formModel.DeleteForm(id);
            if (response)
            {
                SuccessMessage msg = new SuccessMessage("deleted");
                responseMessage = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }

            return responseMessage;

        }
    }
}
