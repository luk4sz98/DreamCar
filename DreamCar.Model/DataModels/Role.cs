using DreamCar.Model.DataModels.Enums;
using Microsoft.AspNetCore.Identity;

namespace DreamCar.Model.DataModels
{
    public class Role : IdentityRole<int>
    {
        public RoleType RoleType { get; set; }
        public Role()
        {

        }

        public Role(string name, RoleType roleType) : base(name)
        {
            RoleType = roleType;
        }
    }
}
