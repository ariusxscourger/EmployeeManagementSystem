namespace EmployeeManagementSystem.Entity
{
    public class EmployeeAudit
    {
        public int AuditId { get; set; }
        public int EmployeeId { get; set; }
        public string ActionType { get; set; }
        public string ChangedData { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }

}
