using MyNotes.Application.Model;

namespace MyNotes.ViewModel
{
    public class TodoGroup : List<ItemDto>
    {
        public string DateLabel { get; }
        public DateTime Date { get; }

        public TodoGroup(DateTime date, IEnumerable<ItemDto> items) : base(items)
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
