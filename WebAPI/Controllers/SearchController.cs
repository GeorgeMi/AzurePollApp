using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class SearchController: ApiController
    {
        FormModel formModel = new FormModel();

     //   [RequireToken]
        public IEnumerable<FormDTO> Get(string id)
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
            list = formModel.GetAllForms(token, id, page_nr, per_page);
            return list;
        }
    }
}