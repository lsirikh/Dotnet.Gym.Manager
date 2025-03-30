using Caliburn.Micro;
using Dotnet.Gym.Message.Accounts;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.Db.Services;
using System;

namespace Dotnet.Gym.Manager.Gui.ViewModels;
/****************************************************************************
    Purpose      :                                                          
    Created By   : GHLee                                                
    Created On   : 2/27/2025 2:46:10 PM                                                    
    Department   : SW Team                                                   
    Company      : Sensorway Co., Ltd.                                       
    Email        : lsirikh@naver.com                                         
****************************************************************************/
public class UpdateServiceViewModel : UserViewModel
{
    #region - Ctors -
    public UpdateServiceViewModel(IUserModel model, IEventAggregator eventAggregator, ILogService log)
        : base(model, eventAggregator, log)
    {

    }
    #endregion
    #region - Implementation of Interface -
    #endregion
    #region - Overrides -
    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _dbService = IoC.Get<IDbServiceForGym>();
        _cts = new CancellationTokenSource();

        if(ActivePeriod != null)
        {
            ActivePeriod.StartDate = DateTime.UtcNow;
            ActivePeriod.EndDate = DateTime.UtcNow;
        }
        else
        {
            ActivePeriod = new ActivePeriod();
            ActivePeriod.StartDate = DateTime.UtcNow;
            ActivePeriod.EndDate = DateTime.UtcNow;
        }

        return base.OnActivateAsync(cancellationToken);
    }
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    public void SetOneMonthButton()
    {
        if (ActivePeriod == null) return;
        ActivePeriod.EndDate = ActivePeriod.StartDate?.AddMonths(1);
        Refresh();
    }

    public void SetSixMonthButton()
    {
        if (ActivePeriod == null) return;
        ActivePeriod.EndDate = ActivePeriod.StartDate?.AddMonths(6);
        Refresh();
    }

    public void SetOneYearButton()
    {
        if (ActivePeriod == null) return;
        ActivePeriod.EndDate = ActivePeriod.StartDate?.AddYears(1);
        Refresh();
    }

    public async void ClickCancelButton()
    {

        await TryCloseAsync(false);
    }

    public async void ClickOkButton()
    {
        if (_cts == null || _cts.IsCancellationRequested)
        {
            _cts = new CancellationTokenSource();
        }

        if (_dbService == null || ActivePeriod == null) return;

        await _dbService.UpdateActiveUserAsync(ActivePeriod, _cts.Token);

        await TryCloseAsync(true);
    }
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    private DateTime _testDate;

    public DateTime TestDate
    {
        get { return _testDate; }
        set { _testDate = value; NotifyOfPropertyChange(() => TestDate); }
    }

    #endregion
    #region - Attributes -
    private IDbServiceForGym? _dbService;
    private CancellationTokenSource? _cts;
    #endregion

}
