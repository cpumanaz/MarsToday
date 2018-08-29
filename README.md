# MarsToday

The MarsToday web application allows you to retrieve images for days specified in the App_Data\DateList.txt file. 
If you are interested in images for a particular date, or something interesting was found on Mars, add the date and browse the images.
By using the Photos tab, you can review thumbnails, and click on a thumbnail to see the fullsize image and choose to download it.

Requirements:
  .NET Core 2.0 (SDK, Runtime)
  Docker CE (Windows Containers)
  
  Configuration:
    Application Settings in appsettings.json.
      APIURI - The NASA API for Mars photos.
      APIVersion - The version of the mars phootos api. Default is v1.
      APIKey - The API key you get by signing up here:
          https://api.nasa.gov/index.html#apply-for-an-api-key
      RoverName - The Mars rover images are gathered from. Default is curiosity.
      Camera - Images listed will be from the camera defined here. The MAST camera is used by default as it has the best images.
      
      
 Enjoy!
 


