using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AnimationHintTextbox.UI.Units;

public class AnimationHintTextbox : TextBox
{
    public static readonly DependencyProperty CornerRadiusProperty;
    public static readonly DependencyProperty HintTextColorProperty;
    public static readonly DependencyProperty HintTextProperty;
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue (CornerRadiusProperty); }
        set { SetValue (CornerRadiusProperty, value); }
    }

    public SolidColorBrush HintTextColor
    {
        get { return (SolidColorBrush)GetValue (HintTextColorProperty); }
        set { SetValue (HintTextColorProperty, value); }
    }

    public string HintText
    {
        get { return (string)GetValue (HintTextProperty); }
        set { SetValue (HintTextProperty, value); }
    }

    static AnimationHintTextbox()
    {
        CornerRadiusProperty = DependencyProperty.Register ("CornerRadius", typeof (CornerRadius), typeof (AnimationHintTextbox), new PropertyMetadata (null));
        HintTextColorProperty =DependencyProperty.Register ("HintTextColor", typeof (SolidColorBrush), typeof (AnimationHintTextbox), new PropertyMetadata (null));
        HintTextProperty =DependencyProperty.Register ("HintText", typeof (string), typeof (AnimationHintTextbox), new PropertyMetadata (null));
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationHintTextbox), new FrameworkPropertyMetadata(typeof(AnimationHintTextbox)));
    }

    public AnimationHintTextbox()
    {
        this.GotFocus += AnimationHintTextbox_GotFocus;
        this.LostFocus += AnimationHintTextbox_LostFocus;
    }

    private void AnimationHintTextbox_LostFocus(object sender, RoutedEventArgs e)
    {
        this.FocusedExitAction ();
    }

    private void AnimationHintTextbox_GotFocus(object sender, RoutedEventArgs e)
    {
        this.FocusedEnterAction ();
    }

    Viewbox vb;
    Border bdr;
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate ();
        vb = GetTemplateChild ("PART_HintArea") as Viewbox;
        bdr = GetTemplateChild ("PART_BDR") as Border;

        this.CaretBrush = this.Foreground;
    }

    private void FocusedEnterAction()
    {
        var topPoint = -(this.vb.ActualHeight / 2);

        if (String.IsNullOrEmpty (this.Text) == false && Canvas.GetTop(this.vb) == topPoint)
            return;
        Duration duration = new Duration (new System.TimeSpan (0, 0, 0, 0, 100));

        DoubleAnimation da = new DoubleAnimation (0, topPoint, duration);
        this.vb.BeginAnimation (Canvas.TopProperty, da);
        
        this.bdr.BorderBrush = this.Foreground;
    }

    private void FocusedExitAction()
    {
        var topPoint = -(this.vb.ActualHeight / 2);
        if (String.IsNullOrEmpty (this.Text) == false && Canvas.GetTop (this.vb) == topPoint)
            return;

        Duration duration = new Duration (new System.TimeSpan (0, 0, 0, 0, 100));

        DoubleAnimation da = new DoubleAnimation (topPoint, 0, duration);
        this.vb.BeginAnimation (Canvas.TopProperty, da);

        this.bdr.BorderBrush = this.HintTextColor;
    }
}
