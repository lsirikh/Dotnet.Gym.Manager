using Caliburn.Micro;
using Dotnet.Gym.Message.Accounts;
using Dotnet.Gym.Message.Enums;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using System;

namespace Dotnet.Gym.Manager.Gui.ViewModels;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/11/2025 2:27:39 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class UserViewModel : BaseCustomViewModel<IUserModel>
{
    
    #region - Ctors -
    public UserViewModel(IUserModel model, IEventAggregator eventAggregator, ILogService log)
        : base(model, eventAggregator, log)
    {
    }
    #endregion
    #region - Implementation of Interface -
    #endregion
    #region - Overrides -
    public override void Dispose()
    {
        _model = new UserModel();
        GC.Collect();
    }
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    private EnumServiceState CheckServiceState()
    {
        if (ActivePeriod == null || !ActivePeriod.StartDate.HasValue || !ActivePeriod.EndDate.HasValue)
            return EnumServiceState.Expired; // 서비스 기간 없음 또는 날짜정보가 불완전


        var today = DateTime.Today;
        var startDate = ActivePeriod.StartDate.Value.Date;
        var endDate = ActivePeriod.EndDate.Value.Date;

        if (today < startDate)
            return EnumServiceState.Pending; // 아직 시작되지 않은 등록기간

        if (today > endDate)
            return EnumServiceState.Expired; // 만료된 상태

        var daysToExpire = (endDate - today).Days;

        if (daysToExpire <= 7)
            return EnumServiceState.Expiring_Soon; // 만료 7일 전 상태

        return EnumServiceState.Active; // 정상 이용 가능 상태
    }
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    public string UserName
    {
        get => _model.UserName;
        set
        {
            _model.UserName = value;
            NotifyOfPropertyChange(() => UserName);
        }
    }

    public string MobilePhone
    {
        get => _model.MobilePhone;
        set
        {
            _model.MobilePhone = value;
            NotifyOfPropertyChange(() => MobilePhone);
        }
    }

    public int Age
    {
        get => _model.Age;
        set
        {
            _model.Age = value;
            NotifyOfPropertyChange(() => Age);
        }
    }

    public EnumGenderType Gender
    {
        get => _model.Gender;
        set
        {
            _model.Gender = value;
            NotifyOfPropertyChange(() => Gender);
        }
    }

    public DateTime RegisterDate
    {
        get => _model.RegisterDate;
        set
        {
            _model.RegisterDate = value;
            NotifyOfPropertyChange(() => RegisterDate);
        }
    }

    public EnumTrueFalse IsActiveState
    {
        get => _model.IsActive;
        set
        {
            _model.IsActive = value;
            NotifyOfPropertyChange(() => IsActive);
        }
    }

    public ActivePeriod? ActivePeriod
    {
        get => _model.ActivePeriod;
        set
        {
            _model.ActivePeriod = value;
            CheckServiceState();
            NotifyOfPropertyChange(() => ActivePeriod);
        }
    }

    public LockerModel? Locker
    {
        get => _model.Locker;
        set
        {
            _model.Locker = value;
            NotifyOfPropertyChange(() => Locker);
        }
    }

    public EnumServiceState ServiceState
    {
        get => CheckServiceState();
        set
        {
            _serviceState = value;
            NotifyOfPropertyChange(() => ServiceState);
        }
    }

    public DateTime CreatedAt
    {
        get => _model.CreatedAt;
        set
        {
            _model.CreatedAt = value;
            NotifyOfPropertyChange(() => CreatedAt);
        }
    }

    public DateTime UpdatedAt
    {
        get => _model.UpdatedAt;
        set
        {
            _model.UpdatedAt = value;
            NotifyOfPropertyChange(() => UpdatedAt);
        }
    }
    #endregion
    #region - Attributes -
    private EnumServiceState _serviceState;
    #endregion

}