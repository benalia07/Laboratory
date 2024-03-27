using Glab.App.Grades;
using Glab.Domains.Models.Grades;
using Glab.Domains.Models.Laboratories;
using GLAB.App.Memebers;

using GLAB.Domains.Models.Members;
using GLAB.Web1.Components.Members.Models;
using Microsoft.AspNetCore.Components;
using System.Xml.Linq;

namespace GLAB.Web1.Components.Layout
{
    public partial class RegisterMemberComponent
    {
        
        [Inject] public IMemberService MemberService { get; set; }
        [Inject] public IGradeService gradeService { get; set; }

        private RegisterMemberModel registerMemberModel = new RegisterMemberModel();
        private List<Grade> Grades;
        private Grade grade;
        private bool loaded;
        private bool hasError = false;
        private string errorMessage = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                loaded = false;
                Grades = await gradeService.GetGrades();         
                loaded = true;
                StateHasChanged();
            }
            catch (Exception e)
            {
                errorMessage = $"ERROR RETRIEVING GRADES: {e.Message}";

                hasError = true;
            }
           
        }
        private async Task RegisterMember()
        {
            
            hasError = false;
            errorMessage = default;
            try
            {
                Console.Write(registerMemberModel.GradeId);
                grade = await gradeService.GetGradeById(registerMemberModel.GradeId);


                Member memberToRegister = new Member()
                {
                    
                    PhoneNumber= registerMemberModel.PhoneNumber,
                    NIC = registerMemberModel.NIC,
                    GradeId = registerMemberModel.GradeId,
                    
                };

                await MemberService.SetMember(memberToRegister);

            }
            catch (Exception e)
            {
                errorMessage = $"ERROR OF CREATIOG THE TEAM : {e.Message}";
                hasError = true;
            }


        }




    }
}

