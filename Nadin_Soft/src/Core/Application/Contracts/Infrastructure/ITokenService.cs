using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        string BuildToken(ApplicationUser user, bool rememberme);
    }
}
