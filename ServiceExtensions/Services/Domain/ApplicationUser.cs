using Extensions.EntityFrameWorkCore.MsSql;

namespace SampleApplication.Domain
{
    public class ApplicationUser : AuditEntityBase
    {
        public int User_Id { get; set; }
        public string LoginId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string UserHierarchy { get; set; }
        public int Manager1 { get; set; }
        public int Manager2 { get; set; }
    }
}
