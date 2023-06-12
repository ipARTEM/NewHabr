﻿#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NewHabr.Domain.Dto;

public class ArticleQueryParametersDto : QueryParameters
{
    [FromQuery(Name = QueryParametersDefinitions.FromDateTime)]
    [Range(-62135596800000, 253402300799999)]
    public long From { get; set; }

    [FromQuery(Name = QueryParametersDefinitions.ToDateTime)]
    [Range(-62135596800000, 253402300799999)]
    public long To { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    [FromQuery(Name = QueryParametersDefinitions.OrderBy)]
    [StringLength(10)]
    public string OrderBy { get; set; } = QueryParametersDefinitions.OrderingTypes.Descending;

    [FromQuery]
    public string Search { get; set; }

    [FromQuery]
    public int Category { get; set; }

    [FromQuery]
    public int Tag { get; set; }

    [FromQuery]
    public QueryParametersDefinitions.RatingOrderBy ByRating { get; set; } = QueryParametersDefinitions.RatingOrderBy.None;
}
