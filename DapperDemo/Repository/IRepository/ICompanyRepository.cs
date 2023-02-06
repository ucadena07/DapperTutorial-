using DapperDemo.Models;

namespace DapperDemo.Repository.IRepository
{
    public interface ICompanyRepository
    {
        Company Find(int id);
        List<Company> FindAll();    
        Company Add(Company company);
        Company2 Add2(Company2 company);

        Company Update(Company company);
        void Remove(int id);

    }
}
