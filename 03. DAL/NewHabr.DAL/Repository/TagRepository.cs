﻿using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class TagRepository : RepositoryBase<Tag, int>, ITagRepository
{
    public TagRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<Tag?> GetByIdIncludeAsync(int id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges)
            .Include(c => c.Articles)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Tag?> GetByIdAsync(int id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<IReadOnlyCollection<Tag>> GetAvaliableAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetAvailable(trackChanges).ToListAsync(cancellationToken);
    }
    public override void Delete(Tag tag) => Set.Remove(tag);

    public async Task<Tag?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(q => q.Name == name, trackChanges)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
