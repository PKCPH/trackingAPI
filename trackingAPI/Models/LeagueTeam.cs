﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace trackingAPI.Models;

public class LeagueTeam
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [ForeignKey("League")]
    public Guid LeagueId { get; set; }
    [ForeignKey("Team")]
    public Guid TeamId { get; set; }
    [DefaultValue(true)]
    public bool? InTournament { get; set; }
}


