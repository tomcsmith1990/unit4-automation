using System;

namespace Unit4.Automation.Interfaces
{
    internal interface ICache<T>
    {
        T Fetch();
    }
}