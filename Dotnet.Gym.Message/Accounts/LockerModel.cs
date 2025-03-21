using Dotnet.Gym.Message.Base;
using Newtonsoft.Json;
using System;

namespace Dotnet.Gym.Message.Accounts;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 3/19/2025 6:40:53 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class LockerModel :BaseModel, ILockerModel
{
    #region - Ctors -
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
    [JsonProperty(PropertyName = "locker", Order = 0)]
    public string? Locker { get; set; } = string.Empty;
    [JsonProperty(PropertyName = "shoe_locker", Order = 1)]
    public string? ShoeLocker { get; set; } = string.Empty;
    #endregion
    #region - Attributes -
    #endregion
}