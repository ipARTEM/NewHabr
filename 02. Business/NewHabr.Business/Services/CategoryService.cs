﻿using AutoMapper;
using NewHabr.Domain.Contracts;
using NewHabr.Domain.Dto;
using NewHabr.Domain.Exceptions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;


    public CategoryService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }


    public async Task<CategoryDto> GetByIdAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _repositoryManager
            .CategoryRepository
            .GetByIdAsync(categoryId, false, cancellationToken);

        if (category is null)
            throw new CategoryNotFoundException(categoryId);

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _repositoryManager.CategoryRepository.GetAvaliableAsync(false, cancellationToken);
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken)
    {
        var category = await _repositoryManager
            .CategoryRepository
            .GetByNameAsync(request.Name, false, cancellationToken);

        if (category is not null)
        {
            throw new CategoryAlreadyExistsException(request.Name);
        }

        var newCategory = _mapper.Map<Category>(request);
        _repositoryManager.CategoryRepository.Create(newCategory);
        await _repositoryManager.SaveAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(newCategory);
    }

    public async Task UpdateAsync(int id, CategoryUpdateRequest categoryToUpdate, CancellationToken cancellationToken)
    {
        var targetCategory = await _repositoryManager.CategoryRepository.GetByIdAsync(id, trackChanges: true, cancellationToken);

        if (targetCategory is null)
        {
            throw new CategoryNotFoundException(id);
        }

        var categoryWithSameName = await _repositoryManager
            .CategoryRepository
            .GetByNameAsync(categoryToUpdate.Name, false, cancellationToken);

        if (categoryWithSameName is not null)
        {
            throw new CategoryAlreadyExistsException(categoryToUpdate.Name);
        }

        _mapper.Map(categoryToUpdate, targetCategory);

        await _repositoryManager.SaveAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _repositoryManager.CategoryRepository.GetByIdIncludeAsync(id, true, cancellationToken);

        if (category is null)
        {
            throw new CategoryNotFoundException(id);
        }

        _repositoryManager.CategoryRepository.Delete(category);
        await _repositoryManager.SaveAsync(cancellationToken);
    }
}
