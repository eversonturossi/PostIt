using System;

namespace PostIt.Models;

public class Note
{
    public int ID { get; set; }
    public int User { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public int UserSent { get; set; }
}
