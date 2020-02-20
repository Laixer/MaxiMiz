using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Maximiz.Storage.Abstraction
{

    /// <summary>
    /// Contract for a storage manager.
    /// </summary>
    public interface IStorageManager
    {

        /// <summary>
        /// Uploads a file to the storage.
        /// </summary>
        /// <param name="file"><see cref="IFormFile"/></param>
        /// <returns>Bool indicating success</returns>
        Task<bool> UploadFile(IFormFile file);

        /// <summary>
        /// Gets all uploaded images from our storage.
        /// </summary>
        /// <param name="query">Search string</param>
        /// <returns><see cref="Uri"/> list for images</returns>
        Task<IEnumerable<Uri>> GetUploadedImages(string query = null);

    }
}
