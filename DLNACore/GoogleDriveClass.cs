using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Download;
namespace DLNAPlayer
{
    class GoogleDriveClass
    {
        static string[] Scopes = { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        static string SoftwareName = "DLNA Player";
        public DriveService service;
    }
    
}
