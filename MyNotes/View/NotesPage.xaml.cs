using MyNotes.ViewModel;

namespace MyNotes.View;

public partial class NotesPage : ContentPage
{
	public NotesPage(NotesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}