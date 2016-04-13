using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Common.Tests
{
    /// <summary>
    /// this is the test class for uóur core functionalities
    /// </summary>
    [TestClass]
    public class ObjectBaseTests
    {
        [TestMethod]
        public void test_clean_property_change()
        {
            // todo: arrange
            var objTest = new TestClass();
            var propertyChanged = false;  
            objTest.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "CleanProp")
                    propertyChanged = true;
            };

            // todo: act
            objTest.CleanProp = "test value";

            // todo: assert
            Assert.IsTrue(propertyChanged, "Changing CleanProp should have set the notiication flag to true.");
        }

        [TestMethod]
        public void test_dirty_set()
        {
            // todo: arrange
            var objTest = new TestClass();

            // todo: assert the state
            Assert.IsFalse(objTest.IsDirty, "Object should be clean.");

            // todo: act
            objTest.DirtyProp = "test value";

            // todo: assert
            Assert.IsTrue(objTest.IsDirty, "Object should be dirty.");
        }

        [TestMethod]
        public void test_property_change_single_subscription()
        {
            // todo: arrange
            var objTest = new TestClass();
            var changeCounter = 0;
            var handler1 = new PropertyChangedEventHandler((s, e) => { changeCounter++; });
            var handler2 = new PropertyChangedEventHandler((s, e) => { changeCounter++; });

            // todo: act
            objTest.PropertyChanged += handler1;
            objTest.PropertyChanged += handler1; // should not duplicate
            objTest.PropertyChanged += handler1; // should not duplicate
            objTest.PropertyChanged += handler2;
            objTest.PropertyChanged += handler2; // should not duplicate

            objTest.CleanProp = "test value";

            // todo: assert
            Assert.IsTrue(changeCounter == 2, "Property change notification should only have been called once.");
        }

        [TestMethod]
        public void test_child_dirty_tracking()
        {
            // todo: arrange
            var objTest = new TestClass();

            // todo: assert the state
            Assert.IsFalse(objTest.IsAnythingDirty(), "Nothing in the object graph should be dirty.");

            // todo: act
            objTest.Child.ChildName = "test value";

            // todo: assert the state
            Assert.IsTrue(objTest.IsAnythingDirty(), "The object graph should be dirty.");

            // todo: act
            objTest.CleanAll();

            // todo: assert
            Assert.IsFalse(objTest.IsAnythingDirty(), "Nothing in the object graph should be dirty.");
        }

        [TestMethod]
        public void test_dirty_object_aggregating()
        {
            // todo: arrange
            var objTest = new TestClass();

            // todo: act
            var dirtyObjects = objTest.GetDirtyObjects();

            // todo: assert the state
            Assert.IsTrue(dirtyObjects.Count == 0, "There should be no dirty object returned.");

            // todo: act
            objTest.Child.ChildName = "test value";
            dirtyObjects = objTest.GetDirtyObjects();

            // todo: assert the state
            Assert.IsTrue(dirtyObjects.Count == 1, "There should be one dirty object.");

            // todo: act
            objTest.DirtyProp = "test value";
            dirtyObjects = objTest.GetDirtyObjects();

            // todo: assert the state
            Assert.IsTrue(dirtyObjects.Count == 2, "There should be two dirty object.");

            // todo: act
            objTest.CleanAll();
            dirtyObjects = objTest.GetDirtyObjects();

            // todo: assert
            Assert.IsTrue(dirtyObjects.Count == 0, "There should be no dirty object returned.");
        }

        [TestMethod]
        public void test_object_validation()
        {
            // todo: arrange
            var objTest = new TestClass();

            // todo: assert the state
            Assert.IsFalse(objTest.IsValid, "Object should not be valid as one its rules should be broken.");

            // todo: act
            objTest.StringProp = "Some value";

            // todo: assert
            Assert.IsTrue(objTest.IsValid, "Object should be valid as its property has been fixed.");
        }
    }
}
