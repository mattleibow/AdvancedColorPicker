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
using System.Threading.Tasks;

#if __UNIFIED__
using CoreGraphics;
using UIKit;
#else
using MonoTouch.UIKit;

using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
using nfloat = System.Single;
#endif

namespace AdvancedColorPicker
{
    public class ColorPickerViewController : UIViewController
    {
        private ColorPickerView colorPicker;

        public ColorPickerViewController()
        {
            Initialize(UIColor.FromHSB(0.5984375f, 0.5f, 0.7482993f));
        }

        public ColorPickerViewController(nfloat hue, nfloat saturation, nfloat brightness)
        {
            Initialize(UIColor.FromHSB(hue, saturation, brightness));
        }

        public ColorPickerViewController(UIColor color)
        {
            Initialize(color);
        }

        protected void Initialize(UIColor color)
        {
            colorPicker = new ColorPickerView(color);
        }

        public UIColor SelectedColor
        {
            get { return colorPicker.SelectedColor; }
            set { colorPicker.SelectedColor = value; }
        }

        public event EventHandler<ColorPickedEventArgs> ColorPicked
        {
            add { colorPicker.ColorPicked += value; }
            remove { colorPicker.ColorPicked -= value; }
        }

        public override void LoadView()
        {
            View = colorPicker;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.All;
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        public override bool ShouldAutorotate()
        {
            return true;
        }

        public static void PresentModal(UIViewController parent, string title, UIColor initialColor, Action<UIColor> done)
        {
            Present(parent, title, initialColor, done, null);
        }
        
        public static void Present(UIViewController parent, string title, UIColor initialColor, Action<UIColor> done, Action<UIColor> colorPicked)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var picker = new ColorPickerViewController
            {
                Title = title,
                SelectedColor = initialColor
            };
            picker.ColorPicked += (_, args) =>
            {
                if (colorPicked != null)
                {
                    colorPicked(args.SelectedColor);
                }
            };

            var pickerNav = new UINavigationController(picker);
            pickerNav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
            pickerNav.NavigationBar.Translucent = false;

            var doneBtn = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            picker.NavigationItem.RightBarButtonItem = doneBtn;
            doneBtn.Clicked += delegate
            {
                if (done != null)
                {
                    done(picker.SelectedColor);
                }

                // hide the picker
                parent.DismissViewController(true, null);
            };

            // show the picker
            parent.PresentViewController(pickerNav, true, null);
        }

        public static Task<UIColor> PresentAsync(UIViewController parent, string title, UIColor initialColor)
        {
            return PresentAsync(parent, title, initialColor, null);
        }

        public static Task<UIColor> PresentAsync(UIViewController parent, string title, UIColor initialColor, Action<UIColor> colorPicked)
        {
            var tcs = new TaskCompletionSource<UIColor>();
            Present(parent, title, initialColor, color =>
            {
                tcs.SetResult(color);
            }, colorPicked);
            return tcs.Task;
        }
    }
}
