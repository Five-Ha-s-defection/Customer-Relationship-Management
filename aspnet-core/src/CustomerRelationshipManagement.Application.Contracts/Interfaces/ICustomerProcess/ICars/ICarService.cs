using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Cars;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICars
{
    public interface ICarService:IApplicationService
    {
        /// <summary>
        /// 添加车架号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<CarDto>> AddCar(CreateUpdateCarDto dto);
    }
}
