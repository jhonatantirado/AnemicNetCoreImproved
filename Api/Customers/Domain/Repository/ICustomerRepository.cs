using System.Collections.Generic;
using EnterprisePatterns.Api.Common.Domain.ValueObject;
namespace EnterprisePatterns.Api.Customers.Domain.Repository
{
    interface ICustomerRepository
    {
        IReadOnlyList<Customer> GetList();

        Customer GetByEmail(Email email);

        Customer GetById (long id);

        void Add (Customer customer);
    }
}
