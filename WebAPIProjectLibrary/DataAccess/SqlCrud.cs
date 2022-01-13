using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIProjectLibrary.Models;

namespace WebAPIProjectLibrary.DataAccess
{
    public class SqlCrud
    {
        private readonly string _connectionString;

        private SqlDataAccess db = new SqlDataAccess();

        public SqlCrud(string connectionString)
        {
            _connectionString = connectionString;
        }
        public FullSongModel GetOneSongById(int songId)
        {
            FullSongModel output = new FullSongModel();
            string sql = "select Id, Title, FileData from dbo.Songs where Id = @Id;";
            var basicSongModel = db.LoadData<SongModel, dynamic>(sql, new { Id = songId }, _connectionString).First();
            output.Id = basicSongModel.Id;
            output.Title = basicSongModel.Title;
            output.FileData = basicSongModel.FileData;

            sql = "select Artists.Id, Artists.Name from(dbo.Artists inner join dbo.SongArtist on Artists.Id = SongArtist.ArtistId) where SongArtist.SongId = @SongId;";
            ArtistModel artistModel = db.LoadData<ArtistModel, dynamic>(sql, new { SongId = songId }, _connectionString).First();
            output.Artist = artistModel;

            sql = "select Albums.Id, Albums.Title from(dbo.Albums inner join dbo.SongAlbum on Albums.Id = SongAlbum.AlbumId) where SongAlbum.SongId = @SongId;";
            AlbumModel albumModel = db.LoadData<AlbumModel, dynamic>(sql, new { SongId = songId }, _connectionString).First();
            output.Album = albumModel;

            return output;
        }

        public List<FullSongModel> GetAllSongs()
        {
            List<FullSongModel> output = new List<FullSongModel>();
            string sql = "select Id, Title, FileData from dbo.Songs;";
            var basicSongList = db.LoadData<SongModel, dynamic>(sql, new { }, _connectionString);
            foreach (var song in basicSongList)
            {
                FullSongModel fullSong = new FullSongModel();
                fullSong.Id = song.Id;
                fullSong.Title = song.Title;
                fullSong.FileData = song.FileData;

                sql = "select Artists.Id, Artists.Name from(dbo.Artists inner join dbo.SongArtist on Artists.Id = SongArtist.ArtistId) where SongArtist.SongId = @SongId;";
                ArtistModel artist = db.LoadData<ArtistModel, dynamic>(sql, new { SongId = fullSong.Id }, _connectionString).First();
                fullSong.Artist = artist;

                sql = "select Albums.Id, Albums.Title from(dbo.Albums inner join dbo.SongAlbum on Albums.Id = SongAlbum.AlbumId) where SongAlbum.SongId = @SongId;";
                AlbumModel album = db.LoadData<AlbumModel, dynamic>(sql, new { SongId = fullSong.Id }, _connectionString).First();
                fullSong.Album = album;

                output.Add(fullSong);
            }
            return output;
        }

        public void SaveFullSongModel(FullSongModel song)
        {

            string sql = "insert into dbo.Songs (Title, FileData) values (@Title, @FileData);";
            db.SaveData(sql, new { song.Title, song.FileData }, _connectionString);

            sql = "select Id from dbo.Songs where Title = @Title and FileData = @FileData;";
            int songId = db.LoadData<IdLookupModel, dynamic>(sql, new { song.Title, song.FileData }, _connectionString).First().Id;

            sql = "select Id from dbo.Artists where Name = @Name";
            var artistIdList = db.LoadData<IdLookupModel, dynamic>(sql, new { song.Artist.Name }, _connectionString);
            if (artistIdList.Count == 0)
            {
                sql = "insert into dbo.Artists (Name) values (@Name);";
                db.SaveData(sql, new { song.Artist.Name }, _connectionString);
            }
            sql = "select Id from dbo.Artists where Name = @Name;";
            int artistId = db.LoadData<IdLookupModel, dynamic>(sql, new { song.Artist.Name }, _connectionString).First().Id;

            sql = "insert into dbo.SongArtist (SongId, ArtistId) values (@SongId, @ArtistId);";
            db.SaveData(sql, new { SongId = songId, ArtistId = artistId }, _connectionString);

            sql = "select AlbumId from dbo.SongAlbum where SongId = @Id";
            var albumIdList = db.LoadData<IdLookupModel, dynamic>(sql, new { Id = songId }, _connectionString);

            if (albumIdList.Count == 0)
            {
                sql = "insert into dbo.Albums (Title) values (@Title);";
                db.SaveData(sql, new { song.Album.Title }, _connectionString);
            }
            sql = "select Id from dbo.Albums where Title = @Title;";
            int albumId = db.LoadData<IdLookupModel, dynamic>(sql, new { song.Album.Title }, _connectionString).First().Id;

            sql = "insert into dbo.SongAlbum (SongId, AlbumId) values (@SongId, @AlbumId);";
            db.SaveData(sql, new { SongId = songId, AlbumId = albumId }, _connectionString);

            sql = "select Id from dbo.ArtistAlbum where AlbumId = @AlbumId and ArtistId = @ArtistId;";
            var artistAlbumIdList = db.LoadData<IdLookupModel, dynamic>(sql, new { AlbumId = albumId, ArtistId = artistId }, _connectionString);
            if (artistAlbumIdList.Count == 0)
            {
                sql = "insert into dbo.ArtistAlbum (AlbumId, ArtistId) values (@AlbumId, @ArtistId);";
                db.SaveData(sql, new { AlbumId = albumId, ArtistId = artistId }, _connectionString);
            }

        }
    }
}

