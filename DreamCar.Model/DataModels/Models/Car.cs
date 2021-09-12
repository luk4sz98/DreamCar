using DreamCar.Model.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.Model.DataModels
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        /*
         * <summary> Fields describing the basic details of the vehicle </summary>
         */
        #region Vechicle Details
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public string Color { get; set; }
        public short ProductionYear { get; set; }
        public int Mileage { get; set; }
        public string VIN { get; set; }
        public byte Doors { get; set; }
        public byte Seats { get; set; }
        public string OriginCountry { get; set; }
        public bool IsImported { get; set; }
        public DateTime FirstRegistration { get; set; }
        public bool RegisterdInPoland { get; set; }
        public bool FirstOwner { get; set; }
        public bool ASOServiced { get; set; }
        public bool AccidentFree { get; set; }
        public decimal Price { get; set; }
        public short EngineCapacity { get; set; }
        public short Power { get; set; }
        public bool IsDamaged { get; set; }
        public bool IsRighthandDrive { get; set; }
        public FuelType Fuel { get; set; }
        public GearboxType Gearbox { get; set; }
        public ColorType ColorType { get; set; }
        public BodyType Body { get; set; }
        public DriveType Drive { get; set; }
        #endregion


        public virtual Advert Advert { get; set; }
        public virtual IEnumerable<CarEquipment> CarEquipment { get; set; }

    }
}
