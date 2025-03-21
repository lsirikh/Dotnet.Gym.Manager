using Dotnet.Gym.Message.Base;
using Dotnet.Gym.Message.Enums;
using Ironwall.Dotnet.Libraries.Base.Models;

namespace Dotnet.Gym.Message.Accounts;
public interface IUserModel : IBaseModel
{
    public int Age { get; set; }
    public EnumGenderType Gender { get; set; }
    public EnumTrueFalse IsActive { get; set; }
    public string MobilePhone { get; set; }
    public DateTime RegisterDate { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ActivePeriod? ActivePeriod { get; set; }
    public LockerModel? Locker { get; set; }
}