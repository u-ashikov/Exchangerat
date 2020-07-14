namespace Exchangerat.Requests.Services.Contracts.RequestTypes
{
    using Exchangerat.Requests.Models.Models.RequestTypes;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequestTypeService
    {
        Task<Result<IEnumerable<RequestTypeBaseInfoModel>>> GetAll();
    }
}
