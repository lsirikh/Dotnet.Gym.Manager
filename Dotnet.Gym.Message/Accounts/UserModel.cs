using Dotnet.Gym.Message.Base;
using Dotnet.Gym.Message.Enums;
using Newtonsoft.Json;
using System;

namespace Dotnet.Gym.Message.Accounts;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 1/31/2025 1:45:47 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class UserModel : BaseModel, IUserModel
{
    #region - Ctors -
    public UserModel()
    {
    }
    public UserModel(int id, string name, string phone, int age, EnumGenderType gender, ActivePeriod activePeriod, LockerModel locker, DateTime register) : base(id)
    {
        UserName = name;
        MobilePhone = phone;
        Age = age;
        Gender = gender;
        ActivePeriod = activePeriod;
        Locker = locker;
        RegisterDate = register;
    }
    public UserModel(IUserModel model): base(model) 
    {
        UserName = model.UserName;
        MobilePhone = model.MobilePhone;
        Age = model.Age;
        Gender = model.Gender;
        ActivePeriod = model.ActivePeriod;
        Locker = model.Locker;
        RegisterDate = model.RegisterDate;
    }
    #endregion
    #region - Implementation of Interface -
    #endregion
    #region - Overrides -
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    [JsonProperty(PropertyName ="user_name", Order = 1)]
    public string UserName { get; set; } = string.Empty;

    [JsonProperty(PropertyName ="mobile", Order = 2)]
    public string MobilePhone { get; set; } = string.Empty;

    [JsonProperty(PropertyName ="age", Order = 3)]
    public int Age { get; set; }

    [JsonProperty(PropertyName ="gender", Order = 4)]
    public EnumGenderType Gender { get; set; }

    [JsonProperty(PropertyName = "register_date", Order = 5)]
    public DateTime RegisterDate { get; set; } = DateTime.Today;

    [JsonProperty(PropertyName ="is_active", Order = 6)]
    public EnumTrueFalse IsActive { get; set; } = EnumTrueFalse.True;

    [JsonProperty(PropertyName ="active_period", Order = 7)]
    public ActivePeriod? ActivePeriod { get; set; }

    [JsonProperty(PropertyName = "locker", Order = 8)]
    public LockerModel? Locker { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Today;
    public DateTime UpdatedAt { get; set; }
    #endregion
    #region - Attributes -
    #endregion
}