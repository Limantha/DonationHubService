using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _transactionService.GetPagedAsync(
                    pageNumber,
                    pageSize,
                    cancellationToken);

                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create(
            [FromBody] CreateTransactionRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _transactionService.CreateAsync(request, cancellationToken);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }
    }
}
