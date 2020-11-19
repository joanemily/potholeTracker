using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Claim = SampleApi.Models.Claim;

namespace SampleApi.DAL
{
    public interface IClaimDAO
    {

        int FileClaim(Claim claim);

        IList<Claim> ViewAllClaims();
    }
}
