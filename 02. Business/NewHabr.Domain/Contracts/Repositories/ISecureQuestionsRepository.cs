﻿using System;
using NewHabr.Domain.Models;

namespace NewHabr.Domain.Contracts;

public interface ISecureQuestionsRepository : IRepository<SecureQuestion, int>
{
    Task<ICollection<SecureQuestion>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken);
    Task<SecureQuestion?> GetByIdAsync(int id, bool trackChanges, CancellationToken cancellationToken);
}

