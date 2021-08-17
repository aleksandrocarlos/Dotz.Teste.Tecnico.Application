using System;
using System.Data;

namespace Dotz.Teste.Tecnico.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Begin();
        void Commit();
        void Rollback();
        IDbConnection Connection { get; }

    }
}
