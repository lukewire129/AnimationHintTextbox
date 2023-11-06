using Jamesnet.Wpf.Controls;
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

    double topPoint = 0.0;
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged (sizeInfo);

        topPoint = -(sizeInfo.NewSize.Height/ 2);
    }

    private void FocusedEnterAction()
    {
        if (String.IsNullOrEmpty (this.Text) == false && Canvas.GetTop(this.vb) == topPoint)
            return;
        var sb = new Storyboard ();
        
        ValueItem TopMove = GetValueItem (To: topPoint,
                                        path: new PropertyPath (Canvas.TopProperty));
        ValueItem WidthSize = GetValueItem (From: 100,
                                            To: 70,
                                            path: new PropertyPath (Viewbox.WidthProperty));
        sb.Children.Add(TopMove);
        sb.Children.Add (WidthSize);
        sb.Begin (this.vb);


        this.bdr.BorderBrush = this.Foreground;
    }

    private ValueItem GetValueItem(PropertyPath path, double From=0.0, double To = 0.0)
    {
        ValueItem valueItem = new ValueItem ();
        valueItem.TargetName = this.vb.Name;
        valueItem.Property = path;
        valueItem.From = From;
        valueItem.To = To;
        valueItem.Duration = new Duration (new System.TimeSpan (0, 0, 0, 0, 500));
        valueItem.Mode = Jamesnet.Wpf.Animation.EasingFunctionBaseMode.ExponentialEaseInOut;
        return valueItem;
    }

    private void FocusedExitAction()
    {
        if (String.IsNullOrEmpty (this.Text) == false && Canvas.GetTop (this.vb) == topPoint)
            return;

        var sb = new Storyboard ();

        ValueItem TopMove = GetValueItem (From: topPoint,
                                          path: new PropertyPath (Canvas.TopProperty));

        ValueItem WidthSize = GetValueItem (From: 70, 
                                            To: 100, 
                                            path: new PropertyPath (Viewbox.WidthProperty));
        sb.Children.Add (TopMove);
        sb.Children.Add (WidthSize);


        sb.Begin (this.vb);

        this.bdr.BorderBrush = this.HintTextColor;
    }
}
