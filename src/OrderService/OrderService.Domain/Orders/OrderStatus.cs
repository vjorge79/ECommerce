namespace OrderService.Domain.Orders;

public enum OrderStatus
{
    Created = 0,          // após criar pedido
    PendingPayment = 1,   // opcional, se separar
    Paid = 2,             // após PaymentAuthorized
    PaymentFailed = 3,    // após PaymentFailed
    Shipped = 4,          // após envio
    Cancelled = 5         // cancelado
}