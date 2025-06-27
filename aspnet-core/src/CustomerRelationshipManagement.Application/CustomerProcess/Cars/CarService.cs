using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Cars;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICars;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CustomerProcess.Cars
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class CarService: ApplicationService,ICarService
    {
        private readonly IRepository<CarFrameNumber> cRep;

        public CarService(IRepository<CarFrameNumber> cRep)
        {
            this.cRep = cRep;
        }

        /// <summary>
        /// 添加车架号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CarDto>> AddCar(CreateUpdateCarDto dto)
        {
            try
            {
                var car = ObjectMapper.Map<CreateUpdateCarDto, CarFrameNumber>(dto);
                var list = await cRep.InsertAsync(car);
                return ApiResult<CarDto>.Success(ResultCode.Success, ObjectMapper.Map<CarFrameNumber, CarDto>(list));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
