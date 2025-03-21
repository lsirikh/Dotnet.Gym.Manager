using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using Caliburn.Micro;
using Dotnet.Gym.Manager.Gui.Models;
using Dotnet.Gym.Manager.Gui.ViewModels;
using Dotnet.Gym.Manager.Gui.ViewModels.Components;
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
    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        base.OnStartup(sender, e);
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
            var setup = new SetupModel();
            _configuration?.GetSection("AppSettings").Bind(setup);
            builder.RegisterInstance(setup).AsSelf().SingleInstance();

            builder.RegisterModule(new DbModule(_log, setup.IpDbServer, setup.PortDbServer, setup.DbDatabase, setup.UidDbServer, setup.PasswordDbServer)); // 2
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
            builder.RegisterType<SetupViewModel>().SingleInstance();
            builder.RegisterType<LockerInfoViewModel>().SingleInstance();
            builder.RegisterType<ConfirmViewModel>().SingleInstance();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected override void StartPrograme()
    {
        DispatcherService.Invoke((System.Action)(async () =>
        {
            _log.Info($"ShellViewModel will be activated...");
            await DisplayRootViewForAsync<ShellViewModel>();

        }));
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
    #endregion
    #region - Attributes -
    private IConfigurationRoot? _configuration;
    #endregion
}
