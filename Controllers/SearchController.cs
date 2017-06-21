using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using DemoApp1.Domain;
using DemoApp1.Providers;
using DemoApp1.Util;
using Nest;
using System.Linq;

namespace DemoApp1.Controllers
{
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly IStudentProvider _studentProvider;
        private readonly IClassProvider _classProvider;
        private readonly IFirehoseProvider _firehoseProvider;
        private readonly ISearchProvider _searchProvider;
        private readonly ISchemaValidator<StudentSearchRequest> _studentSearchReqValidator;
        private readonly ISchemaValidator<ClassSearchRequest> _classSearchReqSchemaValidator;

        public SearchController(IStudentProvider studentProvider, IClassProvider classProvider, ISchemaValidator<ClassSearchRequest> classSearchReqSchemaValidator, ISearchProvider searchProvider,
                                  ISchemaValidator<StudentSearchRequest> studentSearchReqValidator)
                                //,  IFirehoseProvider firehoseProvider)
        {
            _studentProvider = studentProvider;
            _classProvider = classProvider;
            //_firehoseProvider = firehoseProvider;
            _searchProvider = searchProvider;
            _classSearchReqSchemaValidator = classSearchReqSchemaValidator;
            _studentSearchReqValidator = studentSearchReqValidator;
        }
    }
}