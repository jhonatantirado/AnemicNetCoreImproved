﻿using System.Collections.Generic;
using EnterprisePatterns.Api.Movies.Domain.Entity;
namespace EnterprisePatterns.Api.Movies.Domain.Repository
{
    public interface IMovieRepository
    {
        IReadOnlyList<Movie> GetList();
        Movie Read(long id);
    }
}
