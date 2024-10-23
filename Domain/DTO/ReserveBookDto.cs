namespace Domain.DTO
{
    public class ReserveBookDto
    {
        public string BookId { get; set; } = "";
        public uint RequestedCount { get; set; } = 0;

        public ReserveBookDto() { }

        public ReserveBookDto(string bookId, uint requestedCount)
        {
            BookId = bookId;
            RequestedCount = requestedCount;
        }
    }
}
