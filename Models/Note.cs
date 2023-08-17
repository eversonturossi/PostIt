using System;

namespace PostIt.Models;

public class Note
{
    [Key]
    public int ID { get; set; }
    public int User { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ReadAt { get; set; }
    public int UserSent { get; set; }
}
