﻿using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {

        // TODO - salvarea in fisier

        /// <summary>
        /// salvam in fisier
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        private const string PrizesFile = "PrizeModels.csv";

        public PrizeModel CreatePrize(PrizeModel model)
        {
            // *load the text file
            // *convert the text to List<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // find last id and add +1 to it
            int currentId = 1;

            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            
            model.Id = currentId;

            prizes.Add(model);

            // convert the prizes to list <strig>
            // save the list<string> to the text file

            prizes.SaveToPrizeFile(PrizesFile);

            return model;

        }
    }
}