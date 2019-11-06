﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.DTOs
{
    public class AnnotationsDto
    {
        public string URL { get; set; }
        public int AnnotationId { get; set; }
        public int UserId { get; set; }
        public int HistoryId { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }

    }
}
