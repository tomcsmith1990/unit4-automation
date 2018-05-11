using System;
using System.Data;

namespace Unit4.Interfaces
{
    internal interface IUnit4Engine
    {
        DataSet RunReport(string resql);
    }
}