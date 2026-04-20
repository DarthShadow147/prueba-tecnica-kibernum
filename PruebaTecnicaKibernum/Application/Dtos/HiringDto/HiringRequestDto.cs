namespace PruebaTecnicaKibernum.Application.Dtos.HiringDto
{
    public class HiringRequestDto
    {
        public int Id { get; set; }
        public string Applicant { get; set; } = null!;
        public string Event { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public string Status { get; set; } = null!;
        public string CharacterName { get; set; } = null!;
    }
}
