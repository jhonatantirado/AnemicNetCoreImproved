using System.Collections.Generic;
using EnterprisePatterns.Api.Common.Domain.ValueObject;
namespace EnterprisePatterns.Api.Customers.Domain.Repository
{
    public interface ICustomerRepository
    {
        IReadOnlyList<Customer> GetList();

        Customer GetByEmail(Email email);

        Customer Read (long id);

        void Create (Customer customer);
    }
}
