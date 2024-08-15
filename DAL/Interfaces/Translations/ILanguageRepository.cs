using System;
using System.Threading.Tasks;
using Base.Interfaces;

namespace DAL.Interfaces.Translations;

public interface ILanguageRepository : IEntityRepository<DTO.Translations.Language>
{
    public Task<Guid?> GetLanguageIdAsync(string language);
}