using System;
using System.Collections.Generic;

namespace DBContext.Models
{
    public partial class Album
    {
        public Album()
        {
            AlbumToProperties = new HashSet<AlbumToProperties>();
            TrackToAlbum = new HashSet<TrackToAlbum>();
        }

        public int IdAlbum { get; set; }
        public string Name { get; set; }
        public long? Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime PublicationDate { get; set; }
        public long IdCloudPath { get; set; }

        public virtual CloudPath IdCloudPathNavigation { get; set; }
        public virtual ICollection<AlbumToProperties> AlbumToProperties { get; set; }
        public virtual ICollection<TrackToAlbum> TrackToAlbum { get; set; }
    }
}
