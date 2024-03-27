using System.ComponentModel.DataAnnotations;

namespace GLAB.Web1.Components.Members.Models
{
    public class RegisterMemberModel
    {
        [Required(ErrorMessage = "the member's FirstName is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "the member's LastName is required.")]
        public string NIC { get; set; }
        [Required(ErrorMessage = "the member's email is required .")]
        public string GradeId { get; set; }
        [Required(ErrorMessage = "Password is required .")]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "Confirm password  is required .")]
        public string ConfirmPassWord { get; set; }

    }
}
