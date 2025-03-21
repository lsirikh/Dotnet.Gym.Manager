using Dotnet.Gym.Message.Enums;
using Ironwall.Dotnet.Libraries.Base.Models;

namespace Dotnet.Gym.Message.Contacts;
public interface IEmsMessageModel : IBaseModel
{
    int UserId { get; set; }
    List<ImageModel>? AttachedImages { get; set; }
    string? Destination { get; set; }
    string? Message { get; set; }
    EnumMsgType MsgType { get; set; }
    EnumNoticeType NoticeType { get; set; }
    string Receiver { get; set; }
    DateTime Reservation { get; set; }
    string Sender { get; set; }
    DateTime SendTime { get; set; }
    string? Title { get; set; }
}