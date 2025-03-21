using Ironwall.Dotnet.Libraries.Base.Models;
using Newtonsoft.Json;
using System;

namespace Dotnet.Gym.Message.Base;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 1/31/2025 1:43:35 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class BaseModel : IBaseModel
{
    #region - Ctors -
    public BaseModel()
    {
        
    }
    public BaseModel(int id)
    {
        Id = id;
    }

    public BaseModel(IBaseModel model)
    {
        Id = model.Id;
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
    [JsonProperty(PropertyName = "id", Order = 0)]
    public int Id { get; set; }
    #endregion
    #region - Attributes -
    #endregion
}