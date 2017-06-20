using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DemoApp1.Domain;
using DemoApp1.Providers;
using DemoApp1.Util;

namespace DemoApp1.Controllers
{
    [Route("api/[controller]")]
    public class ClassesController : Controller
    {
        private readonly IStudentProvider _studentProvider;
        private readonly IClassProvider _classProvider;
        private readonly IFirehoseProvider _firehoseProvider;
        private readonly ISchemaValidator<Class> _classSchemaValidator;

        public ClassesController(IStudentProvider studentProvider, IClassProvider classProvider, ISchemaValidator<Class> classSchemaValidator,  IFirehoseProvider firehoseProvider)
        {
            _studentProvider = studentProvider;
            _classProvider = classProvider;
            _firehoseProvider = firehoseProvider;
            _classSchemaValidator = classSchemaValidator;
        }

        // GET api/classes/{uuid}
        [HttpGet("{classId}")]
        public async Task<ObjectResult> Get(string classId)
        {
            
        }
        
        [HttpPost("[action]")]
        public async Task<ObjectResult> Create([FromBody]string classJson)
        {
            
        }

        [HttpPost("{classId}/[action]")]
        public async Task<ObjectResult> Edit(string classId, [FromBody]string classJson)
        {
        }

        [HttpDelete("{classId}/[action]")]
        public async Task<ObjectResult> Delete(string classId)
        {
        }

        [HttpGet("[action]")]
        public async Task<ObjectResult> GetAll()
        {
            
        }


    }
}