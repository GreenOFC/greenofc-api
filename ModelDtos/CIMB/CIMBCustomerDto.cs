using Newtonsoft.Json;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBCustomerDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("customerType")]
        public string CustomerType { get; private set; } = "NORMAL";

        [JsonProperty("nationalId")]
        public string NationalId { get; set; }

        [JsonProperty("previousIdNumber")]
        public string PreviousIdNumber { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("maritalStatus")]
        public string MaritalStatus { get; set; }

        [JsonProperty("education")]
        public string Education { get; set; }

        [JsonProperty("usCitizen")]
        public bool UsCitizen { get; private set; } = false;

        [JsonProperty("usResident")]
        public bool UsResident { get; private set; } = false;

        [JsonProperty("fatcaDeclaration")]
        public bool FatcaDeclaration { get; private set; } = false;

        [JsonProperty("hasRelatedPersonAtCimb")]
        public bool HasRelatedPersonAtCimb { get; private set; } = false;

        [JsonProperty("residentOfVietNamStatus")]
        public bool ResidentOfVietNamStatus { get; private set; } = true;

        [JsonProperty("currentAddress")]
        public CIMBCustomerAddressDto CurrentAddress { get; set; }

        [JsonProperty("contacts")]
        public IEnumerable<CIMBCustomerContactDto> Contacts { get; set; }
    }
}
