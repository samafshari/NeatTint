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
    public class TransparencyEffect : ShaderEffect
    {
        public TransparencyEffect()
        {
            var ps = new PixelShader();
            ps.UriSource = new Uri("pack://application:,,,/NeatTint;component/Transparency.ps");
            this.PixelShader = ps;
        }


        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(Input), typeof(TransparencyEffect), 0);

        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(TransparencyEffect),
              new UIPropertyMetadata(1.0d, PixelShaderConstantCallback(0)));
    }
}
