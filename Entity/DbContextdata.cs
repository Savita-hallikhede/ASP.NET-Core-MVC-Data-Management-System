using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Entity
{
    public class DbContextdata:DbContext
    {
        public DbContextdata(DbContextOptions<DbContextdata> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Person> Persons { get; set; }  

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    Country obj = new Country()
        //    {
        //        CountryId = Guid.NewGuid(),
        //        CountryName = "India"
        //    };

        //    modelBuilder.Entity<Country>().HasData(obj);

        //}

        public int sp_insert_person(Person person)
        {
            SqlParameter p_obj = new SqlParameter("@PersonID", person.PersonId);
            SqlParameter p_obj2 = new SqlParameter("@PersonName",person.PersonName);
            SqlParameter p_obj3 = new SqlParameter("@PersonEmail",person.PersonEmail);
            SqlParameter p_obj4 = new SqlParameter("@DateOfBirth",person.DateOfBirth);
            SqlParameter p_obj5 = new SqlParameter("@Gender", person.Gender);
            SqlParameter p_obj6 = new SqlParameter("@CountryId",person.CountryId);
            SqlParameter p_obj7 = new SqlParameter("@Country", person.Country);
            SqlParameter p_obj8 = new SqlParameter("@Address",person.Address);
            SqlParameter p_obj9 = new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters);



            List<SqlParameter> list_sql = new List<SqlParameter>();
            list_sql.Add(p_obj);
            list_sql.Add(p_obj2);
            list_sql.Add(p_obj3);
            list_sql.Add(p_obj4);
            list_sql.Add(p_obj5);
            list_sql.Add(p_obj6);
            list_sql.Add(p_obj7);
            list_sql.Add(p_obj8);
            list_sql.Add(p_obj9);

            return Database.ExecuteSqlRaw(
                "exec Inserting_data_into_sp @PersonID,@PersonName,@PersonEmail,@DateOfBirth,@Gender,@CountryId,@Country,@Address,@ReceiveNewsLetters", list_sql);
                

        }


        public async Task<List<Person>> GetPersonData()
        {
            List<Person> getall = await Persons.FromSqlRaw("Execute GetAll Persons").ToListAsync();
            return getall;
        }

        
    }
}
