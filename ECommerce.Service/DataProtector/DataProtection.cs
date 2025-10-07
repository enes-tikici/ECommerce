using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.DataProtector
{
    public class DataProtection : IDataProtection
    {
        private readonly IDataProtector _dataProtector;

        public DataProtection(IDataProtectionProvider provider)
        {
            _dataProtector = provider.CreateProtector("AppDataProtector");
        }
        public string Protected(string text)
        {
            return _dataProtector.Protect(text);
        }

        public string Unprotected(string unprotectedText)
        {
            return _dataProtector.Unprotect(unprotectedText);
        }
    }
}
