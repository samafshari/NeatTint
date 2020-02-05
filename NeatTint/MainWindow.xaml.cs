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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public double Red
        {
            get => fx.Tint.R / 255.0;
            set
            {
                fx.Tint = Color.FromRgb((byte)(value * 255.0), fx.Tint.G, fx.Tint.B);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RedBytes));
                RaisePropertyChanged(nameof(Hex));
            }
        }
        
        public double Green
        {
            get => fx.Tint.G / 255.0;
            set
            {
                fx.Tint = Color.FromRgb(fx.Tint.R, (byte)(value * 255.0), fx.Tint.B);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(GreenBytes));
                RaisePropertyChanged(nameof(Hex));
            }
        }

        public double Blue
        {
            get => fx.Tint.B / 255.0;
            set
            {
                fx.Tint = Color.FromRgb(fx.Tint.R, fx.Tint.G, (byte)(value * 255.0));
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(BlueBytes));
                RaisePropertyChanged(nameof(Hex));
            }
        }
        
        public int RedBytes
        {
            get => fx.Tint.R;
            set
            {
                fx.Tint = Color.FromRgb((byte)value, fx.Tint.G, fx.Tint.B);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Red));
                RaisePropertyChanged(nameof(Hex));
            }
        }
        
        public int GreenBytes
        {
            get => fx.Tint.G;
            set
            {
                fx.Tint = Color.FromRgb(fx.Tint.R, (byte)value, fx.Tint.B);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Green));
                RaisePropertyChanged(nameof(Hex));
            }
        }

        public int BlueBytes
        {
            get => fx.Tint.B;
            set
            {
                fx.Tint = Color.FromRgb(fx.Tint.R, fx.Tint.G, (byte)value);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Blue));
                RaisePropertyChanged(nameof(Hex));
            }
        }

        public string Hex
        {
            get
            {
                var hex = fx.Tint.ToString();
                return "#" + hex.Substring(3);
            }
            set
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(value);
                    fx.Tint = color;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(Red));
                    RaisePropertyChanged(nameof(Green));
                    RaisePropertyChanged(nameof(Blue));
                    RaisePropertyChanged(nameof(RedBytes));
                    RaisePropertyChanged(nameof(GreenBytes));
                    RaisePropertyChanged(nameof(BlueBytes));
                }
                catch
                {

                }
            }
        }
    }
}
