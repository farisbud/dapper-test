using pusdafi.Models;

namespace pusdafi.Interface
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Races>> getAll();
        
        Task<Races> getByIdAsync(int id);

        Task<IEnumerable<Races>> getClubByCity(string city);

        bool Add(Races races);

        bool Remove(Races races);

        bool Update(Races races);

        bool Save();
     
        //Task<List<Races>> TableColumnDisplay();
    }
}
