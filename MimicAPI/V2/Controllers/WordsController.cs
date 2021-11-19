using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V2.Controllers
{

    // -- /api/v2.0/words

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class WordsController : ControllerBase
    {
        [HttpGet("", Name = "GetAll")]
        public string GetAll()
        {

            return "Version 2.0.0 - IN DEVELOPMENT";
        }
    }
}
