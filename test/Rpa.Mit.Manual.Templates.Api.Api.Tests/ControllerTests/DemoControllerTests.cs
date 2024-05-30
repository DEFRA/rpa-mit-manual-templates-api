using Rpa.Mit.Manual.Templates.Api.Api.Controllers;
using Rpa.Mit.Manual.Templates.Api.Api.Models;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Exceptions;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

using AutoFixture;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.Controllers;

public class DemoControllerTests
{
    private readonly ILogger<DemoController> _loggerMock;
    private readonly IItemService _itemServiceMock;
    private readonly IMapper _mapperMock;
    private readonly DemoController _sut;
    private readonly Fixture _fixture;

    public DemoControllerTests()
    {
        _loggerMock = Substitute.For<ILogger<DemoController>>();
        _itemServiceMock = Substitute.For<IItemService>();
        _mapperMock = Substitute.For<IMapper>();
        _sut = new DemoController(_itemServiceMock, _mapperMock, _loggerMock);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllItemsAsync_ReturnsOkResult()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>(3).ToList();
        _itemServiceMock.GetAllItemsAsync().Returns(items);

        // Act
        var result = await _sut.GetAllItemsAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(items);
    }

    [Fact]
    public async Task GetItemByIdAsync_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var itemId = 1;
        var item = _fixture.Create<Item>();
        _itemServiceMock.GetItemByIdAsync(itemId).Returns(item);

        // Act
        var result = await _sut.GetItemByIdAsync(itemId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task GetItemByIdAsync_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var itemId = 0;
        _itemServiceMock.GetItemByIdAsync(Arg.Any<int>()).Throws<ItemNotFoundException>();

        // Act
        var result = await _sut.GetItemByIdAsync(itemId);

        // Assert
        result.Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task CreateItemAsync_ValidItem_ReturnsCreatedResult()
    {
        // Arrange
        var itemRequest = _fixture.Create<ItemRequest>();
        var item = _fixture.Create<Item>();
        _mapperMock.Map<Item>(itemRequest).Returns(item);
        _itemServiceMock.CreateItemAsync(Arg.Any<Item>()).Returns(item);

        // Act
        var result = await _sut.CreateItemAsync(itemRequest);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Should().BeOfType<ObjectResult>()
            .Which.Value.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task UpdateItemAsync_ValidItem_ReturnsOkResult()
    {
        // Arrange
        var itemRequest = _fixture.Create<ItemRequest>();
        var item = _fixture.Create<Item>();
        _mapperMock.Map<Item>(itemRequest).Returns(item);
        _itemServiceMock.UpdateItemAsync(Arg.Any<Item>()).Returns(item);

        // Act
        var result = await _sut.UpdateItemAsync(itemRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task UpdateItemAsync_ItemNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var itemRequest = _fixture.Create<ItemRequest>();
        var item = _fixture.Create<Item>();
        _mapperMock.Map<Item>(itemRequest).Returns(item);
        _itemServiceMock.UpdateItemAsync(Arg.Any<Item>()).Throws<ItemNotFoundException>();

        // Act
        var result = await _sut.UpdateItemAsync(itemRequest);

        // Assert
        result.Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task DeleteItemAsync_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        _itemServiceMock.DeleteItemAsync(Arg.Any<int>()).Returns(item);

        // Act
        var result = await _sut.DeleteItemAsync(item.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteItemAsync_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var itemId = 0;
        _itemServiceMock.DeleteItemAsync(Arg.Any<int>()).Throws<ItemNotFoundException>();

        // Act
        var result = await _sut.DeleteItemAsync(itemId);

        // Assert
        result.Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
