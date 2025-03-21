using Dotnet.Gym.Message.Accounts;
using Dotnet.Gym.Message.Base;
using Dotnet.Gym.Message.Enums;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices.JavaScript;

namespace Dotnet.Gym.Message.Contacts;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/3/2025 3:37:24 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class EmsMessageModel : BaseModel, IEmsMessageModel
{
    #region - Ctors -
    public EmsMessageModel()
    {
    }

    public EmsMessageModel(int id, int userId, EnumNoticeType nType, string sender, string receiver, string? msg, EnumMsgType mType, string? title, string? destination, DateTime reservation, List<ImageModel>? attachedImages) : base(id)
    {
        Id = id;
        UserId = userId;
        NoticeType = nType;
        Sender = sender;
        Receiver = receiver;
        Message = msg;
        MsgType = mType;
        Title = title;
        Destination = destination;
        Reservation = reservation;
        AttachedImages = attachedImages;
    }

    public EmsMessageModel(IEmsMessageModel model) : base(model)
    {
        
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
    /// <summary>
    /// API의 user_id가 아니라 DB와 관련되어서 FK이다.
    /// </summary>
    [JsonProperty("user_id", Order = 1)]
    public int UserId { get; set; }

    [JsonProperty("notice_type", Order = 2)]
    public EnumNoticeType NoticeType { get; set; }

    [JsonProperty("sender", Order = 3)]
    public string Sender { get; set; } = string.Empty;

    [JsonProperty("receiver", Order = 4)]
    public string Receiver { get; set; } = string.Empty;

    [JsonProperty("message", Order = 5)]
    public string? Message { get; set; } = string.Empty;

    [JsonProperty("msg_type", Order = 6)]
    public EnumMsgType MsgType { get; set; }

    [JsonProperty("title", Order = 7)]
    public string? Title { get; set; } = string.Empty;

    [JsonProperty("destination", Order = 8)]
    public string? Destination { get; set; } = string.Empty;

    [JsonProperty("reservation", Order = 9)]
    public DateTime Reservation { get; set; }

    [JsonProperty("attached_images", Order = 10)]
    public List<ImageModel>? AttachedImages { get; set; }

    [JsonProperty("send_time", Order = 11)]
    public DateTime SendTime { get; set; }
    #endregion
    #region - Attributes -
    #endregion
}