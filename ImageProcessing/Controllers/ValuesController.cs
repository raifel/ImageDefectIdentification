using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImageProcessing.Services;
using Microsoft.Azure;
using Swashbuckle.Swagger.Annotations;

namespace ImageProcessing.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public string Get(int id, string actualImageContainer, string firstImage, string secondImage)
        {
            CompleteImageComarision(actualImageContainer, firstImage, CloudConfigurationManager.GetSetting("referenceImageFile1"), CloudConfigurationManager.GetSetting("referenceImageContainer"));
            CompleteImageComarision(actualImageContainer, secondImage, CloudConfigurationManager.GetSetting("referenceImageFile2"), CloudConfigurationManager.GetSetting("referenceImageContainer"));
            return "Completed the Process";
        }


        private void CompleteImageComarision(string actualImageContainer, string actualImage, string referenceImage, string referenceImageContainer)
        {
            var df = new BlobStorageService();
            var motionDetector = new MotionDetector();
            motionDetector.ProcessFrame(df.ReadImageContent(referenceImage, referenceImageContainer));
            var result = new ImageComparisionResult();
            result.UploadImageAfterProcess(motionDetector.ProcessFrame(df.ReadImageContent(actualImage, actualImageContainer)), actualImageContainer);
        }


        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(int id)
        {
        }

    }
}
