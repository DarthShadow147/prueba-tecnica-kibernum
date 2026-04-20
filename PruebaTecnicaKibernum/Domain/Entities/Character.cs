namespace PruebaTecnicaKibernum.Domain.Entities
{
    public class Character
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime ImportedAt { get; set; }

        public ICollection<HiringRequest> HiringRequest { get; set; } = [];
    }
}
