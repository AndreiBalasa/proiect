using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// Reprezinta una dintre echipe din matchup 
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Reprezinta scorul pentru aceasta echipa
        /// </summary>
        public double Score { get; set; }
        
        /// <summary>
        /// Reprezinta matchap-ul precedent din care aceasta echipa a iesit castigatoare
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
    }
}
