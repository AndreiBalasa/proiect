﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        /// <summary>
        /// Echipele ce participa in acest meci
        /// </summary>
        /// 
        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// Echipa castigatoare
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// Runda respectiva
        /// </summary>
        public int MatchupRound { get; set; }



    }
}
