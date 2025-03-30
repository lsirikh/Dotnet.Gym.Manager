using Caliburn.Micro;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using System;

namespace Dotnet.Gym.Manager.Gui.ViewModels.Components;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 3/24/2025 5:49:49 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class InformViewModel : BasePanelViewModel
{
    #region - Ctors -
    public InformViewModel(IEventAggregator eventAggregator, ILogService log)
        : base(eventAggregator, log)
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

    public async void OnClickOkButton()
    {
        await TryCloseAsync(true);
    }

    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -

    public string Title
    {
        get { return _title; }
        set { _title = value; NotifyOfPropertyChange(() => Title); }
    }
    public string Content
    {
        get { return _content; }
        set { _content = value; NotifyOfPropertyChange(() => Content); }
    }
    #endregion
    #region - Attributes -
    private string _title = "확인";
    private string _content = string.Empty;
    #endregion
}
