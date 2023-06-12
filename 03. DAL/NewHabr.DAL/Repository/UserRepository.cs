﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository.Impl;
public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<User>> GetBannedUsersAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(u => u.Banned, trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByIdWithLikesAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges)
            .Include(u => u.ReceivedLikes)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<User>> GetByRoleIdAsync(int id, bool trackChanges, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<User>> GetDeletedUsersAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(u => u.Deleted, trackChanges).IgnoreQueryFilters().ToListAsync(cancellationToken);
    }

    public async Task<int> GetReceivedLikesCountAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(userId, false)
            .Include(e => e.ReceivedLikes)
            .Select(user => user.ReceivedLikes.Count)
            .SingleAsync(cancellationToken);
    }

    public async Task<User?> GetUserByLoginAsync(string login, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(u => u.UserName == login, trackChanges).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<User>> GetUsersByLoginAsync(ICollection<string> usernames, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(user => usernames.Contains(user.UserName), trackChanges)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<UserLikedUser>> GetUserLikedUsersAsync(Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetAll(trackChanges)
            .Include(user => user.ReceivedLikes)
            .Where(author => author.ReceivedLikes.Any(u => u.Id == userId))
            .Select(row => new UserLikedUser
            {
                UserId = userId, //кто поставил лайк
                Id = row.Id,     //кому поставили лайк
                UserName = row.UserName
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUsersCountWithSecureQuestionIdAsync(int id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(u => u.SecureQuestionId == id, trackChanges)
            .CountAsync(cancellationToken);
    }

    public async Task<ICollection<User>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetAll(trackChanges)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserInfo?> GetUserInfoAsync(Guid userId, Guid whoAskingId, CancellationToken cancellationToken)
    {
        return await FindByCondition(user => user.Id == userId, false)
            .Select(user => new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                Patronymic = user.Patronymic,
                LastName = user.LastName,
                Description = user.Description,
                BirthDay = user.BirthDay,
                Banned = user.Banned,
                BannedAt = user.BannedAt,
                BanReason = user.BanReason,
                BanExpiratonDate = user.BanExpiratonDate,
                ReceivedLikes = user.ReceivedLikes.Count(),
                IsLiked = Guid.Empty.Equals(whoAskingId) ? false : user.ReceivedLikes.Any(sender => sender.Id == whoAskingId)
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<User>> GetBannedUsersReadyToBeUnbannedAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        Expression<Func<User, bool>> readyToUnban = user => user.Banned
                                                            && user.BanExpiratonDate.HasValue
                                                            && user.BanExpiratonDate.Value.CompareTo(DateTimeOffset.UtcNow) < 0;
        return await FindByCondition(readyToUnban, trackChanges)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsBanned(Guid userId, CancellationToken cancellationToken)
    {
        return await FindByCondition(user => user.Id.Equals(userId), false)
            .Select(user => user.Banned)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
