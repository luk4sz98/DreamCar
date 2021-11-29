using DreamCar.Model.DataModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class FilterVm
    {
        public BodyType? Body { get; set; }

        [RegularExpression(@"([A-Z]{1}[a-z]{1,})|([A-Z]{1,3}[a-z]{1,}|([A-Z]{3}))", ErrorMessage = "Nieprawidłowa nazwa marki samochodu")]
        [StringLength(20, ErrorMessage = "Nazwa marki musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 3)]
        public string Brand { get; set; }

        [RegularExpression(@"^[A-Za-z0-9\s{0,1}]+$", ErrorMessage = "Niepoprwany model samochodu")]
        [StringLength(20, ErrorMessage = "Model samochodu musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 3)]
        public string Model { get; set; }

        [StringLength(20, ErrorMessage = "Generacja samochodu nie może być dłuższa niż {1} oraz krótsza niż {2}", MinimumLength = 1)]
        [RegularExpression(@"^[A-Za-z0-9-{0,1}]+$", ErrorMessage = "To pole nie może zawierać znaków specjalnych")]
        public string Generation { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinProductionYear { get; set; }
        public int? MaxProductionYear { get; set; }
        public FuelType? Fuel { get; set; }
        public int? MinMileage { get; set; }
        public int? MaxMileage { get; set; }
    }
}
