namespace PruebaTecnicaKibernum.Application.Dtos.CharacterDto
{
    public class CharacterQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public string? Status { get; set; }
    }
}
