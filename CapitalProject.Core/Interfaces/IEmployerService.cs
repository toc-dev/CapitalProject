using CapitalProject.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalProject.Core.Interfaces
{
    public interface IEmployerService
    {
        Task<DisplayCustomQuestionDTO> CreateCustomQuestion(CreateCustomQuestionDTO model);
        Task<DisplayCustomQuestionDTO> UpdateCustomQuestion(string id, UpdateCustomQuestionDTO model);
        Task DeleteCustomQuestion(string id);
    }
}
