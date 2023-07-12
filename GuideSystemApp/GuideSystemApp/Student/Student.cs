using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student
{
    public class Student
    {
        public string FIO;
        public string Group;
        public string Passport;
        public string AdmissionDate;
        public Student(string FIO, string Group, string Passport, string AdmissionDate)
        {
            this.FIO = FIO;
            this.Group = Group;
            this.Passport = Passport;
            this.AdmissionDate = AdmissionDate;
        }

    }
}
