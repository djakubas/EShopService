using Moq;
using System.Security.Principal;
using User.Application;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace User.Application.Tests
{
    public class PasswordValidateServiceTests
    {
        [Fact]
        public void CheckPasswordsAndReturnsAllErrors()
        {
            var identityOptions = new IdentityOptions
            {
                Password = new PasswordOptions
                {
                    RequireLowercase = true,
                    RequiredLength = 10,
                    RequireNonAlphanumeric = true,
                    RequireDigit = true,
                    RequireUppercase = true,
                }
                
            };
            var service = new PasswordValidateService(identityOptions);
            //var mockProvider = new Mock<IdentityOptions>();
            //mockProvider.Setup(p => p).Returns(identityOptions);
            var password = "";
            var result = service.ValidatePassword(password);

            Assert.Contains(result, e => e.Contains("at least 10 characters"));
            Assert.Contains(result, e => e.Contains("digit"));
            Assert.Contains(result, e => e.Contains("uppercase"));
            Assert.Contains(result, e => e.Contains("lowercase"));
            Assert.Contains(result, e => e.Contains("non-alphanumeric"));
        }
    }
}