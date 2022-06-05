using CarDirectory.Web.Controllers.Fines.Dto;
using CarDirectrory.Core.Fines;
using CarDirectrory.Core.Fines.Models;
using CarDirectrory.Core.Fines.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarDirectory.Web.Controllers.Fines;


[ApiController]
[Route("[controller]")]
public class FineController
{
    private readonly IFineService _fineService;

    public FineController(IFineService fineService)
    {
        _fineService = fineService;
    }
    
    private FineDto ConvertToFineDto(Fine fine)
    {
        return new FineDto()
        {
            Id = fine.Id,
            StateNumber = fine.StateNumber,
            Price = fine.Price,
            ReceiptDate = fine.ReceiptDate,
            IsPayed = fine.IsPayed
        };
    }

    [HttpGet]
    public async Task<IEnumerable<FineDto>> GetCarFines(string stateNumber, CancellationToken token)
    {
        return (await _fineService.GetCarFinesAsync(stateNumber, token)).Select(ConvertToFineDto);
    }

    [HttpPost]
    public Task CreateFine(CreateFineDto fine, CancellationToken token)
    {
        return _fineService.CreateFineAsync(new CreateFineModel()
        {
            StateNumber = fine.StateNumber,
            Price = fine.Price
        }, token);
    }

    [HttpPost("{id}")]
    public Task PayFine(string id, CancellationToken token)
    {
        return _fineService.PayFineAsync(id, token);
    }
}