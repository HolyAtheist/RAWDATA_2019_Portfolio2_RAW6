﻿using DatabaseService.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseService.Services
{
    public interface IAnnotationService
    {
        Annotations CreateAnnotations(AnnotationsDto annotationObject);
        Annotations GetAnnotation(int annotationId);
        List<PostAnnotationsDto> GetUserAnnotationsMadeOnAPost(int userId, int postId, PagingAttributes pagingAttributes);
        List<AnnotationsDto> GetAllAnnotationsOfUser(int userId, int postId, PagingAttributes pagingAttributes); // Not used for now -> returns the simple annotation without question and such */

       // List<AnnotationsDto> GetAllAnnotationsOfUser(int userId, int postId, PagingAttributes pagingAttributes);
        bool UpdateAnnotation(int annotationId, string annotationBody);
        bool DeleteAnnotation(int id);
        bool CreateAnnotation_withFunction(Annotations newAnnotation, out Annotations annotationFromDb);
    }
}
