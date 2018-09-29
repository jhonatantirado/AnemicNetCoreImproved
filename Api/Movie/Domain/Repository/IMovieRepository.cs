using System.Collections.Generic;
using EnterprisePatterns.Api.Movies.Domain.Entity;
namespace EnterprisePatterns.Api.Movies.Domain.Repository
{
    interface IMovieRepository
    {
        IReadOnlyList<EnterprisePatterns.Api.Movies.Domain.Entity.Movie> GetList();
        Movie GetById(long id);
    }
}
