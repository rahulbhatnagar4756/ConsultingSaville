using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.Models;

public class FrogElementModel:ICloneable
{
    public string? FormsUUID { get; set; }
    public DateTime? DateStart { get; set; }
    public int? FrogStatusId { get; set; }
    public string? FrogElementsUUID { get; set; }
    public string? FrogElementsUUIDLink { get; set; }
    public int? FrogControlInputTypesId { get; set; }
    public int? FrogLookupTypesId { get; set; }
    public string? FrogSQLStoredProceduresUUID { get; set; }
    public string? FrogSQLStoredProceduresIdData { get; set; }
    public string? Name { get; set; }
    public string? Label { get; set; }
    public string? Placeholder { get; set; }
    public string? ResultValue { get; set; }
    public string? ResultText { get; set; }
    public string? CSSClass { get; set; }
    public string? Style { get; set; }
    public string? URL { get; set; }
    public int? OnClickStatus { get; set; }
    public int? OrderVal { get; set; }
    public bool? IsRequired { get; set; }
    public int? FrogSecurityPermissionsId { get; set; }
    public string? FrogSecurityPermissions { get; set; }

    public object Clone()
    {
        return new FrogElementModel()
        {
            FormsUUID = this.FormsUUID,
            DateStart = this.DateStart,
            FrogStatusId = this.FrogStatusId,
            FrogElementsUUID = this.FrogElementsUUID,
            FrogElementsUUIDLink = this.FrogElementsUUIDLink,
            FrogControlInputTypesId = this.FrogControlInputTypesId,
            FrogLookupTypesId = this.FrogLookupTypesId,
            FrogSQLStoredProceduresUUID = this.FrogSQLStoredProceduresUUID,
            FrogSQLStoredProceduresIdData = this.FrogSQLStoredProceduresIdData,
            Name = this.Name,
            Label = this.Label,
            Placeholder = this.Placeholder,
            ResultValue = this.ResultValue,
            ResultText = this.ResultText,
            CSSClass = this.CSSClass,
            Style = this.Style,
            URL = this.URL,
            OnClickStatus = this.OnClickStatus,
            OrderVal = this.OrderVal,
            IsRequired = this.IsRequired,
            FrogSecurityPermissionsId = this.FrogSecurityPermissionsId,
            FrogSecurityPermissions = this.FrogSecurityPermissions
        };
    }
}
