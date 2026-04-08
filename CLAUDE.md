# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
# Build for Windows (development)
dotnet build MyNotes/MyNotes.csproj -f net8.0-windows10.0.19041.0

# Run on Windows
dotnet run --project MyNotes/MyNotes.csproj -f net8.0-windows10.0.19041.0

# Build for Android
dotnet build MyNotes/MyNotes.csproj -f net8.0-android

# Add EF Core migration (run from solution root)
dotnet ef migrations add <MigrationName> --project MyNotes.Infrastucture.Sqlite --startup-project MyNotes -- -f net8.0-windows10.0.19041.0

# Apply migrations
dotnet ef database update --project MyNotes.Infrastucture.Sqlite --startup-project MyNotes
```

## Architecture

The solution follows Clean Architecture with four projects:

- **MyNotes.Domain** — Entities (`Item`, `Notes`) and repository interfaces (`IItemRepository`, `INotesRepository`). No dependencies on other projects.
- **MyNotes.Application** — DTOs (`ItemDto`, `NotesDto`) and services (`ItemService`, `NotesService`) that map between entities and DTOs and call repository interfaces.
- **MyNotes.Infrastucture.Sqlite** — EF Core `TodoContext` (SQLite), repository implementations, and migrations. DB file stored at `Environment.SpecialFolder.LocalApplicationData/MyNotesDb.db`.
- **MyNotes** — .NET MAUI app (View + ViewModel + DI wiring in `MauiProgram.cs`).

## MAUI App Patterns

**MVVM via CommunityToolkit.Mvvm:**
- All ViewModels extend `BaseViewModel : ObservableObject`.
- Use `[ObservableProperty]` for bindable fields (generates the property); use `[RelayCommand]` for commands.
- `[NotifyPropertyChangedFor(nameof(X))]` notifies derived computed properties like `CanCreate`.

**Navigation (Shell):**
- Two flyout sections: `MainPage` (Todos) and `NotesPage` (Notes).
- `NotesCreatePage` is pushed modally via `Shell.Current.GoToAsync(nameof(NotesCreatePage))`.
- Data is passed back from `NotesCreatePage` to `NotesPage` through Shell query parameters (`action`, `id`, `opId`, `createdText`, `headTitle`). The `opId` (new Guid per save) deduplicates repeated `OnAppearing` calls.
- ViewModels receive navigation data via `IQueryAttributable.ApplyQueryAttributes` (for the create page) or `OnAppearing(IDictionary<string, object> query)` called from the page's code-behind (for the list page).

**DI Registration (`MauiProgram.cs`):**
- Pages registered as `Singleton`; ViewModels and services as `Transient`.
- `TodoContext` registered as `Singleton` (no explicit lifetime override — defaults to transient but context is configured internally to always use the same DB path).

## Key Conventions

- DTOs live in `MyNotes.Application.Model`; domain entities live in `MyNotes.Domain.Entities`. Services convert between them — never expose entities to the ViewModel layer.
- Note: the infrastructure project is intentionally misspelled as `MyNotes.Infrastucture.Sqlite` (missing 'r'). Keep this consistent.
- `_lastOpId` in `NotesViewModel` guards against duplicate create/update calls when the page re-appears.
