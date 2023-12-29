using backEnd.Data;
using backEnd.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Controllers{
    [ApiController]
    [Route("/api")]
    public class AssetController:ControllerBase{
        private readonly AssetDbContexte assetDbContexte;

        public AssetController(AssetDbContexte assetDbContexte){
            this.assetDbContexte=assetDbContexte;
        }
        // Post metheod Add One Asset
        [HttpPost]
public async Task<ActionResult> AddOneAsset([FromBody]Asset asset)
        {
            DateTime currentDate = DateTime.Now;
            string stringDate=currentDate.ToString();
            asset.date=stringDate;

            await  assetDbContexte.Assets.AddAsync(asset);
            await assetDbContexte.SaveChangesAsync();
            return Ok(asset);
        }

        // Get All Assets 
        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets=await assetDbContexte.Assets.ToListAsync();
            return Ok(assets);
        }

        // Delete One User : Delete Method
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> deleteOneAsset([FromRoute] int Id){

            var existingAsset = await assetDbContexte.Assets.FirstOrDefaultAsync(x=>x.id==Id);
            if(existingAsset!= null){
                assetDbContexte.Remove(existingAsset);
                await assetDbContexte.SaveChangesAsync();
                return Ok(existingAsset);
            }

            return NotFound("Asset Not Found");

        }

        // Update One Asset : Put Method
        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> updateOneAsset([FromRoute] int Id, [FromBody] Asset asset){

           var existingAsset = await assetDbContexte.Assets.FirstOrDefaultAsync(x=>x.id==Id);
           if (existingAsset!=null){

            existingAsset.product=asset.product;
            existingAsset.type=asset.type;
            existingAsset.vendor=asset.vendor;
            existingAsset.version=asset.version;
            existingAsset.name=asset.name;
            DateTime currentDate = DateTime.Now;
            string stringDate=currentDate.ToString();
            existingAsset.date=stringDate;
            await assetDbContexte.SaveChangesAsync();
            
            return Ok(existingAsset);

           }
           return NotFound("Asset Not Found");


        }

        // Get One User : GET Method
        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> getOneAsset([FromRoute] int Id){

            var existingAsset = await assetDbContexte.Assets.FirstOrDefaultAsync(x=>x.id==Id);
            if(existingAsset!= null){
                return Ok(existingAsset);
            }

            return NotFound("Asset Not Found");

        }


        [HttpGet]
        [Route("search/vendor/{query}")]
        public async Task<ActionResult<IEnumerable<Asset>>> Search(string query)
        {
            return await assetDbContexte.Assets
                .Where(e => e.vendor.StartsWith(query))
                .ToListAsync();
        }

          [HttpGet]
          [Route("search/")]
        public async Task<ActionResult<IEnumerable<Asset>>> SearchAll()
        {
            var assets=await assetDbContexte.Assets.ToListAsync();
            return Ok(assets);
        }


        [HttpGet]
        [Route("people")]
        public async Task<double> analysePeople(){

            List<Asset> liste = await assetDbContexte.Assets.Where(e => e.type=="people").ToListAsync();
            return liste.Count;
        }


        [HttpGet]
        [Route("object")]
        public async Task<double> analyseObject(){

            List<Asset> liste = await assetDbContexte.Assets.Where(e => e.type=="object").ToListAsync();
            return liste.Count;
        }



        [HttpGet]
        [Route("software")]
        public async Task<double> analyseSoftware(){

            List<Asset> liste = await assetDbContexte.Assets.Where(e => e.type=="software").ToListAsync();
            return liste.Count;
        }



        [HttpGet]
        [Route("hardware")]
        public async Task<double> analyseHardware(){

            List<Asset> liste = await assetDbContexte.Assets.Where(e => e.type=="hardware").ToListAsync();
            return liste.Count;
        }


        [HttpGet]
        [Route("Search/By/Name/{Name}")]
        public async Task<ActionResult<Asset> >SearchAssetByName(string Name){

            List<Asset> liste = await assetDbContexte.Assets.Where(e => e.name==Name).ToListAsync();
            return Ok(liste[0]);   
        }




    }
}  