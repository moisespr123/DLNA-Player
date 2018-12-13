# DLNA Player
DLNA Player is a software that acts as a server and as a controller. Simply, select a renderer, drag and drop your music files, and play them to your renderer.

![](https://moisescardona.me/files/DLNA-Player-v0.2.PNG)
The software will play the next track when the current track finishes playing. Because it is a controller software, you can also pause and resume, stop, or play the next or previous tracks. Also, you can use the Track Seek Bar to seek the audio track.

Additionally, you can play back music files stored in your Google Drive account:  
![](https://moisescardona.me/files/2018-07-10/1.webp)

# Configuring the local DLNA server
1. Check that the IP Address shown is the same as your PC is currently using. This is needed as the software will use a local server to send the media files to your renderer.
2. If the IP Address is different or you want to change the port number, do so and press the "Apply" button.

# Playing back music files
1. Scan for Media Renderers
2. Select your media rendenderer. If you don't see it in the list, retry the scan and check your renderer is configured propertly
3. Drag and drop your music files (Or Go to File -> Open Files)
4. Double click the track you want to play or press the "Play" button
5. Enjoy!

# Playing music files stored in Google Drive.
1. Make sure you follow the **Step 1** instructions here: [https://developers.google.com/drive/v3/web/quickstart/dotnet](https://developers.google.com/drive/v3/web/quickstart/dotnet)
2. The `client_secret.json` file needs to be stored in the same path where you extracted this software.
3. Go to Cloud -> Google Drive 
4. Authorize the app
5. Browse your drive content. There are 3 methods to add files to the play queue: 1. Double click a file to add it, 2. Select several files using the SHIFT or CONTROL key, then press Add Selected Files, or 3. Press the Add All Files button
6. Play back the files selecting a renderer device and double clicking the file to play or pressing the Play button in the main form.

# Playing an Audio CD.
1. Go to File -> Open Audio CD.
2. Choose the drive letter.
3. Press Open.
4. Choose Renderer.
5. Double click the track you want to play or press the "Play" button.

# Playing music from your Tidal library
This software uses the [OpenTidl](https://github.com/jackfagner/OpenTidl) C# library to provide Tidal streaming support.

1. Go to Cloud -> Tidal
2. Login with your username and password
3. Click an album from your library. The tracks will appear on the right panel.
4. Double click a track to add to the queue. You can also press the Enter key or select an item and press the "Add selected tracks" button. Multiple tracks can be selected.
5. Play back the files by selecting a rendered and pressing the Play button (or double click a track)


Tested using a HiBy R3 player Hi Res Player. Also works with foobar2000 using the DLNA plugin.

Developed using the C# language, using Visual Studio 2017.

Uses open source code from the following CodeProject projects:
* [DLNAMediaServer](https://www.codeproject.com/Articles/1079847/DLNA-Media-Server-to-feed-Smart-TVs)
* [DLNACore](https://www.codeproject.com/articles/893791/dlna-made-easy-with-play-to-from-any-device)
* [C Sharp Ripper](https://www.codeproject.com/articles/5458/c-sharp-ripper")

Also uses the [OpenTidl](https://github.com/jackfagner/OpenTidl) C# library to provide Tidal streaming support
