﻿using EspionSpotify.Models;
using SpotifyAPI.Web.Models;
using System.Threading.Tasks;

namespace EspionSpotify.MediaTags
{
    public interface ISpotifyAPI
    {
        Task<bool> UpdateTrack(Track track, string forceQueryTitle = null);

        void MapSpotifyTrackToTrack(Track track, FullTrack spotifyTrack);

        void MapSpotifyAlbumToTrack(Track track, FullAlbum spotifyAlbum);
    }
}
