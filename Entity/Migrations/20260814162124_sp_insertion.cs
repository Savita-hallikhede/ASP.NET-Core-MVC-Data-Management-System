using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class sp_insertion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_datainsertion = @"
            create procedure Inserting_data_into_sp
            ( 
            @PersonID uniqueidentifier,
            @PersonName nvarchar(40),
            @PersonEmail varchar(50),
            @DateOfBirth datetime2(7),
            @Gender varchar(20),
            @CountryId uniqueidentifier,
            @Country varchar(50),
            @Address varchar(50),
            @ReceiveNewsLetters bit
            )
            AS
            BEGIN
              insert into [dbo].[Persons]
              (
                 PersonID,
                 PersonName,
                 PersonEmail,
                 DateOfBirth,
                 Gender,
                 CountryId,
                 Country,
                 Address,
                 ReceiveNewsLetters
               )
              VALUES
             (
            @PersonID,
            @PersonName,
            @PersonEmail,
            @DateOfBirth,
            @Gender,
            @CountryId,
            @Country,
            @Address,
            @ReceiveNewsLetters
            );
            END";
            migrationBuilder.Sql(sp_datainsertion);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop procedure Inserting_data_into_sp");
        }
    }
}
