using System;
using System.Windows.Input;

namespace Dotnet.Gym.Manager.Gui.Commands{
    /****************************************************************************
       Purpose      :                                                          
       Created By   : GHLee                                                
       Created On   : 3/6/2025 10:39:24 AM                                                    
       Department   : SW Team                                                   
       Company      : Sensorway Co., Ltd.                                       
       Email        : lsirikh@naver.com                                         
    ****************************************************************************/
    public class RelayCommand<T> : ICommand
    {
        #region - Ctors -
        /// <summary>
        /// RelayCommand 생성자
        /// </summary>
        /// <param name="execute">실행할 메서드를 람다식으로 전달</param>
        /// <param name="canExecute">메서드 실행 가능 여부를 판단하는 람다식(옵션)</param>
        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            // 필수 인자를 체크하여 null 방지
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;  // null이면 실행 가능 여부 검사 없이 언제나 실행 가능
        }
        #endregion
        #region - Implementation of Interface -
        #endregion
        #region - Overrides -
        #endregion
        #region - Binding Methods -
        #endregion
        #region - Processes -
        /// <summary>
        /// ICommand.CanExecute 구현
        /// 이 커맨드가 현재 실행 가능 상태인지 여부를 반환합니다.
        /// </summary>
        /// <param name="parameter">UI에서 전달된 파라미터</param>
        /// <returns>실행 가능 여부 (true/false)</returns>
        public bool CanExecute(object? parameter)
        {
            // _canExecute가 없다면 항상 실행 가능
            if (_canExecute == null)
                return true;

            // parameter가 T 타입으로 변환되는 경우에만 실행 가능 여부 검사
            if (parameter is T validParam)
                return _canExecute(validParam);

            // 타입이 맞지 않으면 false 처리 (또는 true로 가정할 수도 있음)
            return false;
        }

        /// <summary>
        /// ICommand.Execute 구현
        /// </summary>
        /// <param name="parameter">UI에서 전달된 파라미터</param>
        public void Execute(object? parameter)
        {
            // parameter를 T로 캐스팅 성공 시, 실제 메서드(_execute) 호출
            if (parameter is T validParam)
            {
                _execute(validParam);
            }
            else
            {
                // 만약 파라미터가 없어도 실행이 가능하다면, default(T)로 전달
                // 필요하다면 아래 주석을 풀거나, 예외를 던지는 식으로 처리 가능
                // _execute(default(T)!);

                // 또는 parameter가 null이지만 T가 Nullable일 수도 있음을 고려
                // 여기서는 아무 동작 없이 종료
            }
        }

        /// <summary>
        /// CanExecuteChanged 이벤트를 수동으로 발생시켜
        /// 버튼 등 UI 요소가 다시 실행 가능 상태를 갱신하도록 합니다.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        #region - IHanldes -
        #endregion
        #region - Properties -
        #endregion
        #region - Attributes -
        /// <summary>
        /// CanExecute 상태가 바뀌면 UI에 알리는 이벤트입니다.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        // 실제로 실행될 메서드를 람다식(Action<T>) 형태로 보관
        private readonly Action<T> _execute;

        // 실행 가능 여부를 판단하는 함수를 Func<T, bool> 형태로 보관 (선택적)
        private readonly Func<T, bool>? _canExecute;
        #endregion

    }
}