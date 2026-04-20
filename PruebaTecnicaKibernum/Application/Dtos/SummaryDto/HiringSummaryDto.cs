namespace PruebaTecnicaKibernum.Application.Dtos.SummaryDto
{
    public class HiringSummaryDto
    {
        public int Pending { get; set; }
        public int InProgress { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }

        public string? MostRequestedCharacter { get; set; }
    }
}
