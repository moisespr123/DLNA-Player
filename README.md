# DLNA Player
DLNA Player is a software that acts as a server and as a controller. Simply, select a renderer, drag and drop your music files, and play them to your renderer.

![](https://moisescardona.me/files/DLNA-Player-v0.1.PNG)
The software will play the next track when the current track finishes playing. Because it is a controller software, you can also pause and resume, stop, or play the next or previous tracks. Also, you can use the Track Seek Bar to seek the audio track.

# Configuring the local DLNA server
1. Check that the IP Address shown is the same as your PC is currently using. This is needed as the software will use a local server to send the media files to your renderer.
2. If the IP Address is different or you want to change the port number, do so and press the "Apply" button.

# Playing back music files
1. Scan for Media Renderers
2. Select your media rendenderer. If you don't see it in the list, retry the scan and check your renderer is configured propertly
3. Drag and drop your music files
4. Double click the track you want to play or press the "Play" button
5. Enjoy!

Tested using a HiBy R3 player Hi Res Player

Developed using the C# language, using Visual Studio 2017.

Uses open source code from the following CodeProject projects:
* [DLNAMediaServer](https://www.codeproject.com/Articles/1079847/DLNA-Media-Server-to-feed-Smart-TVs)
* [DLNACore](https://www.codeproject.com/articles/893791/dlna-made-easy-with-play-to-from-any-device)
