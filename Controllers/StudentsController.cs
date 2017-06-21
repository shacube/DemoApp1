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
    public class StudentsController : Controller
    {
        private readonly IStudentProvider _studentProvider;
        private readonly IClassProvider _classProvider;
        private readonly IFirehoseProvider _firehoseProvider;
        private readonly ISearchProvider _searchProvider;
        private readonly ISchemaValidator<Student> _studentSchemaValidator;
        private readonly ISchemaValidator<StudentUpdateRequest> _studentUpdateReqSchemaValidator;

        public StudentsController(IStudentProvider studentProvider, IClassProvider classProvider, ISchemaValidator<Student> studentSchemaValidator, ISearchProvider searchProvider,
                                  ISchemaValidator<StudentUpdateRequest> studentUpdateReqSchemaValidator)
                                //,  IFirehoseProvider firehoseProvider)
        {
            _studentProvider = studentProvider;
            _classProvider = classProvider;
            //_firehoseProvider = firehoseProvider;
            _searchProvider = searchProvider;
            _studentSchemaValidator = studentSchemaValidator;
            _studentUpdateReqSchemaValidator = studentUpdateReqSchemaValidator;
        }

        // GET /students/{uuid}
        [HttpGet("{studentId}")]
        public async Task<ObjectResult> Get(string studentId)
        {
            Guid _studentId;            

            try
            {
                if(!Guid.TryParse(studentId, out _studentId))
                {       
                    return BadRequest(studentId);
                }

                var response = await _studentProvider.Retrieve(studentId);

                return response.Success ? StatusCode((int)HttpStatusCode.OK, response) : StatusCode((int)HttpStatusCode.NotFound, response);
            }
            catch(Exception ex)
            {
                /*                
                await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"StudentsController: Get(), StudentId: {studentId}, Error: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.StudentRetrieveFailed.ToString("f"),
                                                                                                        Exception = ex, Message = ex.Message});                                                                                            
            }            
        }
        
        [HttpPost("[action]")]
        public async Task<ObjectResult> Create([FromBody]string studentJson)
        {
            IList<string> errors;

            try
            {
                if(!_studentSchemaValidator.IsValid(studentJson, out errors))
                {
                    return BadRequest(studentJson);
                }

                var response = await _studentProvider.Create(JsonConvert.DeserializeObject<Student>(studentJson));

                if(response.Success)
                {
                    /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IStudent>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.StudentCreateEdit,                            
                            Timestamp = DateTime.UtcNow,
                            Payload = new ActivityPayload<IStudent>(){
                                Data = response.Data                                
                            }
                        }
                    );
                    */
                    await _searchProvider.PublishToElastic(response.Data);

                    return StatusCode((int)HttpStatusCode.Created, response);
                }
                else
                {
                    /*                
                    await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"StudentsController: Create(), RequestJson: {studentJson}, ErrorResponse: {response}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */                    
                    return StatusCode((int)HttpStatusCode.InternalServerError, response);
                }
            }
            catch(Exception ex)
            {
                /*                
                await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"StudentsController: Create(), RequestJson: {studentJson}, Exception: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IStudent>(){Success = false, ResponseStatus = CustomResponseErrors.StudentCreateFailed.ToString("f"),
                                                                                                            Exception = ex, Message = ex.Message});            
            }                    
        }

        [HttpPost("{studentId}/[action]")]
        public async Task<ObjectResult> Edit(string studentId, [FromBody]string studentUpdateRequestJson)
        {
            
            IList<string> errors;

            try
            {
                if(!_studentUpdateReqSchemaValidator.IsValid(studentUpdateRequestJson, out errors))
                {
                    return BadRequest(studentUpdateRequestJson);
                }
                
                var response = await _studentProvider.Update(JsonConvert.DeserializeObject<StudentUpdateRequest>(studentUpdateRequestJson));

                if(response.Success)
                {
                    /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IClass>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.StudentEdit,                            
                            Timestamp = DateTime.UtcNow,
                            Payload = new ActivityPayload<IClass>(){
                                Data = response.Data                                
                            }
                        }
                    );
                    */
                    await _searchProvider.UpdateElastic(response.Data);

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    /*                
                    await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"StudentsController: Edit(), RequestJson: {studentUpdateRequestJson}, ErrorResponse: {response}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */                    
                    return StatusCode((int)HttpStatusCode.InternalServerError, response);
                }
            }
            catch(Exception ex)
            {
                /*                
                await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"StudentsController: Edit(), RequestJson: {studentUpdateRequestJson}, Exception: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.StudentUpdateFailed.ToString("f"),
                                                                                                            Exception = ex, Message = ex.Message});            
            }
        }

        [HttpDelete("{studentId}/[action]")]
        public async Task<ObjectResult> Delete(string studentId)
        {
            Guid _studentId;

            try
            {
                if(!Guid.TryParse(studentId, out _studentId))
                {       
                    return BadRequest(studentId);
                }

                var response = await _studentProvider.Delete(studentId);
                if(response.Success)
                {
                    List<Task> deleteTasks = new List<Task>(), updateIndexTasks = new List<Task>();
                    var retrieveUpdatedTasks = new List<Task<IProviderResponse<IClass>>>();                    

                    updateIndexTasks.Add(_searchProvider.RemoveFromElastic(response.Data));
                    /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IClass>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.StudentDelete,                            
                            Timestamp = DateTime.UtcNow,
                            Payload = new ActivityPayload<IClass>(){
                                Data = response.Data                                
                            }
                        }
                    );
                    */

                    foreach(var classCode in response.Data.EnrolledClassCodes)
                    {
                        deleteTasks.Add(_classProvider.UnEnroll(classCode, response.Data.StudentId));
                    }
                    await Task.WhenAll(deleteTasks);
                    foreach(var classCode in response.Data.EnrolledClassCodes)
                    {
                        retrieveUpdatedTasks.Add(_classProvider.Retrieve(classCode));
                    }
                    await Task.WhenAll(retrieveUpdatedTasks);

                    foreach(var completedTask in retrieveUpdatedTasks)
                    {
                        if(completedTask.Result.Success)
                        {
                            var updatedClass = completedTask.Result.Data;
                            updateIndexTasks.Add(_searchProvider.UpdateElastic(updatedClass));
                        }
                    }

                    await Task.WhenAll(updateIndexTasks);

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    /*                
                    await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"ClassesController: Edit(), RequestJson: {classUpdateRequestJson}, ErrorResponse: {response}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */                    
                    return StatusCode((int)HttpStatusCode.InternalServerError, response);
                }
            }
            catch(Exception ex)
            {
                /*                
                await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"ClassesController: Delete(), ClassId: {classId}, Exception: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.StudentUpdateFailed.ToString("f"),
                                                                                                            Exception = ex, Message = ex.Message}); 
            }
        }

        /*
        [HttpGet("[action]")]
        public async Task<ObjectResult> GetAll()
        {
            
        }
        */

        [HttpPost("{studentId}/[action]/{classCode}")]
        public async Task<ObjectResult> Enroll(string studentId, string classCode)
        {
            Guid _studentId, _classCode;   
            var enrollTasks = new List<Task<IProviderResponse<bool>>>(); 
            var updateIndexTasks = new List<Task>();        

            try
            {
                if(!Guid.TryParse(studentId, out _studentId))
                {       
                    return BadRequest(studentId);
                }

                if(!Guid.TryParse(classCode, out _classCode))
                {       
                    return BadRequest(classCode);
                }

                var studentEnrollTask = _studentProvider.Enroll(classCode, studentId);
                enrollTasks.Add(studentEnrollTask);
                enrollTasks.Add(_classProvider.Enroll(classCode, studentId));

                await Task.WhenAll(enrollTasks);
                var updatedStudentResponseTask = _studentProvider.Retrieve(studentId);
                var updatedClassResponseTask = _classProvider.Retrieve(classCode);
                /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IStudent>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.Enroll,                            
                            Timestamp = DateTime.UtcNow,
                            Payload = new ActivityPayload<IStudent>(){
                                Data = updatedStudentResponse.Data                                
                            }
                        }
                    );
                */
                await Task.WhenAll(updatedStudentResponseTask, updatedClassResponseTask);

                updateIndexTasks.AddRange(new Task[]{_searchProvider.UpdateElastic(updatedStudentResponseTask.Result.Data),
                                                     _searchProvider.UpdateElastic(updatedClassResponseTask.Result.Data)});
                await Task.WhenAll(updateIndexTasks);                                                                     

                return (!enrollTasks.Any(p => !p.Result.Success)) ? StatusCode((int)HttpStatusCode.OK, studentEnrollTask.Result) : StatusCode((int)HttpStatusCode.InternalServerError, studentEnrollTask.Result);
            }
            catch(Exception ex)
            {
                /*                
                await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"StudentsController: Enroll(), StudentId: {studentId}, ClassCode: {classCode}, Error: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.StudentUpdateFailed.ToString("f"),
                                                                                                        Exception = ex, Message = ex.Message});                                                                                            
            }
        }

        [HttpPost("{studentId}/[action]/{classCode}")]
        public async Task<ObjectResult> UnEnroll(string studentId, string classCode)
        {
            Guid _studentId, _classCode;   
            var unenrollTasks = new List<Task<IProviderResponse<bool>>>(); 
            var updateIndexTasks = new List<Task>();        

            try
            {
                if(!Guid.TryParse(studentId, out _studentId))
                {       
                    return BadRequest(studentId);
                }

                if(!Guid.TryParse(classCode, out _classCode))
                {       
                    return BadRequest(classCode);
                }

                var studentUnenrollTask = _studentProvider.UnEnroll(classCode, studentId);
                unenrollTasks.Add(studentUnenrollTask);
                unenrollTasks.Add(_classProvider.UnEnroll(classCode, studentId));

                await Task.WhenAll(unenrollTasks);
                var updatedStudentResponseTask = _studentProvider.Retrieve(studentId);
                var updatedClassResponseTask = _classProvider.Retrieve(classCode);
                /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IStudent>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.Unenroll,                            
                            Timestamp = DateTime.UtcNow,
                            Payload = new ActivityPayload<IStudent>(){
                                Data = updatedStudentResponse.Data                                
                            }
                        }
                    );
                */
                await Task.WhenAll(updatedStudentResponseTask, updatedClassResponseTask);

                updateIndexTasks.AddRange(new Task[]{_searchProvider.UpdateElastic(updatedStudentResponseTask.Result.Data),
                                                     _searchProvider.UpdateElastic(updatedClassResponseTask.Result.Data)});
                await Task.WhenAll(updateIndexTasks);                                                                     

                return (!unenrollTasks.Any(p => !p.Result.Success)) ? StatusCode((int)HttpStatusCode.OK, studentUnenrollTask.Result) : StatusCode((int)HttpStatusCode.InternalServerError, studentUnenrollTask.Result);
            }
            catch(Exception ex)
            {
                /*                
                await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"StudentsController: UnEnroll(), StudentId: {studentId}, ClassCode: {classCode}, Error: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.StudentUpdateFailed.ToString("f"),
                                                                                                        Exception = ex, Message = ex.Message});                                                                                            
            }
        }        

    }
}