using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.Graphics;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using PInvoke;
using System.Runtime.InteropServices;
using Windows.Storage.Pickers;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;
using Syncfusion.XlsIO;
using System.ComponentModel;
using static randomtest.BlankPage3;
using Microsoft.UI.Xaml.Media.Animation;
using System.Text.Json;
using Windows.UI.StartScreen;
using System.Threading.Tasks;

namespace randomtest
{
    public class DataWithIndex
    {
        public string Data { get; set; }
        public int Index { get; set; }
    }
    public sealed partial class BlankPage2 : Page, INotifyPropertyChanged
    {

        public DataWithIndex[] Data
        {
            get { return GlobalData.Data; }
            set
            {
                if (GlobalData.Data != value)
                {
                    GlobalData.Data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }
        public DataWithIndex[] data;
        private string address;

        // ����һ�������������洢����

        // ����һ�������������洢��ַ
        public string Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Data = new DataWithIndex[0];
            string jsonData = JsonSerializer.Serialize(Data);
            ApplicationData.Current.LocalSettings.Values["Data"] = jsonData;
        }
        private async Task<int> ColumnLabelToIndex(string columnLabel)
        {
            int index = 0;
            if (columnLabel.Length != 1 || columnLabel[0] < 'A' || columnLabel[0] > 'Z')
            {
                ContentDialog Dialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "����",
                    Content = "����������A~Z�ĵ����ַ�",
                    CloseButtonText = "ȷ��"
                };

                ContentDialogResult result = await Dialog.ShowAsync();
                return -1;
            }
            for (int i = 0; i < columnLabel.Length; i++)
            {
                index *= 26;
                index += columnLabel[i] - 'A' + 1;
            }
            return index - 1;  // 0-based index
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            int l;
            if (int.TryParse(LineInput, out int line))
            {
                l = line;
                if (l <= 0)
                {
                    ContentDialog Dialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "����",
                        Content = "��������Ч���� [int]",
                        CloseButtonText = "ȷ��"
                    };

                    ContentDialogResult result = await Dialog.ShowAsync();
                    return;
                }
            }
            else
            {
                ContentDialog Dialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "����",
                    Content = "��������Ч���� [int]",
                    CloseButtonText = "ȷ��"
                };

                ContentDialogResult result = await Dialog.ShowAsync();
                return;
            }
            FileOpenPicker fileOpenPicker = new()
            {
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { ".xls", ".xlsx", ".xlsm", ".xlsb" },
            };

            nint windowHandle = WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);

            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            if (file != null)
            {
              
                 Address = file.Path;  // ���µ�ַ

                // ����һ��ExcelEngineʵ��

                using (ExcelEngine excelEngine = new ExcelEngine())
                {                
                    IApplication application = excelEngine.Excel;

                    // ��Excel�ļ�
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        IWorkbook workbook = application.Workbooks.Open(stream);

                        // ��ȡ��һ��������
                        IWorksheet worksheet = workbook.Worksheets[0];

                        int columnIndex = await ColumnLabelToIndex(ColInput);
                        if (columnIndex < 0) return;
                        if (worksheet.Columns.Length <= columnIndex)
                        {
                            ContentDialog Dialog = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "����",
                                Content = "�����Ʋ�����",
                                CloseButtonText = "ȷ��"
                            };

                            ContentDialogResult result = await Dialog.ShowAsync();
                            return;
                        }
                       IRange[] cells = worksheet.Columns[columnIndex].Cells;
                        if (l > cells.Length)
                        {
                            ContentDialog Dialog = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot,
                                Title = "����",
                                Content = "��ʼλ�ó�����Χ",
                                CloseButtonText = "ȷ��"
                            };

                            ContentDialogResult result = await Dialog.ShowAsync();
                            return;
                        }
                        // ����һ���������洢����
                        Data = new DataWithIndex[cells.Length-l+1];
                        int cnt = 0;
                        // �����ݴӵ�Ԫ���Ƶ�����
                        for (int i = l-1; i < cells.Length; i++)
                        {
                            Data[i+1-l] = new DataWithIndex { Data = cells[i].Value, Index = ++cnt};
                        }


                    }
                }
            }
        }


        public BlankPage2()
        {
            this.InitializeComponent();
            this.DataContext = this;  // ������д���
            Address = "��֧��Excel�ļ�";
        }
        public string LineInput
        {
            get { return lineInput; }
            set
            {
                if (lineInput != value)
                {
                    lineInput = value;
                    ApplicationData.Current.LocalSettings.Values["LineInput"] = LineInput;
                    OnPropertyChanged(nameof(LineInput));
                }
            }
        }
        private string lineInput="2";
            
        public string ColInput
        {
            get { return colInput; }
            set
            {
                if (colInput != value)
                {
                    colInput = value;
                    ApplicationData.Current.LocalSettings.Values["ColInput"] = ColInput;
                    OnPropertyChanged(nameof(ColInput));
                }
            }
        }
        private string colInput="A";

        private void UpdateLine(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            LineInput = textBox.Text;
        }

        private void UpdateCol(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            ColInput = textBox.Text;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            string jsonData = JsonSerializer.Serialize(Data);
            ApplicationData.Current.LocalSettings.Values["Data"] = jsonData;
            ApplicationData.Current.LocalSettings.Values["LineInput"] = LineInput;
            ApplicationData.Current.LocalSettings.Values["ColInput"] = ColInput;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Data"))
            {
                string jsonData = (string)ApplicationData.Current.LocalSettings.Values["Data"];
                Data = JsonSerializer.Deserialize<DataWithIndex[]>(jsonData);
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("LineInput"))
            {
                LineInput = (string)ApplicationData.Current.LocalSettings.Values["LineInput"];
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("ColInput"))
            {
                ColInput = (string)ApplicationData.Current.LocalSettings.Values["ColInput"];
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
    }
}
