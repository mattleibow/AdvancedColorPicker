/*
 * This code is licensed under the terms of the MIT license
 *
 * Copyright (C) 2012 Yiannis Bourkelis & Matthew Leibowitz
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
using UIKit;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
using nfloat = System.Single;
#endif

using AdvancedColorPicker;

namespace AdvancedColorPickerDemo
{
    public class ContainerController : UIViewController
    {
        public ContainerController()
        {
            Title = "Pick a color!";
            View.BackgroundColor = UIColor.FromRGB(0.3f, 0.8f, 0.3f);

            var pickAColorBtn = UIButton.FromType(UIButtonType.RoundedRect);
            pickAColorBtn.Frame = new CGRect(UIScreen.MainScreen.Bounds.Width / 2 - 50, UIScreen.MainScreen.Bounds.Height / 2 - 22, 100, 44);
            pickAColorBtn.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
            pickAColorBtn.SetTitle("Pick a color!", UIControlState.Normal);
            pickAColorBtn.TouchUpInside += async delegate
            {
                var alert = new UIAlertView("Hi", "hi there", null, null);

                // get a color from the user
                var color = await ColorPickerViewController.PresentAsync(NavigationController, "Pick a color!", View.BackgroundColor);

                // changethe background
                View.BackgroundColor = color;

                // set the title to the hex value
                nfloat red, green, blue, alpha;
                color.GetRGBA(out red, out green, out blue, out alpha);
                Title = string.Format("#{0:X2}{1:X2}{2:X2}", (int)(red * 255), (int)(green * 255), (int)(blue * 255));
            };
            View.AddSubview(pickAColorBtn);
        }
    }
}
