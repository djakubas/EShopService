using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using User.Application;
using User.Domain.Models;
using Xunit;

namespace User.Application.Tests
{
    public class JwtTokenServiceTests
    {
        [Fact]
        public void GenerateToken_WithAllParameters_ShouldReturnValidToken()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<JwtSettings>>();
            optionsMock.Setup(o => o.Value).Returns(new JwtSettings
            {
                Key = "test-secret-key-1234567890",
                Issuer = "test-issuer",
                Audience = "test-audience",
                ExpiresInMinutes = 30
            });

            var userId = "test-user-id";
            var roles = new List<string> { "test-user-role" };

            var jwtTokenService = new JwtTokenService(optionsMock.Object);

            // Act
            var token = jwtTokenService.GenerateToken(userId, roles);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.NotNull(jwtToken);
        }
    }
}