using System;
using System.Collections.Generic;
using DreamCar.Model.DataModels.Enums;

namespace DreamCar.ViewModels.VM
{
    public class AdvertThreadVm
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public IEnumerable<ImageVm> Images { get; set; }
        public IEnumerable<MessageVm> Messages { get; set; }
        public Guid AdvertId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public int UserId { get; set; }

    }
}