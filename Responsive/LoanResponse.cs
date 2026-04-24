namespace LibrarySystem.Responsive;

public class LoanResponse
{
    public int Id { get; set; }
    
    // campos para mostrar nombres reales en lugar de solo IDs
    public required string BookTitle { get; set; }
    public required string UserName { get; set; }
    
    public required string LoanDate { get; set; }
    public required string ReturnDate { get; set; } 
}