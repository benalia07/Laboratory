using BlazorApp2.Components.Pages;
using GLAB.App.Memebers;
using GLAB.App.Teams;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;

namespace GLAB.Web1.Components.Pages
{
    public partial class AddMember
    {

        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private IMemberService memberService { get; set; }

        [Inject] private ITeamService teamService { get; set; }

        private List<Team> teams;

        private CreateMemberModel member = new CreateMemberModel();

        private bool hasError = false;
        private string errorMessage = string.Empty;
        private string success = string.Empty;

        private async Task loadData()
        {
            teams = await teamService.GetTeams();
        }

        protected override async Task OnInitializedAsync()
        {
            await loadData();
        }

        private async Task addMember()
        {
            hasError = false;
            errorMessage = string.Empty;
            success = string.Empty;
            try
            {
                Member newMember = new Member()
                {
                    MemberId = Guid.NewGuid().ToString(),
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    Status = 0,
                    TeamId = member.Team
                };

                await memberService.CreateMember(newMember);
                navigationManager.NavigateTo("/MemberRegistration");
                success = "The member added successfully";
                member.FirstName = "";
                member.Email = "";
                member.LastName = "";
                member.Team = "";
                member.Phone = "";
            }
            catch (Exception ex)
            {
                hasError = true;
                errorMessage = $"Error {ex.Message}";

            }
        }
    }
}
