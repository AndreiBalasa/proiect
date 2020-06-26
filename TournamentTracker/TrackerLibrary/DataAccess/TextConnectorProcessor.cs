using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;


// *load the text file
// *convert the text to List<PrizeModel>
// find id and add +1 to it
// convert the prizes to list <strig>
// save the list<string> to the text file

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        public static List<string> LoadFile(this string file)
        {
            if(!File.Exists(file))
            {

                return new List<string>();

            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {

                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();

                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]); 
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);

                output.Add(p);
            }

            return output;
        }



        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {

            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] colls = line.Split(',');

                PersonModel p = new PersonModel();

                p.Id = int.Parse(colls[0]);
                p.FirstName = colls[1];
                p.Lastname = colls[2];
                p.EmailAdress = colls[3];
                p.CellphoneNumber = colls[4];

                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConvetToTeamModels(this List<string> lines, string peopleFileName)
        {

            //id , teamName , list of ids separated by |
            // 1 , bajetii , 1|3|5

            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] colls = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(colls[0]);
                t.TeamName = colls[1];

                string[] personIds = colls[2].Split('|');

                
              
                    foreach (string id in personIds)
                    {
                        t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                    }
                
                output.Add(t);
            }

            return output;
        }


        public static List<TournamentModel> ConvertToTournamentModels (
            this List<string> lines, 
            string TeamFileName,
            string PeopleFileName,
            string PrizesFileName)
        {
            // id,tournamentName,EntryFee,(id|id|id - lista de echipe),(id|id|id-lista de prizes),(Rounds - <list<list<matchupmodel>> - id^id^id|id^id^id|id^id^id)
            // comentariul de mai sus e ca sa ma ajute sa vad cum ar trebui sa fie structura fisierului

            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = TeamFileName.FullFilePath().LoadFile().ConvetToTeamModels(PeopleFileName);
            List<PrizeModel> prizes = PrizesFileName.FullFilePath().LoadFile().ConvertToPrizeModels();


            foreach (string line in lines)
            {
                string[] colls = line.Split(',');
                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(colls[0]);
                tm.TournamentName = colls[1];
                tm.EntryFee = decimal.Parse(colls[2]);


                string[] teamIds = colls[3].Split('|');

                foreach (var id in teamIds)
                {
                    //t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());

                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());  //adauga in entered team prima echipa care coresc cu id ul 
                }

                string[] prizeIds = colls[4].Split('|');

                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }


                //TODO - La fel si pt rounds...
                output.Add(tm);
            }

            return output;

        }

        public static void SaveToTournamentFile(this List<TournamentModel> models,string fileName)
        {

            List<string> lines = new List<string>();

            foreach(TournamentModel tm in models)
            {

                lines.Add($@"{ tm.Id },
                            { tm.TournamentName },
                            { tm.EntryFee },
                            { ConvertTeamListToString(tm.EnteredTeams) },
                            { ConvertPrizesToString(tm.Prizes) },
                            { ConvertRoundsToString(tm.Rounds) }");

            }

            File.WriteAllLines(fileName.FullFilePath(), lines);

        }


        private static string ConvertRoundsToString(List<List<MatchupModel>> rounds)
        {

            string output = "";

            if (rounds.Count == 0)
            {

                return "";

            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            output = output.Substring(0, output.Length - 1); // am scos ultimul "|" din string

            return output;

        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {

                return "";

            }

            foreach (MatchupModel p in matchups)
            {
                output += $"{ p.Id }^";
            }

            output = output.Substring(0, output.Length - 1); // am scos ultimul "^" din string

            return output;


        }

        private static string ConvertPrizesToString(List<PrizeModel> prizes)
        {

            string output = "";

            if (prizes.Count == 0)
            {

                return "";

            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1); // am scos ultimul "|" din string

            return output;

        }


        private static string ConvertTeamListToString(List<TeamModel> teams)
        {

            string output = "";

            if (teams.Count == 0)
            {

                return "";

            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            output = output.Substring(0, output.Length - 1); // am scos ultimul "|" din string

            return output;

        }



        public static void SaveToTeamFile(this List<TeamModel> models , string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel team in models)
            {
                lines.Add($"{ team.Id },{ team.TeamName},{ ConvertPeopleListToString(team.TeamMembers) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);

        }

        private static string ConvertPeopleListToString(List<PersonModel> people) // ia lista persoanelor si o transforma intr un string
        {

            string output = "";

            if(people.Count == 0)
            {

                return "";

            }

            foreach (PersonModel p in people)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1); // am scos ultimul | din string

            return output;
        }

        public static void SaveToPeopleFile(this List<PersonModel> models , string fileName)
        {

            List<string> lines = new List<string>();


            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.Lastname },{ p.EmailAdress },{ p.CellphoneNumber }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models , string fileName)
        {

            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
    }
}
