/*
 * This code is licensed under the terms of the MIT license
 *
 * Copyright (C) 2012 Yiannis Bourkelis
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights 
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is furnished
 * to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;

#if __UNIFIED__
using CoreGraphics;
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
using nfloat = System.Single;
#endif

namespace AdvancedColorPicker
{
    public class ColorPickerView : UIView
    {
        private CGSize satBrightIndicatorSize;
        private HuePickerView huePicker;
        private SaturationBrightnessPickerView satbrightPicker;
        private SelectedColorPreviewView preview;
        private HueIndicatorView hueIndicator;
        private SaturationBrightnessIndicatorView satBrightIndicator;

        public ColorPickerView()
        {
            Initialize();
        }

        public ColorPickerView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }
        
        public ColorPickerView(UIColor color)
            : base()
        {
            Initialize();
            SelectedColor = color;
        }
        
        protected void Initialize()
        {
            satBrightIndicatorSize = new CGSize(28, 28);

            var selectedColorViewHeight = (nfloat)60;
            var viewSpace = (nfloat)1;

            preview = new SelectedColorPreviewView();
            preview.Frame = new CGRect(0, 0, Bounds.Width, selectedColorViewHeight);
            preview.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            preview.Layer.ShadowOpacity = 0.6f;
            preview.Layer.ShadowOffset = new CGSize(0, 7);
            preview.Layer.ShadowColor = UIColor.Black.CGColor;

            satbrightPicker = new SaturationBrightnessPickerView();
            satbrightPicker.Frame = new CGRect(0, selectedColorViewHeight + viewSpace, Bounds.Width, Bounds.Height - selectedColorViewHeight - selectedColorViewHeight - viewSpace - viewSpace);
            satbrightPicker.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            satbrightPicker.ColorPicked += HandleColorPicked;
            satbrightPicker.AutosizesSubviews = true;

            huePicker = new HuePickerView();
            huePicker.Frame = new CGRect(0, Bounds.Bottom - selectedColorViewHeight, Bounds.Width, selectedColorViewHeight);
            huePicker.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin;
            huePicker.HueChanged += HandleHueChanged;

            var pos = huePicker.Frame.Width * huePicker.Hue;
            hueIndicator = new HueIndicatorView();
            hueIndicator.Frame = new CGRect(pos - 10, huePicker.Bounds.Y - 2, 20, huePicker.Bounds.Height + 2);
            hueIndicator.UserInteractionEnabled = false;
            hueIndicator.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
            huePicker.AddSubview(hueIndicator);

            var pos2 = new CGPoint(satbrightPicker.Saturation * satbrightPicker.Frame.Size.Width,
                                   satbrightPicker.Frame.Size.Height - (satbrightPicker.Brightness * satbrightPicker.Frame.Size.Height));
            satBrightIndicator = new SaturationBrightnessIndicatorView();
            satBrightIndicator.Frame = new CGRect(pos2.X - satBrightIndicatorSize.Width / 2, pos2.Y - satBrightIndicatorSize.Height / 2, satBrightIndicatorSize.Width, satBrightIndicatorSize.Height);
            satBrightIndicator.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin;
            satBrightIndicator.UserInteractionEnabled = false;
            satbrightPicker.AddSubview(satBrightIndicator);

            AddSubviews(satbrightPicker, huePicker, preview);
        }

        public UIColor SelectedColor
        {
            get
            {
                return UIColor.FromHSB(satbrightPicker.Hue, satbrightPicker.Saturation, satbrightPicker.Brightness);
            }
            set
            {
                nfloat hue = 0, brightness = 0, saturation = 0, alpha = 0;
                value.GetHSBA(out hue, out saturation, out brightness, out alpha);
                huePicker.Hue = hue;
                satbrightPicker.Hue = hue;
                satbrightPicker.Brightness = brightness;
                satbrightPicker.Saturation = saturation;
                preview.BackgroundColor = value;

                PositionIndicators();

                satbrightPicker.SetNeedsDisplay();
                huePicker.SetNeedsDisplay();
            }
        }

        public event EventHandler<ColorPickedEventArgs> ColorPicked;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            PositionIndicators();
        }

        private void PositionIndicators()
        {
            PositionHueIndicatorView();
            PositionSatBrightIndicatorView();
        }

        private void PositionSatBrightIndicatorView()
        {
            Animate(0.3f, 0f, UIViewAnimationOptions.AllowUserInteraction, () =>
            {
                var x = satbrightPicker.Saturation * satbrightPicker.Frame.Size.Width;
                var y = satbrightPicker.Frame.Size.Height - (satbrightPicker.Brightness * satbrightPicker.Frame.Size.Height);
                var pos = new CGPoint(x, y);
                var rect = new CGRect(
                    pos.X - satBrightIndicatorSize.Width / 2,
                    pos.Y - satBrightIndicatorSize.Height / 2,
                    satBrightIndicatorSize.Width,
                    satBrightIndicatorSize.Height);
                satBrightIndicator.Frame = rect;
            }, null);
        }

        private void PositionHueIndicatorView()
        {
            Animate(0.3f, 0f, UIViewAnimationOptions.AllowUserInteraction, () =>
            {
                var pos = huePicker.Frame.Width * huePicker.Hue;
                var rect = new CGRect(
                    pos - 10,
                    huePicker.Bounds.Y - 2,
                    20,
                    huePicker.Bounds.Height + 2);
                hueIndicator.Frame = rect;
            }, () =>
            {
                hueIndicator.Hidden = false;
            });
        }

        private void HandleColorPicked(object sender, EventArgs e)
        {
            PositionSatBrightIndicatorView();
            preview.BackgroundColor = UIColor.FromHSB(satbrightPicker.Hue, satbrightPicker.Saturation, satbrightPicker.Brightness);

            OnColorPicked(new ColorPickedEventArgs(SelectedColor));
        }

        private void HandleHueChanged(object sender, EventArgs e)
        {
            PositionHueIndicatorView();
            satbrightPicker.Hue = huePicker.Hue;
            satbrightPicker.SetNeedsDisplay();

            HandleColorPicked(sender, e);
        }
        
        protected virtual void OnColorPicked(ColorPickedEventArgs e)
        {
            var handler = ColorPicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
