using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Types
{
    public class ServiceMassage
    {
        public bool IsSucced { get; set; }
        public string Massage { get; set; }

    }

    public class ServiceMassage<T> : ServiceMassage
    {
        public T? Data { get; set; }
    }
}
