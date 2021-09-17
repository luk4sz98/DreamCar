using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DreamCar.ViewModels.VM
{
    public class CheckedEquVm
    {
        [BindProperty]
        public IEnumerable<int> Checked { get; set; }
    }
}
