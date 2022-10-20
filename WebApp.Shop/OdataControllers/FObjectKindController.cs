using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApp.Shop.OdataControllers
{
    public class FObjectKindController : ODataController
    {
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(new List<FObjectKind>
            {
                new FObjectKind{Id=Guid.NewGuid(),Name="01",FObjectType=FObjectType.Folder},
                new FObjectKind{Id=Guid.NewGuid(),Name="02",FObjectType=FObjectType.Folder},
                new FObjectKind{Id=Guid.NewGuid(),Name="03",FObjectType=FObjectType.Folder},
                new FObjectKind{Id=Guid.NewGuid(),Name="04",FObjectType=FObjectType.Folder},
                new FObjectKind{Id=Guid.NewGuid(),Name="05",FObjectType=FObjectType.Folder},
                new FObjectKind{Id=Guid.NewGuid(),Name="06",FObjectType=FObjectType.Folder}
            }.AsQueryable());
        }
    }
}
