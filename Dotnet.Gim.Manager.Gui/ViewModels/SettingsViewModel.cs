using Caliburn.Micro;
using Dotnet.Gym.Manager.Gui.Models;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using System;

namespace Dotnet.Gym.Manager.Gui.ViewModels;
    /****************************************************************************
       Purpose      :                                                          
       Created By   : GHLee                                                
       Created On   : 2/28/2025 11:08:14 AM                                                    
       Department   : SW Team                                                   
       Company      : Sensorway Co., Ltd.                                       
       Email        : lsirikh@naver.com                                         
    ****************************************************************************/
public class SettingsViewModel : BasePanelViewModel
{
    #region - Ctors -
    public SettingsViewModel(ILogService log,
                            IEventAggregator eventAggregator,
                            SetupModel setupModel) : base(log:log, eventAggregator:eventAggregator)
    {
        _setupModel = setupModel;
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
    public SetupModel SetupModel
    {
        get { return _setupModel; }
        set { _setupModel = value; NotifyOfPropertyChange(() => SetupModel); }
    }

    #endregion
    #region - Attributes -
    private SetupModel? _setupModel;
    #endregion
}
