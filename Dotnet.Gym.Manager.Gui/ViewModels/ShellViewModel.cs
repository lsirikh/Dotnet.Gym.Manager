using Caliburn.Micro;
using Dotnet.Gym.Manager.Gui.Models;
using Dotnet.Gym.Manager.Gui.ViewModels.Components;
using Dotnet.Gym.Manager.Gui.Views;
using Ironwall.Dotnet.Libraries.Base.Models;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.Db.Services;
using Ironwall.Dotnet.Libraries.Db.Utils;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Conductors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Dotnet.Gym.Manager.Gui.ViewModels;
/****************************************************************************
       Purpose      : It becomes the starting point of the initial loading screen.                                                          
       Created By   : GHLee                                                
       Created On   : 1/31/2025 11:01:50 AM                                                     
       Department   : SW Team                                                   
       Company      : Sensorway Co., Ltd.                                       
       Email        : lsirikh@naver.com                                         
 ****************************************************************************/
internal class ShellViewModel : ConductorOneViewModel
                                , IHandle<InitialLoadFinishMessage>
{
    #region - Ctors -
    public ShellViewModel(IEventAggregator eventAggregator
                        , ILogService log
                        , UserInfoListViewModel userInfoListViewModel
                        , LockerInfoViewModel lockerInfoViewModel
                        , SettingsViewModel settingsViewModel
                        , SetupModel setup
                        , IDbServiceForGym dbService
                        , IExcelImporter excelImporter
                        ): base(eventAggregator, log)
    {
        UserInfoListViewModel = userInfoListViewModel;
        LockerInfoViewModel = lockerInfoViewModel;
        SettingsViewModel = settingsViewModel;
        _excelImporter = excelImporter;
        _setup = setup;
        _dbService = dbService;
    }
    #endregion
    #region - Implementation of Interface -
    #endregion
    #region - Overrides -
    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _eventAggregator?.SubscribeOnUIThread(this);
        await base.OnActivateAsync(cancellationToken);
        await UserInfoListViewModel.ActivateAsync();
        _ = InformPopup(_excelImporter.Result);
    }

    protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        _eventAggregator?.Unsubscribe(this);
        await base.OnDeactivateAsync(close, cancellationToken);
    }
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    public async void OnActiveTab(object sender, SelectionChangedEventArgs args)
    {
        try
        {
            if (!(args.Source is TabControl)) return;

            // Check if we have a selected TabItem
            if (args.AddedItems.Count == 0 || !(args.AddedItems[0] is TabItem tabItem))
                return;

            // Extract the content from the selected TabItem
            var tabContent = tabItem.Content as ContentControl;
            //var contentControl = (tapItem as ContentControl)?.Content as ContentControl;

            // Deactivate previous ViewModel if any
            if (selectedViewModel != null)
                await selectedViewModel.DeactivateAsync(true);

            // If tabContent is a ContentControl, use its Content; otherwise, use tabContent directly.
            var viewContent = (tabContent is ContentControl cc) ? cc.Content : tabContent;

            if (viewContent is UserInfoListView)
            {
                selectedViewModel = UserInfoListViewModel;
            }
            else if (viewContent is LockerInfoView)
            {
                selectedViewModel = LockerInfoViewModel;
            }
            else if (viewContent is SettingsView)
            {
                selectedViewModel = SettingsViewModel;
            }
            else
            {
                selectedViewModel = null;
            }

            // Activate the selected ViewModel if not null
            if (selectedViewModel != null)
                await selectedViewModel.ActivateAsync();
        }
        catch (Exception ex)
        {
            _log?.Error($"Raised Exception in {nameof(OnActiveTab)} : {ex.Message}");
        }
    }

    //public async Task LoadExcelDataAsync(CancellationToken token = default)
    //{
    //    try
    //    {
    //        //await _dbService.DeleteAllUsersAsync();

    //        // Excel 파일이 위치한 디렉토리
    //        var currentDir = AppDomain.CurrentDomain.BaseDirectory;
    //        var excelDir = Path.Combine(currentDir, _setup.ExcelFolder);

    //        // 폴더 내 첫 번째 Excel 파일 찾기
    //        var excelFiles = Directory.GetFiles(excelDir, "*.xlsx");
    //        if (excelFiles.Length == 0)
    //        {
    //            _log?.Warning("No Excel files found in the directory.");
    //            return;
    //        }

           
    //        var memberExcelFile = excelFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Contains("회원", StringComparison.OrdinalIgnoreCase));
    //        if (memberExcelFile == null) return;

    //        // 파일이 열려 있는 경우 대비하여 임시 폴더에 복사
    //        var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
    //        File.Copy(memberExcelFile, tempFilePath, overwrite: true);
    //        var ret = await _excelImporter.ImportExcelToDbAsync(tempFilePath, token);
    //        _ = InformPopup(ret);
    //        UserInfoListViewModel.OnClickReloadButton(null, null);

    //        // 파일 사용 후 삭제
    //        File.Delete(tempFilePath);

    //    }
    //    catch (Exception ex)
    //    {
    //        _log?.Error($"Error during Excel data import: {ex.Message}");
    //    }
    //}

    private async Task InformPopup(bool ret)
    {
        try
        {
            await Task.Delay(2000);
            var window = IoC.Get<IWindowManager>();
            var viewModel = IoC.Get<InformViewModel>();
            if (ret) {
                viewModel.Title = "사용자 정보 업데이트";
                viewModel.Content = "회원등록 엑셀 정보를 정상적으로 업데이트 했습니다..";
                await window.ShowDialogAsync(viewModel);
                UserInfoListViewModel.OnClickReloadButton(null, null);
                _log?.Info("Excel data import completed successfully.");
            }
            else
            {
                
                viewModel.Title = "사용자 정보 업데이트 오류";
                viewModel.Content = "회원등록 엑셀 정보를 업데이트 하는 도중에 에러가 발생하였습니다. 관리자에게 연락해주세요.";
                await window.ShowDialogAsync(viewModel);
                _log?.Info("Excel data import was failed...");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task HandleAsync(InitialLoadFinishMessage message, CancellationToken cancellationToken)
    {
        await InformPopup(message.Result);
    }
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    public UserInfoListViewModel UserInfoListViewModel { get; }
    public LockerInfoViewModel LockerInfoViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; set; }

    #endregion
    #region - Attributes -
    private IExcelImporter _excelImporter;
    private SetupModel _setup;
    private IDbServiceForGym _dbService;
    BasePanelViewModel? selectedViewModel;
    #endregion
}
