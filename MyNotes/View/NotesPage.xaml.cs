using MyNotes.ViewModel;
using System.Threading.Tasks;

namespace MyNotes.View;

public partial class NotesPage : ContentPage, IQueryAttributable
{
    IDictionary<string, object> _query;

    public NotesPage(NotesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _query = query;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is NotesViewModel vm)
        {
            await vm.OnAppearing(_query);
            if(vm.GetNotesCommand.CanExecute(null))
                vm.GetNotesCommand.Execute(null);
        }
    }
}