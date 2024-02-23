using System;
using System.Collections.Generic;

namespace Events.Data.Entities;

public partial class Agenda
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public bool IsDeleted { get; set; }
}
