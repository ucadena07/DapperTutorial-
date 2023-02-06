using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository.IRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace DapperDemo.Repository
{
    public class CompanyRepoDapper : ICompanyRepository
    {

        private readonly IDbConnection db;

        public CompanyRepoDapper(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
    
        public Company Add(Company company)
        {
            var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode); SELECT CAST(SCOPE_IDENTITY() as int);";
            //var id = db.Query<int>(sql, new { @Name = company.Name, @Address =  company.Address, @City = company.City, @State = company.State, @PostalCode = company.PostalCode }).FirstOrDefault();
            var id = db.Query<int>(sql, company).FirstOrDefault();
            company.CompanyId = id;
            return company;
        }

        public Company2 Add2(Company2 company)
        {
            throw new NotImplementedException();
        }

        public Company Find(int id)
        {
            var sql = "SELECT * FROM Companies WHERE CompanyId = @Id";
            return db.Query<Company>(sql, new {@Id = id}).FirstOrDefault();  
        }

        public List<Company> FindAll()
        {
            var sql = "SELECT * FROM Companies";
            return db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            db.Execute(sql, new { @Id = id });
            return;
        }

        public Company Update(Company company)
        {
            var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            db.Execute(sql, company);
            return company;
        }
    }
}
