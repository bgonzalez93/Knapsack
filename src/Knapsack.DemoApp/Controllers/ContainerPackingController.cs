using System.Collections.Generic;
using Knapsack.ContainerPacking;
using Knapsack.ContainerPacking.Entities;
using Knapsack.DemoApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Knapsack.DemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerPackingController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public ActionResult<List<ContainerPackingResult>> Post([FromBody]ContainerPackingRequest request)
        {
            return PackingService.Pack(request.Containers, request.ItemsToPack, request.AlgorithmTypeIDs);
        }
    }
}