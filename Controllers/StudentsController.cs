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
    public class StudentsController : Controller
    {
        private readonly IStudentProvider _studentProvider;
        private readonly IClassProvider _classProvider;
        private readonly IFirehoseProvider _firehoseProvider;
        private readonly ISchemaValidator<Student> _studentSchemaValidator;

        public StudentsController(IStudentProvider studentProvider, IClassProvider classProvider, ISchemaValidator<Student> studentSchemaValidator,  IFirehoseProvider firehoseProvider)
        {
            _studentProvider = studentProvider;
            _classProvider = classProvider;
            _firehoseProvider = firehoseProvider;
            _studentSchemaValidator = studentSchemaValidator;
        }

        // GET api/students/{uuid}
        [HttpGet("{studentId}")]
        public async Task<ObjectResult> Get(string studentId)
        {
            
        }
        
        [HttpPost("[action]")]
        public async Task<ObjectResult> Create([FromBody]string studentJson)
        {
            
        }

        [HttpPost("{studentId}/[action]")]
        public async Task<ObjectResult> Edit(string studentId, [FromBody]string studentJson)
        {
        }

        [HttpDelete("{studentId}/[action]")]
        public async Task<ObjectResult> Delete(string studentId)
        {
        }

        [HttpGet("[action]")]
        public async Task<ObjectResult> GetAll()
        {
            
        }

        [HttpPost("{studentId}/[action]/{classCode}")]
        public async Task<ObjectResult> Enroll(string studentId, string classCode)
        {
        }

        [HttpPost("{studentId}/[action]/{classCode}")]
        public async Task<ObjectResult> UnEnroll(string studentId, string classCode)
        {
        }


    }
}