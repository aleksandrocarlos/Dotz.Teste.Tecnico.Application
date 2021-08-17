using System;
using System.Collections.Generic;

namespace Dotz.Teste.Tecnico.Domain.Queries
{
    public class SaldoExtratoQueryResult
    {
        public Decimal? Saldo { get; set; }
        public List<ExtratoClienteQueryResult> Extrato { get; set; }
    }
}
