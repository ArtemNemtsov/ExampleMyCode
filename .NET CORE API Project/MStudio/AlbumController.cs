using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediaStudioService;
using MediaStudioService.Core.Classes;

namespace MediaStudio.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        AlbumService albumService;
        public AlbumController(AlbumService albumService)
        {
            this.albumService = albumService;
        }

        // GET: Albums
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Responce>>> GetAlbumList()
        {
            return new ObjectResult(await albumService.GetAlbumListAsync());
        }

        // GET: Albums/5
        [HttpGet("{id}")]
        public ActionResult<Responce> GetAlbum(int id)
        {
            return new ObjectResult(albumService.GetAlbum(id));
        }
    }
}
