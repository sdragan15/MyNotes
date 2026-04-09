using MyNotes.Application.Model;

namespace MyNotes.ViewModel
{
    public class NotesGroup : List<NotesDto>
    {
        public string DateLabel { get; }
        public DateTime Date { get; }

        public NotesGroup(DateTime date, IEnumerable<NotesDto> items) : base(items)
        {
            Date = date;
            DateLabel = FormatDate(date);
        }

        private static string FormatDate(DateTime date)
        {
            var today = DateTime.Today;
            if (date.Date == today) return "Today";
            if (date.Date == today.AddDays(-1)) return "Yesterday";
            return date.ToString("d MMMM yyyy");
        }
    }
}
