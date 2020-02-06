using RedCorners.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using Path = System.IO.Path;
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

        ObjectStorage<Colorize> storage;
        Colorize fx;

        BitmapImage Image { get; set; }

        public Visibility DropTextVisibility => File.Exists(InputPath) ? Visibility.Collapsed : Visibility.Visible;

        public string InputPath
        {
            get => fx.InputPath;
            set
            {
                if (!File.Exists(value)) return;
                fx.InputPath = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DropTextVisibility));
                fx.Load();
                UpdateImage();

                var fi = new FileInfo(value);
                OutputPath = Path.Combine(fi.DirectoryName, $"{Path.GetFileNameWithoutExtension(value)}c{fi.Extension}");
            }
        }

        public string OutputPath
        {
            get => fx.OutputPath;
            set
            {
                fx.OutputPath = value;
                RaisePropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            storage = new ObjectStorage<Colorize>();
            fx = storage.Data;

            fx.Load();
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
                UpdateImage();
            }
        }

        public float Lightness
        {
            get => fx.Lightness;
            set
            {
                fx.Lightness = value;
                RaisePropertyChanged();
                UpdateImage();
            }
        }

        public float Value
        {
            get => fx.Value;
            set
            {
                fx.Value = value;
                RaisePropertyChanged();
                UpdateImage();
            }
        }

        public float Strength
        {
            get => fx.Strength;
            set
            {
                fx.Strength = value;
                RaisePropertyChanged();
                UpdateImage();
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                InputPath = files[0];
            }
        }

        private void btnBrowseInput_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.DefaultExt = "png";
            dialog.ShowDialog();

            InputPath = dialog.FileName;
        }

        private void btnBrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.DefaultExt = "png";
            dialog.ShowDialog();

            OutputPath = dialog.FileName;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            fx.Save();
            storage.Save();
            MessageBox.Show(caption: "Success", messageBoxText: "Save completed.", button: MessageBoxButton.OK);
        }
    }
}
