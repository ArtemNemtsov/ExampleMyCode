using DBContext.Connect;
using System;
using System.Collections.Generic;
using MediaStudioService.ApiModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MediaStudioService.Core.Classes;
using DBContext.Models;
using MediaStudioService.ModelBulder;
using MediaStudioService.Minio;
using System.Linq;

namespace MediaStudioService
{
    public class AlbumService
    {
        private readonly MediaStudioContext postgres;
        private readonly AlbumBulder bulder;
        public AlbumService(MediaStudioContext context, CloudServiceManager cloudManager) 
        { 
            postgres = context;
            bulder = new AlbumBulder(context, cloudManager);
        }

        public Responce GetAlbum(int idAlbum)
        {
            try
            {
                if (!AlbumExists(idAlbum)) throw new InvalidOperationException($"Ошибка! В базе данных не найден альбом с id {idAlbum}!");
                var postgresAlbum = postgres.Album.FindAsync(idAlbum).Result;
                var outputAlbum = bulder.BuldAlbum(postgresAlbum);
                outputAlbum.Tracks = bulder.GetListTracksAsync(postgresAlbum).Result;
                return RespоnceManager.CreateSucces(outputAlbum);
            }
            catch (Exception ex)
            {
                return RespоnceManager.CreateError(ex.Message);
            }
        }

        public async Task<Responce> GetAlbumListAsync()
        {
            List<OutputAlbum> albumsRespone = new List<OutputAlbum>();
            try
            {
                var postgeresAlbums = await postgres.Album.ToListAsync();
                foreach (var album in postgeresAlbums)
                {
                    var outputAlbum = bulder.BuldAlbum(album);
                    albumsRespone.Add(outputAlbum);
                }
                return RespоnceManager.CreateSucces(albumsRespone);
            }
            catch (Exception ex)
            {
                return RespоnceManager.CreateError(ex.Message);
            }
        }

        private bool AlbumExists(int id)
        {
            return postgres.Album.Any(e => e.IdAlbum == id);
        }
    }
}
