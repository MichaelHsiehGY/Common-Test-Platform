using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;


namespace Organization
{

        public class DictionaryPropertyGridAdapter :DictionaryBase, ICustomTypeDescriptor
        {
           public IDictionary _dictionary=null;
           public List<Para> list = new List<Para>();
           
          

            public void Add(Para emp)
            {
                this.Dictionary.Add(emp.Name, emp.Value);
                list.Add(emp);
            }

            public void Remove(Para emp)
            {
               
                this.Dictionary.Remove(emp.Name);
                list.Remove(emp);
            }


            public Para this[int index]
            {
                get
                {
                    return (Para)this.Dictionary[list[index].Name];
                   
                }
            }

 
            public string GetComponentName()
            {
                return TypeDescriptor.GetComponentName(this, true);
            }

            public EventDescriptor GetDefaultEvent()
            {
                return TypeDescriptor.GetDefaultEvent(this, true);
            }

            public string GetClassName()
            {
                return TypeDescriptor.GetClassName(this, true);
            }
            //
            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                return TypeDescriptor.GetEvents(this, attributes, true);
            }

            EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
            {
                return TypeDescriptor.GetEvents(this, true);
            }

            public TypeConverter GetConverter()
            {
                return TypeDescriptor.GetConverter(this, true);
            }

            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                _dictionary = this.Dictionary;
                return _dictionary;

            }

            public AttributeCollection GetAttributes()
            {
                return TypeDescriptor.GetAttributes(this, true);
            }

            public object GetEditor(Type editorBaseType)
            {
                return TypeDescriptor.GetEditor(this, editorBaseType, true);
            }

            public PropertyDescriptor GetDefaultProperty()
            {
                return null;
            }


            PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
            {
                return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
            }

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                _dictionary = this.Dictionary;
                ArrayList properties = new ArrayList();
                foreach (DictionaryEntry e in _dictionary)
                {
                    properties.Add(new DictionaryPropertyDescriptor(_dictionary, e.Key,list));
                }

                PropertyDescriptor[] props =
                    (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

                return new PropertyDescriptorCollection(props);
            }
            

        }
        class DictionaryPropertyDescriptor : PropertyDescriptor
        { 
            IDictionary _dictionary;
            List<Para> uList;
            object _key;
            //public Para uPara;

            internal DictionaryPropertyDescriptor(IDictionary d, object key, List<Para> mList)
                : base(key.ToString(), null)
            {
                _dictionary = d;
                _key = key;
                uList = mList;
            }

            public override Type PropertyType
            {
                get { return _dictionary[_key].GetType(); }
            }

            public override void SetValue(object component, object value)
            {
                _dictionary[_key] = value;
                foreach (Para mPara in uList)
                {
                    if (mPara.Name == _key.ToString())
                    {
                        mPara.Value = value.ToString();
                    }
                }
                
            }

            public override object GetValue(object component)
            {
                return _dictionary[_key];
            }

             public override bool IsReadOnly
            {
                get { return false; }

            }

            public override Type ComponentType
            {
                get { return null; }
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override void ResetValue(object component)
            {
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }


            public override string Description
            {
                get
                {
                    foreach (Para mPara in uList)
                    {
                        if (mPara.Name==_key.ToString())
                        {
                            return mPara.Description;
                        }
                    }
                    return "";
                }
            }

           
        }

        }



