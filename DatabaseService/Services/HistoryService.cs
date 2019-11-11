using DatabaseService.Modules;

namespace DatabaseService.Services
{
    public class HistoryService : IHistoryService
    {
        
        AppContext database;

        public HistoryService()
        {
            database = new AppContext();
        }
        
        public bool Add(AnnotationsDto annotationObject)
        {
            throw new System.NotImplementedException();
        }

        public History Get(int historyId)
        {
            return database.History.Find(historyId);
        }

        public bool Delete(int historyId)
        {
            if (HistoryExist(historyId))
            {
                var appUser = database.AppUser.Find(historyId);
                database.AppUser.Remove(appUser);

                var result = database.SaveChanges();
                return result > 0;
            }

            return false;
        }

        public bool HistoryExist(int historyId)
        {
            var result = database.History.Find(historyId);
            return result != null;
        }
        
    }
}