using PruebaTecnicaKibernum.Domain.Enums;

namespace PruebaTecnicaKibernum.Domain.Entities
{
    public class HiringRequest
    {
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; } = null!;
        public string Applicant { get; set; } = null!;
        public string Event { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
