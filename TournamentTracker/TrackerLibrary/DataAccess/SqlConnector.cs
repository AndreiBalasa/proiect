using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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


           
        
    }
}
