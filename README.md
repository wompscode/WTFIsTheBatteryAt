# ![logo](logo_128.png) WTFIsTheBatteryAt 
I didn't like PlayStation Accessories, and wanted a better way to see my battery usage. Now we're here.  
Pre-compiled releases available [here](https://github.com/wompscode/WTFIsTheBatteryAt/releases).  
WTFIsTheBatteryAt checks for an update on startup, and will prompt you if you'd like to come back and grab a new version. (implemented in release 1.1.1)  

# Dependencies
Visual Studio Community 2022 was used in the creation of this application. Usage of a newer/older version may yield inconsistencies.  
`HidSharp`:  `2.1.0` ([NuGet](https://www.nuget.org/packages/HidSharp/2.1.0))  
  
# Compiling
Should be as straight forward as cloning the repository, ensuring that you have [HidSharp](https://www.nuget.org/packages/HidSharp/2.1.0) installed, loading it into Visual Studio and then building the solution.  
If you would like to avoid debug functionality being enabled by default in the final build, ensure that you are compiling as `Release` and not `Debug`.
  
# Debugging
Enabling debug functionality in general usage is not required, and generally *not recommended* - however, if you'd like to figure out where it's getting hung up on or just like having a console in the background, you can run a compiled release executable with `--debug` or click the `Warning %` label in the Settings tab, and it'll relaunch with debug functionality enabled.
  
# Limitations
I do not own a DualSense Edge, or more than one DualSense, so I cannot guarantee that it works with either a DualSense Edge *or* more than one DualSense connected.  
If you test this, and it is non functional - you are free to make an issue, but I make no guarantees it will be resolved fast (if at all).
  
# Credits / Acknowledgements
Thanks to WujekFoliarz's work on Wujek-Dualsense-API - without it, this project would probably not exist.   
This project uses parts of its code, although rewritten in order to only contain functionality I required (battery reading, lightbar colour, et cetera.).  
  
Thank you to my friend, lexd0g, for suggesting the tray icon contain the battery percentage.