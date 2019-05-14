using Interfaces.Models;
using System.Threading.Tasks;

namespace SampleApplication.Interfaces
{
    public interface IPNotesService
    {
        Task<UserInfo> GetUserById(string LoginId);

        Task<bool> UpdateUser();
    }
}
