using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pusdafi.Data;
using pusdafi.Interface;
using pusdafi.Models;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using pusdafi.ViewModes.Club;
using System.Linq;
using System.Data.Common;

namespace pusdafi.Repository
{
    public class ClubRepositoryDapper : IClubRepository
    {
        private readonly IDbConnection db;
        //private readonly IWebHostEnvironment _webHostEnvironment;

        public ClubRepositoryDapper(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConn"));
            // _webHostEnvironment = webHostEnvironment;
        }

        public bool Add(Club club)
        {

            var sql2 = @"Insert Into Address (Street, City, State) Values (@street, @city, @state);" + "Select CAST(SCOPE_IDENTITY() as int);";
            var sql = @"Insert Into Clubs (Title,Description,Image,AddressId,ClubCategory) VALUES (@title,@description,@Image,@addressId,@clubCategory);" + "Select CAST(SCOPE_IDENTITY() as int);";

            var IdAddress = db.Query<int>(sql2, new
            {
                @street = club.Address.Street,
                @city = club.Address.City,
                @state = club.Address.State,

            }).Single();
            club.Address.Id = IdAddress;
            // string uniqueFileName = getUploadImage(club);

            var id = db.Query<int>(sql, new
            {
                @title = club.Title,
                @description = club.Description,
                @image = club.Image,
                @addressId = IdAddress,
                @ClubCategory = club.Club_Category.Id
            }).Single();
            club.Id = id;

            //club.Address.Id = AddressID;

            return true;

        }

        public bool Delete(Club club)
        {
            
            var parameters = new DynamicParameters();
            parameters.Add("@id", club.Id);

             db.Query<Club>("sp_delete_clubs", parameters, commandType: CommandType.StoredProcedure);

            return true; 
        }

        public async Task<IEnumerable<Club>> getAll()
        {
            var sql = @"select c.Id, c.Title,c.Description,c.Image, cc.Id, cc.Category from Clubs c inner join Clubs_category cc on cc.Id = c.ClubCategory";

            IEnumerable<Club> result = await db.QueryAsync<Club, Club_Category, Club>(sql, (club, club_cat) =>
            {
                club.Club_Category = club_cat;

                return club;
                //split on "Id" dari tabel Club category
            }, splitOn: "Id").ConfigureAwait(false);

            return result.ToList();
        }

        public async Task<IEnumerable<Club_Category>> getCategory()
        {
            var sql = @"select * from Clubs_category";

            IEnumerable<Club_Category> result = await db.QueryAsync<Club_Category>(sql).ConfigureAwait(false);

            return result.ToList();
        }

        public async Task<Club> GetByAsync(int id)
        {
            var sql = @"select c.Id,c.Title,c.Description,c.Image,a.Id,a.Street,a.City,a.State from Clubs c join Address a on a.Id = c.AddressId where c.Id =@clubId";


            var result = await db.QueryAsync<Club, Address, Club>(sql, (club, address) =>
            {
                club.Address = address;
                return club;
            }, new { clubId = id }).ConfigureAwait(false);

            return result.FirstOrDefault();

            //var result = await db.QueryAsync<Club>(sql, new { clubId = id }).ConfigureAwait(false);

            //return result.FirstOrDefault();

        }

        public Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            throw new NotImplementedException();
        }

        public bool save()
        {
            //var saved = db.SaveChanges();
            //return saved > 0 ? true : false;
            throw new NotImplementedException();


        }

        public bool Update(Club club)
        {
            var cek = club.AddressId;
            var sql = @"Update Clubs set Title = @title,Description = @description, Image = @image ,ClubCategory = @catId 
                        where Id = @clubId";
            var sql2 = @"Update Address set	Street = @street,	City = @city	,State = @state 
                        where Id = @addressId";

            db.Execute(sql, new
            {
                @title = club.Title,
                @description = club.Description,
                @image = club.Image,
                @catId = club.ClubCategory,
                @clubId = club.Id
            });
            db.Execute(sql2, new
            {
                @street = club.Address.Street,
                @city = club.Address.City,
                @state = club.Address.State,
                @addressId = club.AddressId
            });

            return true;
            //throw new NotImplementedException();
        }

        public async Task<Club?> EditByAsync(int id)
        {
            var sql = @"select c.Id,c.Title,c.Description,c.Image,c.ClubCategory,a.Id,a.Street,a.City,a.State,cc.Id ,cc.Category  from Clubs c join Address a on a.Id = c.AddressId join Clubs_category cc on cc.Id =c.ClubCategory  where c.Id =@clubId";
            var result = await db.QueryAsync<Club, Address, Club_Category, Club>(sql, (club, address, club_category) =>
            {
                return new Club
                {
                    Id = club.Id,
                    Title = club.Title,
                    Description = club.Description,
                    Image = club.Image,
                    AddressId = club.AddressId,
                    Address = address,
                    ClubCategory = club.ClubCategory,
                    Club_Category = club_category


                };

            }, new { clubId = id }

            ).ConfigureAwait(false);
            // var cek = result.FirstOrDefault();
            return result.FirstOrDefault();
            //throw new NotImplementedException();
        }


    }
}
