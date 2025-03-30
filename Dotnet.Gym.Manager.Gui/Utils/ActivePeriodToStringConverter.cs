using Dotnet.Gym.Message.Accounts;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Dotnet.Gym.Manager.Gui.Utils;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/12/2025 11:28:34 AM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class ActivePeriodToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ActivePeriod activePeriod)
        {
            return $"{activePeriod.StartDate:yyyy-MM-dd} ~ {activePeriod.EndDate:yyyy-MM-dd}";
        }
        return "N/A"; // null일 경우 기본 값
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // 편집은 XAML에서 직접 처리하므로 필요 없음
    }
}
