using System.Data;

namespace EnterprisePatterns.Api.Common.Application
{
    public interface IUnitOfWork
    {
        bool BeginTransaction(IsolationLevel isolationLevel);
        void Commit(bool commit);
        void Rollback(bool rollback);
    }
}
