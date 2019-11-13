﻿using AutoMapper;
using DatabaseService;
using DatabaseService.Modules;
using DatabaseService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/annotations")]
    [Authorize]
    public class AnnotationsController : ControllerBase
    {
        private IAnnotationService _annotationService;
        private IMapper _mapper;

        public AnnotationsController(IAnnotationService annotationService, IMapper mapper)
        {
            _annotationService = annotationService;
            _mapper = mapper;
        }


        /// <summary>
        /// Get all annotations that belong to the logged in user : http://localhost:5001/api/annotations plus Authorization Bearer <valid_tokenvalue> in Headers
        /// </summary>
        /// <returns>Array of Json Annotation Objects plus relevant status code</returns>
        [HttpGet]
        public ActionResult GetAllAnnotationsOfUser([FromQuery] PagingAttributes pagingAttributes) //needs-pagination
        {
            int userIdFromToken = GetAuthUserId();
            var listOfAnnotations = _annotationService.GetAllAnnotationsByUserId(userIdFromToken, pagingAttributes);
            if (listOfAnnotations.Count == 0)
            {
                return NotFound();
            }
            return Ok(listOfAnnotations);
        }
        /// <summary>
        /// Get all annoations of a post from a user
        /// based on postId/ questionId and not historyId from url and userId from token
        /// http://localhost:5001/api/annotations/user/{questionId}
        /// header: valid user token;
        /// </summary>
        /// <param name="postId"></param>
        /// <returns>List of annotationsDto</returns>
        [HttpGet("user/{postId}")]
        public ActionResult GetAnnotationsByPostId(int postId, [FromQuery] PagingAttributes pagingAttributes) 
        {
            int userIdFromToken = GetAuthUserId();
            var listOfAnnotations = _annotationService.GetAnnotationsWithPostId(userIdFromToken, postId, pagingAttributes);
            if (listOfAnnotations.Count == 0)
            {
                return NotFound();
            }
            foreach(AnnotationsDto res in listOfAnnotations)
            {
                var annotationUrl = AddUrlsToAnnotations(res);
                res.AddAnnotationUrl = annotationUrl.AddAnnotationUrl;
                res.URL = annotationUrl.URL;
            }
            return Ok(listOfAnnotations);
        }

        /// <summary>
        /// Testing this api in postman: http://localhost:5001/api/annotations/2
        /// </summary>
        /// <param name="annotationId"></param>
        /// <returns>AnnotationDto</returns>
        [HttpGet("{annotationId}", Name = nameof(GetAnnotation))] // fancy way to have strings checked by the compiler
        public ActionResult GetAnnotation(int annotationId)
        {
            var returnedAnnotation = _annotationService.GetAnnotation(annotationId);
            if (returnedAnnotation == null)
            {
                return NotFound();
            }
            //with the helper class and the mapper we are setting the annotation type result (in returnAnnotation)
            //to AnnotationDto class type
            //so the magic is not much as it still requires some manual work for mapping. 
            return Ok(CreateLink(returnedAnnotation));
        }

        /// <summary>
        /// This function calls the create anew annotation from db function
        /// Testing with postman:
        ///  in request: POST http://localhost:5001/api/annotations  plus valid token of the user
        ///  in body: 
        ///         {
        ///             "HistoryId": 19, //fix namig because it is postid
        ///             "Body": "This call takes in userId, HistoryId and the body; but returns all the things from AnnotationsDto"
        ///         }
        /// </summary>
        /// <param name="annotationObj"></param>
        /// <returns>Returns the </returns>
        [HttpPost (Name = nameof(AddAnnotation))]
        public ActionResult AddAnnotation(AnnotationsDto annotationObj)
        {
            var newAnnotation = new Annotations
            {
                UserId = GetAuthUserId(),
                HistoryId = annotationObj.HistoryId,
                Body = annotationObj.Body
            };
            if (_annotationService.CreateAnnotation_withFunction(newAnnotation, out newAnnotation))
            {
                return Ok(CreateLink(newAnnotation));
            }
            return BadRequest();

        }

        /// <summary>
        /// API that needs id of annotation in the request path and body text in body of request 
        /// Testing with postman:
        ///                     request path: http://localhost:5001/api/annotations/52
        ///                     request body:
        ///                     {
        ///                     "Body": "he body of the PUT request can have all annotationsDto values, but it will only update the .Body value"
        ///                     }
        /// </summary>
        /// <param name="annotationId"></param>
        /// <param name="annotation"></param>
        /// <returns>
        ///         Success: 204 NoContent
        ///         Fail: 404 BadRequest
        /// </returns>
        [HttpPut("{annotationId}")]
        public ActionResult UpdateAnnotation(int annotationId,[FromBody] AnnotationsDto annotation)
        {
            //need to encode body before sending to db - this can also be done inside the UpdateAnnotation function.
            if (_annotationService.UpdateAnnotation(annotationId, annotation.Body))
            {
                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// Delete annotation by providing the id;
        /// Testing with postman: DELETE http://localhost:5001/api/annotations/52
        /// </summary>
        /// <param name="annotationId"></param>
        /// <returns> Success: 200 Ok ; Fail 404 Not Found</returns>
        [HttpDelete("{annotationId}")]
        public ActionResult DeleteData(int annotationId)
        {
            if (_annotationService.DeleteAnnotation(annotationId))
            {
                return Ok();
            }
            return NotFound();
        }



        /// <summary>
        /// DTO Annotations Mapper used with GET annotation
        /// </summary>
        /// <param name="annotation"></param>
        /// <returns>AnnotationsDto</returns>
        private AnnotationsDto CreateLink(Annotations annotation)
        {
            var annotationDto = _mapper.Map<AnnotationsDto>(annotation);
            annotationDto.AnnotationId = annotation.Id;
            annotationDto.URL = Url.Link(
                    nameof(GetAnnotation),
                    new { AnnotationId = annotation.Id });
            annotationDto.AddAnnotationUrl = Url.ActionLink(nameof(AddAnnotation));
            return annotationDto;
        }

        /// <summary>
        /// Simple parsing function in order to add the needed urls ---> this is not a mapping function like the one above
        /// For object of type AnnotationsDto that misses the Url and AddAnnotationUrl string values
        /// </summary>
        /// <param name="annotation"></param>
        /// <returns></returns>
        private AnnotationsDto AddUrlsToAnnotations(AnnotationsDto annotation)
        {
            annotation.URL = Url.Link(
                    nameof(GetAnnotation),
                    new { annotation.AnnotationId });
            annotation.AddAnnotationUrl = Url.ActionLink(nameof(AddAnnotation));
            return annotation;
        }

        /// <summary>
        /// Get the authenticated user's id from token claim
        /// </summary>
        /// <returns>integer authenticated user's id from token</returns>
        private int GetAuthUserId()
        {
            var userIdFromToken = int.Parse(this.User.Identity.Name);
            return userIdFromToken;
        }


    }
}
