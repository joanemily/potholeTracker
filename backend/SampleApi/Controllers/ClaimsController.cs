using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApi.DAL;
using SampleApi.Models;

namespace SampleApi.Controllers
{
    [Route("api/claim")]
    [ApiController]
    public class ClaimsController : Controller
    {
        private IClaimDAO claimDAO;
        private IUserDAO userDAO;

        public ClaimsController(IClaimDAO claimDAO, IUserDAO userDAO)
        {
            this.claimDAO = claimDAO;
            this.userDAO = userDAO;
        }

        [HttpPost]
        public IActionResult FileClaim(Claim claim)
        {
            claim.UserId = userDAO.GetUser(claim.Username).Id;
            claimDAO.FileClaim(claim);
            //int newClaimId;
            return Ok();
        }

        [HttpGet("viewAllClaims")]
        public IActionResult ViewClaims()
        {
            IList<Claim> claims = claimDAO.ViewAllClaims();
            return new JsonResult(claims);
        }

    }
}