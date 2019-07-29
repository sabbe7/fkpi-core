using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FKPI_API.DAL;
using FKPI_API.Dtos;
using FKPI_API.Helpers;
using FKPI_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FKPI_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class KPIController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public KPIController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var kpis = (await unitOfWork.KPIRepository.GetAsync()).OrderBy(x => x.Name).ToList();

            return Ok(kpis);
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(int id)
        {
            var kpi = await unitOfWork.KPIRepository.FindAsync(id);

            var list = kpi.Formula.Split(';');

            var tokens = new List<TokenDto>();

            // generate token list form KPI list and for each account id retrieve the account name
            foreach (var item in list)
            {
                if (Helper.IsOperator(item))
                {
                    var operatorToken = new TokenDto()
                    {
                        Name = item,
                        Id = Helper.GenerateId() // id needed for displaying operators correctly
                    };

                    tokens.Add(operatorToken);
                }
                else
                {
                    var account = await unitOfWork.AccountRepository.FindAsync(Int32.Parse(item));

                    var accountToken = new TokenDto()
                    {
                        Name = account.Name,
                        Id = account.AccountId.ToString()
                    };

                    tokens.Add(accountToken);
                }
            }

            var formatted = new
            {
                name = kpi.Name,
                tokens = tokens
            };

            return Ok(formatted);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] KPI input)
        {
            unitOfWork.KPIRepository.AddKPI(input);

            await unitOfWork.SaveAsync();

            return Ok(input);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] KPI input)
        {
            unitOfWork.KPIRepository.UpdateKPI(input);

            await unitOfWork.SaveAsync();

            return Ok(input);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var kpi = await unitOfWork.KPIRepository.FindAsync(id);

            unitOfWork.KPIRepository.DeleteKPI(kpi);

            await unitOfWork.SaveAsync();

            return Ok(kpi);
        }

        [HttpGet("evaluate")]
        public async Task<IActionResult> Evaluate(int id)
        {
            var kpi = await unitOfWork.KPIRepository.FindAsync(id);

            var values = unitOfWork.KPIRepository.Evaluate(kpi);

            return Ok(new
            {
                name = kpi.Name,
                values = values
            });
        }
    }
}
