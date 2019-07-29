using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FKPI_API.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FKPI_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // by default retrieve root accounts
            var accounts = (await unitOfWork.AccountRepository.GetAsync(x => !x.ParentAccountId.HasValue)).ToList();

            return Ok(accounts);
        }

        [HttpGet("children")]
        public async Task<IActionResult> Children(int? id)
        {
            var accounts = (await unitOfWork.AccountRepository.ChildrenAsync(id))
                .Select(x => new
                {
                    id = x.AccountId,
                    text = x.Name,
                    parentId = x.ParentAccountId
                })
                .ToList();

            return Ok(accounts);
        }
    }
}
