using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Exceptions;
using Rpa.Mit.Manual.Templates.Api.Core.Services;

using AutoFixture;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace Rpa.Mit.Manual.Templates.Api.Core.Tests.ServicesTests;

public class ItemServiceTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<ItemService> _mockLogger;
    private readonly ItemService _sut;

    public ItemServiceTests()
    {
        _fixture = new Fixture();
        _mockLogger = Substitute.For<ILogger<ItemService>>();
        _sut = new ItemService(_mockLogger);
    }

    [Fact]
    public async Task GetAllItemsAsync_Returns_AllItems()
    {
        // Act
        var result = await _sut.GetAllItemsAsync();
        // Assert
        result.Should().BeOfType<List<Item>>();
        result.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Item()
    {
        //Arrange
        var item = _fixture.Create<Item>();
        var createdItem = await _sut.CreateItemAsync(item);
        // Act
        var result = await _sut.GetItemByIdAsync(createdItem.Id);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async void GetByIdAsync_Throws_ItemNotFoundException()
    {
        //Arrange
        var id = 0;
        // Act & Assert
        var test = () => _sut.GetItemByIdAsync(id);
        await test.Should().ThrowAsync<ItemNotFoundException>();
    }

    [Fact]
    public async Task CreateItemAsync_Returns_CreatedItem()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        // Act
        var result = await _sut.CreateItemAsync(item);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateItemAsync_Returns_UpdatedItem()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        item.Id = 1;
        // Act
        var result = await _sut.UpdateItemAsync(item);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateItemAsync_Throws_ItemNotFoundException()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        item.Id = 0;
        // Act & Assert
        var test = () => _sut.UpdateItemAsync(item);
        await test.Should().ThrowAsync<ItemNotFoundException>();
    }

    [Fact]
    public async Task DeleteItemAsync_Returns_DeletedItem()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        var createdItem = await _sut.CreateItemAsync(item);
        // Act
        var result = await _sut.DeleteItemAsync(createdItem.Id);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteItemAsync_Throws_ItemNotFoundException()
    {
        // Arrange
        var id = 0;
        // Act & Assert
        var test = () => _sut.DeleteItemAsync(id);
        await test.Should().ThrowAsync<ItemNotFoundException>();
    }
}