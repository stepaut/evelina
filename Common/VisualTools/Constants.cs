using Avalonia.Platform.Storage;

namespace VisualTools
{
    public static class Constants
    {
        public const string DB_EXTENSION = "evedb";

        public static FilePickerFileType DbFileType { get; } = new("Evelina project")
        {
            Patterns = new[] { $"*.{DB_EXTENSION}" },
        };
    }
}