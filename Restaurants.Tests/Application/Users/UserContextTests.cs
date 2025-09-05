using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Xunit;

namespace Restaurants.Tests.Application.Users
{
    public class UserContextTests
    {
        private static ClaimsPrincipal CreateClaimsPrincipal(
            string userId = "123",
            string email = "test@example.com",
            IEnumerable<string>? roles = null,
            string? nationality = null,
            string? dateOfBirth = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            if (nationality != null)
            {
                claims.Add(new Claim("Nationality", nationality));
            }

            if (dateOfBirth != null)
            {
                claims.Add(new Claim("DateOfBirth", dateOfBirth));
            }

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public void GetCurrentUser_ReturnsCurrentUser_WhenUserIsAuthenticated()
        {
            // Arrange
            var principal = CreateClaimsPrincipal(
                userId: "42",
                email: "user@domain.com",
                roles: new[] { "Admin", "User" },
                nationality: "FR",
                dateOfBirth: "1990-01-01"
            );

            var httpContext = new DefaultHttpContext { User = principal };
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            var userContext = new UserContext(accessorMock.Object);

            // Act
            var currentUser = userContext.GetCurrentUser();

            // Assert
            Assert.NotNull(currentUser);
            Assert.Equal("42", currentUser!.Id);
            Assert.Equal("user@domain.com", currentUser.Email);
            Assert.Contains("Admin", currentUser.Roles);
            Assert.Contains("User", currentUser.Roles);
            Assert.Equal("FR", currentUser.Nationality);
            Assert.Equal(new DateOnly(1990, 1, 1), currentUser.DateOfBirth);
        }

        [Fact]
        public void GetCurrentUser_ReturnsNull_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var identity = new ClaimsIdentity(); // Not authenticated
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            var userContext = new UserContext(accessorMock.Object);

            // Act
            var currentUser = userContext.GetCurrentUser();

            // Assert
            Assert.Null(currentUser);
        }

        [Fact]
        public void GetCurrentUser_Throws_WhenUserContextIsMissing()
        {
            // Arrange
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns((HttpContext?)null);

            var userContext = new UserContext(accessorMock.Object);

            // Act 
            Action action = () => userContext.GetCurrentUser();

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Equal("User context is not present", exception.Message);
        }
    }
}
