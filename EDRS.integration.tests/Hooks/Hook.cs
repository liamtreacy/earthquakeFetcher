using System;
using BoDi;
using TechTalk.SpecFlow;

namespace tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }
        
        [AfterScenario]
        public async Task ClearData()
        {}

    }
}