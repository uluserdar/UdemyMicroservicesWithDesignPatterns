using EventSourcing.Shared.Events;
using EventSourcing.WebAPI.DTOs;
using EventStore.ClientAPI;
using System;

namespace EventSourcing.WebAPI.EventStores
{
    public class ProductStream : AbstractStream
    {
        public static string streamName => "ProductStream";
        public static string groupName => "replay";

        public ProductStream(IEventStoreConnection eventStoreConnection) : base(streamName, eventStoreConnection)
        {
        }

        public void Created(CreateProductDto createProductDto)
        {
            Events.AddLast(new ProductCreatedEvent
            {
                Id = Guid.NewGuid(),
                Name=createProductDto.Name,
                Price=createProductDto.Price,
                Stock=createProductDto.Stock,
                UserId=createProductDto.UserId
            });
        }

        public void NameChanged(ChangeProductNameDto changeProductNameDto)
        {
            Events.AddLast(new ProductNameChangedEvent
            {
                Id=changeProductNameDto.Id,
                ChangedName=changeProductNameDto.Name
            });
        }

        public void PriceChanged(ChangeProductPriceDto changeProductPriceDto)
        {
            Events.AddLast(new ProductPriceChangedEvent
            {
                Id = changeProductPriceDto.Id,
                ChangedPrice = changeProductPriceDto.Price
            });
        }

        public void Deleted(Guid id)
        {
            Events.AddLast(new ProductDeletedEvent
            {
                Id = id
            });
        }
    }
}
