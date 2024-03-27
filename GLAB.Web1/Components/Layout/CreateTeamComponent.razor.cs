
using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLAB.App;
using GLAB.Domains.Models.Laboratories;
using GLAB.Web1.Components.Teams.Models;
using GLAB.App.Teams;
using GLAB.App.Laboratories;
using Glab.Domains.Models.Laboratories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.SqlServer.Server;

namespace GLAB.Web1.Components.Layout
{
    public partial class CreateTeamComponent
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject] public ITeamService createTeam { get; set; }
        [Inject] public ILabService laboratoryService { get; set; }
        private bool loaded = false;

        private List<Laboratory> Laboratories;
        private Laboratory laboratory;
    
        private CreateTeamModel teamModel = new CreateTeamModel();

        private bool hasError = false;
        private string errorMessage = string.Empty;
        protected  override async Task OnInitializedAsync()
        {
            try
            {
                loaded = false;
                
               Laboratories = await laboratoryService.GetLaboratories();
                StateHasChanged();
                
                loaded = true;
            }
            catch (Exception e)
            {
                errorMessage = $"ERROR RETRIEVING LABORATORIES: {e.Message}";
                
                hasError = true;
            }
        }
        private async Task CreateTeam()
        {
            hasError = false;
            errorMessage = default;
            try
            {
                Console.Write(teamModel.LaboratoryId);
                laboratory = await laboratoryService.GetLaboratoryById(teamModel.LaboratoryId);

                Team teamtocreate = new Team()
                {
                    LaboratoryId = teamModel.LaboratoryId,
                    TeamId = Guid.NewGuid().ToString(),
                    TeamName = teamModel.TeamName,
                    Status = TeamStatus.Blocked,
                    TeamAcronyme= teamModel.TeamAcronyme



                };

                await createTeam.CreateTeam(teamtocreate);
                navigationManager.NavigateTo("/add-member");


            }
            catch (Exception e)
            {
                errorMessage = $"ERROR OF CREATIOG THE TEAM : {e.Message}";
                hasError = true;
            }



        }


    }
}