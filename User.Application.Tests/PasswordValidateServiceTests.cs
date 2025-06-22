using Moq;
using System.Security.Principal;
using User.Application;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Options;

namespace User.Application.Tests
{

    public class PasswordValidateServiceTests
    {
        [Fact]
        public void ValidatePassword_WithWrongPassword_ReturnsAllErrors()
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
            IOptions<IdentityOptions> options = Options.Create(identityOptions);
            var service = new PasswordValidateService(options);
            //var mockProvider = new Mock<IdentityOptions>();
            //mockProvider.Setup(p => p).Returns(identityOptions);
            var password = "";
            var result = service.ValidatePassword(password);

            Assert.Contains(result, e => e.Contains("at least 10 characters"));
            Assert.Contains(result, e => e.Contains("digit"));
            Assert.Contains(result, e => e.Contains("uppercase"));
            Assert.Contains(result, e => e.Contains("lowercase"));
            Assert.Contains(result, e => e.Contains("non-alphanumeric"));

            Assert.Equal(5, result.Count);


        }

        [Fact]
        public void ValidatePassword_WithCorrectPassword_ReturnsNoErrors()
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
            IOptions<IdentityOptions> options = Options.Create(identityOptions);
            var service = new PasswordValidateService(options);
            var password = "01234567aB_";
            var result = service.ValidatePassword(password);

            Assert.Empty(result);
            
        }
    }
}