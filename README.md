# WTFIsTheBatteryAt
I got sick of PlayStation Accessories, so this now exists.  
Compile and use it as you want.  
  
# Debugging
If you would like to view logs via console, go into the settings of the Project Solution, and set the Application Type to `Console Application`.  
The application will function identically, just with a console in the background.  
  
# Compiling
You will need HIDSharp. Install it from NuGet.  
Set the solution configuration at the top of Visual Studio from `Debug` to `Release`, to avoid any debug stuff in regular usage.  
That's all.  
  
  
Thanks to WujekFoliarz for Wujek-Dualsense-API, even though I no longer directly use it.  
This project uses parts of its code, I just didn't want to use the whole library not only to keep the project slimmer, but also because there was some functionality that bothered me.  
Without it, this project would probably not exist. Because I wouldn't want to probe and figure out how to read the battery/set the lightbar colour otherwise.