﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewHabr.Domain.Dto;

public class UserForManipulationDto
{
    [MaxLength(30)]
    public string? FirstName { get; set; }

    [MaxLength(30)]
    public string? LastName { get; set; }

    [MaxLength(30)]
    public string? Patronymic { get; set; }

    [Range(-62135596800000, 253402300799999)]
    public long? BirthDay { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }
}
