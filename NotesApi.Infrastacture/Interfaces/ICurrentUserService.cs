using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastacture.Interfaces
{
    public interface ICurrentUserService
    {
        string Email { get; }
        int? Id { get; }
        string Role { get; }
    }
}
