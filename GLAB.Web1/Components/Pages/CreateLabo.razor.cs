using Glab.Domains.Models.Laboratories;
using GLAB.App.Laboratories;
using Microsoft.AspNetCore.Components;
namespace GLAB.Web1.Components.Pages
{
    public partial class CreateLabo
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        private ILabService labService { get; set; }
        private CreateLaboModel newlabo = new CreateLaboModel();

        private async Task save()
        {
            Laboratory labToCreate = new Laboratory()
            {
                LaboratoryId = Guid.NewGuid().ToString(),
                Name = newlabo.Name,
                Acronyme = newlabo.Acronyme,
                Adresse = newlabo.Adresse,
                CreationDate = DateTime.Now,
                Departement = newlabo.Departement,
                Email = newlabo.Email,
                Faculty = newlabo.Faculty,
                Logo ="aaa",
                NumAgrement = newlabo.NumAgrement,
                PhoneNumber = newlabo.PhoneNumber,
                Status = GLAB.Domains.Models.Laboratories.LaboratoryStatus.Bloqued,
                University = newlabo.University,
                WebSite = newlabo.WebSite
            };

            await labService.CreateLaboratory(labToCreate);
            navigationManager.NavigateTo("/home");
        }

    }
}

