using System;

namespace Extensions.EntityFrameWorkCore.MsSql
{
    /// <summary>
    /// audit entity interface
    /// </summary>
    public interface IAuditEntity
    {
        DateTime? CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
    }

    /// <summary>
    /// audit entity interface implementation
    /// </summary>
    public abstract class AuditEntityBase : IAuditEntity
    {
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
