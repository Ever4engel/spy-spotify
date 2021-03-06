﻿using EspionSpotify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EspionSpotify.MediaTags
{
    public class MapperID3
    {
        public string CurrentFile { get; set; }
        public int? Count { get; set; }
        public bool OrderNumberInMediaTagEnabled { get; set; }
        public Track Track { get; set; }
        
        private readonly IFileSystem _fileSystem;

        internal MapperID3(string currentFile, Track track, bool orderNumberInMediaTagEnabled, int? count = null):
            this(fileSystem: new FileSystem(), currentFile, track, orderNumberInMediaTagEnabled, count) { }

        public MapperID3(IFileSystem fileSystem, string currentFile, Track track, bool orderNumberInMediaTagEnabled, int? count = null)
        {
            _fileSystem = fileSystem;
            CurrentFile = currentFile;
            Track = track;
            OrderNumberInMediaTagEnabled = orderNumberInMediaTagEnabled;
            Count = count;
        }

        public async Task MapTags(TagLib.Tag tags)
        {
            var trackNumber = GetTrackNumber();
            if (trackNumber.HasValue)
            {
                tags.Track = (uint)trackNumber.Value;
            }

            tags.Title = Track.Title;
            tags.Subtitle = Track.TitleExtended;

            tags.AlbumArtists = Track.AlbumArtists ?? new[] { Track.Artist };
            tags.Performers = Track.Performers ?? new[] { Track.Artist };

            tags.Album = Track.Album;
            tags.Genres = Track.Genres;

            tags.Disc = (uint)(Track.Disc ?? 0);
            tags.Year = (uint)(Track.Year ?? 0);

            await FetchMediaPictures();

            tags.Pictures = GetMediaPictureTag();
        }

        internal async Task SaveMediaTags()
        {
            var mp3 = TagLib.File.Create(CurrentFile);
            
            await MapTags(mp3.Tag);

            if (_fileSystem.File.Exists(CurrentFile))
            {
                mp3.Save();
            }

            mp3.Dispose();
        }

        private async Task FetchMediaPictures()
        {
            var taskGetArtXL = GetAlbumCover(Track.ArtExtraLargeUrl);
            var taskGetArtL = GetAlbumCover(Track.ArtLargeUrl);
            var taskGetArtM = GetAlbumCover(Track.ArtMediumUrl);
            var taskGetArtS = GetAlbumCover(Track.ArtSmallUrl);

            await Task.WhenAll(taskGetArtXL, taskGetArtL, taskGetArtM, taskGetArtS);

            Track.ArtExtraLarge = await taskGetArtXL;
            Track.ArtLarge = await taskGetArtL;
            Track.ArtMedium = await taskGetArtM;
            Track.ArtSmall = await taskGetArtS;
        }

        private TagLib.IPicture[] GetMediaPictureTag()
        {
            var pictures = (new TagLib.IPicture[4]
            {
                GetAlbumCoverToPicture(Track.ArtExtraLarge),
                GetAlbumCoverToPicture(Track.ArtLarge),
                GetAlbumCoverToPicture(Track.ArtMedium),
                GetAlbumCoverToPicture(Track.ArtSmall)
            }).Where(x => x != null);

            return pictures.Any() ? new[] { pictures.First() } : null;
        }

        private int? GetTrackNumber()
        {
            if (OrderNumberInMediaTagEnabled && Count.HasValue)
            {
                return Count.Value;
            }

            return Track.AlbumPosition;
        }

        private static TagLib.Picture GetAlbumCoverToPicture(byte[] data)
        {
            if (data == null) return null;

            return new TagLib.Picture
            {
                Type = TagLib.PictureType.FrontCover,
                MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                Data = data
            };
        }

        private static async Task<byte[]> GetAlbumCover(string link)
        {
            if (string.IsNullOrWhiteSpace(link)) return null;

            try
            {
                var request = WebRequest.Create(link);
                using (var response = await request.GetResponseAsync())
                {
                    var stream = response.GetResponseStream();
                    if (stream == null)
                    {
                        response.Dispose();
                        return null;
                    }
                    using (var reader = new BinaryReader(stream))
                    {
                        using (var memory = new MemoryStream())
                        {
                            var buffer = reader.ReadBytes(4096);
                            while (buffer.Length > 0)
                            {
                                await memory.WriteAsync(buffer, 0, buffer.Length);
                                buffer = reader.ReadBytes(4096);
                            }
                            response.Dispose();

                            return memory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
