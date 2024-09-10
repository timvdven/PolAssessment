using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Models.Dto;
using PolAssessment.Common.Lib.Models;

namespace PolAssessment.AnprWebApi.Services;

public interface IAnprQueryService
{
    Task<IEnumerable<AnprRecord>> GetAnprRecords(AnprRequest anprRequest);
}

public class AnprQueryService(IAnprDataDbContext anprDataDbContext) : IAnprQueryService
{
    private readonly IAnprDataDbContext _dbContext = anprDataDbContext;

    public async Task<IEnumerable<AnprRecord>> GetAnprRecords(AnprRequest anprRequest)
    {
        var queryable = GetAnprRecordsQuery(anprRequest);

        var page = anprRequest.Page ?? default;
        var pageSize = anprRequest.PageSize ?? int.MaxValue;

        var anprRecords = await queryable
            .Where(x => 
                (!anprRequest.StartDate.HasValue || x.ExactDateTime >= anprRequest.StartDate.Value) &&
                (!anprRequest.EndDate.HasValue || x.ExactDateTime <= anprRequest.EndDate.Value) &&
                (string.IsNullOrWhiteSpace(anprRequest.Plate) || x.LicensePlate == anprRequest.Plate))
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        return anprRecords;
    }

    private IQueryable<AnprRecord> GetAnprRecordsQuery(AnprRequest anprRequest)
    {
        if (anprRequest.MinimumUploadDate.HasValue)
        {
            return _dbContext.AnprRecords
                .Join(_dbContext.AnprRecordUploadUsers, x => x.Id, y => y.AnprRecordId, (x, y) => new { x, y.UploadDateTime })
                .Where(x => x.UploadDateTime >= anprRequest.MinimumUploadDate)
                .Select(x => x.x);
        }
        else
        {
            return _dbContext.AnprRecords;
        }
    }
}