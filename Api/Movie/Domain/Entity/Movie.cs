using System;
using EnterprisePatterns.Api.Common.Domain;
using EnterprisePatterns.Api.Common.Domain.ValueObject;
using EnterprisePatterns.Api.Customers;

namespace EnterprisePatterns.Api.Movies.Domain.Entity
{
    public abstract class Movie : EnterprisePatterns.Api.Common.Domain.Entity
    {
        public virtual string Name { get; protected set; }
        protected virtual LicensingModel LicensingModel { get; set; }

        public abstract ExpirationDate GetExpirationDate();

        public virtual Dollars CalculatePrice(CustomerStatus status)
        {
            decimal modifier = 1 - status.GetDiscount();
            return GetBasePrice() * modifier;
        }

        protected abstract Dollars GetBasePrice();
    }

    public class TwoDaysMovie : Movie
    {
        public override ExpirationDate GetExpirationDate()
        {
            return (ExpirationDate)DateTime.UtcNow.AddDays(2);
        }

        protected override Dollars GetBasePrice()
        {
            return Dollars.Of(4);
        }
    }

    public class LifeLongMovie : Movie
    {
        public override ExpirationDate GetExpirationDate()
        {
            return ExpirationDate.Infinite;
        }

        protected override Dollars GetBasePrice()
        {
            return Dollars.Of(8);
        }
    }
}
