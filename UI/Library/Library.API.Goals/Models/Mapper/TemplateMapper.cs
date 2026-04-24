using Library.API.Goals.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Mapper
{
    public static class TemplateMapper
    {
        public static TemplateBasicModel FromTemplatesModelToTemplateBasicModel(this TemplatesModel template)
        {
            return new TemplateBasicModel
            {
                TemplatesUUID = template.TemplatesUUID, 
                Name = template.Name,
                Code = template.Code,
                Description = template.Description
            };
        }
    }
}
