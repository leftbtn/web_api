using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiServices.BLLServices
{
    public class BaseService
    {
        public GBLLService BLLService { get; set; }

        public myproEntities db { get { return BLLService.DbContext; } }

        public string GetCurrentLoginAccountId()
        {
            return string.Empty;
        }
    }
}
