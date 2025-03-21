using Dotnet.Gym.Message.Base;
using Newtonsoft.Json;
using System;

namespace Dotnet.Gym.Message.Accounts;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 1/31/2025 1:55:08 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class ActivePeriod : BaseModel, IActivePeriod
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
    [JsonProperty(PropertyName = "start", Order = 1)]
    public DateTime? StartDate { get; set; }
    [JsonProperty(PropertyName = "end", Order = 1)]
    public DateTime? EndDate { get; set; }
    #endregion
    #region - Attributes -
    #endregion
}