﻿namespace OMS.Domain.Aggregates.OrderAggregate
{
    enum OrderStatus
    {
        ReadyToBeFulfilled,
        PaymentProceesed,
        Fulfilled
    }
}