using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleApi.DAL;
using SampleApi.Models;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("api/potholes")]
    public class PotholesController : Controller
    {

        // Add ref to DAO
        private IPotholeDAO potholeDAO;

        public PotholesController(IPotholeDAO potholeDAO)
        {
            this.potholeDAO = potholeDAO;
        }

        // Get a list of all potholes
        [HttpGet("")]
        public IActionResult GetPotholes()
        {
            IList<Pothole> potholes = potholeDAO.GetPotholes();

            return new JsonResult(potholes);
        }

        /// <summary>
        /// Get a list of potholes that is filtered by their status.
        /// </summary>
        /// <param name="status">Depending on the preference of the user, you can look at reported, inspected or repaired potholes.</param>
        /// <returns></returns>
        [HttpGet("filtered")]
        public IActionResult GetFilteredPotholes(string status)
        {
            IList<Pothole> potholes = potholeDAO.GetFilteredPotholes(status);
            return new JsonResult(potholes);
        }

        /// <summary>
        /// Deletes a pothole that has been selected by the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("detail/{id}")]
        public IActionResult DeletePothole(int id)
        {
            potholeDAO.DeletePothole(id);
            return Ok();
        }

        /// <summary>
        /// Gets the detail of a specific pothole to display on the page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public IActionResult GetPothole(int id)
        {
            Pothole pothole = potholeDAO.GetSinglePothole(id);
            return new JsonResult(pothole);
        }

        /// <summary>
        /// Updates the pothole information that user inputs.
        /// Specifically, the date of inspection and begin and finish dates for the repairs.
        /// It also updates the severity level if needed.
        /// </summary>
        /// <param name="pothole"></param>
        /// <returns></returns>
        [HttpPut("detail/{id}")]
        public IActionResult UpdatePothole(Pothole pothole)
        {
            potholeDAO.UpdatePothole(pothole);

            return Ok();
        }

        /// <summary>
        /// Adds a pothole to the system when a user reports it.
        /// </summary>
        /// <param name="pothole"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPothole(Pothole pothole)
        {
            int newPotholeId = potholeDAO.AddPothole(pothole);
            Pothole newPothole = potholeDAO.GetSinglePothole(newPotholeId);

            return new JsonResult(newPothole);
        }




    }
}
