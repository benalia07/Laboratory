namespace GLAB.Domains.Models.Members;

public class Member
{
    public string MemberId { get; set; }
    public string TeamId { get; set; }
    public string GradeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string NIC { get; set; }
    public string PhoneNumber { get; set; }
    public byte[] Image { get; set; } = new byte[] { };
    public MemberStatus Status { get; set; }

    public string gradeId { get; set; }
}
