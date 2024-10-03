using Customer.Application.Features.Commands.CreateCustomer;
using Customer.Application.Features.Queries;
using Customer.Application.Features.Queries.GetCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
    {
    [Route("api/customers")]
    [ApiController]
    public class CustomersController(IMediator mediator) : ControllerBase
        {
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            await mediator.Send(command);
            
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetCustomerById(Guid id)
        {
            var result = await mediator.Send(new CustomerQuery(id));
            
            return Ok(result);
        }
        }
    }