using EventSourcing.WebAPI.DTOs;
using MediatR;

namespace EventSourcing.WebAPI.Commands
{
    public class CreateProductCommand:IRequest
    {
        public CreateProductDto CreateProductDto { get; set; }
    }
}
