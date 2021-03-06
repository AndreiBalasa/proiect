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
        private const string PeopleFile = "PersonModels.csv";
        private const string TeamFile = "TeamModel.csv";
        private const string TournamentFile = "TournamentModel.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            // *load the text file
            // *convert the text to List<PrizeModel>
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            // find last id and add +1 to it
            int currentId = 1;

            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            people.Add(model);

            // convert the prizes to list <strig>
            // save the list<string> to the text file

            people.SaveToPeopleFile(PeopleFile);

            return model;
        }

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

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamFile.FullFilePath().LoadFile().ConvetToTeamModels(PeopleFile);



            int currentId = 1;

            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamFile(TeamFile);

            return model;

        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels(TeamFile,PeopleFile,PrizesFile);


            int currentId = 1;

            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;

            }

            model.Id = currentId;

            tournaments.Add(model);

            tournaments.SaveToTournamentFile(TournamentFile);

            return model;

        }

        public List<PersonModel> GetPerson_All()
        {

            
            return PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

        }

        public List<TeamModel> GetTeam_ALL()
        {
            return TeamFile.FullFilePath().LoadFile().ConvetToTeamModels(PeopleFile); 
        }

        public List<PrizeModel> GetPrizes_All()
        {
            return PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();
        }
    }
}
