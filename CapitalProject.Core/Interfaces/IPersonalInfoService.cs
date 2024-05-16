using CapitalProject.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalProject.Core.Interfaces
{
    public interface IPersonalInfoService
    {
        public Task<PersonalInformationDisplayDTO> ProvidePersonalInformation(PersonalInformationDTO model);
    }
}
