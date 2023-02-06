using Dapper;
using DapperDemo.Models;
using DapperDemo.Repository.IRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DapperDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnection db;

        public EmployeeRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public Employee Add(Employee employee)
        {
            var sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId); SELECT CAST(SCOPE_IDENTITY() as int)";
            //var id = db.Query<int>(sql, new { @Name = company.Name, @Address =  company.Address, @City = company.City, @State = company.State, @PostalCode = company.PostalCode }).FirstOrDefault();
            var id = db.Query<int>(sql, employee).FirstOrDefault();
            employee.EmployeeId = id;
            return employee;
        }

        public Employee Find(int id)
        {
            var sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return db.Query<Employee>(sql, new { @Id = id }).FirstOrDefault();
        }

        public List<Employee> FindAll()
        {
            var sql = "SELECT * FROM Employees";
            return db.Query<Employee>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            db.Execute(sql, new { @Id = id });
            return;
        }

        public Employee Update(Employee employee)
        {
            var sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            db.Execute(sql, employee);
            return employee;
        }
    }
}
