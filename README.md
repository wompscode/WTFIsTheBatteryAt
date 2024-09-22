# WTFIsTheBatteryAt
I got sick of PlayStation Accessories, so this now exists.  
Compile and use it as you want.  

The icon is not pretty, but I'll probably update it eventually. (maybe)  
  
# Debugging
If you would like to view logs via console, go into the settings of the Project Solution, and set the Application Type to `Console Application`.  
The application will function identically, just with a console in the background.
  
If you want to debug via a release build, or just would prefer a console in the background, you can also just run the executable with `--debug` (or click the Warning % label in the Settings tab!), and it will show a console.
  
# Compiling
You will need HIDSharp. Install it from NuGet.  
Set the solution configuration at the top of Visual Studio from `Debug` to `Release`, to avoid any debug stuff in regular usage.  
That's all.  
  
# Limitations
I do not own a DualSense Edge, or more than one DualSense, so I cannot guarantee that it works with either a DualSense Edge *or* more than one DualSense connected.  
If you test this, and it is non functional, you are free to make an issue, but I make no guarantees it will be resolved fast.
  
Thanks to WujekFoliarz for Wujek-Dualsense-API, even though I no longer directly use it.  
This project uses parts of its code, I just didn't want to use the whole library not only to keep the project slimmer, but also because there was some functionality that bothered me.  
Without it, this project would probably not exist. Because I wouldn't want to probe and figure out how to read the battery/set the lightbar colour otherwise.