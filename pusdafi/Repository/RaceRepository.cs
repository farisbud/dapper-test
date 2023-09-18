using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using pusdafi.Data;
using pusdafi.Interface;
using pusdafi.Models;
using System.Data;

namespace pusdafi.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDBContext _context;
        private IConfiguration _configuration;

        public  RaceRepository(ApplicationDBContext context, IConfiguration configuration) 
        { 
            _context = context;
            _configuration = configuration;
        }

        public bool Add(Races races)
        {
            _context.Add(races);
            return Save();
        }

        public async Task<IEnumerable<Races>> getAll()
        {
            return await _context.Race.ToListAsync(); 
        }

        public async Task<Races> getByIdAsync(int id)
        {
            return await _context.Race.Include(x => x.Address).FirstOrDefaultAsync(i=> i.Id == id);
           // return await _context.Race.FromSql("select * from race join address on race.AddressId = address.Id where race.Id = {0}", id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Races>> getClubByCity(string city)
        {
            return await _context.Race.Where(c => c.Address.City.Contains(city)).ToListAsync(); 
        }

        public bool Remove(Races races)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


        public bool Update(Races races)
        {
            _context.Update(races);
            return Save();
        }

        //public static List<Races> TableColumnDisplay()
        //{
        //    List<Races> jt = new List<Races>();

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString: "DefaultConn"))
        //        {
        //            using (SqlCommand sqlconn = new SqlCommand("select * from race"))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    sqlconn.Connection = con;
        //                    con.Open();
        //                    sda.SelectCommand = sqlconn;
        //                    SqlDataReader sdr = sqlconn.ExecuteReader();
        //                    while (sdr.Read())
        //                    {
        //                        Races Trace = new Races();
        //                        Trace.Id = (int)sdr["Id"];
        //                        Trace.Title = sdr["Title"].ToString();
        //                        jt.Add(Trace);
        //                    }
        //                }
        //                return jt
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return jt;
        //    }
        //}

    }
}
