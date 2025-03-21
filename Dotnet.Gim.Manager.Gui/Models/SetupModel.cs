using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet.Gym.Manager.Gui.Models;
/****************************************************************************
       Purpose      : This class has Properties for the basic program settings
       Created By   : GHLee                                                
       Created On   : 1/31/2025 11:01:50 AM                                                     
       Department   : SW Team                                                   
       Company      : Sensorway Co., Ltd.                                       
       Email        : lsirikh@naver.com                                         
 ****************************************************************************/
public class SetupModel
{
    public string Url { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string IpDbServer { get; set; } = string.Empty;
    public int PortDbServer { get; set; } = 3306; //MariaDb based Port
    public string DbDatabase { get; set; } = string.Empty;
    public string UidDbServer { get; set; } = string.Empty;
    public string PasswordDbServer { get; set; } = string.Empty;
    public string ExcelFolder { get; set; } = string.Empty;
    public bool IsLoadExcel { get; set; }
}
