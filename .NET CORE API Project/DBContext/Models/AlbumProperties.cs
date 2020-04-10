using System;
using System.Collections.Generic;

namespace DBContext.Models
{
    public partial class AlbumProperties
    {
        public int IdAlbumProperties { get; set; }
        public int IdAlbum { get; set; }
        public int IdProp { get; set; }

        public virtual Album IdAlbumNavigation { get; set; }
        public virtual Properties IdPropNavigation { get; set; }
    }
}
