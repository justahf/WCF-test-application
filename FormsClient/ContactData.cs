using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsClient
{
    public class ContactData
    {
        public ContactData()
        {
            name = "noname";
            number = 0;
        }

        public ContactData(string _name, int _number)
        {
            name = _name;
            number = _number;
        }
        public string name;
        public int number;
    }
}
