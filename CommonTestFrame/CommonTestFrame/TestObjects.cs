using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Organization;

namespace CommonTestFrame
{
    class TestObject
    {
        public int Index = -1;
        public bool IsSelect = false;
        public string Driver;
        public string Class;
        public string FunctionName;
        public string DisplayText;
        public int Loop = 1;
        public string ExecuteType = "Normal";
        public bool IsHasBeenExecuted = false;
        public string ParaCollection;
        public string ReturnType = "";
        public bool IsJudge = true;
        public bool IsShowResult = true;
        public int FunExecuteStatus = 0;
        public int FailIndex = -1;
        public SpecCollection SpecCollection;
        public string TestTime = "";
        public string Description = "";
        public object Instance = null;
    }




    class TestObjects : CollectionBase
    {
        public void Add(TestObject newTestObject)
        {  
            //Dictionary.Add(newTestObject.FunctionName, newTestObject);
            this.List.Add(newTestObject);
        }
        public void Remove(TestObject newTestObject)
        {
            //Dictionary.Remove(FunctionName);
            this.List.Remove(newTestObject);
        }
        public TestObjects()
        {
        }

        public TestObject this[int index]
        {
            get
            {
                return (TestObject)this.List[index];
            }
        }
        //public TestObject this[string FunctionName]
        //{
        //    get
        //    {
        //        return (TestObject)Dictionary[FunctionName];
        //    }
   /*         set
            {
                Dictionary[FunctionName] = value;
            }*/

        }

/*        public new IEnumerator GetEnumerator()
        {
            foreach (object testModule in Dictionary.Values)
                yield return (TestModule)testModule;
        }*/
    //}


 
}
