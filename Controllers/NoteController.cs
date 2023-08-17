using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PostIt.Controllers;

public class NoteController : Controller
{
    private List<Note> GetNoteList()
    {
        var notes = new List<Note>
        {
            new Note { ID = 1, User = 1, Message = "Hello, world!", CreatedAt = DateTime.Now.AddDays(-2), UserSent = 1 },
            new Note { ID = 2, User = 2, Message = "ASP.NET is great!", CreatedAt = DateTime.Now.AddDays(-1), UserSent = 1 },
            new Note { ID = 3, User = 1, Message = "Having a good time learning.", CreatedAt = DateTime.Now, UserSent = 1 },

            new Note { ID = 1, User = 1, Message = "Hello, world!", CreatedAt = DateTime.Now.AddDays(-2), UserSent = 1 },
            new Note { ID = 2, User = 2, Message = "ASP.NET is great!", CreatedAt = DateTime.Now.AddDays(-1), UserSent = 1 },
            new Note { ID = 3, User = 1, Message = "Having a good time learning.", CreatedAt = DateTime.Now, UserSent = 1 },

            new Note { ID = 1, User = 1, Message = "Hello, world!", CreatedAt = DateTime.Now.AddDays(-2), UserSent = 1  , ReadAt = DateTime.Now},
            new Note { ID = 2, User = 2, Message = "ASP.NET is great!", CreatedAt = DateTime.Now.AddDays(-1), UserSent = 1  , ReadAt = DateTime.Now},
            new Note { ID = 3, User = 1, Message = "Having a good time learning.", CreatedAt = DateTime.Now, UserSent = 1  , ReadAt = DateTime.Now},

            new Note { ID = 1, User = 1, Message = "Hello, world!", CreatedAt = DateTime.Now.AddDays(-2), UserSent = 1 , ReadAt = DateTime.Now },
            new Note { ID = 2, User = 2, Message = "ASP.NET is great!", CreatedAt = DateTime.Now.AddDays(-1), UserSent = 1 , ReadAt = DateTime.Now },
            new Note { ID = 3, User = 1, Message = "Having a good time learning.", CreatedAt = DateTime.Now, UserSent = 1  , ReadAt = DateTime.Now},

            new Note { ID = 1, User = 1, Message = "Hello, world!", CreatedAt = DateTime.Now.AddDays(-2), UserSent = 1 , ReadAt = DateTime.Now },
            new Note { ID = 2, User = 2, Message = "ASP.NET is great!", CreatedAt = DateTime.Now.AddDays(-1), UserSent = 1  , ReadAt = DateTime.Now},
            new Note { ID = 3, User = 1, Message = "Having a good time learning.", CreatedAt = DateTime.Now, UserSent = 1 , ReadAt = DateTime.Now}
        };
        return notes;
    }

    public IActionResult Index()
    {
        var notes = GetNoteList();
        return View("Views/Note.cshtml", notes);
    }
}

