using CleanAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.Services.Interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(User user);
    }
}
