using Dapper;
using Dapper.Contrib.Extensions;
using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository.IRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace DapperDemo.Repository
{
    public class CompanyRepoDapperContib : ICompanyRepository
    {

        private readonly IDbConnection db;

        public CompanyRepoDapperContib(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
    
        public Company Add(Company company)
        {
            var id = db.Insert(company);
            company.CompanyId = (int)id;
            return company;
        }

        public Company2 Add2(Company2 company)
        {
            var test = this.db.Query<int>("usp_AddCompany", company, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return company;
        }

        public Company Find(int id)
        {
           return db.Get<Company>(id);  
        }

        public List<Company> FindAll()
        {
            return db.GetAll<Company>().ToList();
        }

        public void Remove(int id)
        {
            db.Delete(new Company { CompanyId = id});   
            return;
        }

        public Company Update(Company company)
        {
            db.Update(company);

            return company;
        }
    }
}
