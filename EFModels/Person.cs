using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SignalrDemo.EFModels;

[Table("person")]
public partial class Person
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "character varying")]
    public string? Name { get; set; }

    [Column(TypeName = "character varying")]
    public string? Username { get; set; }

    [Column(TypeName = "character varying")]
    public string? Password { get; set; }
}
