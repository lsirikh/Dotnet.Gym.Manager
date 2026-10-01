# Dotnet Gym Manager

운동시설의 회원 정보와 이용 기간을 관리하고 문자 발송 서비스를 연결하는 .NET 8 / WPF 애플리케이션입니다. 화면 처리와 회원·메시지 모델을 별도 프로젝트로 나눴습니다.

## 구성

- [Dotnet.Gym.Manager.Gui](Dotnet.Gym.Manager.Gui): 회원 관리 화면과 서비스 연결
- [Dotnet.Gym.Message](Dotnet.Gym.Message): 회원, 이용 기간, 사물함, 문자 관련 데이터 모델
- `asp_example`: 외부 문자 서비스 연동 참고 자료

## 개발 환경

Windows / .NET 8, WPF, Caliburn.Micro, Autofac, ClosedXML을 사용합니다. Aligo 문자 API와 데이터베이스 연결은 별도 설정이 필요합니다.

현재 `.csproj`에는 외부 `Ironwall.Dotnet.Libraries` 프로젝트 참조가 들어 있습니다. 관련 라이브러리와 참조 경로를 준비한 뒤 빌드하세요. 실제 문자 발송은 계정 설정과 발송 대상 확인 후 사용해야 합니다.
