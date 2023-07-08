using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Переделанные_хэш_таблицы__под_курсач_
{
    internal class Item

    {
        public Student student;
        public int status;
        public int hash;


        public Item(Student student, int hash)
        {
            this.hash = hash;
            this.student = new Student(student.FIO, student.Group, student.Passport, student.AdmissionDate);
            status = 1;
        }
        public Item()
        {
            student = new Student("", "", "", "");
            status = 0;
        }
    }
}
