
CREATE PROCEDURE usp_GetEmployees
    @SearchTerm NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.Email,
        e.PhoneNumber,
        e.HireDate,
        e.Salary,
        j.JobTitle,
        d.DepartmentName,
        m.FirstName + ' ' + m.LastName AS ManagerName,
        e.Status
    FROM Employees e
    LEFT JOIN Jobs j ON e.JobId = j.JobId
    LEFT JOIN Departments d ON e.DepartmentId = d.DepartmentId
    LEFT JOIN Employees m ON e.ManagerId = m.EmployeeId
    WHERE 
        @SearchTerm IS NULL OR
        e.FirstName LIKE '%' + @SearchTerm + '%' OR
        e.LastName LIKE '%' + @SearchTerm + '%' OR
        e.Email LIKE '%' + @SearchTerm + '%' OR
        e.PhoneNumber LIKE '%' + @SearchTerm + '%' OR
        j.JobTitle LIKE '%' + @SearchTerm + '%' OR
        d.DepartmentName LIKE '%' + @SearchTerm + '%' OR
        e.Status LIKE '%' + @SearchTerm + '%'
    ORDER BY e.FirstName, e.LastName;
END;

EXEC usp_GetEmployees @SearchTerm = 'John';
