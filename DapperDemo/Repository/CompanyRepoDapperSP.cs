using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository.IRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace DapperDemo.Repository
{
    public class CompanyRepoDapperSP : ICompanyRepository
    {

        private readonly IDbConnection db;

        public CompanyRepoDapperSP(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
    
        public Company Add(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            this.db.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("CompanyId");


            return company;
        }

        public Company2 Add2(Company2 company)
        {
            var test = this.db.Query<int>("usp_AddCompany", company, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return company;
        }

        public Company Find(int id)
        {
            var sql = "usp_GetCompany";
            return db.Query<Company>(sql, new { @CompanyId = id},commandType: CommandType.StoredProcedure).FirstOrDefault();  
        }

        public List<Company> FindAll()
        {
            return db.Query<Company>("usp_GetALLCompany", commandType: CommandType.StoredProcedure).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            db.Execute(sql, new { @Id = id });
            return;
        }

        public Company Update(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            this.db.Execute("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);

            return company;
        }
    }
}
