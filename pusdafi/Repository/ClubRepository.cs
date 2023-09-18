using Microsoft.EntityFrameworkCore;
using pusdafi.Data;
using pusdafi.Interface;
using pusdafi.Models;

namespace pusdafi.Repository
{
    public class ClubRepository // : IClubRepository
    {
        private readonly ApplicationDBContext _context;

        public ClubRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public bool Add(Club club)
        {
            _context.Add(club);
            return save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return save();
        }

        public async Task<IEnumerable<Club>> getAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByAsync(int id)
        {
            return await _context.Clubs.Include(x => x.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return save();
        }
    }
}
