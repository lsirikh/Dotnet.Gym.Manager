using Ironwall.Dotnet.Libraries.Base.Models;

namespace Dotnet.Gym.Message.Accounts;

public interface ILockerModel : IBaseModel
{
    string? Locker { get; set; }
    string? ShoeLocker { get; set; }
}