using FluentAssertions;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Tests
{
    public class HotelServiceTests
    {
        private readonly Mock<IHotelRepository> _mockHotelRepository;
        private readonly HotelService _hotelService;

        public HotelServiceTests()
        {
            _mockHotelRepository = new Mock<IHotelRepository>();
            _hotelService = new HotelService(_mockHotelRepository.Object);
        }

        [Fact]
        public async Task GetHotelByIdAsync_WhenHotelDoesNotExist_ReturnsNull()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            _mockHotelRepository
                .Setup(repo => repo.GetByIdAsync(nonExistentId, cancellationToken))
                .ReturnsAsync((Hotel?)null);

            // Act
            var result = await _hotelService.GetHotelByIdAsync(nonExistentId, cancellationToken);

            // Assert
            result.Should().BeNull();

            _mockHotelRepository.Verify(
                repo => repo.GetByIdAsync(nonExistentId, cancellationToken),
                Times.Once);
        }
    }
}
