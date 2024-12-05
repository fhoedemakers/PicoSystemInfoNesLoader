# CHANGELOG

# General Info

Download and extract the zipfile PicoSystemInfoNesLoader.zip to a folder of choice, then start PicoSystemInfoNesLoader.exe in subfolder PicoSystemInfoNesLoader. It can take a minute before the application starts.

> When you see the message "Windows protected your PC", click on "More Info", then click on "Run anyway". See [Readme](README.md) for more info.

Tested on Windows 11 x64.

## v0.8-alpha

Fixes:

- Fix filename handling in ProcessStartInfo arguments. [#8](https://github.com/fhoedemakers/PicoSystemInfoNesLoader/issues/8)
  
Features:

- none
  
## v0.7-alpha

Fixes:

- Fixed issue [#5](https://github.com/fhoedemakers/PicoSystemInfoNesLoader/issues/5) Black console screen shown when application is built using GitHub Action. Created new BuildAndRelease.yml action which runs on windows-latest. Disabled the old dotnet.yml workflow

Features:

- none.
   
## v0.6-alpha

Fixes:

- Fixed version check. VersionInfo.cs will be updated automatically by the build process when build is triggered by a tag.

Features:

- none

## v0.5-alpha

Fixes:

- none

Features:

- Automatic build and release creation using GitHub actions.

## v0.4-alpha

Fixes:

 - Fix program crash, because of wrong assumptions made of the output from picotool.exe. Made parsing more robust now.
 
Features:

 - none

## v0.3-alpha

Fixes:

- removed unused mapper 6

Features:

- none
  
## v0.2-alpha

Fixes: 

- Update check for ui
- Added picture to main form

Features:

- none

