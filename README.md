# AdvancedColorPicker

An open source color picker component for Xamarin.iOS that is very easy to use.

## Online Demos

You can view live demos through https://appetize.io/:

  - [iPhoneDemo][3] 
  - [iPad Demo][4] 

## Usage

AdvancedColorPicker is very simple and easy to use. 
There are three main ways to use AdvancedColorPicker: 

  1. `Present` & `PresentAsync`
  2. `ColorPickerViewController`
  3. `ColorPickerView`

### Present & PresentAsync

There are two helper methods that allow for quickly presenting a color picker. 
The first is `PresentAsync` that returns the selected color:

    UIColor color = await ColorPickerViewController.PresentAsync(
        NavigationController, 
        "Pick a color!",
        View.BackgroundColor);
    
    // use selected color
        
In the case when async method aren't preferrable, there is the synchronous
`Present` method that takes a callback:

    ColorPickerViewController.Present(
        NavigationController, 
        "Pick a color!",
        View.BackgroundColor,
        color => {
            // use selected color
        });

![AdvancedColorPicker][1]

### ColorPickerViewController

There is the `ColorPickerViewController`, which is a stand-alone controller that can be used
to preesent a color picker to the user:

    // create the picker
    var picker = new ColorPickerViewController {
        Title = "Pick a color!",
        SelectedColor = View.BackgroundColor
    };
    
    // events for colors as they are picked
    picker.ColorPicked += (sender, e) => {
        // use selected color
        View.BackgroundColor = e.SelectedColor;
    }
    
    // create the picker popup
    var pickerNav = new UINavigationController(picker);
    pickerNav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
    pickerNav.NavigationBar.Translucent = false;
    var doneBtn = new UIBarButtonItem(UIBarButtonSystemItem.Done);
    picker.NavigationItem.RightBarButtonItem = doneBtn;
    doneBtn.Clicked += delegate {
        // "Done" was clicked
        
        // use selected color
        View.BackgroundColor = picker.SelectedColor;
        
        // hide the picker
        NavigationController.DismissModalViewController(true);
    };
    
    // show the picker
    NavigationController.PresentModalViewController(pickerNav, true);

### ColorPickerView

If there is need to embed the picker into another view, this can be done
using `ColorPickerView`:

    var colorPicker = new ColorPickerView();
    colorPicker.ColorPicked += (sender, e) => {
        var color = e.SelectedColor;
        
        // use selected color
    };

### ColorPickerViewController

There is the `ColorPickerViewController`, which is a stand-alone controller that can be used
to preesent a color picker to the user:

    // create the picker
    var picker = new ColorPickerViewController {
        Title = "Pick a color!",
        SelectedColor = View.BackgroundColor
    };
    
    // events for colors as they are picked
    picker.ColorPicked += (sender, e) => {
        // use selected color
        View.BackgroundColor = e.SelectedColor;
    }
    
    // create the picker popup
    var pickerNav = new UINavigationController(picker);
    pickerNav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
    pickerNav.NavigationBar.Translucent = false;
    var doneBtn = new UIBarButtonItem(UIBarButtonSystemItem.Done);
    picker.NavigationItem.RightBarButtonItem = doneBtn;
    doneBtn.Clicked += delegate {
        // "Done" was clicked
        
        // use selected color
        View.BackgroundColor = picker.SelectedColor;
        
        // hide the picker
        NavigationController.DismissModalViewController(true);
    };
    
    // show the picker
    NavigationController.PresentModalViewController(pickerNav, true);

![AdvancedColorPicker][2]

## Getting Colors

Both `ColorPickerView` and `ColorPickerViewContoller` have the `SelectedColor` event 
that can be used to detect when the selected color changes. 
There will be multiple events as the user drags a finger on the screen:

    var colorPicker = new ColorPickerView();
    colorPicker.ColorPicked += (sender, e) => {
        var color = e.SelectedColor;
        
        // use selected color
    };

To get the last color that was selected, we can use the `SelectedColor` property:

    var colorPicker = new ColorPickerView();
    // ...
    var color = colorPicker.SelectedColor;

## License

AdvancedColorPicker is licensed under the terms of the MIT license.

If you use this component in your projects consider adding the following in you app about screen:

> This app uses AdvancedColorPicker developed by Yiannis Bourkelis & Matthew Leibowitz

[1]: https://raw.githubusercontent.com/mattleibow/AdvancedColorPicker/master/Images/iPhone_colorpicker.png
[2]: https://raw.githubusercontent.com/mattleibow/AdvancedColorPicker/master/Images/iPad_landscape_colorpicker.png
[3]: https://appetize.io/app/1uzmreu5wh2vzm79vpag1nwmqm?device=iphone5s&scale=75&orientation=portrait&autoplay=true
[4]: https://appetize.io/app/1uzmreu5wh2vzm79vpag1nwmqm?device=ipadair&scale=50&orientation=landscape&deviceColor=black&autoplay=true
