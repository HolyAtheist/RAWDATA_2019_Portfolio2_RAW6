﻿using DatabaseService.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseService.Services
{
    public interface IHistoryService
    {
        bool Add(AnnotationsDto annotationObject);
        History Get(int historyId);
        bool Delete(int historyId);
        bool HistoryExist(int id);
    }
}
