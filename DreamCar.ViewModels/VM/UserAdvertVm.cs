using DreamCar.Model.DataModels.Enums;
using System;
using System.Collections.Generic;

namespace DreamCar.ViewModels.VM
{
    public class UserAdvertVm
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Localization { get; set; }
        public int Visited { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? CheckedAt { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public bool ToNegotiate { get; set; }
        public bool Brutto { get; set; }
        public bool VAT { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<ImageVm> Images { get; set; }
        public CarVm Car { get; set; }
        public IEnumerable<EquipmentVm> Equipment { get; set; }
        public UserVm User { get; set; }
    }
}
