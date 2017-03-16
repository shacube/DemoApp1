using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Couchbase;
using Couchbase.Core;
using DemoApp1.Models.Profile;
using DemoApp1.Domain;

namespace DemoApp1.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IAmazonS3 _storageClient;
        private static readonly string _profileBucketName = "venus";

        public ProfileController(IAmazonS3 storageClient)
        {
            _storageClient = storageClient;
        }

        [HttpGet("View/{userName}")]                
        public async Task<IActionResult> Retrieve(string userName)
        {
            if(string.IsNullOrEmpty(userName))
                return View("Error");
            
            var userRepository = ClusterHelper.GetBucket(_profileBucketName);

            if(userName == "shacube")
            {
                await userRepository.UpsertAsync<ProfileModel>(userName, new ProfileModel(){
                    FirstName = "Saquib",
                    LastName = "Patla",
                    UserName = "shacube",
                    DisplayPic = null
                });
            }

            var userProfileResult = await userRepository.GetAsync<ProfileModel>(userName);

            return View("Info", userProfileResult.Value);
        }

        //public async Task<IActionResult> Create(string userJson)

        //public async Task<ObjectResult> UploadImage()
    }
}
