using pusdafi.Models;

namespace pusdafi.Interface
{
    public interface IClubRepository
    {

        Task<IEnumerable<Club>> getAll();

        Task<IEnumerable<Club_Category>> getCategory();
        //Task<List<Club>> getAll();
        //List<Club> getAll();

        Task<Club> GetByAsync(int id);

        Task<Club> EditByAsync(int id);

        Task<IEnumerable<Club>> GetClubByCity(string city);

        bool Add(Club club);

        bool Update(Club club);

        bool Delete(Club club);

        bool save();

      //  string getUploadImage(Club club);
        //private string uploadImage(Club club);

    }
}
