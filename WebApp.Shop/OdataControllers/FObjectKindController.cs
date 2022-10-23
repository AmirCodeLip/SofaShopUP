using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.WebModels.FileManager;
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

        [EnableQuery]
        public IQueryable<FObjectKind> Get()
        {
            return fileManagerStructure.GetFObjectKindsFromFolder();
        }
    }
}
