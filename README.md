# Dotnet Gym Manager

### Current Version : v1.2.0

### Goal
> 운동시설을 관리하기 위한 기본 SW이다. 현재는 Aligo를 문자 보내기 위한 기본 기능을 중심으로 회원 등록 및 관리 기능이 중심이다.

### Site : Common

<hr>

## 1. Dotnet.Gym.Message 소개

### 1.1 개요
`Dotnet.Gym.Message`는 **Gym Manager 시스템의 메시지 구조를 정의하는 라이브러리**입니다.  
사용자, 문자 메시지, 이미지 및 기타 다양한 데이터를 포함하는 **데이터 모델과 Enum 정의**를 포함하고 있습니다.  

이 라이브러리는 `Newtonsoft.Json`을 사용하여 **JSON 직렬화 및 역직렬화** 기능을 제공하며,  
`Ironwall.Dotnet.Libraries.Base`를 기반으로 동작합니다.

#### 1.2. 프로젝트 구성

#### **📂 Accounts**
> 사용자의 계정 관련 모델 정의

- `ActivePeriod.cs`  
  - 사용자의 **활동 기간**을 저장하는 모델
- `IActivePeriod.cs`  
  - `ActivePeriod`의 인터페이스
- `LockerModel.cs`  
  - 사용자 **락커 정보** 모델
- `ILockerModel.cs`  
  - `LockerModel`의 인터페이스
- `UserModel.cs`  
  - 사용자 정보를 저장하는 모델
- `IUserModel.cs`  
  - `UserModel`의 인터페이스

#### **📂 Base**
> 기본 모델 정의

- `BaseModel.cs`  
  - **모든 모델의 기본 클래스**

#### **📂 Contacts**
> 문자 메시지 및 이미지 데이터 모델 정의

- `EmsMessageModel.cs`  
  - EMS 문자 메시지 정보를 저장하는 모델
- `IEmsMessageModel.cs`  
  - `EmsMessageModel`의 인터페이스
- `ImageModel.cs`  
  - **이미지 정보**를 저장하는 모델
- `IImageModel.cs`  
  - `ImageModel`의 인터페이스

#### **📂 Enums**
> 시스템에서 사용되는 Enum 정의

- `EnumGenderType.cs`  
  - **성별(Enum)** (`MALE`, `FEMALE`, `NONE`)
- `EnumMsgType.cs`  
  - **문자 메시지 유형(Enum)** (`SMS`, `LMS`, `MMS`)
- `EnumNoticeType.cs`  
  - **공지 유형(Enum)** (`GENERAL`, `WARNING`, `ERROR`)
- `EnumSearchCondition.cs`  
  - **검색 조건(Enum)**
- `EnumServiceState.cs`  
  - **서비스 상태(Enum)**
- `EnumTrueFalse.cs`  
  - **참/거짓 Enum**

#### **📂 Providers**
> 메시지 및 사용자 데이터 제공자

- `EmsMessageProvider.cs`  
  - EMS 문자 메시지 관리 및 저장소
- `UserProvider.cs`  
  - 사용자 데이터 관리 및 저장소

#### 개발 환경
- **.NET Version**: `net8.0-windows`
- **언어**: `C#`
- **JSON 직렬화/역직렬화**: `Newtonsoft.Json`
- **기반 라이브러리**: `Ironwall.Dotnet.Libraries.Base`

---

## 2. Dotnet.Gym.Manager.Gui 소개

### 2.1 개요
`Dotnet.Gym.Manager.Gui`는 **Gym Manager 시스템의 GUI 애플리케이션**으로,  
실제 실행 파일이 생성되는 프로젝트입니다.  
이 프로젝트는 WPF 기반으로 개발되었으며, **MVVM 패턴**을 따릅니다.

**주요 특징:**
- `Caliburn.Micro`를 활용한 MVVM 아키텍처 적용
- `MahApps.Metro` 및 `Material Design` UI 라이브러리 사용
- `Autofac`을 이용한 DI(Dependency Injection) 적용
- `Microsoft.Extensions.Configuration`을 사용한 설정 관리

#### 2.2. 프로젝트 구성

#### **📂 Behaviors**
> WPF 동작 관련 사용자 정의 Behavior

- `DialogHostBehaviors.cs`  
  - **DialogHost**를 제어하는 Behavior
- `NumericOnlyBehavior.cs`  
  - 숫자 입력만 허용하는 Behavior

#### **📂 Commands**
> MVVM 패턴에서 사용되는 커맨드 클래스

- `RelayCommand.cs`  
  - **명령 실행을 위한 기본 클래스**

#### **📂 Models**
> 애플리케이션의 데이터 모델 정의

- `SetupModel.cs`  
  - **환경 설정 모델**

#### **📂 Resources**
> XAML 스타일 및 리소스 관리

- `Generic.xaml`  
  - **공통 스타일 정의**

#### **📂 Utils**
> 유틸리티 클래스

- `ActivePeriodToStringConverter.cs`  
  - ActivePeriod 데이터를 문자열로 변환하는 **ValueConverter**

#### **📂 ViewModels**
> MVVM 패턴의 `ViewModel` 폴더

##### 📂 Components
> **각 기능별 ViewModel 정의**

- `AddUserViewModel.cs`  
  - 사용자 추가 기능 ViewModel
- `EmsMessageViewModel.cs`  
  - EMS 메시지 관리 ViewModel
- `LockerInfoViewModel.cs`  
  - **락커 정보 관리 ViewModel**
- `SettingsViewModel.cs`  
  - **설정 관리 ViewModel**
- `SetupViewModel.cs`  
  - **초기 설정 ViewModel**
- `ShellViewModel.cs`  
  - **애플리케이션 Shell ViewModel**
- `UpdateServiceViewModel.cs`  
  - **서비스 업데이트 관련 ViewModel**
- `UserEditViewModel.cs`  
  - **사용자 정보 수정 ViewModel**
- `UserInfoListViewModel.cs`  
  - **사용자 목록 관리 ViewModel**
- `UserViewModel.cs`  
  - **개별 사용자 관리 ViewModel**

#### **📂 Views**
> **WPF UI 화면 (XAML)**
##### 📂 Components
> **다양한 UI 컴포넌트 화면**

- `ConfirmView.xaml`  
  - **확인 창**
- `AddUserView.xaml`  
  - **사용자 추가 화면**
- `EmsMessageView.xaml`  
  - **EMS 메시지 화면**
- `LockerInfoView.xaml`  
  - **락커 정보 화면**
- `SettingsView.xaml`  
  - **설정 화면**
- `SetupView.xaml`  
  - **초기 설정 화면**
- `ShellView.xaml`  
  - **메인 컨테이너**
- `UpdateServiceView.xaml`  
  - **서비스 업데이트 화면**
- `UserEditView.xaml`  
  - **사용자 정보 수정 화면**
- `UserInfoListView.xaml`  
  - **사용자 목록 화면**
  
#### 개발 환경
- **.NET Version**: `net8.0-windows7.0`
- **언어**: `C#`
- **UI 프레임워크**: `WPF`
- **MVVM 패턴 적용**: `Caliburn.Micro`
- **의존성 주입 (DI)**: `Autofac`
- **설정 관리**: `appsettings.json`


#### 설정 파일 (appsettings.json)

다음 내용을 포함한 `appsettings.json` 파일을 `Dotnet.Gym.Manager.Gui` 폴더 내부에 생성해 두어야 합니다.

```json
{
    "AppSettings": {
        "Url": "https://apis.aligo.in/", // Aligo API Url   
        "Username": "User", // Aligo API ID  
        "Password": "",
        "ApiKey": "imafakeapikey",  // Aligo API 키
        "Phone": "", // Aligo API Sender 연락처  
        "IpDbServer": "127.0.0.1", // Maria DB IP 주소
        "PortDbServer": 3306, // Maria DB Port
        "DbDatabase": "gymdb", // Maria DB 데이터베이스 이름
        "UidDbServer": "root",  // Maria DB 사용자 ID
        "PasswordDbServer": "****", // Maria DB 사용자 비밀번호
        "ExcelFolder": "C:\\Users\\", // Excel 폴더 경로 ("회원"이라는 단어가 포함된 첫 번째 엑셀 파일)
        "IsLoadExcel": true // 실행 시 Excel 파일을 파싱할지 여부
    }
}
```

---
### Update Date: 2025/03/16
* Version : v1.0.0

1. 초기 설정 및 Initial update...
2. Readme.md 파일 구성

---
### Update Date: 2025/03/30
* Version : v1.2.0

1. Ironwall.Dotnet.Libraries의 Ironwall.Dotnet.Libraries.Db에 ExcelImporter 연동 로직 구현
2. ExpireSoonDay Settings 설정 업데이트 : x Day 해당일을 설정하면 오늘 날짜로 부터 ExpireSoonDay이후에 서비스 마감되는 사람을 필터링할 수있다(마감일 기준).
3. ExpireAfterDay Setting 설정 업데이트 : -x Day 해당일을 설정하면 오늘이전 부터 오늘 - x Day 만큼 기간을 조회 해준다(마감일 기준). 

<hr>
