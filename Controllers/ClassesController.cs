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

namespace DemoApp1.Controllers
{
    [Route("[controller]")]
    public class ClassesController : Controller
    {
        private readonly IStudentProvider _studentProvider;
        private readonly IClassProvider _classProvider;
        private readonly IFirehoseProvider _firehoseProvider;
        private readonly ISearchProvider _searchProvider;
        private readonly ISchemaValidator<Class> _classSchemaValidator;
        private readonly ISchemaValidator<ClassUpdateRequest> _classUpdateReqSchemaValidator;

        public ClassesController(IStudentProvider studentProvider, IClassProvider classProvider, ISchemaValidator<Class> classSchemaValidator, ISearchProvider searchProvider, 
                                ISchemaValidator<ClassUpdateRequest> classUpdateReqSchemaValidator) 
                                //IFirehoseProvider firehoseProvider)
        {
            _studentProvider = studentProvider;
            _classProvider = classProvider;
            // _firehoseProvider = firehoseProvider;
            _searchProvider = searchProvider;
            _classSchemaValidator = classSchemaValidator;
            _classUpdateReqSchemaValidator = classUpdateReqSchemaValidator;
        }

        // GET /classes/{uuid}
        [HttpGet("{classId}")]
        public async Task<ObjectResult> Get(string classId)
        {
            Guid _classId;            

            try
            {
                if(!Guid.TryParse(classId, out _classId))
                {       
                    return BadRequest(classId);
                }

                var response = await _classProvider.Retrieve(classId);

                return response.Success ? StatusCode((int)HttpStatusCode.OK, response) : StatusCode((int)HttpStatusCode.NotFound, response);
            }
            catch(Exception ex)
            {
                /*                
                await _firehoseProvider.PublishAsync(Topics.Logs, new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"ClassesController: Get(), ClassId: {classId}, Error: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.ClassRetrieveFailed.ToString("f"),
                                                                                                        Exception = ex, Message = ex.Message});                                                                                            
            }            
        }
        
        [HttpPost("[action]")]
        public async Task<ObjectResult> Create([FromBody]string classJson)
        {
            IList<string> errors;

            try
            {
                if(!_classSchemaValidator.IsValid(classJson, out errors))
                {
                    return BadRequest(classJson);
                }

                var response = await _classProvider.Create(JsonConvert.DeserializeObject<Class>(classJson));

                if(response.Success)
                {
                    /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IClass>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.ClassCreate,                            
                            Timestamp = DateTime.UtcNow,
                            Payload = new ActivityPayload<IClass>(){
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
                                                                                                    { Data = string.Format($"ClassesController: Create(), RequestJson: {classJson}, ErrorResponse: {response}")},
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
                                                                                                    { Data = string.Format($"ClassesController: Create(), RequestJson: {studentJson}, Exception: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.ClassCreateFailed.ToString("f"),
                                                                                                            Exception = ex, Message = ex.Message});            
            }
        }

        [HttpPost("{classId}/[action]")]
        public async Task<ObjectResult> Edit(string classId, [FromBody]string classUpdateRequestJson)
        {
            IList<string> errors;
            Guid _classId;

            try
            {
                if(!Guid.TryParse(classId, out _classId))
                {       
                    return BadRequest(classId);
                }

                if(!_classUpdateReqSchemaValidator.IsValid(classUpdateRequestJson, out errors))
                {
                    return BadRequest(classUpdateRequestJson);
                }
                
                var response = await _classProvider.Update(JsonConvert.DeserializeObject<ClassUpdateRequest>(classUpdateRequestJson));

                if(response.Success)
                {
                    /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IClass>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.ClassEdit,                            
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
                                                                                                    { Data = string.Format($"ClassesController: Edit(), RequestJson: {classUpdateRequestJson}, Exception: {ex}")},
                                                                                          ActivityType = ActivityType.Log});
                                                                                          */
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.ClassUpdateFailed.ToString("f"),
                                                                                                            Exception = ex, Message = ex.Message});            
            }
        }

        [HttpDelete("{classId}")]
        public async Task<ObjectResult> Delete(string classId)
        {
            Guid _classId;

            try
            {
                if(!Guid.TryParse(classId, out _classId))
                {       
                    return BadRequest(classId);
                }

                var response = await _classProvider.Delete(classId);
                if(response.Success)
                {
                    List<Task> deleteTasks = new List<Task>(), updateIndexTasks = new List<Task>();
                    var retrieveUpdatedTasks = new List<Task<IProviderResponse<IStudent>>>();
                    /*
                    await _firehoseProvider.PublishAsync(Topics.Logs | Topics.SearchIndexes, new Activity<IClass>()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ActivityType = ActivityType.ClassDelete,                            
                            Timestamp = DateTime.UtcNow,
                            Payload = new ActivityPayload<IClass>(){
                                Data = response.Data                                
                            }
                        }
                    );
                    */

                    updateIndexTasks.Add(_searchProvider.RemoveFromElastic(response.Data));

                    foreach(var studentId in response.Data.EnrolleeIds)
                    {
                        deleteTasks.Add(_studentProvider.UnEnroll(response.Data.Code, studentId));
                    }
                    await Task.WhenAll(deleteTasks);
                    foreach(var studentId in response.Data.EnrolleeIds)
                    {
                        retrieveUpdatedTasks.Add(_studentProvider.Retrieve(studentId));
                    }
                    await Task.WhenAll(retrieveUpdatedTasks);

                    foreach(var completedTask in retrieveUpdatedTasks)
                    {
                        if(completedTask.Result.Success)
                        {
                            var updatedStudent = completedTask.Result.Data;
                            updateIndexTasks.Add(_searchProvider.UpdateElastic(updatedStudent));
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
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProviderResponse<IClass>(){Success = false, ResponseStatus = CustomResponseErrors.ClassUpdateFailed.ToString("f"),
                                                                                                            Exception = ex, Message = ex.Message}); 
            }
        }

        /*
        [HttpGet("[action]")]
        public async Task<ObjectResult> GetAll()
        {
            
        }
        */

        
    }
}