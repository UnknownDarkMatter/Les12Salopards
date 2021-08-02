using System;
using System.Collections.Generic;
using System.Text;

namespace Les12Salopards.Entities
{
    public interface IContrat
    {
        ICondamné Condamné { get; set; }
        decimal MiseAPrixVivant { get; set; }
        decimal MiseAPrixMort { get; set; }
    }
}
