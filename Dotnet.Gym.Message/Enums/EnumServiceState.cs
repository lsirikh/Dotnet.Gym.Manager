using System;

namespace Dotnet.Gym.Message.Enums;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 3/11/2025 11:33:09 AM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public enum EnumServiceState
{
    Pending,           // 결제 또는 승인 대기 상태
    Active,            // 정상적으로 등록되어 이용 가능한 상태
    Expiring_Soon,     // 만료일 7일 이내로 임박한 상태
    Expired,           // 등록 기간 만료된 상태
}