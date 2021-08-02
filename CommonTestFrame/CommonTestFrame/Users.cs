using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace CommonTestFrame
{
	
	public class Users : CollectionBase
	{
		public void Add( User emp )
		{
            this.List.Add(emp);
		}
			
        public void Remove(User emp)
		{
            this.List.Remove(emp);
		}

        public User this[int index] 
		{
			get
			{
                return (User)this.List[index];
			}
		}

	}

    public class User
    {
        private string _userName = "";
        private string _userType = "";
        private string _password = "";
        private string _phone = "";
        private string _email = "";
        private string _remark="";
        private string _lastLoginTime = "";



        public string UserName
        {
            get {return _userName; }
            set { _userName = value; }
        }

        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        public string LastLoginTime
        {
            get { return _lastLoginTime; }
            set { _lastLoginTime = value; }
        }
    
    
    
    
    
    }

   
}
