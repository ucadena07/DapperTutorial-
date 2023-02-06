using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository.IRepository;

namespace DapperDemo.Repository
{
    public class CompanyRepositoryEF : ICompanyRepository
    {

        private readonly ApplicationDbContext _db;

        public CompanyRepositoryEF(ApplicationDbContext db)
        {
            _db = db;
        }
        public Company Add(Company company)
        {
            _db.Companies.Add(company);
            _db.SaveChanges();
            return company;

        }

        public Company2 Add2(Company2 company)
        {
            throw new NotImplementedException();
        }

        public Company Find(int id)
        {
            return _db.Companies.Find(id);    
        }

        public List<Company> FindAll()
        {
            return _db.Companies.ToList();
        }

        public void Remove(int id)
        {
            Company company = _db.Companies.Find(id);
            _db.Companies.Remove(company);
            _db.SaveChanges();
            return;
        }

        public Company Update(Company company)
        {
            _db.Companies.Update(company);
            _db.SaveChanges();
            return company;
        }
    }
}
