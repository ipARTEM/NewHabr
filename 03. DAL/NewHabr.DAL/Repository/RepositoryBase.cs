﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewHabr.DAL.EF;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Models;

namespace NewHabr.DAL.Repository;

public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : struct
{
    private readonly ApplicationContext _context;
    protected DbSet<TEntity> Set;

    public RepositoryBase(ApplicationContext context)
    {
        _context = context;
        Set = _context.Set<TEntity>();
    }

    public void Create(TEntity data)
    {
        Set.Add(data);
    }

    public virtual void Delete(TEntity data)
    {
        data.Deleted = true;
        Update(data);
    }

    public void Update(TEntity data)
    {
        Set.Update(data);
    }

    protected IQueryable<TEntity> FindAll(bool trackChanges)
    {
        return trackChanges ? Set : Set.AsNoTracking();
    }

    protected IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges)
    {
        return FindAll(trackChanges).Where(expression);
    }

    protected IQueryable<TEntity> GetById(TKey id, bool trackChanges)
    {
        return FindByCondition(e => e.Id.Equals(id) && !e.Deleted, trackChanges);
    }

    protected IQueryable<TEntity> GetAll(bool trackChanges)
    {
        return FindAll(trackChanges);
    }

    protected IQueryable<TEntity> GetDeleted(bool trackChanges)
    {
        return FindByCondition(a => a.Deleted, trackChanges);
    }

    protected IQueryable<TEntity> GetAvailable(bool trackChanges)
    {
        return FindByCondition(a => !a.Deleted, trackChanges);
    }
}
