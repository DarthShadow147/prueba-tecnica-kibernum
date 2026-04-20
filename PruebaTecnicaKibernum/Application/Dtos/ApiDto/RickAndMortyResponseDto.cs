namespace PruebaTecnicaKibernum.Application.Dtos.ApiDto
{
    public class RickAndMortyResponseDto
    {
        public InfoDto Info { get; set; } = null!;
        public List<CharacterApiDto> Results { get; set; } = new();
    }

    public class InfoDto
    {
        public int Count { get; set; }
        public int Pages { get; set; }
    }

    public class CharacterApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public OriginDto Origin { get; set; } = null!;
        public string Image { get; set; } = null!;
    }

    public class OriginDto
    {
        public string Name { get; set; } = null!;
    }
}
