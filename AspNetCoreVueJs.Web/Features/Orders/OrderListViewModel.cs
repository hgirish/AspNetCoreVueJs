using System;

namespace AspNetCoreVueJs.Web.Features.Orders
{
    internal class OrderListViewModel
    {
        public int Id { get; set; }
        public DateTime Placed { get; set; }
        public int Items { get; set; }
        public decimal Total { get; set; }
        public string PaymentStatus { get; set; }
        public string Customer { get; internal set; }
    }
}