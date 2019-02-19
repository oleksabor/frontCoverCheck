### mp3 front cover check

This utility checks mp3 files for front cover image presense. mp3 name is listed in the console if no image was set.

Current directory is used to check for files by default. You can override this with `-d` option like
```
dotnet run mp3Pictures.csproj  -d c:\somewhere\path
```

Addtionally you can check mp3 front cover picture for max|min size (in pixels) using `-maxP` or `-minP` options
Please note that only first mp3 front cover image is checked.

Program was built using **Mono.Options** and **taglib**
