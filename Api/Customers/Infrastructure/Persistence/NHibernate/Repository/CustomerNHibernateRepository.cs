using System.Collections.Generic;
using System.Linq;
using EnterprisePatterns.Api;
using EnterprisePatterns.Api.Customers.Domain.Repository;
using EnterprisePatterns.Api.Common.Domain.ValueObject;
using EnterprisePatterns.Api.Common.Infrastructure.Persistence.NHibernate;

namespace EnterprisePatterns.Api.Customers.Infrastructure.Persistence.NHibernate.Repository
{
    class CustomerNHibernateRepository : BaseNHibernateRepository<Customer>, ICustomerRepository
    {
        public CustomerNHibernateRepository(UnitOfWorkNHibernate unitOfWork) : base(unitOfWork)
        {
        }
        public IReadOnlyList<Customer> GetList()
        {
            return _unitOfWork
                .Query<Customer>()
                .ToList();
        }

        public Customer GetByEmail(Email email)
        {
            return _unitOfWork
                .Query<Customer>()
                .SingleOrDefault(x => x.Email == email.Value);
        }
    }
}
