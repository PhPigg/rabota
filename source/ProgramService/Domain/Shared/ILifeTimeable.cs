using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shared
{
    public interface ILifeTimeable
    {
        EntityLifeTime LifeTime { get; set; }  
    }
}
