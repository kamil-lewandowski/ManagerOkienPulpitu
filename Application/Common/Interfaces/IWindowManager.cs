namespace Application.Common.Interfaces;

public interface IWindowManager
{
    /// <summary>
    /// Otwiera nowe okno z procesem. Jeżeli proses istnieje to zostaje przeniesiony na wierzch pulpitu.
    /// </summary>
    /// <param name="name">Ścieżka do procesu</param>
    void Open(string name);

    /// <summary>
    /// Zamyka okno procesu.
    /// </summary>
    /// <param name="name">Ścieżka do procesu</param>
    void Close(string name);

    /// <summary>
    /// Pobiera aktualnie otwarte okno znajdujące się na wierzchu.
    /// </summary>
    /// <returns>Ścieżka aktualnie otwartego okna</returns>
    string? GetTopWindow();

    /// <summary>
    /// Przełączenie na okno poniżej wskazanego w parametrze.
    /// </summary>
    /// <param name="name">Ścieżka do procesu</param>
    /// <returns>Ścieżka aktualnie otwartego okna</returns>
    string? OpenPrevious(string name);
}