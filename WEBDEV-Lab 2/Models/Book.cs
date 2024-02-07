using System.ComponentModel.DataAnnotations;

namespace WEBDEV_Lab_2.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Review { get; set; }
}
