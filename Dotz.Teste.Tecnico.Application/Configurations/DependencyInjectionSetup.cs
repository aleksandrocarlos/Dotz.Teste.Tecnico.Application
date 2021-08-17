using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Infra.Data.Data.Repositories;
using Dotz.Teste.Tecnico.Infra.Data.Dotz.Teste.Tecnico.Infra.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dotz.Teste.Tecnico.Application.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IPerfilRepository, PerfilRepository>();
            services.AddTransient<IEnderecoRepository, EnderecoRepository>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<ICreditoUsuarioRepository, CreditoUsuarioRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
        }
    }
}
