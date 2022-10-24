using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApp.Shop.OdataControllers
{
    public class FObjectKindController : ODataController
    {
        public readonly FileManagerStructure fileManagerStructure;
        public FObjectKindController(FileManagerStructure fileManagerStructure)
        {
            this.fileManagerStructure = fileManagerStructure;
        }


        [EnableQuery, Authorize]
        public async Task<IQueryable<FObjectKind>> Get(Guid? folderID)
        {
            return await fileManagerStructure.GetFObjectKindsFromFolder(this.HttpContext, folderID);
        }
    }
}
