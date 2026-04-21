using Microsoft.Extensions.Logging;
using Moq;
using PruebaTecnicaKibernum.Application.Dtos.ApiDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Application.Services;
using PruebaTecnicaKibernum.Domain.Entities;

namespace UnitTest
{
    public class CharacterServiceTests
    {
        private readonly Mock<ICharacterRepository> _RepoMock = new();
        private readonly Mock<IRickAndMortyService> _ApiMock = new();
        private readonly Mock<ILogger<CharacterService>> _LoggerMock = new();

        [Fact]
        public async Task ImportCharacterAsync_ShouldAddNewCharacters_WhenNotExists()
        {
            //Arrange
            var lService = new CharacterService(_RepoMock.Object, _ApiMock.Object, _LoggerMock.Object);

            _ApiMock.Setup(x => x.GetCharactersAsync(It.IsAny<int>()))
                .ReturnsAsync(new RickAndMortyResponseDto
                {
                    Info = new InfoDto { Pages = 1 },
                    Results =
                    [
                        new() 
                        { 
                            Id = 1, 
                            Name = "Rick", 
                            Status = "Alive",
                            Species = "Human",
                            Gender = "Male",
                            Image = "https://rickandmortyapi.com/api/character/avatar/1.jpeg",
                            Origin = new OriginDto
                            {
                                Name = "Earth (C-137)"
                            }
                        }
                    ]
                });

            _RepoMock.Setup(x => x.ExistsByExternalIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            //Act
            await lService.ImportCharacterAsync();

            //Assert
            _RepoMock.Verify(x => x.AddAsync(It.IsAny<Character>()), Times.Once);
            _RepoMock.Verify(x => x.SaveChangesAsync(), Times.AtLeastOnce);
        }
    }
}
