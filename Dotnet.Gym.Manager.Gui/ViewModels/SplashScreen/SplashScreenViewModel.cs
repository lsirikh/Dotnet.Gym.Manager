using Caliburn.Micro;
using Ironwall.Dotnet.Libraries.Base.Models;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using System;
using System.Reflection;

namespace Dotnet.Gym.Manager.Gui.ViewModels.SplashScreen;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 3/24/2025 1:48:47 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class SplashScreenViewModel : BasePanelViewModel
                                    , IHandle<SplashScreenMessage>
{
    #region - Ctors -
    public SplashScreenViewModel(IEventAggregator eventAggregator, ILogService log) 
        : base(eventAggregator, log)
    {
        // 어셈블리 참조를 얻기
        var assembly = Assembly.GetEntryAssembly();

        
        // 각 속성을 안전하게 가져오기
        var desc = assembly?.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        Description = desc ?? "No Description";

        var com = assembly?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
        Company = com ?? "No Company";

        var p = assembly?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
        Product = p ?? "No Product";

        var t = assembly?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        Title = t ?? "No Title";

        var ver = assembly?.GetName().Version;
        if (ver != null)
        {
            Version = $"{ver.Major}.{ver.Minor}";
        }
        else
        {
            Version = "No Version";
        }
    }
    #endregion
    #region - Implementation of Interface -
    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _eventAggregator.SubscribeOnUIThread(this);
        return base.OnActivateAsync(cancellationToken);
    }
    #endregion
    #region - Overrides -
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    #endregion
    #region - IHanldes -
    public Task HandleAsync(SplashScreenMessage message, CancellationToken cancellationToken)
    {
        try
        {
            SplashScreenText = $"[{message.Title}]{message.Message}";
        }
        catch (Exception ex)
        {

            _log?.Error(ex.Message);
        }
        return Task.CompletedTask;
    }
    #endregion
    #region - Properties -
    public string? SplashScreenText
    {
        get => _splashScreenText;
        set
        {
            _splashScreenText = value;
            NotifyOfPropertyChange(() => SplashScreenText);
        }
    }
    #endregion
    #region - Attributes -
    private string? _splashScreenText = string.Empty;
    public string? Version { get; } 
    public string? Company { get; }
    public string? Product { get; }
    public string? Title { get; }
    public string? Description { get; }

    
    #endregion
}