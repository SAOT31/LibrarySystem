namespace LibrarySystem.Responsive;

public class UserResponse
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    
   
    public required string PenaltyStatus { get; set; } 
}