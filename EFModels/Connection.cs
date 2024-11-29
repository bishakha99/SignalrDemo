using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SignalrDemo.EFModels;

[Table("connections")]
public partial class Connection
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("person_id")]
    public Guid? PersonId { get; set; }

    [Column("signalr_id", TypeName = "character varying")]
    public string? SignalrId { get; set; }

    [Column("time_stamp")]
    public TimeOnly? TimeStamp { get; set; }
}
