using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services;

public class AttachmentService : IAttachmentService
{
    private readonly IRepository<Attachment> _repository;

    public AttachmentService(IRepository<Attachment> repository)
    {
        _repository = repository;
    }

    public async ValueTask<bool> DeleteAsync(Expression<Func<Attachment, bool>> expression)
    {
        var file = await _repository.GetAsync(expression, null);

        if (file is null)
            throw new ZaminEducationException(400, "Attachment not found");

        FileHelper.Remove(file.Path);

        file.Delete();

        _repository.Delete(file);

        await _repository.SaveChangesAsync();

        return true;
    }

    public async ValueTask<Attachment> DownloadAsync(AttachmentForCreationDto dto)
    {
        var (fileName, filePath) = await FileHelper.SaveAsync(dto);

        var newAttachement = new Attachment()
        {
            Name = fileName,
            Path = filePath,
        };

        newAttachement.Create();

        newAttachement = await _repository.AddAsync(newAttachement);
        await _repository.SaveChangesAsync();

        return newAttachement;
    }

    public async ValueTask<Attachment> GetAsync(Expression<Func<Attachment, bool>> expression)
    {
        var existAttachement = await _repository.GetAsync(expression, null);

        return existAttachement is null 
            ? throw new ZaminEducationException(404, "Attachment not found.") 
            : existAttachement;
    }

    public async ValueTask<Attachment> UpdateAsync(long id, Stream stream)
    {
        var existAttachment = await _repository.GetAsync(a => a.Id == id, null);

        if (existAttachment is null)
            throw new ZaminEducationException(400, "Attachment not found.");

        await FileHelper.SaveAsync(new()
        {
            FileName = existAttachment.Name,
            Stream = stream,
        },
        true);

        existAttachment.Update();

        existAttachment = _repository.Update(existAttachment);
        await _repository.SaveChangesAsync();

        return existAttachment;
    }
}
