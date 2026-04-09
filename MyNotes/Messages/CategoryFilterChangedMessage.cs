namespace MyNotes.Messages
{
    public record CategoryFilterChangedMessage(int? CategoryId, string? CategoryName);
}
