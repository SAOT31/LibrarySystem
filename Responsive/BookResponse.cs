namespace LibrarySystem.Responsive;

public class BookResponse
{
   
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Author { get; set; }
    
  
    public required string IsAvailable { get; set; }
}