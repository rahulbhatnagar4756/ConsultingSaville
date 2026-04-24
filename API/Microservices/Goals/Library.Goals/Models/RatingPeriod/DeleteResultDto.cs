namespace Library.Goals.Models.RatingPeriod
{
    /// <summary>
    /// DTO for delete operations response
    /// </summary>
    public class DeleteResultDto
    {
        public string UUID { get; set; } = string.Empty;
        public bool isValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
