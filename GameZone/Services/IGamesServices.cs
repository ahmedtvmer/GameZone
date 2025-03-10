namespace GameZone.Services
{
    public interface IGamesServices
    {
        IEnumerable<Game> GetAll(int currentPage = 1, int pageSize = 6);
        Game GetGame(int id);
        Task AddGame(CreateGameViewModel model);
        bool RemoveGame(int id);
        Task UpdateGame(int id, EditGameViewModel model);
        IEnumerable<Game> GetGamesBySearch(int currentPage = 1, int PageSize = 6, string searchTerm = "");

        int GetCount(string searchTerm);

    }
}
