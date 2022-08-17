using EventSourcing.WebAPI.DTOs;
using MediatR;

namespace EventSourcing.WebAPI.Commands
{
    public class ChangeProductPriceCommand:IRequest
    {
        public ChangeProductPriceDto ChangeProductPriceDto { get; set; }
    }
}
