using System;
using System.Collections.Generic;

namespace Dex.Com.Expresso
{
    public interface IParentObject
    {
        List<Object> ChildObjectList { get; set; }
    }
}

