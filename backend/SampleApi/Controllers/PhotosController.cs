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
    [Route("api/photos")]
    [ApiController]
    public class PhotosController : Controller
    {

        private IPhotoDAO photoDAO;
        private IUserDAO userDAO;

        //Dependency injection
        public PhotosController(IPhotoDAO photoDAO, IUserDAO userDAO)
        {
            this.photoDAO = photoDAO;
            this.userDAO = userDAO;
        }


        //Method called when photos need to be uploaded from vue.
        [HttpPost("upload")]
        public IActionResult UploadPhoto(Photo photo)
        {
            //If the same user calls the method a second time on the same page, the photo will pass in the user id and not the username.
            //so if the user name is not empty...
            if (photo.Username != null)
            {
                //get the user id with the username.
                photo.UserId = userDAO.GetUser(photo.Username).Id;
            }
            //If the username is empty, then the user id was saved
            //So call the DAO method to upload the photo information into the db for future use.
            int newPhotoId = photoDAO.UploadPhoto(photo);
            //Return the photo information to the vue.
            Photo photo1 = photoDAO.GetPhoto(newPhotoId);
            return new JsonResult(photo1);
        }


        /// <summary>
        /// When a pothole detail page loads, get a list photos that are associated with that particular pothole
        /// so they can be displayed on the page.
        /// </summary>
        /// <param name="potholeId">gets the photos associated with this specific pothole.</param>
        /// <returns>List of photos</returns>
        [HttpGet("load")]
        public IActionResult GetPhotos(int potholeId)
        {
            //Using the pothole id send a method to the sql db to get the list of photos and their paths.
            List<Photo> photos = photoDAO.GetPhotos(potholeId);
            //Return the photos to the vue.
            return new JsonResult(photos);
        }

    }
}