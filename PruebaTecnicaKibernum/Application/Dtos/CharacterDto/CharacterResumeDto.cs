namespace PruebaTecnicaKibernum.Application.Dtos.CharacterDto
{
    public class CharacterResumeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
