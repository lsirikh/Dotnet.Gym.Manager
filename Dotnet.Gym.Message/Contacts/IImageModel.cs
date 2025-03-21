using Ironwall.Dotnet.Libraries.Base.Models;

namespace Dotnet.Gym.Message.Contacts;

public interface IImageModel : IBaseModel
{
    int EmsMessageId { get; set; }
    string Base64Image { get; set; }
    string ContentType { get; set; }
    string FileName { get; set; }
}