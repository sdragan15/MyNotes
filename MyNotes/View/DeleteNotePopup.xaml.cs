using CommunityToolkit.Maui.Views;

namespace MyNotes.View
{
    public partial class DeleteNotePopup : Popup
    {
        public DeleteNotePopup(string noteHeader)
        {
            InitializeComponent();
            MessageLabel.Text = $"Delete \"{noteHeader}\"?";
        }

        private void OnDeleteClicked(object? sender, EventArgs e) => Close(true);
        private void OnCancelClicked(object? sender, EventArgs e) => Close(false);
    }
}
