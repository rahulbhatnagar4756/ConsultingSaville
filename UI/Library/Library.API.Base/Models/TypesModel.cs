using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Base.Models
{

    public class TypesModel: ICloneable, ISelection
    {
        public string? UUID { get; set; }
        public string? TrackingUUID { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
        public bool? isSelected { get; set; } = false;

        public object Clone()
        {
            return new TypesModel
            {
                UUID = this.UUID,
                TrackingUUID = this.TrackingUUID,
                Name = this.Name,
                FullName = this.FullName,
                Description = this.Description,
                Value = this.Value,
                isSelected = this.isSelected
            };
        }
    }
}
