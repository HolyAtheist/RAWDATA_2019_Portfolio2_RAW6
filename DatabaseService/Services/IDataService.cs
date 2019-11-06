﻿using System.Collections.Generic;


namespace DatabaseService
{
    public interface IDataService
    {
        IList<Questions> GetQuestions(PagingAttributes pagingAttributes);
        int NumberOfQuestions();
        public Questions GetQuestion(int questionId);
        IList<Search> Search(string searchstring, int? searchtypecode, PagingAttributes pagingAttributes);
        IList<WordRank> WordRank(string searchstring, int? searchtypecode, PagingAttributes pagingAttributes);
    }
}
