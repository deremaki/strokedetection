using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stroke_detection.Processing.Bitmap.LUTMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stroke_detection.Processing.Bitmap.LUTMethods.Tests
{
    [TestClass()]
    public class LUTProcessTests
    {
        [TestMethod()]
        public void ProcessIntTest()
        {
            LUTProcess test = new LUTProcess();
            Assert.IsNull(test.ProcessInt(null));
            Assert.IsNull(test.ProcessInt(new int[5,5]));
            test.SetLUT(new int[10]);
            Assert.IsNull(test.ProcessInt(null));
            Assert.IsNotNull(test.ProcessInt(new int[1000, 10000]));
        }
    }
}