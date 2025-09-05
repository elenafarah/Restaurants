using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Api.Middlewares;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.Tests.Api.Middlewares
{
    public class ErrorHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
        private readonly ErrorHandlingMiddleware _middleware;
        private readonly DefaultHttpContext _context;

        public ErrorHandlingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            _middleware = new ErrorHandlingMiddleware(_loggerMock.Object);
            _context = new DefaultHttpContext();
        }

        [Fact]
        public async Task InvokeAsync_WhenNoException_CallsNext()
        {
            // Arrange
            var wasCalled = false;
            RequestDelegate next = ctx =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            // Act
            await _middleware.InvokeAsync(_context, next);

            // Assert
            Assert.True(wasCalled);
            Assert.Equal(200, _context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_WhenNotFoundException_Returns404()
        {
            // Arrange
            var message = "Not found!";
            RequestDelegate next = ctx => throw new NotFoundException("Restaurant",message);

            // Act
            await _middleware.InvokeAsync(_context, next);

            // Assert
            Assert.Equal(404, _context.Response.StatusCode);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WhenForbidException_Returns403()
        {
            // Arrange
            var message = "Forbidden!";
            RequestDelegate next = ctx => throw new ForbidException(message);

            // Act
            await _middleware.InvokeAsync(_context, next);

            // Assert
            Assert.Equal(403, _context.Response.StatusCode);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WhenOtherException_Returns500()
        {
            // Arrange
            var message = "Unexpected error!";
            RequestDelegate next = ctx => throw new Exception(message);

            // Act
            await _middleware.InvokeAsync(_context, next);

            // Assert
            Assert.Equal(500, _context.Response.StatusCode);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
