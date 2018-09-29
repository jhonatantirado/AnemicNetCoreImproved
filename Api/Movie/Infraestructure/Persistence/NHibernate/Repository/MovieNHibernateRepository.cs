using System.Collections.Generic;
using System.Linq;
using EnterprisePatterns.Api;
using EnterprisePatterns.Api.Movies.Domain.Entity;
using EnterprisePatterns.Api.Movies.Domain.Repository;
using EnterprisePatterns.Api.Common.Infrastructure.Persistence.NHibernate;

namespace EnterprisePatterns.Api.Movies.Infrastructure.Persistence.NHibernate.Repository
{
    public class MovieNHibernateRepository : BaseNHibernateRepository<Movie>, IMovieRepository
    {
        public MovieNHibernateRepository(UnitOfWorkNHibernate unitOfWork) : base(unitOfWork)
        {
        }

        public IReadOnlyList<Movie> GetList()
        {
            return _unitOfWork.Query<Movie>().ToList();
        }

        
    }
}
