// --- BEGIN LICENSE BLOCK ---
/*
 * Copyright (c) 2017-present WeWantToKnow AS
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
// --- END LICENSE BLOCK ---

#if UNITY_5_3_OR_NEWER
using Uplift;
using Uplift.Schemas;
using Uplift.Common;
using NUnit.Framework;
using System;
using Uplift.Testing.Helpers;
using System.IO;
using System.Linq;

namespace Uplift.Testing.Integration
{
    [TestFixture]
    class CustomizedFileLocation
    {
        private UpliftManager manager;
        private string upfile_path;
        private string pwd;

        [OneTimeSetUp]
        protected void Init()
        {
            UpliftManagerExposer.ClearAllInstances();

            pwd = Directory.GetCurrentDirectory();
        }

        [SetUp]
        protected void BeforeEach()
        {
            // Upfile Cleanup
            UpfileExposer.ClearInstance();
            UpliftManagerExposer.ClearAllInstances ();

            // Move to test running directory
            Helper.InitializeRunDirectory();
            Directory.SetCurrentDirectory(Helper.testRunDirectoryName);
        }

        [TearDown]
        protected void AfterEach()
        {
            // Remove (hopefully) installed files
            Directory.SetCurrentDirectory(pwd);
        }

        [OneTimeTearDown]
        protected void CleanUp()
        {
            // Clean the Upfile
            UpfileExposer.ClearInstance();
            Helper.ClearRunDirectory();
        }

        [Test]
        public void WhenUpfileNotModified()
        {
            // Upfile Setup
            upfile_path = Helper.GetLocalFilePath("..", "TestData", "CustomizedFileLocation", "Upfile_NotModified.xml");

            try
            {
                UpfileExposer.SetInstance(UpfileExposer.LoadTestXml(upfile_path));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Make sure you are running the test from UpliftTesting/TestResults. The Upfile.xml uses the current path to register the repositories.");
                Assert.IsTrue(false, "The test could not run correctly. See console message.");
            }
            Upfile.Instance();
            manager = UpliftManager.Instance();

            manager.InstallDependencies();

            // Directories existence
            Assert.IsTrue(Directory.Exists("UPackages"), "Directory UPackages does not exist");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5"), "Package directory does not exist under UPackages");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Media"), "Media directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Example"), "Example directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Example/Adv"), "Example advanced directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("Assets"), "Directory Assets does not exist");
            Assert.IsTrue(Directory.Exists("Assets/UPackages"), "Directory UPackages under Assets does not exist");
            Assert.IsTrue(Directory.Exists("Assets/UPackages/package_c~1.3.5"), "Package directory does not exist under Assets/UPackages");
            Assert.IsTrue(Directory.Exists("Assets/UPackages/package_c~1.3.5/Media"), "Media directory does not exist under Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("Assets/UPackages/package_c~1.3.5/Example"), "Example directory does not exist under Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("Assets/UPackages/package_c~1.3.5/Example/Adv"), "Example advanced directory does not exist under Assets/UPackages/package_c~1.3.5");

            // Files under Assets/UPackages
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/C1.cs"), "File C1 did not get copied to Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/C2.cs"), "File C2 did not get copied to Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/Media/M1.txt"), "File M1 did not get copied to Assets/UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/Media/M2.txt"), "File M2 did not get copied to Assets/UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/Example/E1.txt"), "File E1 did not get copied to Assets/UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/Example/E2.txt"), "File E2 did not get copied to Assets/UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/Example/Adv/E3.txt"), "File E2 did not get copied to Assets/UPackages/package_c~1.3.5/Example/Adv");

            // Files under UPackages
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/C1.cs"), "File C1 did not get copied to UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/C2.cs"), "File C2 did not get copied to UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Media/M1.txt"), "File M1 did not get copied to UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Media/M2.txt"), "File M2 did not get copied to UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/E1.txt"), "File E1 did not get copied to UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/E2.txt"), "File E2 did not get copied to UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/Adv/E3.txt"), "File E2 did not get copied to UPackages/package_c~1.3.5/Example/Adv");
        }

        [Test]
        public void WhenUpfileModifiedNoSkip()
        {
            // Upfile Setup
            upfile_path = Helper.GetLocalFilePath("..", "TestData", "CustomizedFileLocation", "Upfile_Modified_NoSkip.xml");

            try
            {
                UpfileExposer.SetInstance(UpfileExposer.LoadTestXml(upfile_path));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Make sure you are running the test from UpliftTesting/TestResults. The Upfile.xml uses the current path to register the repositories.");
                Assert.IsTrue(false, "The test could not run correctly. See console message.");
            }
            Upfile.Instance();
            manager = UpliftManager.Instance();

            manager.InstallDependencies();

            // Directories existence
            Assert.IsTrue(Directory.Exists("UPackages"), "Directory UPackages does not exist");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5"), "Package directory does not exist under UPackages");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Media"), "Media directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Example"), "Example directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Example/Adv"), "Example advanced directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("Assets"), "Directory Assets does not exist");
            Assert.IsTrue(Directory.Exists("Assets/UPackages"), "Directory UPackages under Assets does not exist");
            Assert.IsTrue(Directory.Exists("Assets/UPackages/package_c~1.3.5"), "Package directory does not exist under Assets/UPackages");
            Assert.IsTrue(Directory.Exists("Assets/Media"), "Media directory does not exist under Assets");
            Assert.IsTrue(Directory.Exists("Assets/Media/package_c~1.3.5"), "Package directory does not exist under Assets/Media");
            Assert.IsTrue(Directory.Exists("Examples"), "Examples directory does not exist");
            Assert.IsTrue(Directory.Exists("Examples/package_c~1.3.5"), "Package directory does not exist under Examples");
            Assert.IsTrue(Directory.Exists("Examples/package_c~1.3.5/Adv"), "Advanced directory does not exist under Examples/package_c~1.3.5");

            // Files under Assets
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/C1.cs"), "File C1 did not get copied to Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/C2.cs"), "File C2 did not get copied to Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Assets/Media/package_c~1.3.5/M1.txt"), "File M1 did not get copied to Assets/Media/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Assets/Media/package_c~1.3.5/M2.txt"), "File M2 did not get copied to Assets/Media/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Examples/package_c~1.3.5/E1.txt"), "File E1 did not get copied to Examples/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Examples/package_c~1.3.5/E2.txt"), "File E2 did not get copied to Examples/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Examples/package_c~1.3.5/Adv/E3.txt"), "File E2 did not get copied to Examples/package_c~1.3.5/Adv");

            // Files under UPackages
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/C1.cs"), "File C1 did not get copied to UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/C2.cs"), "File C2 did not get copied to UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Media/M1.txt"), "File M1 did not get copied to UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Media/M2.txt"), "File M2 did not get copied to UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/E1.txt"), "File E1 did not get copied to UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/E2.txt"), "File E2 did not get copied to UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/Adv/E3.txt"), "File E2 did not get copied to UPackages/package_c~1.3.5/Example/Adv");
        }

        [Test]
        public void WhenUpfileModifiedAndSkip()
        {
            // Upfile Setup
            upfile_path = Helper.GetLocalFilePath("..", "TestData", "CustomizedFileLocation",  "Upfile_Modified_Skip.xml");

            try
            {
                UpfileExposer.SetInstance(UpfileExposer.LoadTestXml(upfile_path));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Make sure you are running the test from UpliftTesting/TestResults. The Upfile.xml uses the current path to register the repositories.");
                Assert.IsTrue(false, "The test could not run correctly. See console message.");
            }
            Upfile.Instance();
            manager = UpliftManager.Instance();

            manager.InstallDependencies();

            // Directories existence
            Assert.IsTrue(Directory.Exists("UPackages"), "Directory UPackages does not exist");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5"), "Package directory does not exist under UPackages");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Media"), "Media directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Example"), "Example directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("UPackages/package_c~1.3.5/Example/Adv"), "Example advanced directory does not exist under UPackages/package_c~1.3.5");
            Assert.IsTrue(Directory.Exists("Assets"), "Directory Assets does not exist");
            Assert.IsTrue(Directory.Exists("Assets/UPackages"), "Directory UPackages under Assets does not exist");
            Assert.IsTrue(Directory.Exists("Assets/UPackages/package_c~1.3.5"), "Package directory does not exist under Assets/UPackages");
            Assert.IsTrue(Directory.Exists("Assets/Media"), "Media directory does not exist under Assets");
            Assert.IsTrue(Directory.Exists("Examples"), "Examples directory does not exist");
            Assert.IsTrue(Directory.Exists("Examples/Adv"), "Advanced directory does not exist under Examples");

            // Files under Assets
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/C1.cs"), "File C1 did not get copied to Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Assets/UPackages/package_c~1.3.5/C2.cs"), "File C2 did not get copied to Assets/UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("Assets/Media/M1.txt"), "File M1 did not get copied to Assets/Media");
            Assert.IsTrue(File.Exists("Assets/Media/M2.txt"), "File M2 did not get copied to Assets/Media");
            Assert.IsTrue(File.Exists("Examples/E1.txt"), "File E1 did not get copied to Examples");
            Assert.IsTrue(File.Exists("Examples/E2.txt"), "File E2 did not get copied to Examples");
            Assert.IsTrue(File.Exists("Examples/Adv/E3.txt"), "File E2 did not get copied to Examples/Adv");

            // Files under UPackages
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/C1.cs"), "File C1 did not get copied to UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/C2.cs"), "File C2 did not get copied to UPackages/package_c~1.3.5");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Media/M1.txt"), "File M1 did not get copied to UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Media/M2.txt"), "File M2 did not get copied to UPackages/package_c~1.3.5/Media");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/E1.txt"), "File E1 did not get copied to UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/E2.txt"), "File E2 did not get copied to UPackages/package_c~1.3.5/Example");
            Assert.IsTrue(File.Exists("UPackages/package_c~1.3.5/Example/Adv/E3.txt"), "File E2 did not get copied to UPackages/package_c~1.3.5/Example/Adv");
        }
    }
}
#endif
