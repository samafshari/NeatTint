using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeatTint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string m = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(m));

        Colorize fx;

        BitmapImage Image { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            fx = new Colorize
            {
                Saturation = 1.0f,
                Lightness = 0.0f,
                Value = 0.5f,
                TintR = 1,
                TintG = 0,
                TintB = 0,
                InputPath = @"E:\Projects\NeatTint\NeatTint\blackcamera@3x.png"
            };
            UpdateImage();
        }

        void UpdateImage()
        {
            Image = fx.GetBitmapImage();
            RaisePropertyChanged(nameof(Image));
            img.Source = Image;
        }

        public float Red
        {
            get => fx.TintR;
            set
            {
                fx.TintR = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RedBytes));
                RaisePropertyChanged(nameof(Hex));
                UpdateImage();
            }
        }
        
        public float Green
        {
            get => fx.TintG;
            set
            {
                fx.TintG = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(GreenBytes));
                RaisePropertyChanged(nameof(Hex));
                UpdateImage();
            }
        }

        public float Blue
        {
            get => fx.TintB;
            set
            {
                fx.TintB = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(BlueBytes));
                RaisePropertyChanged(nameof(Hex));
                UpdateImage();
            }
        }
        
        public int RedBytes
        {
            get => Colorize.ToByte(fx.TintR);
            set
            {
                fx.TintR = Colorize.ToFloat((byte)value);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Red));
                RaisePropertyChanged(nameof(Hex));
                UpdateImage();
            }
        }
        
        public int GreenBytes
        {
            get => Colorize.ToByte(fx.TintG);
            set
            {
                fx.TintG = Colorize.ToFloat((byte)value);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Green));
                RaisePropertyChanged(nameof(Hex));
                UpdateImage();
            }
        }

        public int BlueBytes
        {
            get => Colorize.ToByte(fx.TintB);
            set
            {
                fx.TintB = Colorize.ToFloat((byte)value);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Blue));
                RaisePropertyChanged(nameof(Hex));
                UpdateImage();
            }
        }

        public string Hex
        {
            get
            {
                var hex = Color.FromRgb((byte)RedBytes, (byte)GreenBytes, (byte)BlueBytes).ToString();
                return "#" + hex.Substring(3);
            }
            set
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(value);
                    RedBytes = color.R;
                    GreenBytes = color.G;
                    BlueBytes = color.B;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(Red));
                    RaisePropertyChanged(nameof(Green));
                    RaisePropertyChanged(nameof(Blue));
                    RaisePropertyChanged(nameof(RedBytes));
                    RaisePropertyChanged(nameof(GreenBytes));
                    RaisePropertyChanged(nameof(BlueBytes));
                    UpdateImage();

                }
                catch
                {

                }
            }
        }

        public float Saturation
        {
            get => fx.Saturation;
            set
            {
                fx.Saturation = value;
                RaisePropertyChanged();
            }
        }

        public float Lightness
        {
            get => fx.Lightness;
            set
            {
                fx.Lightness = value;
                RaisePropertyChanged();
            }
        }

        public float Value
        {
            get => fx.Value;
            set
            {
                fx.Value = value;
                RaisePropertyChanged();
            }
        }
    }
}
