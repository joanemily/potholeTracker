using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public interface IPotholeDAO
    {
        IList<Pothole> GetPotholes();

        Pothole GetSinglePothole(int potholeId);

        /// <summary>
        /// Updates the inspection/repair status of a given pothole
        /// </summary>
        /// <param name="pothole"></param>
        void UpdatePothole(Pothole pothole);


        /// <summary>
        /// Gets a list of potholes that need to be set up for inspection.
        /// </summary>
        
        void DeletePothole(int potholeId);

        IList<Pothole> GetFilteredPotholes(string filter);

       int AddPothole(Pothole pothole);


    }
}