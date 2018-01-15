using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using apiServices;
using apiServices.BLLServices;
using CommonFrameWork;
using web_api.Models.Other;

namespace web_api.Controllers
{
    public class BaseController : ApiController
    {
        protected myproEntities db
        {
            get
            {
                return BLLService.DbContext;
            }
        }
        protected GBLLService BLLService { get; set; }

        protected JqGridJson jqGrid;
        public BaseController()
        {
            BLLService = new GBLLService(new myproEntities());
        }


        protected bool Transaction(Func<bool> fun, ref string msg, Func<Exception, string> exceptionAction = null)
        {
            var result = false;



            using (var scope = new TransactionScope())
            {
                try
                {
                    result = fun();
                    if (result) scope.Complete();
                }
                catch (Exception ex)
                {

                    msg = ex.Message;
                    if (exceptionAction != null)
                    {
                        msg = exceptionAction(ex);
                    }
                }
                finally { }
            }

            return result;
        }

        public class JqGridJson
        {
            public int page { get; set; }
            public int records { get; set; }
            public int total { get; set; }
            public List<JqgridRow> rows { get; set; }
            public JqGridJson()
            {
                rows = new List<JqgridRow>();
            }

            public class JqgridRow
            {
                public JqgridRow()
                {
                    cell = new List<object>();
                }
                public string id { get; set; }
                public List<object> cell { get; set; }
            }
        }


        public BlogServices BlogServices { get { return BLLService.BlogServices; } }
        public CarouslService CarouslService { get { return BLLService.CarouslService; } }
        public AccountServices AccountServices { get { return BLLService.AccountServices; } }
        public CommentServices CommentServices { get { return BLLService.CommentServices; } }

    }
}