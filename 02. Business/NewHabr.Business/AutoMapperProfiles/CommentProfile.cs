﻿using AutoMapper;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Models;

namespace NewHabr.Business.AutoMapperProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ReverseMap();

        CreateMap<CommentCreateRequest, Comment>();
        CreateMap<CommentUpdateRequest, Comment>();

        CreateMap<CommentModel, CommentDto>();
    }
}
