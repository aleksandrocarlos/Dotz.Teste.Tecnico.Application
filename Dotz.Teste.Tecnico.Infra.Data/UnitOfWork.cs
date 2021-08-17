using Dotz.Teste.Tecnico.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Dotz.Teste.Tecnico.Infra.Data
{
    namespace Dotz.Teste.Tecnico.Infra.Data
    {
        public class UnitOfWork : IUnitOfWork
        {
            private IDbConnection _connection;
            private IDbTransaction _transaction;
            private bool _disposed;
            public IDbConnection Connection { get { return _connection; } }
            private IConfiguration _configuration;

            public UnitOfWork(IConfiguration configuration)
            {

                _configuration = configuration;

                string mySqlConnectionStringName = _configuration["ConnectionStrings:DefaultConnection"];

                _connection = new MySql.Data.MySqlClient.MySqlConnection(mySqlConnectionStringName);

                _connection.Open();
            }

            public UnitOfWork(string connectionString)
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    try
                    {
                        _connection = new MySqlConnection(connectionString);
                        _connection.Open();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                    throw new Exception($"Connection string cannot be empty.");
            }

            public void Begin()
            {
                _transaction = _connection.BeginTransaction();
            }

            public void Commit()
            {
                _transaction.Commit();
                Dispose();
            }

            public void Rollback()
            {
                _transaction.Rollback();
                Dispose();
            }

            public void Dispose()
            {
                if (_transaction != null) { _transaction.Dispose(); }
                if (_connection != null) { _connection.Close(); }

                _transaction = null;
                _connection = null;

                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        if (_connection != null) { _connection = null; }
                        if (_transaction != null) { _transaction = null; }
                    }
                    _disposed = true;
                }
            }

            ~UnitOfWork()
            {
                Dispose(false);
            }

        }
    }
}
