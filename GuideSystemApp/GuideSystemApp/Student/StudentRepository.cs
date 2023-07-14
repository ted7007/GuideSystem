using System.Collections.Generic;
using System.IO;
using System.Text;
using GuideSystemApp.Student.RB;
using GuideSystemApp.Student.Hash;
using GuideSystemApp.Student.List;
using GuideSystemApp.Student;

namespace GuideSystemApp.Student
{
    public class StudentRepository
    {

        public List<Student> StudentArray;
        public HashTable HashTable;
        public RB_Tree StudentFIO;
        public RB_Tree Group;
        public RB_Tree AdmissionDate;


        public StudentRepository()
        {
            this.StudentArray = new List<Student>();
            StudentFIO= new RB_Tree();
            Group= new RB_Tree();
            AdmissionDate= new RB_Tree();
            HashTable = new HashTable(20);

        }

        public void ReadFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {

                int count = int.Parse(reader.ReadLine()); // Преобразование строку в число



                // Записываем данные в массив
                for (int i = 0; i < count; i++)
                {
                    string[] studentstr = reader.ReadLine().Split('/');
                    Student student = new Student(studentstr[0], studentstr[1], studentstr[2], studentstr[3]);
                    StudentArray.Add(student);
                }
            }

            CreateIndexes();
        }
        private void CreateIndexes()
        {
            for (int i = 0; i < StudentArray.Count; i++)
            {
                StudentFIO.Insert(StudentArray[i].FIO, i);
                Group.Insert(StudentArray[i].Group, i);               
                AdmissionDate.Insert(StudentArray[i].AdmissionDate, i);
                HashTable.Add(StudentArray[i].Passport, i);
            }
        }

        public void Add(Student student)
        {
            StudentArray.Add(student);
            AddToIndexes(StudentArray.Count - 1);
        }
        private void AddToIndexes(int i)
        {
            StudentFIO.Insert(StudentArray[i].FIO, i);
            Group.Insert(StudentArray[i].Group, i);
            AdmissionDate.Insert(StudentArray[i].AdmissionDate, i);
            HashTable.Add(StudentArray[i].Passport, i);
        }

        public List<Student> GetAll()
        {
            return StudentArray;
        }

        public void WriteToFile(string path)
        {

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(StudentArray.Count);

                foreach (var student in StudentArray)
                {
                    writer.WriteLine($"{student.FIO}\\{student.Group}\\{student.Passport}\\{student.AdmissionDate}");
                }
            }
        }

        public void Delete(Student student)
        {
            int res = Find(student);
            if (res == -1)
                return;
            var removeItem = StudentArray[res];
            int index = StudentArray.Count - 1;
            if (index == res)
            {
                StudentFIO.Delete(removeItem.FIO, res);
                Group.Delete(removeItem.Group, res);
                AdmissionDate.Delete(removeItem.AdmissionDate, res);

                HashTable.Delete(removeItem.Passport, res);
            }
            else
            {
            StudentArray[res] = StudentArray[index];
            StudentArray[index] = removeItem;
            StudentArray.Remove(removeItem);

            //удаляем

            StudentFIO.Delete(removeItem.FIO, res);
            Group.Delete(removeItem.Group, res);
            AdmissionDate.Delete(removeItem.AdmissionDate, res);

            HashTable.Delete(removeItem.Passport, res);
                //заменяем
                PrintStudentFIO();
                PrintStudentGroup();
                PrintStudentAdmissionDate();
                PrintHashTable();

            removeItem = StudentArray[res];

            StudentFIO.EditValue(removeItem.FIO, res, index);
            Group.EditValue(removeItem.Group, res, index);
            AdmissionDate.EditValue(removeItem.AdmissionDate, res, index);

            HashTable.Edit(removeItem.Passport, res);
            }
            

        }

        private int Find(Student student)
        {

            var res = HashTable.Search(student.Passport);
            return res;
        }

        public void PrintStudentFIO()
        {
            Console.WriteLine("Дерево ФИО:");
            StudentFIO.PrintTree();
        }

        public void PrintStudentGroup()
        {
            Console.WriteLine("Дерево Группы:");
            Group.PrintTree();
        }

        public void PrintStudentAdmissionDate()
        {
            Console.WriteLine("Дерево Даты поступления:");
            AdmissionDate.PrintTree();
        }

        public void PrintHashTable()
        {
            Console.WriteLine("Хэш-таблица:");
            HashTable.Print();

        }
    }
        
}
