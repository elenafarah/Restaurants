﻿using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Tests.Application.Users
{
    public class CurrentUserTests
    {
        // TestMethod_Scenario_ExpectResult
        /*
        [Fact()]
        public void IsInRole_WithMatchingRole_ShouldReturnTrue()
        {
            // Arrange

            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);
            
            // Act

            var isInRole = currentUser.IsInRole(UserRoles.Admin);

            // Assert

            isInRole.Should().BeTrue();
        }
        */
        
        [Fact()]
        public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
        {
            // Arrange

            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // Act

            var isInRole = currentUser.IsInRole(UserRoles.Owner);

            // Assert

            isInRole.Should().BeFalse();
        }

        [Fact()]
        public void IsInRole_WithMatchingRole_ShouldReturnTrue()
        {
            // Arrange

            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // Act

            var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

            // Assert

            isInRole.Should().BeFalse();
        }

        [Theory()]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.User)]
        public void IsInRole_WithMatchingRoleCase_ShouldReturnTrue(string roleName)
        {
            // Arrange

            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // Act

            var isInRole = currentUser.IsInRole(roleName);

            // Assert

            isInRole.Should().BeTrue();
        }
    }
}
