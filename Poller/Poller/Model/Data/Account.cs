using System;

namespace Poller.Model.Data
{
    public class Account
    {
        public Guid Id { get; set; }
        public string SecondaryId { get; set; }
        public string Publisher { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
    }
}
