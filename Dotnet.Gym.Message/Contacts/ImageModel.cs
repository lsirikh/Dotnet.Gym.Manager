using Dotnet.Gym.Message.Base;
using Newtonsoft.Json;
using System;

namespace Dotnet.Gym.Message.Contacts;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/3/2025 5:04:12 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/

public class ImageModel : BaseModel, IImageModel
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

    [JsonProperty("", Order = 1)]
    public int EmsMessageId {  get; set; }
    [JsonProperty("base64_image", Order = 1)]
    public string Base64Image { get; set; } = string.Empty;

    [JsonProperty("file_name", Order = 2)]
    public string FileName { get; set; } = string.Empty;

    [JsonProperty("content_type", Order = 3)]
    public string ContentType { get; set; } = string.Empty;
    #endregion
    #region - Attributes -
    #endregion
}