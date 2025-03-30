using Caliburn.Micro;
using DocumentFormat.OpenXml.EMMA;
using Dotnet.Gym.Manager.Gui.Models;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using Dotnet.Gym.Manager.Gui.ViewModels.Components;

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
                            SetupModel setupModel) 
                            : base(log:log, eventAggregator:eventAggregator)
    {
        _setupModel = setupModel;
    }
    #endregion
    #region - Implementation of Interface -
    #endregion
    #region - Overrides -
    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        SetupModel = _setupModel;
        return base.OnActivateAsync(cancellationToken);
    }
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    public void ReloadSettings(object sender, RoutedEventArgs e)
    {
        try
        {
            // 1) appsettings.json 파일 경로
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(baseDir, "appsettings.json");

            // 2) 기존 appsettings.json 내용을 읽어서 JObject로 파싱
            var json = File.ReadAllText(filePath);
            var jObject = JsonConvert.DeserializeObject<JObject>(json);

            // 3) "AppSettings" 섹션에 접근
            var appSettingsSection = jObject["AppSettings"];
            if (appSettingsSection == null)
            {
                // 섹션이 없으면 반환 (또는 필요 시 새 섹션 생성)
                _log?.Warning("'AppSettings' section not found in appsettings.json");
                return;
            }

            // 4) SetupModel의 각 속성에 JSON 값 대입
            //    문자열은 ToString(), 정수나 bool은 Value<int>() / Value<bool>() 등 사용
            SetupModel.Url = appSettingsSection["Url"]?.ToString() ?? "";
            SetupModel.Username = appSettingsSection["Username"]?.ToString() ?? "";
            SetupModel.Password = appSettingsSection["Password"]?.ToString() ?? "";
            SetupModel.ApiKey = appSettingsSection["ApiKey"]?.ToString() ?? "";
            SetupModel.Phone = appSettingsSection["Phone"]?.ToString() ?? "";
            SetupModel.IpDbServer = appSettingsSection["IpDbServer"]?.ToString() ?? "";
            // PortDbServer가 null이거나 변환 불가하면 기본값(3306) 사용
            if (appSettingsSection["PortDbServer"] != null && int.TryParse(appSettingsSection["PortDbServer"].ToString(), out var port))
            {
                SetupModel.PortDbServer = port;
            }
            SetupModel.DbDatabase = appSettingsSection["DbDatabase"]?.ToString() ?? "";
            SetupModel.UidDbServer = appSettingsSection["UidDbServer"]?.ToString() ?? "";
            SetupModel.PasswordDbServer = appSettingsSection["PasswordDbServer"]?.ToString() ?? "";
            SetupModel.ExcelFolder = appSettingsSection["ExcelFolder"]?.ToString() ?? "";
            // bool 처리
            if (appSettingsSection["IsLoadExcel"] != null && bool.TryParse(appSettingsSection["IsLoadExcel"].ToString(), out var loadExcel))
            {
                SetupModel.IsLoadExcel = loadExcel;
            }
            // ExpireSoonDay가 null이거나 변환 불가하면 기본값(3) 사용
            if (appSettingsSection["ExpireSoonDay"] != null && int.TryParse(appSettingsSection["ExpireSoonDay"].ToString(), out var expireSoonDay))
            {
                SetupModel.ExpireSoonDay = expireSoonDay;
            }
            // ExpireAfterDay가 null이거나 변환 불가하면 기본값(-10) 사용
            if (appSettingsSection["ExpireAfterDay"] != null && int.TryParse(appSettingsSection["ExpireAfterDay"].ToString(), out var expireAfterDay))
            {
                SetupModel.ExpireAfterDay = expireAfterDay;
            }

            // 5) 필요한 경우, UI가 SetupModel 변경을 인식하도록 PropertyChange 알림
            //    예: NotifyOfPropertyChange(() => SetupModel);
            //    또는 직접 SetupModel의 속성에 setter에서 이벤트가 있으면 반영됨.

            Refresh();
            _log?.Info("Settings reloaded from appsettings.json");
        }
        catch (Exception ex)
        {
            _log?.Error($"Error reload settings: {ex}");
        }
    }

    public async void SaveSettings(object sender, RoutedEventArgs e)
    {
        try
        {
            // appsettings.json 파일 경로
            // (프로젝트 구조나 실행 환경에 맞춰 수정하세요)
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(baseDir, "appsettings.json");

            // 1) 기존 appsettings.json 내용을 읽어서 JObject로 파싱
            var json = File.ReadAllText(filePath);
            var jObject = JsonConvert.DeserializeObject<JObject>(json);

            // 2) "AppSettings" 섹션에 접근
            var appSettingsSection = jObject["AppSettings"];
            if (appSettingsSection == null)
            {
                // 섹션이 없으면 반환 (또는 필요 시 새 섹션 생성)
                _log?.Warning("'AppSettings' section not found in appsettings.json");

                // "AppSettings" 섹션이 없는 경우 생성
                appSettingsSection = new JObject();
                jObject["AppSettings"] = appSettingsSection;
            }

            // 3) SetupModel의 변경된 값들을 JSON에 반영
            appSettingsSection["Url"] = SetupModel.Url;
            appSettingsSection["Username"] = SetupModel.Username;
            appSettingsSection["Password"] = SetupModel.Password;
            appSettingsSection["ApiKey"] = SetupModel.ApiKey;
            appSettingsSection["Phone"] = SetupModel.Phone;
            appSettingsSection["IpDbServer"] = SetupModel.IpDbServer;
            appSettingsSection["PortDbServer"] = SetupModel.PortDbServer;
            appSettingsSection["DbDatabase"] = SetupModel.DbDatabase;
            appSettingsSection["UidDbServer"] = SetupModel.UidDbServer;
            appSettingsSection["PasswordDbServer"] = SetupModel.PasswordDbServer;
            appSettingsSection["ExcelFolder"] = SetupModel.ExcelFolder;
            appSettingsSection["IsLoadExcel"] = SetupModel.IsLoadExcel;
            appSettingsSection["ExpireSoonDay"] = SetupModel.ExpireSoonDay;
            appSettingsSection["ExpireAfterDay"] = SetupModel.ExpireAfterDay;

            // 4) 변환된 JObject를 다시 문자열로 직렬화 (예쁘게 들여쓰기 옵션)
            var output = JsonConvert.SerializeObject(jObject, Formatting.Indented);

            // 5) appsettings.json에 덮어쓰기
            File.WriteAllText(filePath, output);

            // 필요한 경우, Configuration을 다시 Reload할 수도 있음
            // (만약 런타임에 즉시 갱신이 필요하다면)
            _log?.Info("Settings saved successfully!");

            var window = IoC.Get<IWindowManager>();
            var viewModel = IoC.Get<InformViewModel>();
            viewModel.Title = "프로그램 설정 업데이트";
            viewModel.Content = "프로그램 설정을 정상적으로 업데이트 했습니다..";
            await window.ShowDialogAsync(viewModel);
        }
        catch (Exception ex)
        {
            _log?.Error($"Error saving settings: {ex}");
        }
    }
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    public SetupModel? SetupModel
    {
        get { return _setupModel; }
        set { _setupModel = value; NotifyOfPropertyChange(() => SetupModel); }
    }

    #endregion
    #region - Attributes -
    private SetupModel? _setupModel;
    #endregion
}
