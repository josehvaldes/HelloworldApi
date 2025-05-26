using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using HelloWorldApi.Controllers;
using Moq;
namespace HelloWorldApi.UnitTests;

public class Tests
{
    private Mock<ILogger<HealthCheckController>> _mockLogger;
    private HealthCheckController _controller;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<HealthCheckController>>();
        _controller = new HealthCheckController(_mockLogger.Object);
    }

    [Test]
    public void GetStatus_ReturnsHealthyStatus_WithLogs()
    {
        // Arrange
        string testValue = "TestValue";

        // Act
        var result = _controller.GetStatus(testValue) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        dynamic value = ((dynamic)result.Value);
        Assert.That(value, Is.Not.Null);
        Assert.That(value.status, Is.EqualTo($"Healthy with logs: {testValue}"));

        // Verify logs
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"> GetStatus critical Log with log: {testValue}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );

        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"> GetStatus error Log with log: {testValue}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }
}
