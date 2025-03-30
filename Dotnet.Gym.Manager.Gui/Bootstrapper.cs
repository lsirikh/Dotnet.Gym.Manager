using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using Caliburn.Micro;
using Dotnet.Gym.Manager.Gui.Models;
using Dotnet.Gym.Manager.Gui.ViewModels;
using Dotnet.Gym.Manager.Gui.ViewModels.Components;
using Dotnet.Gym.Manager.Gui.ViewModels.SplashScreen;
using Dotnet.Gym.Message.Providers;
using Ironwall.Dotnet.Libraries.Api.Aligo.Modules;
using Ironwall.Dotnet.Libraries.Api.Models;
using Ironwall.Dotnet.Libraries.Base;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.Db.Modules;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dotnet.Gym.Manager.Gui;
/****************************************************************************
       Purpose      : This class is an implementation class that oversees 
                      the process of registering and starting Caliburn.Micro's 
                      instances. Register here for various instances to be used
                      for actual projects.                                                         
       Created By   : GHLee                                                
       Created On   : 1/31/2025 11:01:50 AM                                                    
       Department   : SW Team                                                   
       Company      : Sensorway Co., Ltd.                                       
       Email        : lsirikh@naver.com                                         
    ****************************************************************************/
internal class Bootstrapper : ParentBootstrapper<ShellViewModel>
{
    #region - Ctors -
    public Bootstrapper()
    {
        Initialize();
    }

    #endregion
    #region - Implementation of Interface -
    #endregion
    #region - Overrides -
#if DEBUG

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        try
        {
            //var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            //if (entryAssembly != null)
            //{
            //    var attribute = entryAssembly.CustomAttributes.ToList()[3];
            //    var projectName = attribute.ConstructorArguments[0];
            //    Process[] procs = Process.GetProcessesByName($"{projectName.Value}");

            //    // 두번 이상 실행되었을 때 처리할 내용을 작성합니다.
            //    if (procs.Length > 1)
            //    {
            //        var t = (AssemblyTitleAttribute)entryAssembly.GetCustomAttributes(true)[3];
            //        MessageBox.Show($"{t.Title} is already running...", "Redundant Execution");
            //        Application.Current.Shutdown();
            //    }
            //}


            // 현재 실행 중인 프로세스
            using var current = Process.GetCurrentProcess();
            var processName = current.ProcessName;

            // 동일 프로세스 이름으로 동작하는 프로세스들
            var procs = Process.GetProcessesByName(processName);

            // 2개 이상이면 중복 실행
            if (procs.Length > 1)
            {
                // AssemblyTitle을 통해 사용자에게 표시할 앱 이름 가져오기
                var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                var titleAttribute = entryAssembly?.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), true)
                                                  .OfType<System.Reflection.AssemblyTitleAttribute>()
                                                  .FirstOrDefault();

                var appTitle = titleAttribute?.Title ?? processName;

                MessageBox.Show($"{appTitle} is already running...", "Redundant Execution",
                                MessageBoxButton.OK, MessageBoxImage.Warning);

                // 중복 실행 방지 → 즉시 종료
                Application.Current.Shutdown();
                return;
            }

        }
        catch (Exception ex)
        {
            _log.Error($"중복실행 확인 중 오류 발생: {ex.Message}");
        }

        base.OnStartup(sender, e);
    }
#else

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
            //var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            //if (entryAssembly != null)
            //{
            //    var attribute = entryAssembly.CustomAttributes.ToList()[3];
            //    var projectName = attribute.ConstructorArguments[0];
            //    Process[] procs = Process.GetProcessesByName($"{projectName.Value}");

            //    // 두번 이상 실행되었을 때 처리할 내용을 작성합니다.
            //    if (procs.Length > 1)
            //    {
            //        var t = (AssemblyTitleAttribute)entryAssembly.GetCustomAttributes(true)[3];
            //        MessageBox.Show($"{t.Title} is already running...", "Redundant Execution");
            //        Application.Current.Shutdown();
            //    }
            //}

            // 현재 실행 중인 프로세스 이름 구하기
            using var current = Process.GetCurrentProcess();
            var processName = current.ProcessName;

            // 동일 프로세스 이름으로 동작하는 프로세스들
            var procs = Process.GetProcessesByName(processName);

            // 2개 이상이면 중복 실행
            if (procs.Length > 1)
            {
                // AssemblyTitle을 통해 사용자에게 표시할 앱 이름 가져오기
                var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                var titleAttribute = entryAssembly?.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), true)
                                                  .OfType<System.Reflection.AssemblyTitleAttribute>()
                                                  .FirstOrDefault();

                var appTitle = titleAttribute?.Title ?? processName;

                MessageBox.Show($"{appTitle} is already running...", "Redundant Execution",
                                MessageBoxButton.OK, MessageBoxImage.Warning);

                // 중복 실행 방지 → 즉시 종료
                Application.Current.Shutdown();
                return;
            }


            SplashScreenViewModel = IoC.Get<SplashScreenViewModel>();

                var W = new WindowManager();
                await W.ShowWindowAsync(SplashScreenViewModel);
                SplashScreenViewModel.SplashScreenText = $"Now {SplashScreenViewModel.Title} is loading...";
            }
            catch (Exception ex)
            {
                _log.Error($"중복실행 확인 중 오류 발생: {ex.ToString()}");
            }
            
            base.OnStartup(sender, e);
        }
#endif

    protected override void StartPrograme()
    {
        DispatcherService.Invoke((System.Action)(async () =>
        {

#if DEBUG
            _log.Info($"ShellViewModel will be activated...");
            await DisplayRootViewForAsync<ShellViewModel>();
#else
            await DisplayRootViewForAsync<ShellViewModel>();
            if(SplashScreenViewModel != null)
                await SplashScreenViewModel.TryCloseAsync();
#endif
        }));
    }

    protected override void OnExit(object sender, EventArgs e)
    {
        base.OnExit(sender, e);
    }

    protected override void Configure()
    {
        // 설정 파일 로드
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();

        base.Configure();
    }

    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        try
        {
            builder.RegisterType<SplashScreenViewModel>().SingleInstance();

            var setup = new SetupModel();
            _configuration?.GetSection("AppSettings").Bind(setup);
            builder.RegisterInstance(setup).AsSelf().SingleInstance();

            builder.RegisterModule(new DbModule(_log, setup.IpDbServer, setup.PortDbServer, setup.DbDatabase, setup.UidDbServer, setup.PasswordDbServer, setup.ExcelFolder, setup.IsLoadExcel)); // 2
            builder.RegisterModule(new AligoModule(_log, new ApiSetupModel()
            {
                Url = setup.Url,
                Username = setup.Username,
                Password = setup.Password,
                ApiKey = setup.ApiKey,
                Phone = setup.Phone,
            })); // 2

            builder.RegisterType<UserProvider>().SingleInstance();
            builder.RegisterType<EmsMessageProvider>().SingleInstance();
            
            builder.RegisterType<UserInfoListViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            builder.RegisterType<LockerInfoViewModel>().SingleInstance();
            builder.RegisterType<ConfirmViewModel>().SingleInstance();
            builder.RegisterType<InformViewModel>().SingleInstance();
        }
        catch (Exception)
        {
            throw;
        }
    }


    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -


    protected override IEnumerable<Assembly> SelectAssemblies()
    {

        var currentDirectory = Directory.GetCurrentDirectory();
        var additionalAssemblies = new List<Assembly>();

        try
        {
            //var ollamaAssemblyPath = Path.Combine(currentDirectory, "Ironwall.Libraries.Dotnet.Ollama.Ui.dll");
            //if (File.Exists(ollamaAssemblyPath))
            //{
            //    additionalAssemblies.Add(Assembly.LoadFrom(ollamaAssemblyPath));
            //}
            //else
            //{
            //    _log.Error($"Assembly not found at path: {ollamaAssemblyPath}");
            //}
        }
        catch (Exception ex)
        {
            _log.Error($"Error loading assemblies: {ex.Message}");
            throw;
        }

        return new[] { Assembly.GetExecutingAssembly() }
            .Concat(base.SelectAssemblies())
            .Concat(additionalAssemblies);
    }
    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    public SplashScreenViewModel? SplashScreenViewModel { get; private set; }
    #endregion
    #region - Attributes -
    private IConfigurationRoot? _configuration;
    #endregion
}
