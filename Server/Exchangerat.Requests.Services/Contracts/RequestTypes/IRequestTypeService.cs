namespace Exchangerat.Requests.Services.Contracts.RequestTypes
{
    using Infrastructure;
    using Exchangerat.Requests.Models.RequestTypes;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequestTypeService
    {
        Task<Result<IEnumerable<RequestTypeBaseInfoModel>>> GetAll();
    }
}
