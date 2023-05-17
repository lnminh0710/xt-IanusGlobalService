using System;
using System.Text.Json;
using System.Threading.Tasks;
using IanusGlobalServiceApi.Models;
using IanusGlobalServiceApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace IanusGlobalServiceApi.Controllers
{
    [ApiController]
    [Route("UniqueService")]
    public class UniqueServiceController : ControllerBase
    {
        private readonly IDalAdoService _dalAdoService;
        public UniqueServiceController(IDalAdoService dalAdoService)
        {
            _dalAdoService = dalAdoService;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return $"UniqueService API: {DateTime.Now.ToString()}";
        }

        /// <summary>
        /// Call GlobalService
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Post([FromBody] UniqueServiceModel model)
        {
            var result = _dalAdoService.GlobalService(model.Request.Data);
            return await Task.FromResult(new
            {
                Data = JsonSerializer.Serialize(result)
            });
        }
    }
}
