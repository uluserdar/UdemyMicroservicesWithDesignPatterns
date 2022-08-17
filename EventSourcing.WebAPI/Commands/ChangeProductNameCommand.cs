using EventSourcing.WebAPI.DTOs;
using MediatR;

namespace EventSourcing.WebAPI.Commands
{
    public class ChangeProductNameCommand:IRequest
    {
        public ChangeProductNameDto ChangeProductNameDto { get; set; }
    }
}
