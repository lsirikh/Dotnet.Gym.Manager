using Caliburn.Micro;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using System;

namespace Dotnet.Gym.Manager.Gui.ViewModels.Components;
    /****************************************************************************
       Purpose      :                                                          
       Created By   : GHLee                                                
       Created On   : 2/21/2025 1:01:27 PM                                                    
       Department   : SW Team                                                   
       Company      : Sensorway Co., Ltd.                                       
       Email        : lsirikh@naver.com                                         
    ****************************************************************************/
public class ConfirmViewModel : InformViewModel
{
    #region - Ctors -
    public ConfirmViewModel(IEventAggregator eventAggregator, ILogService log)
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
    public async void OnClickCancelButton()
    {
        await TryCloseAsync(false);
    }
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    #endregion
    #region - Attributes -
    #endregion
}
