﻿using NewHabr.Domain.Dto;
using NewHabr.Domain.ServiceModels;

namespace NewHabr.Domain.Contracts;

public interface IArticleService
{
    Task<ArticleDto> GetByIdAsync(Guid articleId, Guid whoAskingId, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetPublishedAsync(Guid whoAskingId, ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetUnpublishedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task<ArticlesGetResponse> GetDeletedAsync(ArticleQueryParametersDto queryParams, CancellationToken cancellationToken);

    Task<ArticleDto> CreateAsync(ArticleCreateRequest request, Guid creatorId, CancellationToken cancellationToken);

    Task UpdateAsync(Guid articleId, ArticleUpdateRequest articleToUpdate, CancellationToken cancellationToken);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Switch Published state(published or unpublished). If Artice is not approved, approval process starts
    /// </summary>
    Task PublishAsync(Guid id, bool publicationStatus, CancellationToken cancellationToken);

    /// <summary>
    /// Approve article if in WaitApproval state, after approval, article turns published
    /// </summary>
    Task SetApproveStateAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Disapprove article if in WaitApproval state
    /// </summary>
    Task SetDisapproveStateAsync(Guid articleId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets 'Like' mark at article
    /// </summary>
    Task SetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Unsets 'Like' mark at article
    /// </summary>
    Task UnsetLikeAsync(Guid articleId, Guid userId, CancellationToken cancellationToken);

    Task<bool> IsAuthor(Guid articleId, Guid userId, CancellationToken cancellationToken);

}
