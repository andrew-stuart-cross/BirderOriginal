﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Birder2.Services;
using ImageResizeWebApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Birder2.Controllers
{
    [Route("api/[controller]")]
    //[ApiController] 
    public class ImagesController : ControllerBase
    {
        //private readonly IApplicationUserAccessor _userAccessor;
        private readonly IImageApiHelperService _imageApiHelperService;
        private readonly IConfiguration _config;

        public ImagesController(IConfiguration config, IImageApiHelperService imageApiHelperService)
        {
            //_userAccessor = userAccessor;
            _imageApiHelperService = imageApiHelperService;
            _config = config;
        }

        //private readonly AzureStorageConfig storageConfig = null;

        //public ImagesController(IOptions<AzureStorageConfig> config)
        //{
        //    storageConfig = config.Value;
        //}

        // POST /api/images/upload
        //[Route("Post/{solution}/{answer}")]
        //[Route("api/[controller]/upload/{files}")]
        [HttpPost("[action]")]
        //[HttpPost(Name = "[action]")]
        public async Task<IActionResult> Upload([FromForm]ICollection<IFormFile> files, int observationId)
        {
            bool isUploaded = false;

            try
            {

                if (files.Count == 0)

                    return BadRequest("No files received from the upload");

                if (_config["BlobStorageKey"] == string.Empty || _config["BlobStorage:Account"] == string.Empty)

                    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");

                //if (storageConfig.ImageContainer == string.Empty)

                //    return BadRequest("Please provide a name for your image container in the azure blob storage");

                foreach (var formFile in files)
                {
                    if (StorageHelper.IsImage(formFile))
                    {
                        if (formFile.Length > 0)
                        {
                            using (Stream stream = formFile.OpenReadStream())
                            {
                                isUploaded = await StorageHelper.UploadFileToStorage(stream, observationId.ToString(), formFile.FileName); //, storageConfig);
                            }
                        }
                    }
                    else
                    {
                        return new UnsupportedMediaTypeResult();
                    }
                }

                if (isUploaded)
                {
                    _imageApiHelperService.UpdateImagesAttachedValue(observationId, true);
                    //if ("test" != string.Empty)

                    return new AcceptedAtActionResult("GetThumbNails", "Images", null , observationId.ToString());

                    //else
                    
                    //    return new AcceptedResult();
                }
                else

                    return BadRequest("Look like the image couldnt upload to the storage");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/images/thumbnails
        [HttpGet("thumbnails")]
        //[HttpGet]
        //[HttpGet("{containerName}", Name = "thumbnails")]
        public async Task<IActionResult> GetThumbNails(int observationId)
        {
            //var user = await _userAccessor.GetUser();
            var areImagesAvailable = _imageApiHelperService.AreImagesAttachedAsync(observationId);
            if (areImagesAvailable != true)
            {
                return Accepted("Observation status indicates images not available");
            }

            try
            {
                //if (_config["BlobStorageKey"] || _config["BlobStorage:Account"] == string.Empty)

                //    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");

                //if ("test" == string.Empty)

                //    return BadRequest("Please provide a name for your image container in the azure blob storage");

                List<string> thumbnailUrls = await StorageHelper.GetThumbNailUrls(observationId.ToString()); //(storageConfig);

                if(thumbnailUrls.Count == 0)
                {
                    _imageApiHelperService.UpdateImagesAttachedValue(observationId, false);
                }

                return new ObjectResult(thumbnailUrls);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
    /*
        // GET: api/Images
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Images/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Images
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
*/