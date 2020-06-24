using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class SqlConnector : IDataConnection
    {

        // TODO - fa metoda sa salveze in baza de date

        /// <summary>
        ///  Salvez un new prize in baza de date
        /// </summary>
        /// <param name="model"></param>
        /// <returns>the prize info, incluzand si id-ul </returns>

        public PrizeModel CreatePrize(PrizeModel model)
        {
            
            model.Id = 1;

            return model;
        }
    }
}
