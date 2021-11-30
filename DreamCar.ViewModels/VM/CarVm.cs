using DreamCar.Model.DataModels.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.VM
{
    public class CarVm
    {
        public int? Id { get; set; }
        public bool IsImported { get; set; }
        public bool IsDamaged { get; set; }
        public bool IsRightHandDrive { get; set; }
        public bool RegisterdInPoland { get; set; }
        public bool FirstOwner { get; set; }
        public bool ASOServiced { get; set; }
        public bool AccidentFree { get; set; }
        public bool DPF { get; set; }       
       
        public byte Seats { get; set; }
        public string OriginCountry { get; set; }
        public string DayGuaranteePeriod { get; set; }

        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "Miesiąc może zawierac wyłącznie litery")]
        public string MonthGuaranteePeriod { get; set; }
        public string YearGuaranteePeriod { get; set; }
        public DriveType Drive { get; set; }
        
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "To pole może zawierać jedynie cyfry")]
        [StringLength(6, ErrorMessage = "To pole może zawierać co najmniej {2} oraz maksymalnie {1} cyfr", MinimumLength = 4)]
        public string GuaranteePeriodMileage { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Nieprawidłowy znak, dozwolone jedynie cyfry")]
        [StringLength(maximumLength: 4, ErrorMessage = "To poleże może zawierać co najmniej {2} oraz maksymalnie {1} cyfr", MinimumLength = 1)]
        public string CO2Emission { get; set; }

        [Required(ErrorMessage = "Podanie numeru VIN pojazdu jest obowiązkowe")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 17, ErrorMessage = "Numer VIN musi zawierać 17 znaków", MinimumLength = 17)]
        [RegularExpression(@"^[a-zA-Z0-9]{9}[a-zA-Z0-9-]{2}[0-9]{6}$", ErrorMessage = "Nr VIN nie może zawierać specjalnych znaków, małych liter")]
        public string VIN { get; set; }

        [RegularExpression(@"^[A-Z0-9\s{0,1}]+$", ErrorMessage = "Nr rejestracyjny pojazdu może zawierać tylko cyfry i wielkie litery")]
        public string RegistrationNumber { get; set; }

        [DataType(DataType.Date, ErrorMessage = "W formacie daty")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{DD/MM/YYYY}")]
        public DateTime? FirstRegistration{ get; set; }

        [Required(ErrorMessage = "Data produkcji samochodu jest wymagana")]
        public short ProductionYear { get; set; }

        [Required(ErrorMessage = "Marka samochodu jest wymagana")]
        [RegularExpression(@"([A-Z]{1}[a-z]{1,})|([A-Z]{1,3}[a-z]{1,}|([A-Z]{3}))", ErrorMessage = "Nieprawidłowa nazwa marki samochodu")]
        [StringLength(20, ErrorMessage = "Nazwa marki musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 3)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model samochodu jest wymagany")]
        [RegularExpression(@"^[A-Za-z0-9\s{0,1}]+$", ErrorMessage = "Niepoprwany model samochodu")]
        [StringLength(20, ErrorMessage = "Model samochodu musi zawierać co najmniej {2} oraz maksymalnie {1} znaków", MinimumLength = 3)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Musisz wybrać rodzaj paliwa")]
        public FuelType Fuel { get; set; }

        [Required(ErrorMessage = "Musisz podać moc samochodu")]
        [StringLength(4, ErrorMessage = "Moc silnika musi zawierać co najmniej {2} oraz maksymalnie {1} cyfry", MinimumLength = 2)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Moc może zawierać wyłącznie cyfry")]
        public string Power { get; set; }


        [Required(ErrorMessage = "Musisz podać pojemność silnika samochodu")]
        [StringLength(4, ErrorMessage = "Pojemność silnika musi zawierać co najmniej {2} oraz maksymalnie {1} cyfry", MinimumLength = 3)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Moc może zawierać wyłącznie cyfry")]
        public string EngineCapacity { get; set; }

        [Required(ErrorMessage = "Musisz wybrać ilość drzwi")]
        public byte Doors { get; set; }

        [Required(ErrorMessage = "Musisz wybrać typ skrzyni biegów")]
        public GearboxType Gearbox { get; set; }

        [Required(ErrorMessage = "Musisz podać wersje auta, wpisz '-' gdy nie wiesz jaka to wersja")]
        [StringLength(20, ErrorMessage = "Wersja samochodu nie może być dłuższa niż {1} oraz krótsza niż {2}", MinimumLength = 1)]
        [RegularExpression(@"^[A-Za-z0-9()\s{0,1}\.-]+$", ErrorMessage = "To pole nie może zawierać znaków specjalnych")]
        public string Version { get; set; }

        [Required(ErrorMessage = "Musisz podać generacje auta, wpisz '-' gdy nie wiesz jaka to generacja")]
        [StringLength(20, ErrorMessage = "Generacja samochodu nie może być dłuższa niż {1} oraz krótsza niż {2}", MinimumLength = 1)]
        [RegularExpression(@"^[A-Za-z0-9-{0,1}\.-]+$", ErrorMessage = "To pole nie może zawierać znaków specjalnych")]
        public string Generation { get; set; }

        [Required(ErrorMessage = "Musisz wybrać segment samochodu")]
        public BodyType Body { get; set; }

        [Required(ErrorMessage = "Musisz podać kolor samochodu")]
        [StringLength(30, ErrorMessage = "Kolor samochodu musi zawierać co najmniej {2} oraz maksymalnie {1} liter", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "Nazwa koloru może zawierac wyłącznie litery")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Musisz wybrać typ lakieru")]
        public ColorType ColorType { get; set; }

        [Required(ErrorMessage = "Musisz podać przebieg auta")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "To pole może zawierac wyłącznie cyfry")]
        [StringLength(7, ErrorMessage = "Maksymalna ilość cyfr to {1}")]
        public string Mileage { get; set; }
    }
}
