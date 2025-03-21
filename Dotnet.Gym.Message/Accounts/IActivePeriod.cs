
using Ironwall.Dotnet.Libraries.Base.Models;

namespace Dotnet.Gym.Message.Accounts;

public interface IActivePeriod : IBaseModel
{
    DateTime? StartDate { get; set; }
    DateTime? EndDate { get; set; }
}