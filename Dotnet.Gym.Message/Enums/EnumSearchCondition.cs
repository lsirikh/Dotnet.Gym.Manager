using System;

namespace Dotnet.Gym.Message.Enums;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/12/2025 5:12:07 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public enum EnumSearchCondition
{
    None,           // 없음
    Name,           // 이름
    PhoneNumber,    // 전화번호
    ExpiringSoon,   // 만료 7일 전
    Expired         // 만료됨
}