using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;
using System.Windows;

namespace Dotnet.Gym.Manager.Gui.Behaviors;
    /****************************************************************************
       Purpose      :                                                          
       Created By   : GHLee                                                
       Created On   : 3/6/2025 10:34:36 AM                                                    
       Department   : SW Team                                                   
       Company      : Sensorway Co., Ltd.                                       
       Email        : lsirikh@naver.com                                         
    ****************************************************************************/
public static class DialogHostBehaviors
{
    #region DialogOpenedCommand
    public static readonly DependencyProperty DialogOpenedCommandProperty =
        DependencyProperty.RegisterAttached(
            "DialogOpenedCommand",
            typeof(ICommand),
            typeof(DialogHostBehaviors),
            new PropertyMetadata(null, OnDialogOpenedCommandChanged));

    public static void SetDialogOpenedCommand(DependencyObject obj, ICommand value)
        => obj.SetValue(DialogOpenedCommandProperty, value);

    public static ICommand GetDialogOpenedCommand(DependencyObject obj)
        => (ICommand)obj.GetValue(DialogOpenedCommandProperty);

    private static void OnDialogOpenedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DialogHost host)
        {
            // 기존 핸들러 제거
            host.DialogOpened -= Host_DialogOpened;
            // 새 명령이 유효하다면 핸들러 추가
            if (e.NewValue != null)
            {
                host.DialogOpened += Host_DialogOpened;
            }
        }
    }

    private static void Host_DialogOpened(object sender, DialogOpenedEventArgs e)
    {
        var command = GetDialogOpenedCommand(sender as DependencyObject);
        if (command?.CanExecute(e) ?? false)
        {
            command.Execute(e);
        }
    }
    #endregion

    #region DialogClosingCommand
    public static readonly DependencyProperty DialogClosingCommandProperty =
        DependencyProperty.RegisterAttached(
            "DialogClosingCommand",
            typeof(ICommand),
            typeof(DialogHostBehaviors),
            new PropertyMetadata(null, OnDialogClosingCommandChanged));

    public static void SetDialogClosingCommand(DependencyObject obj, ICommand value)
        => obj.SetValue(DialogClosingCommandProperty, value);

    public static ICommand GetDialogClosingCommand(DependencyObject obj)
        => (ICommand)obj.GetValue(DialogClosingCommandProperty);

    private static void OnDialogClosingCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DialogHost host)
        {
            host.DialogClosing -= Host_DialogClosing;
            if (e.NewValue != null)
            {
                host.DialogClosing += Host_DialogClosing;
            }
        }
    }

    private static void Host_DialogClosing(object sender, DialogClosingEventArgs e)
    {
        var command = GetDialogClosingCommand(sender as DependencyObject);
        if (command?.CanExecute(e) ?? false)
        {
            command.Execute(e);
        }
    }
    #endregion
}
