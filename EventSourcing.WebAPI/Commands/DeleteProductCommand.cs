using MediatR;
using System;

namespace EventSourcing.WebAPI.Commands
{
    public class DeleteProductCommand:IRequest
    {
        public Guid Id { get; set; }
    }
}
