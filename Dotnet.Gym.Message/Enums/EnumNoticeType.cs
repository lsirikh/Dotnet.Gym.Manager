using System;

namespace Dotnet.Gym.Message.Enums;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/3/2025 3:37:45 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public enum EnumNoticeType
{
    /// <summary>
    /// 운동(헬스) 기간이 종료될 때 안내
    /// </summary>
    WorkoutEndNotice,

    /// <summary>
    /// 사물함(Locker) 사용 기간이 종료될 때 안내
    /// </summary>
    LockerEndNotice,

    /// <summary>
    /// 신발장(슈즈락커) 사용 기간이 종료될 때 안내
    /// </summary>
    ShoeLockerEndNotice,

    /// <summary>
    /// 일반 공지
    /// </summary>
    NormalNotice,
}