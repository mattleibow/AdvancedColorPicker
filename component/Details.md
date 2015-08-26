# AdvancedColorPicker

<iframe src="https://appetize.io/embed/1uzmreu5wh2vzm79vpag1nwmqm?device=iphone5s&scale=75&autoplay=true&orientation=portrait&deviceColor=black" 
        width="274px" height="587px" frameborder="0" scrolling="no"
        style="float:right;margin-left:1em;">&nbsp;</iframe>

An open source color picker component for Xamarin.iOS that is very easy to use.

## Usage

AdvancedColorPicker is very simple and easy to use. There are two helper methods 
that allow for quickly presenting a color picker. 
The first is `PresentAsync` that returns the selected color:

    var color = await ColorPickerViewController.PresentAsync(
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

If there is need to embed the picker into another view, this can be done
using `ColorPickerView`:

    var colorPicker = new ColorPickerView();
    colorPicker.ColorPicked += (sender, e) => {
        var color = e.SelectedColor;
        
        // use selected color
    };

Compatibility
==============
AdvancedColorPicker is tested on iOS 4.3+, 5.0+, 6.0+, both on iPhone and iPad.

All devices, screen sizes and orientations are supported because AdvancedColorPicker 
does not use images nor nib files, but custom drawing and dynamic views creation 
to display everything.
