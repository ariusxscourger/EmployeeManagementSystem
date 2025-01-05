using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Entity
{
    public class EmployeeAudit
    {
        public int AuditId { get; set; }
        public int EmployeeId { get; set; }
        public required string ActionType { get; set; }
        public required string ChangedData { get; set; }
        public required string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }

}
