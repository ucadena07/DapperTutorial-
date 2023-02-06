using DapperDemo.Models;

namespace DapperDemo.Repository.IRepository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeeWithCompanies(int id);
        Company GetCompanyWithAddresses(int id);
        List<Company> GetAllCompaniesWithEmployees();
        void AddTestCompanyWithEmployees(Company company);
        void RemoveRange(int[] companyId);
        List<Company> FilterCompanyByName(string name);
        void AddTestCompanyWithEmployeesTransaction(Company company);
    }
}
