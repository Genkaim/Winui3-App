using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.


namespace randomtest
{
    public static class GlobalData
    {
        public static DataWithIndex[] Data { get; set; }
    }
    public class NumberWithIndex
    {
        public int Value { get; set; }
        public int Index { get; set; }
        public DataWithIndex CorrespondingData
        {
            get
            {
                if (GlobalData.Data != null && GlobalData.Data.Length != 0)
                {
                    return GlobalData.Data[Value - 1];
                }
                else
                {
                    return new DataWithIndex { Data = "", Index = 0 };
                }
            }
        }
        public Thickness ValueMargin
        {
            get
            {
                return string.IsNullOrEmpty(CorrespondingData.Data) ? new Thickness(115, -1, 0, 0) : new Thickness(-30, -1, 0, 0);
            }
        }
    }



    public sealed partial class BlankPage3 : Page
    {
        private Dictionary<string, int> Dictionary = new Dictionary<string, int>();

        private ViewModel viewModel;
        public class ViewModel : INotifyPropertyChanged
        {
            private int _num;

            public int num
            {
                get { return _num; }
                set
                {
                    if (_num != value)
                    {
                        _num = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _limit = 48;
            public int Limit
            {
                get { return _limit; }
                set
                {
                    if (_limit != value)
                    {
                        _limit = value;
                        ApplicationData.Current.LocalSettings.Values["Limit"] = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ApplicationData.Current.LocalSettings.Values["Limit"] = viewModel.Limit;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (GlobalData.Data != null && GlobalData.Data.Length != 0)
            {
                viewModel.Limit = GlobalData.Data.Length;
            }
            else if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Limit"))
            {
                viewModel.Limit = (int)ApplicationData.Current.LocalSettings.Values["Limit"];
            }
        }


        private DispatcherTimer timer;
        public ObservableCollection<NumberWithIndex> numList { get; } = new ObservableCollection<NumberWithIndex>();
        public BlankPage3()
        {
            this.InitializeComponent();

            viewModel = new ViewModel { num = 114, Limit = 48 };
            this.DataContext = viewModel;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.01); // ���ö�ʱ���ļ��
            timer.Tick += Timer_Tick; // ���ö�ʱ���� Tick �¼�������

        }

        private void Timer_Tick(object sender, object e)
        {
            viewModel.num = new Random().Next(1, viewModel.Limit+1); // ����һ�������
        }
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (startButtonIcon.Glyph == "��")
            {
                startButtonIcon.Glyph = "��";
                timer.Start(); // ������ʱ��
            }
            else
            {
                startButtonIcon.Glyph = "��";
                timer.Stop(); // ֹͣ��ʱ��

                // ����ǰ�� num ֵ��ӵ�������
                numList.Add(new NumberWithIndex { Value = viewModel.num, Index = numList.Count + 1 });

            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                // ����һ���µ� TranslateTransform ������Ӧ�õ� Border
                TranslateTransform transform = new TranslateTransform();
                border.RenderTransform = transform;

                // ����һ���µĶ������ı� TranslateTransform �� Y ����
                DoubleAnimation translateYAnimation = new DoubleAnimation
                {
                    From = -8,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.15))
                };

                // ����һ���µĶ������ı� Border �� Opacity ����
                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.15))
                };

                // ������Ӧ�õ� TranslateTransform �� Border
                Storyboard.SetTarget(translateYAnimation, transform);
                Storyboard.SetTargetProperty(translateYAnimation, "Y");
                Storyboard.SetTarget(opacityAnimation, border);
                Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

                // ����һ���µ� Storyboard ����ʼ����
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(translateYAnimation);
                storyboard.Children.Add(opacityAnimation);
                storyboard.Begin();
            }
        }
        private bool IsDialogOpen = false;
        private async void UpdateLimit(object sender, TextChangedEventArgs e)
        {
            if (GlobalData.Data != null && GlobalData.Data.Length != 0)
            {
                viewModel.Limit = GlobalData.Data.Length;

                if (!IsDialogOpen)
                {
                    IsDialogOpen = true;
                    ContentDialog Dialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "����",
                        Content = "�����ݰ�ʱ�޷��Զ����������Χ",
                        CloseButtonText = "ȷ��"
                    };

                    ContentDialogResult result = await Dialog.ShowAsync();  // �ȴ�ContentDialog�ر�
                    IsDialogOpen = false;
                }
                return;
            }
            if (int.TryParse(NumberInput.Text, out int maxNumber) && (GlobalData.Data == null || GlobalData.Data.Length == 0))
            {
                viewModel.Limit = maxNumber;
            }
            else
            {
                // ��� TextBox ��ֵ����һ����Ч����������ʾһ��������Ϣ
                if (!IsDialogOpen && NumberInput.Text!="")
                {
                    IsDialogOpen = true;
                    ContentDialog Dialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "����",
                        Content = "��������Ч���� [int]",
                        CloseButtonText = "ȷ��"
                    };

                    ContentDialogResult result = await Dialog.ShowAsync();
                    IsDialogOpen = false;
                }
            }
        }



            private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                // ����һ���µĶ������ı� Border �� Height ����
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = 70,
                    To = 114,
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };

                // ������Ӧ�õ� Border
                Storyboard.SetTarget(heightAnimation, border);
                Storyboard.SetTargetProperty(heightAnimation, "Height");

                // ����һ���µ� Storyboard ����ʼ����
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(heightAnimation);
                storyboard.Begin();
            }
        }


    }

}
