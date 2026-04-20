namespace PruebaTecnicaKibernum.Application.Dtos.HiringDto
{
    public class CreateHiringRequestDto
    {
        public int CharacterId { get; set; }
        public string Applicant { get; set; } = null!;
        public string Event { get; set; } = null!;
        public DateTime EventDate { get; set; }
    }
}
