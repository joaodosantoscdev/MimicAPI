using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers.Swagger
{
    public class ActionHidingConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel action)
        {
            // Replace with any logic you want
            if (action.ApiExplorer.GroupName == "v2")
            {
                if (action.ApiExplorer.IsVisible.HasValue)
                {
                    action.ApiExplorer.IsVisible = true;
                }
            }
        }
    }
}
