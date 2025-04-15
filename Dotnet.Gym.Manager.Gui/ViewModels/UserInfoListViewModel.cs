using Caliburn.Micro;
using ClosedXML.Excel;
using Dotnet.Gym.Manager.Gui.Models;
using Dotnet.Gym.Manager.Gui.ViewModels.Components;
using Dotnet.Gym.Message.Accounts;
using Dotnet.Gym.Message.Contacts;
using Dotnet.Gym.Message.Enums;
using Dotnet.Gym.Message.Providers;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.Db.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Dotnet.Gym.Manager.Gui.ViewModels;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/10/2025 6:17:11 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class UserInfoListViewModel : BaseDataGridPanelViewModel<UserViewModel>
{
    
    #region - Ctors -
    public UserInfoListViewModel(IEventAggregator eventAggregator,
                                ILogService log,
                                IDbServiceForGym dbService, 
                                SetupModel setup): base(eventAggregator, log)
    {
        SearchConditions = new ObservableCollection<EnumSearchCondition>((EnumSearchCondition[])Enum.GetValues(typeof(EnumSearchCondition)));
        _dbService = dbService;
        _setup = setup;
    }
    #endregion
    #region - Implementation of Interface -
    #endregion
    #region - Overrides -
    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _windowManager = IoC.Get<IWindowManager>();
        await DataInitialize((_cancellationTokenSource ?? new CancellationTokenSource()).Token ).ConfigureAwait(false);
    }

    protected override Task Uninitialize()
    {
        base.Uninitialize();
        return Task.CompletedTask;
    }

    public override async void OnClickDeleteButton(object sender, RoutedEventArgs e)
    {
        _log.Info("OnClickDeleteButton");
        if (_windowManager == null) return;
        var selectedList = ViewModelProvider.Where(entity => entity.IsSelected).ToList();
        if (!(selectedList.Count > 0)) return;

        var viewModel = IoC.Get<ConfirmViewModel>();
        viewModel.Title = "고객 정보 삭제";
        viewModel.Content = "선택하신 고객의 정보를 삭제 하시겠습니까? 삭제 후에는 DB에서 완전 제거됩니다.";
        var ret = await _windowManager.ShowDialogAsync(viewModel);
        if (ret == true)
        {
            foreach (var item in selectedList)
            {
                await _dbService.DeleteUserAsync(item.Id);
            }
            await _dbService.FetchInstanceAsync();
            await DataInitialize((_cancellationTokenSource ?? new CancellationTokenSource()).Token).ConfigureAwait(false);
            await SelectAll(false);
        }
    }

    public override async void OnClickInsertButton(object sender, RoutedEventArgs e)
    {
        _log.Info("OnClickInsertButton");
        if (_windowManager == null) return;

        var viewModel = new AddUserViewModel(new UserModel(), _eventAggregator ?? IoC.Get<IEventAggregator>(), _log ?? IoC.Get<ILogService>());
        await viewModel.ActivateAsync();
        var ret =  await _windowManager.ShowDialogAsync(viewModel);
        if(ret == true) 
        {
            await _dbService.FetchInstanceAsync();
            await DataInitialize((_cancellationTokenSource ?? new CancellationTokenSource()).Token).ConfigureAwait(false);
        }
    }

    public override async void OnClickReloadButton(object sender, RoutedEventArgs e)
    {
        await _dbService.FetchInstanceAsync();
        await DataInitialize((_cancellationTokenSource ?? new CancellationTokenSource()).Token).ConfigureAwait(false);
    }

    public override void OnClickSaveButton(object sender, RoutedEventArgs e)
    {

    }

    public async void OnClickExtendButton(object sender, RoutedEventArgs e)
    {
        if (_windowManager == null) return;
        if(SelectedItem  == null) return;

        var viewModel = new UpdateServiceViewModel(SelectedItem.Model, _eventAggregator ?? IoC.Get<IEventAggregator>(), _log ?? IoC.Get<ILogService>());
        await viewModel.ActivateAsync();
        var ret = await _windowManager.ShowDialogAsync(viewModel);
        if (ret == true)
        {
            await _dbService.FetchInstanceAsync();
            await DataInitialize((_cancellationTokenSource ?? new CancellationTokenSource()).Token).ConfigureAwait(false);
            await SelectAll(false);
        }
    }

    public async void OnClickSendMessageButton(object sender, RoutedEventArgs e)
    {
        if (_windowManager == null) return;
        //if(SelectedItem  == null) return;
        var selectedList = ViewModelProvider.Where(entity => entity.IsSelected).ToList();

        if (!(selectedList.Count > 0)) return;
        var emses = new List<IEmsMessageModel>();
        foreach (var item in selectedList)
        {
            var user = ViewModelProvider.Where(entity => entity.MobilePhone == item.MobilePhone).FirstOrDefault();
            if (user == null) continue;
            
            emses.Add(new EmsMessageModel
            {
                UserId = item.Id,
                Sender = _setup.Phone,
                Receiver = item.MobilePhone, // 여기에서 파싱 메서드 사용
                Destination = $"{ParseOnlyNumbers(item.MobilePhone)}|{user.UserName}",
                MsgType = EnumMsgType.SMS,
            });
        }

        var viewModel = new EmsMessageViewModel(emses, _eventAggregator ?? IoC.Get<IEventAggregator>(), _log ?? IoC.Get<ILogService>());
        await viewModel.ActivateAsync();
        var ret = await _windowManager.ShowDialogAsync(viewModel);
        /*if (ret == true)
        {
            await _dbService.FetchInstanceAsync();
            await DataInitialize((_cancellationTokenSource ?? new CancellationTokenSource()).Token).ConfigureAwait(false);
            await SelectAll(false);
        }*/
    }

    public async void OnClickUserDetail(object sender, RoutedEventArgs e)
    {
        if (!(e is MouseButtonEventArgs inputEvent))
            return;

        if (_windowManager == null) return;
        if (SelectedItem == null) return;

        var viewModel = new UserEditViewModel(SelectedItem.Model, _eventAggregator ?? IoC.Get<IEventAggregator>(), _log ?? IoC.Get<ILogService>());
        await viewModel.ActivateAsync();
        var ret = await _windowManager.ShowDialogAsync(viewModel);
        if (ret == true)
        {
            await _dbService.FetchInstanceAsync();
            await DataInitialize((_cancellationTokenSource ?? new CancellationTokenSource()).Token).ConfigureAwait(false);
            await SelectAll(false);
        }
    }
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    private async Task DataInitialize(CancellationToken cancellationToken = default)
    {
        try
        {
            IsVisible = false;
            await Task.Delay(100);
            if (cancellationToken.IsCancellationRequested) new TaskCanceledException("Task was cancelled!");
            //ViewModelProvider Setting
            _provider = IoC.Get<UserProvider>();
            SearchCondition = EnumSearchCondition.None;
            DispatcherService.Invoke((System.Action)(() =>
            {
                ViewModelProvider.Clear();
                foreach (var item in _provider)
                {
                    ViewModelProvider.Add(new UserViewModel(item, _eventAggregator ?? IoC.Get<IEventAggregator>(), _log ?? IoC.Get<ILogService>()));
                }

            }));

            await SelectAll(false);

            NotifyOfPropertyChange(() => ViewModelProvider);
        }
        catch (TaskCanceledException ex)
        {
            _log?.Error($"Raised {nameof(TaskCanceledException)}({nameof(DataInitialize)}) : {ex.Message}");
        }
        finally
        {
            
            IsVisible = true;
        }
    }

    public void OnClickSearchButton(object sender, RoutedEventArgs e)
    {
        if ( _provider == null) return;

        var filteredData = _provider.Where(user =>
        {
            switch (SearchCondition)
            {
                case EnumSearchCondition.Name:
                    return user.UserName.Contains(SearchData, StringComparison.OrdinalIgnoreCase);
                case EnumSearchCondition.PhoneNumber:
                    return user.MobilePhone.Contains(SearchData);
                case EnumSearchCondition.ExpiringSoon:
                    return user.ActivePeriod != null &&
                           //user.ActivePeriod.EndDate.Value.Date == DateTime.Now.AddDays(_setup.ExpireSoonDay).Date;
                           user.ActivePeriod.EndDate.Value.Date <= DateTime.Now.AddDays(_setup.ExpireSoonDay).Date &&
                           user.ActivePeriod.EndDate.Value.Date >= DateTime.Now.Date;
                case EnumSearchCondition.Expired:
                    return user.ActivePeriod != null &&
                           user.ActivePeriod.EndDate.Value.Date < DateTime.Now.Date;
                case EnumSearchCondition.ExpiredAfter:
                    return user.ActivePeriod != null &&
                           user.ActivePeriod.EndDate.Value.Date >= DateTime.Now.AddDays(_setup.ExpireAfterDay).Date &&
                           user.ActivePeriod.EndDate.Value.Date < DateTime.Now.Date;
                default:
                    return false;
            }
        }).Select(user => new UserViewModel(user, _eventAggregator ?? IoC.Get<IEventAggregator>(), _log ?? IoC.Get<ILogService>())).ToList();

        // ViewModelProvider를 새로운 컬렉션으로 교체
        ViewModelProvider = new ObservableCollection<UserViewModel>(filteredData);
        NotifyOfPropertyChange(() => ViewModelProvider);
    }

    public async void OnClickClearButton(object sender, RoutedEventArgs e)
    {
        SearchData = null;
        SearchCondition = EnumSearchCondition.None;
        await DataInitialize();
    }

    public async void ConditionSelectionChanged(object sender, RoutedEventArgs e)
    {
        CheckCondition();
        if(SearchCondition == EnumSearchCondition.None)
        {
            SearchData = null;
            await DataInitialize();
        }
    }

    public async void OnClickExportExcel(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "Excel 파일 (*.xlsx)|*.xlsx",
            FileName = $"회원정보_{DateTime.Now:yyyyMMdd}.xlsx",
            DefaultExt = ".xlsx"
        };

        if (dialog.ShowDialog() == true)
        {
            ExportToExcel(dialog.FileName);

            if (_windowManager == null) return;
            var viewModel = IoC.Get<InformViewModel>();
            viewModel.Title = "엑셀 정보 저장";
            viewModel.Content = "고객의 정보가 엑셀로 저장되었습니다.";
            var ret = await _windowManager.ShowDialogAsync(viewModel);
        }
    }

    public void ExportToExcel(string filePath)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("UserList");

        // 헤더 작성
        worksheet.Cell(1, 1).Value = "이름";
        worksheet.Cell(1, 2).Value = "전화번호";
        worksheet.Cell(1, 3).Value = "나이";
        worksheet.Cell(1, 4).Value = "성별";
        worksheet.Cell(1, 5).Value = "등록일";
        worksheet.Cell(1, 6).Value = "이용 여부";
        worksheet.Cell(1, 7).Value = "서비스 상태";
        worksheet.Cell(1, 8).Value = "이용 시작일";
        worksheet.Cell(1, 9).Value = "이용 종료일";
        worksheet.Cell(1, 10).Value = "락커";
        worksheet.Cell(1, 11).Value = "신발장";

        int row = 2;
        foreach (var vm in ViewModelProvider)
        {
            worksheet.Cell(row, 1).Value = vm.UserName;
            worksheet.Cell(row, 2).Value = vm.MobilePhone;
            worksheet.Cell(row, 3).Value = vm.Age;
            worksheet.Cell(row, 4).Value = vm.Gender.ToString();
            worksheet.Cell(row, 5).Value = vm.RegisterDate.ToString("yyyy-MM-dd");
            worksheet.Cell(row, 6).Value = vm.IsActiveState.ToString();
            worksheet.Cell(row, 7).Value = vm.ServiceState.ToString();
            worksheet.Cell(row, 8).Value = vm.ActivePeriod?.StartDate?.ToString("yyyy-MM-dd");
            worksheet.Cell(row, 9).Value = vm.ActivePeriod?.EndDate?.ToString("yyyy-MM-dd");
            worksheet.Cell(row, 10).Value = vm.Locker?.Locker;
            worksheet.Cell(row, 11).Value = vm.Locker?.ShoeLocker;
            row++;
        }

        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(filePath);
    }


    private void CheckCondition()
    {
        switch (SearchCondition)
        {
            case EnumSearchCondition.None:
                {
                    IsSearchable = false;
                    IsInputable = false;
                }
                break;
            case EnumSearchCondition.Name:
            case EnumSearchCondition.PhoneNumber:
                {
                    IsInputable = true;
                    if (!string.IsNullOrEmpty(SearchData))
                        IsSearchable = true;
                    else
                        IsSearchable = false;
                }
                break;
            case EnumSearchCondition.ExpiringSoon:
            case EnumSearchCondition.Expired:
            case EnumSearchCondition.ExpiredAfter:
                IsSearchable = true;
                IsInputable = false;
                break;
            default:
                IsSearchable = false;
                IsInputable = false;
                break;
        }
    }

    private string ParseOnlyNumbers(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        return new string(phoneNumber.Where(char.IsDigit).ToArray());
    }
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    public EnumSearchCondition SearchCondition
    {
        get { return _searchCondition; }
        set 
        { 
            _searchCondition = value; 
            NotifyOfPropertyChange(() => SearchCondition);
        }
    }

    public string? SearchData
    {
        get { return _searchData; }
        set { 
            _searchData = value; 
            NotifyOfPropertyChange(() => SearchData);
            CheckCondition();
        }
    }

    public bool IsInputable
    {
        get { return _isInputable; }
        set { _isInputable = value; NotifyOfPropertyChange(() => IsInputable); }
    }

    public bool IsSearchable
    {
        get { return _isSearchable; }
        set { _isSearchable = value; NotifyOfPropertyChange(() => IsSearchable); }
    }


    public bool IsDeletable
    {
        get { return _isDeletable; }
        set { _isDeletable = value; NotifyOfPropertyChange(() => IsDeletable); }
    }


    public bool IsExtendable
    {
        get { return _isExtendable; }
        set { _isExtendable = value; NotifyOfPropertyChange(() => IsExtendable); }
    }



    public ObservableCollection<EnumSearchCondition> SearchConditions { get; }

    private IDbServiceForGym _dbService;
    private SetupModel _setup;

    #endregion
    #region - Attributes -
    private EnumSearchCondition _searchCondition;
    private string? _searchData;
    private bool _isSearchable;
    private UserProvider? _provider;
    private IWindowManager? _windowManager;
    private bool _isDeletable;
    private bool _isExtendable;
    private bool _isInputable;
    #endregion
}