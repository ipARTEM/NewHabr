﻿using System;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public class SecureQuestionsRepository : RepositoryBase<SecureQuestion, int>, ISecureQuestionsRepository
{
    public SecureQuestionsRepository(ApplicationContext context)
        : base(context)
    {
    }

    public async Task<ICollection<SecureQuestion>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetAll(trackChanges)
            .ToListAsync(cancellationToken);
    }

    public async Task<SecureQuestion?> GetByIdAsync(int id, bool trackChanges, CancellationToken cancellationToken)
    {
        return await GetById(id, trackChanges)
            .FirstOrDefaultAsync();
    }
}
