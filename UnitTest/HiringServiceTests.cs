using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Application.Services;
using PruebaTecnicaKibernum.Domain.Entities;
using PruebaTecnicaKibernum.Domain.Enums;

namespace UnitTest
{
    public class HiringServiceTests
    {
        private readonly Mock<IHiringRequestRepository> _RepoMock = new();
        private readonly Mock<ICharacterRepository> _CharRepoMock = new();
        private readonly Mock<ILogger<HiringRequestService>> _LoggerMock = new();

        [Fact]
        public async Task CreateAsync_ShouldCreateRequest_WhenCharacterExists()
        {
            // Arrange
            var lService = new HiringRequestService(_CharRepoMock.Object, _RepoMock.Object, _LoggerMock.Object);

            _CharRepoMock.Setup(x => x.ExistsByExternalIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var dto = new CreateHiringRequestDto
            {
                CharacterId = 1,
                Applicant = "Netflix",
                Event = "Serie",
                EventDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var lResult = await lService.CreateAsync(dto);

            // Assert
            _RepoMock.Verify(x => x.AddAsync(It.IsAny<HiringRequest>()), Times.Once);
            _RepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus_WhenRequestExists()
        {
            // Arrange
            var lRequest = new HiringRequest
            {
                Id = 1,
                Status = RequestStatus.PENDING
            };

            _RepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(lRequest);

            var service = new HiringRequestService(_CharRepoMock.Object, _RepoMock.Object, _LoggerMock.Object);

            // Act
            await service.UpdateStatusAsync(1, RequestStatus.APPROVED);

            // Assert
            lRequest.Status.Should().Be(RequestStatus.APPROVED);
            _RepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
