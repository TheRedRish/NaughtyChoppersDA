using Microsoft.AspNetCore.Mvc.RazorPages;
using NaughtyChoppersDA.Globals.Utils;

namespace NaughtyChoppersDA.Entities
{
    public class Profile
    {
        private int? _age;

        public Guid? ProfileId { get; set; }
        public string? Name { get; set; }
        public DateOnly? DateOfBirth
        {
            get => DateOfBirth;
            set
            {
                DateOfBirth = value;
                if (DateOfBirth != null)
                {
                    _age = DateUtils.CalculateAge((DateOnly)DateOfBirth);
                }
            }
        }
        public int? Age { get => _age; set => _age = value; }
        public HelicopterModel? Model { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public List<HobbyInterest>? Interests { get; set; }
        public List<HelicopterModel>? HelicopterModelInterests { get; set; }
    }
}