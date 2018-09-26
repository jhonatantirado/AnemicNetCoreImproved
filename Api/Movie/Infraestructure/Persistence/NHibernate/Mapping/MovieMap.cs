using FluentNHibernate;
using FluentNHibernate.Mapping;
using EnterprisePatterns.Api.Movies.Domain.Entity;

namespace EnterprisePatterns.Api.Movies.Infrastructure.Persistence.NHibernate.Mapping
{
    public class MovieMap : ClassMap<EnterprisePatterns.Api.Movies.Domain.Entity.Movie>
    {
        public MovieMap()
        {
            Id(x => x.Id);

            DiscriminateSubClassesOnColumn("LicensingModel");

            Map(x => x.Name);
            Map(Reveal.Member<EnterprisePatterns.Api.Movies.Domain.Entity.Movie>("LicensingModel")).CustomType<int>();
        }
    }

    public class TwoDaysMovieMap : SubclassMap<TwoDaysMovie>
    {
        public TwoDaysMovieMap()
        {
            DiscriminatorValue(1);
        }
    }

    public class LifeLongMovieMap : SubclassMap<LifeLongMovie>
    {
        public LifeLongMovieMap()
        {
            DiscriminatorValue(2);
        }
    }
}
