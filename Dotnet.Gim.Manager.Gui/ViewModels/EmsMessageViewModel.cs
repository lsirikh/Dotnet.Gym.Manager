using Caliburn.Micro;
using Dotnet.Gym.Manager.Gui.Commands;
using Dotnet.Gym.Manager.Gui.Models;
using Dotnet.Gym.Message.Accounts;
using Dotnet.Gym.Message.Contacts;
using Dotnet.Gym.Message.Enums;
using Ironwall.Dotnet.Libraries.Api.Aligo.Models;
using Ironwall.Dotnet.Libraries.Api.Aligo.Services;
using Ironwall.Dotnet.Libraries.Base.Services;
using Ironwall.Dotnet.Libraries.Db.Services;
using Ironwall.Dotnet.Libraries.ViewModel.ViewModels.Components;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Input;
using static log4net.Appender.RollingFileAppender;

namespace Dotnet.Gym.Manager.Gui.ViewModels;
/****************************************************************************
   Purpose      :                                                          
   Created By   : GHLee                                                
   Created On   : 2/11/2025 2:45:12 PM                                                    
   Department   : SW Team                                                   
   Company      : Sensorway Co., Ltd.                                       
   Email        : lsirikh@naver.com                                         
****************************************************************************/
public class EmsMessageViewModel : BasePanelViewModel
{
    #region - Ctors -
    public EmsMessageViewModel(List<IEmsMessageModel> models, IEventAggregator eventAggregator, ILogService log) 
        : base(eventAggregator, log)
    {
        DialogOpenedCommand = new RelayCommand<DialogOpenedEventArgs>(OnDialogOpened);
        DialogClosingCommand = new RelayCommand<DialogClosingEventArgs>(OnDialogClosing);

        _models = models;
    }
    #endregion
    #region - Implementation of Interface -
    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        InitizeProcess();
        
        _dbService = IoC.Get<IDbServiceForGym>();
        _aligoService = IoC.Get<IAligoService>();

        _cts = new CancellationTokenSource();

        
        return base.OnActivateAsync(cancellationToken);
    }

    private void InitizeProcess()
    {
        Receivers = new List<string>();

        foreach (var item in _models)
        {
            Receivers.Add(item.Receiver.ToString());
        }

        var model = _models.FirstOrDefault();
        if (model != null)
        {
            NoticeType = model.NoticeType;
            Sender = model.Sender;
            MsgType = model.MsgType;
            SendTime = model.SendTime;
            
        }

        Reservation = DateTime.Now + TimeSpan.FromMinutes(10);
    }
    #endregion
    #region - Overrides -
    #endregion
    #region - Binding Methods -
    #endregion
    #region - Processes -
    public async void ClickCancelButton()
    {
        await TryCloseAsync(false);
    }

    public async void ClickOkButton()
    {

        if (_cts == null || _cts.IsCancellationRequested)
            _cts = new CancellationTokenSource();

        if (_aligoService == null) return;

        SendTime = DateTime.Now;

        foreach (var item in _models)
        {
            item.NoticeType = NoticeType;
            item.Message = Message;
            item.MsgType = MsgType;
            item.SendTime = DateTime.Now;
            item.Reservation = Reservation;
            item.AttachedImages = AttachedImages;
        }

        //// Act
        HttpResponseMessage? response = null;
        if (_models.Count == 1)
        {
            var model = _models.FirstOrDefault();
            if (model == null) return;
            response = await _aligoService.SendEmsMessageAsync(model, isReserve: IsReservation, testMode: false);

        }else if(_models.Count > 1)
        {
            response = await _aligoService.SendMassEmsMessageAsync(_models, isReserve: IsReservation, testMode: false);
        }

        if (response == null) return;
        var responseBody = await response.Content.ReadAsStringAsync();

        //// JSON을 ResponseModel 객체로 변환
        var json = JsonConvert.DeserializeObject<SendResponseModel>(responseBody);
        if (json == null) return;
        if (json.ResultCode != 1)
        {
            _log?.Error($"{json.Message}");
            return;
        }

        if (_dbService == null) return;
        foreach (var item in _models)
        {
            var insertedId = await _dbService.InsertEmsMessageAsync(item, _cts.Token);
        }

        await TryCloseAsync(true);
    }

    // 예: 이미지 추가
    public void AddImage()
    {
        if (AttachedImages == null)
            AttachedImages = new List<ImageModel>();

        if (AttachedImages.Count >= MAX_IMAGES)
            return;

        // OpenFileDialog 생성
        var openFileDialog = new OpenFileDialog
        {
            // JPEG, PNG 만 선택 가능
            Filter = "Image Files (*.jpeg;*.jpg;*.png)|*.jpeg;*.jpg;*.png|All Files (*.*)|*.*",
            // 기본 폴더: 내 문서 경로
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            // 다중 선택을 허용하지 않으면 false
            Multiselect = false
        };

        // 실제로 다중 선택을 허용하려면 Multiselect=true 후 선택된 파일들을 반복문으로 추가하면 됨

        // 다이얼로그 표시
        bool? result = openFileDialog.ShowDialog();

        // 사용자가 파일을 선택하고 "확인"을 눌렀으면
        if (result == true)
        {
            // 선택한 파일 경로
            var path = openFileDialog.FileName;

            // 파일 확장자 확인 (ContentType 지정)
            string extension = Path.GetExtension(path).ToLowerInvariant();
            string contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            // 파일 -> 바이트 배열 -> Base64 문자열 변환
            byte[] fileBytes = File.ReadAllBytes(path);
            string base64String = Convert.ToBase64String(fileBytes);

            // ImageModel 생성
            var newImage = new ImageModel
            {
                Base64Image = base64String,               // Base64 인코딩된 이미지
                FileName = Path.GetFileName(path),        // 순수 파일 이름
                ContentType = contentType                 // MIME 타입 추정
            };

            // 리스트에 추가
            AttachedImages.Add(newImage);
            NotifyOfPropertyChange(() => AttachedImages);
        }
    }

    // 예: 이미지 제거
    public void RemoveImage(ImageModel image)
    {
        if (AttachedImages == null)
            return;

        AttachedImages.Remove(image);
        NotifyOfPropertyChange(() => AttachedImages);
    }

    private void OnDialogOpened(DialogOpenedEventArgs e)
    {
        // e.Session.Content: XAML에서 Button.CommandParameter로 넘긴 Grid 등
        // 열리는 시점에 Calendar/Clock를 초기화하거나, VM 값으로 Binding을 세팅
    }

    private void OnDialogClosing(DialogClosingEventArgs e)
    {
        if (e.Parameter?.ToString() == "1")
        {
            // OK
            var grid = e.Session.Content as Grid;
            if (grid != null && grid.Children.Count > 0)
            {
                var container = grid.Children[0] as StackPanel;
                if (container != null && container.Children.Count >= 2)
                {
                    var calendar = container.Children[0] as Calendar;
                    var clock = container.Children[1] as Clock;

                    if (calendar != null && clock != null && calendar.SelectedDate.HasValue)
                    {
                        var dateOnly = calendar.SelectedDate.Value;
                        var dateTime = clock.Time;
                        var timeSpan = dateTime.TimeOfDay;              
                        Reservation = dateOnly.Add(timeSpan);           
                    }
                }
            }
        }
    }

    #endregion
    #region - IHanldes -
    #endregion
    #region - Properties -
    public EnumNoticeType NoticeType
    {
        get { return _noticeType; }
        set
        {
            _noticeType = value;
            NotifyOfPropertyChange(() => NoticeType);
        }
    }

    public string Sender
    {
        get { return _sender; }
        set
        {
            _sender = value;
            NotifyOfPropertyChange(() => Sender);
        }
    }
    
    public List<string>? Receivers
    {
        get { return _receivers; }
        set { _receivers = value; NotifyOfPropertyChange(() => Receivers); }
    }
    
    public string? Message
    {
        get { return _message; }
        set
        {
            _message = value;
            NotifyOfPropertyChange(() => Message);
        }
    }

    public EnumMsgType MsgType
    {
        get { return _msgType; }
        set
        {
            _msgType = value;
            NotifyOfPropertyChange(() => MsgType);
        }
    }

    public string? Title
    {
        get { return _title; }
        set
        {
            _title = value;
            NotifyOfPropertyChange(() => Title);
        }
    }

    public string? Destination
    {
        get { return _destination; }
        set
        {
            _destination = value;
            NotifyOfPropertyChange(() => Destination);
        }
    }

    public bool IsReservation
    {
        get { return _isReservation; }
        set { _isReservation = value; NotifyOfPropertyChange(() => IsReservation); }
    }


    public DateTime Reservation
    {
        get { return _reservation; }
        set
        {
            _reservation = value;
            NotifyOfPropertyChange(() => Reservation);
        }
    }

    public List<ImageModel>? AttachedImages
    {
        get { return _attachedImages; }
        set
        {
            _attachedImages = value;
            NotifyOfPropertyChange(() => AttachedImages);
        }
    }

    public DateTime SendTime
    {
        get { return _sendTime; }
        set
        {
            _sendTime = value;
            NotifyOfPropertyChange(() => SendTime);
        }
    }

    ///// <summary>
    ///// 예약 날짜만 관리 (시:분:초는 유지)
    ///// </summary>
    //public DateTime ReservationDate
    //{
    //    get => Reservation.Date;
    //    set
    //    {
    //        // 기존 Reservation의 시,분,초를 유지하면서 날짜만 변경
    //        Reservation = new DateTime(value.Year, value.Month, value.Day,
    //            Reservation.Hour, Reservation.Minute, Reservation.Second);
    //    }
    //}

    ///// <summary>
    ///// 예약 시간만 관리 (년/월/일은 유지)
    ///// </summary>
    //public TimeSpan ReservationTime
    //{
    //    get => new TimeSpan(Reservation.Hour, Reservation.Minute, Reservation.Second);
    //    set
    //    {
    //        Reservation = new DateTime(Reservation.Year, Reservation.Month, Reservation.Day,
    //            value.Hours, value.Minutes, value.Seconds);
    //    }
    //}

    public bool CanAddImage => AttachedImages == null || AttachedImages.Count < MAX_IMAGES;

    public ICommand DialogOpenedCommand { get; }
    public ICommand DialogClosingCommand { get; }

    private List<IEmsMessageModel> _models;
    #endregion
    #region - Attributes -
    private EnumNoticeType _noticeType;
    private string? _sender;
    private string? _message;
    private string? _title;
    private string? _destination;
    private List<ImageModel>? _attachedImages;
    private DateTime _reservation;
    private DateTime _sendTime;
    private bool _isReservation;
    private IDbServiceForGym? _dbService;
    private IAligoService? _aligoService;
    private SetupModel? _setupModel;
    private CancellationTokenSource? _cts;
    private List<string>? _receivers;
    private EnumMsgType _msgType;
    private const int MAX_IMAGES = 3;
    #endregion
}