namespace Library.API.Base.Models
{
    public interface ISelection
    {
        string? UUID { get; set; }
        bool? isSelected { get; set; }
    }
}
