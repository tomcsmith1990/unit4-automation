using System;
using Unit4.Automation.Commands;
using Unit4.Automation.Model;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using Unit4.Automation.Interfaces;
using System.Linq;

namespace Unit4.Automation
{
    internal class OptionsProvider
    {
        [ImportMany(typeof(IOptions))] private IEnumerable<IOptions> _options;

        public OptionsProvider(System.Reflection.Assembly assembly)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        public IEnumerable<Type> Types => _options.Select(x => x.GetType());
    }
}