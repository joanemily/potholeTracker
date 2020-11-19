using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public interface IPhotoDAO
    {
        /// <summary>
        /// Saves the data from
        /// the photo that was uploaded
        /// and stored in vue to sql.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        int UploadPhoto(Photo photo);

        /// <summary>
        /// Gets a single photo's information to save in the vue.
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        Photo GetPhoto(int photoId);

        /// <summary>
        /// Gets a list of photos related to a single pothole.
        /// </summary>
        /// <param name="potholeId"></param>
        /// <returns></returns>
        List<Photo> GetPhotos(int potholeId);


    }
}
