using Dapper;
using DapperDemo.Models;
using DapperDemo.Repository.IRepository;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DapperDemo.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private readonly IDbConnection db;

        public BonusRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public void AddTestCompanyWithEmployees(Company company)
        {
            var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode); SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = db.Query<int>(sql, new { @Name = company.Name, @Address =  company.Address, @City = company.City, @State = company.State, @PostalCode = company.PostalCode }).FirstOrDefault();
            company.CompanyId = id;

            //Regular implementation
            //foreach (var employee in company.Employees)
            //{
            //    employee.CompanyId= id;
            //    var sql1 = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId); SELECT CAST(SCOPE_IDENTITY() as int)";
            //    //var id = db.Query<int>(sql, new { @Name = company.Name, @Address =  company.Address, @City = company.City, @State = company.State, @PostalCode = company.PostalCode }).FirstOrDefault();
            //    db.Query<int>(sql1, employee).FirstOrDefault();

            //}

            //Bulk insert
            var sqlEmp= "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId); SELECT CAST(SCOPE_IDENTITY() as int)";
            company.Employees.Select(it => { it.CompanyId = id; return it; }).ToList();
            db.Execute(sqlEmp, company.Employees);

        }

        public void AddTestCompanyWithEmployeesTransaction(Company company)
        {

            using(var transaction = new TransactionScope())
            {
                try
                {
                    var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode); SELECT CAST(SCOPE_IDENTITY() as int);";
                    var id = db.Query<int>(sql, new { @Name = company.Name, @Address = company.Address, @City = company.City, @State = company.State, @PostalCode = company.PostalCode }).FirstOrDefault();
                    company.CompanyId = id;

                    //Bulk insert
                    var sqlEmp = "INSERT INTO Employeesx (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId); SELECT CAST(SCOPE_IDENTITY() as int)";
                    company.Employees.Select(it => { it.CompanyId = id; return it; }).ToList();
                    db.Execute(sqlEmp, company.Employees);

                    transaction.Complete();
                }
                catch (Exception ex)
                {

                    
                }
            }
        }

        public List<Company> FilterCompanyByName(string name)
        {
            return db.Query<Company>("select * from Companies where Name like '%' + @name + '%'", new { name}).ToList();   
        }

        public List<Company> GetAllCompaniesWithEmployees()
        {
            var sql = "select c.*,e.* from Employees as e inner join Companies as c on e.CompanyId = c.CompanyId";

            var companyDic = new Dictionary<int, Company>();
            var company = db.Query<Company, Employee, Company>(sql,(c,e) =>
            {
                if (!companyDic.TryGetValue(c.CompanyId,out var currentCompany))
                {
                    currentCompany = c;
                    companyDic.Add(c.CompanyId, currentCompany);
                }  
                currentCompany.Employees.Add(e);
                return currentCompany;
            }, splitOn: "EMployeeId"); 


            return company.Distinct().ToList(); 
        }

        public Company GetCompanyWithAddresses(int id)
        {
            var p = new
            {
                CompanyId = id
            };

            var sql = "select * from Companies where CompanyId = @CompanyId; select * from Employees where CompanyId = @CompanyId;";

            Company company;
            using (var lists = db.QueryMultiple(sql, p))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }

            return company;
        }

        public List<Employee> GetEmployeeWithCompanies(int id)
        {
            var sql = "select e.*,c.* from Employees as e inner join Companies as c on e.CompanyId = c.CompanyId";
            if(id != 0)
            {
                sql += " where e.CompanyId = @Id";
            }
            var employee = db.Query<Employee,Company,Employee>(sql, (emp, comp) =>
            {
                emp.Company = comp;
                return emp;
            }, new  { id }, splitOn: "CompanyId");
            return employee.ToList();
        }

        public void RemoveRange(int[] companyId)
        {
            db.Query("delete from companies where CompanyId in @CompanyId", new { companyId });
        }
    }
}
