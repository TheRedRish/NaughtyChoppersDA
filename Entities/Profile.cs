using Microsoft.AspNetCore.Mvc.RazorPages;
using NaughtyChoppersDA.Globals.Utils;
using NaughtyChoppersDA.Repositories;

namespace NaughtyChoppersDA.Entities
{
    public class Profile
    {
        private int? _age;
        private DateTime? _dateOfBirth;

        public Guid? ProfileId { get; set; }
        public string? Name { get; set; }
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                if (value != null)
                {
                    _age = DateUtils.CalculateAge((DateTime)value);
                }

            }
        }
        public int? Age { get => _age; set => _age = value; }
        public HelicopterModel Model { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public List<HobbyInterest> HobbyInterests { get; set; }
        public List<HelicopterModel> HelicopterModelInterests { get; set; }
    }
}