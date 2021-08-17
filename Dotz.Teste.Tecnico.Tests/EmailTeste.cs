using Dotz.Teste.Tecnico.Infra.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dotz.Teste.Tecnico.Tests
{
    [TestClass]
    public class EmailTeste
    {

        [TestMethod]
        public void EmailInvalido()
        {
            var email = "alekscarlos@gmail";
            var result = ValidateEmailService.ValidaEmail(email);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EmailValido()
        {
            var email = "alekscarlos4@gmail.com";
            var result = ValidateEmailService.ValidaEmail(email);
            Assert.IsTrue(result);
        }
    }
}
