using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Enumerators;

public enum IntegrationsEnum
{
    Oasys,
    Jawa
}

public static class IntegrationsUUID
{
    public static readonly Dictionary<IntegrationsEnum, string> Values = new()
    {
        { IntegrationsEnum.Jawa, "673162C9-06B3-4120-9065-BB8F6A0D65F2" },
        { IntegrationsEnum.Oasys, "DB0B34E2-7F45-4C35-8C17-30FFAFBB8151" }
    };
}
