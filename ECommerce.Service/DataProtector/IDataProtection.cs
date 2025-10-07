using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.DataProtector
{
    public interface IDataProtection
    {
        string Protected(string text);
        string Unprotected(string unprotectedText);
    }
}
