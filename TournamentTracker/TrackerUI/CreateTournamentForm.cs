using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester , ITeamRequester
    {

        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_ALL();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void teamOneScoreLabel_Click(object sender, EventArgs e)
        {

        }

        private void entryFeeValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            //PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            //if (p != null)
            //{
            //    availableTeamMembers.Remove(p);
            //    selectedTeamMembers.Add(p);

            //    WireUpLists();
            //}


            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;  // convertesc echipa din dropdown intr un obiect de tip teammodel

            if (t != null)
            {

                availableTeams.Remove(t);
                selectedTeams.Add(t);


                WireUpLists();
            }



        }

        private void deleteSelectedPlayersButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;  // convertesc echipa din dropdown intr un obiect de tip teammodel

            if (t != null)
            {
                selectedTeams.Remove(t);
                availableTeams.Add(t);
                


                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {

            //apelam create prize form
            CreatePrizeForm frm = new CreatePrizeForm(this);

            frm.Show();



        }

        public void PrizeComplete(PrizeModel model) // interfata implementata
        {
            //add the model to the list

            selectedPrizes.Add(model);
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void deleteSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;
            if(p != null)
            {
                selectedPrizes.Remove(p);
                WireUpLists();

            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            //tournament model

            //validate
            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out fee);

            if(!feeAcceptable)
            {
                MessageBox.Show("Enter a valid entry fee");

                return;
            }
            
            TournamentModel tm = new TournamentModel();

            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = fee;

            tm.Prizes = selectedPrizes;
            tm.EnteredTeams = selectedTeams;


            //Create matchups



            //Create tournament entry
            //create all prizes entries
            // si all team entries

            GlobalConfig.Connection.CreateTournament(tm);


          
        }
    }
}
