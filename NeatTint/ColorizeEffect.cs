using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace NeatTint
{
    public class ColorizeEffect : ShaderEffect
    {
        public ColorizeEffect()
        {
            var ps = new PixelShader();
            ps.UriSource = new Uri("pack://application:,,,/NeatTint;component/Colorize.ps");
            this.PixelShader = ps;

            UpdateValues();
        }

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(Input), typeof(ColorizeEffect), 0);

        public double Saturation
        {
            get { return (double)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(ColorizeEffect),
              new UIPropertyMetadata(1.0d, PixelShaderConstantCallback(0)));

        public double Lightness
        {
            get { return (double)GetValue(LightnessProperty); }
            set { SetValue(LightnessProperty, value); }
        }

        public static readonly DependencyProperty LightnessProperty =
            DependencyProperty.Register(nameof(Lightness), typeof(double), typeof(ColorizeEffect),
              new UIPropertyMetadata(0.0d, PixelShaderConstantCallback(1)));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ColorizeEffect),
              new UIPropertyMetadata(0.5d, PixelShaderConstantCallback(2)));

        public Color Tint
        {
            get { return (Color)GetValue(TintProperty); }
            set { SetValue(TintProperty, value); }
        }

        public static readonly DependencyProperty TintProperty =
            DependencyProperty.Register(nameof(Tint), typeof(Color), typeof(ColorizeEffect),
              new UIPropertyMetadata(Colors.Red, PixelShaderConstantCallback(3)));

        void UpdateValues()
        {
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(SaturationProperty);
            UpdateShaderValue(LightnessProperty);
            UpdateShaderValue(ValueProperty);
            UpdateShaderValue(TintProperty);
        }
    }
}
