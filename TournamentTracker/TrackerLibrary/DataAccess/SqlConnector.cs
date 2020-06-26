using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;



//@PlaceNumber int,
//@PlaceName nvarchar(50),
//@PrizeAmount money,
//@PrizePercentage float,
//@Id int =0 output

  
namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection 
    {
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournament")))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.Lastname);
                p.Add("@EmailAddress", model.EmailAdress);
                p.Add("@CellphoneNumber", model.CellphoneNumber);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");

                return model;
            }
        }

        // TODO - fa metoda sa salveze in baza de date

        /// <summary>
        ///  Salvez un new prize in baza de date
        /// </summary>
        /// <param name="model"></param>
        /// <returns>the prize info, incluzand si id-ul </returns>

        public PrizeModel CreatePrize(PrizeModel model)
        {


            
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournament")))
                {
                    var p = new DynamicParameters();
                    p.Add("@PlaceNumber", model.PlaceNumber);
                    p.Add("@PlaceName", model.PlaceName);
                    p.Add("@PrizeAmount", model.PrizeAmount);
                    p.Add("@PrizePercentage", model.PrizePercentage);
                    p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                    model.Id = p.Get<int>("@Id");

                    return model;
                }
            }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournament")))
            {
                
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id= p.Get<int>("@Id");


                foreach (PersonModel t in model.TeamMembers)
                {
                    var tm = new DynamicParameters();
                    tm.Add("@TeamID", model.Id);
                    tm.Add("@PersonId", t.Id);

                    connection.Execute("dbo.spTeamMembers_Insert", tm, commandType: CommandType.StoredProcedure);
                    
                }

                return model;

            }
        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournament")))
            {

                SaveTournament(connection,model);

                SaveTournamentPrizes(connection, model);

                SaveTournamentEntries(connection, model);

                return model;

            }
        }

        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel tem in model.EnteredTeams)
            {

                var et = new DynamicParameters();
                et.Add("@TournamentId", model.Id);
                et.Add("@TeamId", tem.Id);
                et.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentEntries_Insert", et, commandType: CommandType.StoredProcedure);

            }
        }

        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            foreach (PrizeModel pz in model.Prizes)
            {
                var tm = new DynamicParameters();
                tm.Add("@TournamentId", model.Id);
                tm.Add("@PrizeId", pz.Id);
                tm.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentPrizes_insert", tm, commandType: CommandType.StoredProcedure);

            }
        }

        private void SaveTournament(IDbConnection connection,TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);
            model.Id = p.Get<int>("@Id");
        }


        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournament")))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetALL").ToList();
                
            }

            return output;
        }

        public List<TeamModel> GetTeam_ALL()
        {
            List<TeamModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournament")))
            {
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();


                foreach (TeamModel team in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                    
                    team.TeamMembers = connection.Query<PersonModel>("spTeamMembers_GetByTeam", p , commandType: CommandType.StoredProcedure).ToList();
                }

                
            }

            return output;
        }
    }
}
