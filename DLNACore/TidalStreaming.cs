using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTidl;
using OpenTidl.Models;
using OpenTidl.Methods;
namespace DLNAPlayer
{
    public class Tidl
    {
        private OpenTidlSession session;
        private OpenTidlClient client;
        public bool isLoggedIn = false;
        public List<string> AlbumNames = new List<string> { };
        private List<int> AlbumIDs = new List<int> { };
        public List<string> TrackNames = new List<string> { };
        public List<int> TrackIDs = new List<int> { };
        public OpenTidl.Enums.SoundQuality UserSoundQuality;

        public Tidl()
        {
            client = new OpenTidlClient(ClientConfiguration.Default);
        }

        public async Task<bool> login(string username, string password)
        {
            bool loggedIn = false;
            try
            {
                session = await client.LoginWithUsername(username, password);
                loggedIn = true;
                await getSubDetails();
                await getAlbums();
            }
            catch
            {

            }
            isLoggedIn = loggedIn;
            return loggedIn;
        }

        public async Task<bool> getAlbums()
        {
            AlbumNames.Clear();
            AlbumIDs.Clear();
            OpenTidl.Models.Base.JsonList<OpenTidl.Models.Base.JsonListItem<AlbumModel>> albumListOrigData = await session.GetFavoriteAlbums();
            List<AlbumModel> albumList = new List<AlbumModel> { };
            foreach (OpenTidl.Models.Base.JsonListItem<AlbumModel> item in albumListOrigData.Items)
            {
                albumList.Add(item.Item);
            }
            albumList = albumList.OrderBy(item => item.Artist.Name).ThenBy(item => item.Title).ToList();
            foreach (AlbumModel item in albumList)
            {
                AlbumNames.Add(item.Artist.Name + " - " + item.Title);
                AlbumIDs.Add(item.Id);
            }
            return true;
        }
        public async Task<bool> getTracks(int albumId)
        {
            OpenTidl.Models.Base.JsonList<TrackModel> trackList = await client.GetAlbumTracks(AlbumIDs[albumId]);
            TrackNames.Clear();
            TrackIDs.Clear();
            foreach (TrackModel item in trackList.Items)
            {
                if (item.AllowStreaming)
                {
                    TrackIDs.Add(item.Id);
                    TrackNames.Add(item.TrackNumber.ToString() + " - " + item.Title);
                }
            }
            return true;
        }
        private async Task<bool> getSubDetails()
        {
            UserSubscriptionModel sub = await session.GetUserSubscription();
            UserSoundQuality = sub.HighestSoundQuality;
            return true;
        }
        public async Task<String> getStreamURL(int trackId)
        {
            StreamUrlModel streamUrl;
            try
            {
                try
                {
                    streamUrl = await session.GetTrackStreamUrl(trackId, UserSoundQuality, "");
                }
                catch
                {
                    try
                    {
                        streamUrl = await session.GetTrackStreamUrl(trackId, OpenTidl.Enums.SoundQuality.HIGH, "");
                    }
                    catch
                    {
                        streamUrl = await session.GetTrackStreamUrl(trackId, OpenTidl.Enums.SoundQuality.LOW, "");
                    }
                }
                return streamUrl.Url;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
