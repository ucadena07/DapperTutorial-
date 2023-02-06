namespace DapperDemo.Repository.IRepository
{
    public interface IDapperSprocRepo
    {
        void Execute(string name);
        void Execute(string name, object param);
        List<T> List<T>(string name, int id);
        List<T> List<T>(string name, object param);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string name, object param);
        Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> List<T1, T2, T3>(string name, object param);
        List<T> List<T>(string name);
        void QueryExecute(string name, object param);
        void QueryExecute(string name);
        T Single<T>(string name, int id);
        T Single<T>(string name, object param);
    }
}
