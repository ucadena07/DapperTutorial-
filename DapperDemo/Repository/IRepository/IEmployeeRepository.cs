using DapperDemo.Models;

namespace DapperDemo.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Employee Find(int id);
        List<Employee> FindAll();
        Employee Add(Employee employee);
        Employee Update(Employee employee);
        void Remove(int id);
    }
}
