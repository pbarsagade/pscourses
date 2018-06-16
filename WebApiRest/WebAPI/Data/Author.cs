﻿using System;
using System.Collections.Generic;

namespace WebAPI.Data
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Genre { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
